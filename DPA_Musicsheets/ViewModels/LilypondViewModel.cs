using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DPA_Musicsheets.Models.Events;
using DPA_Musicsheets.ViewModels.Editor.Memento;
using DPA_Musicsheets.ViewModels.States.Editor;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private string _text;
        private int textCursorIndex;
        private readonly Stack<Memento> undoStack;
        private readonly Stack<Memento> redoStack;
        private Memento saved;
        private readonly Originator stackOriginator;
        private readonly Originator saveOriginator;
        private EditorState current;
        private readonly Dictionary<string, EditorState> states;

        /// <summary>
        /// This text will be in the textbox.
        /// It can be filled either by typing or loading a file so we only want to set previoustext when it's caused by typing.
        /// </summary>
        public string LilypondText
        {
            get => _text;
            set
            {
                stackOriginator.State = _text;
                if (!_textChangedByLoad)
                {
                    redoStack.Clear();
                    undoStack.Push(stackOriginator.Save());
                }
                _text = value;
                RaisePropertyChanged(() => LilypondText);
            }
        }

        private bool canEdit;
        public bool CanEdit
        {
            get => canEdit;
            set
            {
                canEdit = value;
                RaisePropertyChanged(() => CanEdit);
            }
        }
        public MusicLoader MusicLoader { get; set; }

        private bool _textChangedByLoad;

        public LilypondViewModel(MainViewModel mainViewModel, MusicLoader musicLoader)
        {
            // TODO: Can we use some sort of eventing system so the managers layer doesn't have to know the viewmodel layer and viewmodels don't know each other?
            // And viewmodels don't 
            MusicLoader = musicLoader;
            MusicLoader.LilypondViewModel = this;
            _text = "Your lilypond text will appear here.";
            textCursorIndex = 0;
            OwnEventmanager.Manager.Subscribe("addLilyPondToken", AddSymbol);
            undoStack = new Stack<Memento>();
            redoStack = new Stack<Memento>();
            stackOriginator = new Originator();
            saveOriginator = new Originator();
            saved = saveOriginator.Save();

            states = new Dictionary<string, EditorState>()
            {
                {"Idle", new IdleState() },
                {"Rendering", new RenderingState() },
                {"Playing", new BlockedState() }
            };

            ChangeState("Idle");
            OwnEventmanager.Manager.Subscribe("changeEditorState", ChangeState);
            OwnEventmanager.Manager.Subscribe("onClose", OnClose);
        }

        public void LilypondTextLoaded(string text)
        {
            _textChangedByLoad = true;
            undoStack.Clear();
            redoStack.Clear();
            LilypondText = text;
            saveOriginator.State = text;
            saved = saveOriginator.Save();
            _textChangedByLoad = false;
        }

        private void ChangeState(string newState)
        {
            current = states[newState];
            current.GoInto(this);
        }

        private void OnClose(string param)
        {
            if (saved.State != LilypondText)
            {
                if (MessageBox.Show("There are unsaved changes. Do you want to save these changes?", "Question",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Save();
                }
            }
        }

        private void AddSymbol(string symbol) => LilypondText = LilypondText?.Insert(textCursorIndex, symbol);

        /// <summary>
        /// This occurs when the text in the textbox has changed. This can either be by loading or typing.
        /// </summary>
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>(args =>
        {
            if (_textChangedByLoad)
            {
                return;
            }
            current = new RenderingState();
            current.GoInto(this);
        });

        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            stackOriginator.State = LilypondText;
            redoStack.Push(stackOriginator.Save());
            var memento = undoStack.Pop();
            stackOriginator.Restore(memento);
            _text = stackOriginator.State;
            RaisePropertyChanged(() => LilypondText);
        }, () => undoStack.Any());

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            stackOriginator.State = LilypondText;
            undoStack.Push(stackOriginator.Save());
            var memento = redoStack.Pop();
            stackOriginator.Restore(memento);
            _text = stackOriginator.State;
            RaisePropertyChanged(() => LilypondText);
        }, () => redoStack.Any());

        public ICommand SelectionChangedCommand => new RelayCommand<RoutedEventArgs>(e =>
        {
            TextBox textBox = e.Source as TextBox;
            textCursorIndex = textBox.CaretIndex;
        });

        public ICommand SaveAsCommand => new RelayCommand(Save);

        private void Save()
        {
            // TODO: In the application a lot of classes know which filetypes are supported. Lots and lots of repeated code here...
            // TODO save file event? -> the same as the main view model save?
            // Can this be done better?
            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            bool success = false;
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    MusicLoader.SaveToMidi(saveFileDialog.FileName);
                    success = true;
                }
                else if (extension.EndsWith(".ly"))
                {
                    MusicLoader.SaveToLilypond(saveFileDialog.FileName);
                    success = true;
                }
                else if (extension.EndsWith(".pdf"))
                {
                    MusicLoader.SaveToPDF(saveFileDialog.FileName);
                    success = true;
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
            if (success)
            {
                saveOriginator.State = LilypondText;
                saved = saveOriginator.Save();
            }
        }
        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
