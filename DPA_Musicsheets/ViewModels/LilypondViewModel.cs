using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DPA_Musicsheets.Models.Events;
using DPA_Musicsheets.ViewModels.Editor.Memento;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private MusicLoader _musicLoader;
        private MainViewModel _mainViewModel { get; set; }

        private string _text;
        private int textCursorIndex;
        private Stack<Memento> undoStack;
        private Stack<Memento> redoStack;
        private Originator originator;

        /// <summary>
        /// This text will be in the textbox.
        /// It can be filled either by typing or loading a file so we only want to set previoustext when it's caused by typing.
        /// </summary>
        public string LilypondText
        {
            get => _text;
            set
            {
                originator.State = _text;
                if (!_waitingForRender && !_textChangedByLoad)
                {
                    redoStack.Clear();
                    undoStack.Push(originator.Save());
                }
                _text = value;
                RaisePropertyChanged(() => LilypondText);
            }
        }

        private bool _textChangedByLoad = false;
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _waitingForRender = false;

        public LilypondViewModel(MainViewModel mainViewModel, MusicLoader musicLoader)
        {
            // TODO: Can we use some sort of eventing system so the managers layer doesn't have to know the viewmodel layer and viewmodels don't know each other?
            // And viewmodels don't 
            _mainViewModel = mainViewModel;
            _musicLoader = musicLoader;
            _musicLoader.LilypondViewModel = this;
            _text = "Your lilypond text will appear here.";
            textCursorIndex = 0;
            OwnEventmanager.Manager.Subscribe("addLilyPondToken", AddSymbol);
            undoStack = new Stack<Memento>();
            redoStack = new Stack<Memento>();
            originator = new Originator();
        }

        public void LilypondTextLoaded(string text)
        {
            _textChangedByLoad = true;
            undoStack.Clear();
            redoStack.Clear();
            LilypondText = text;
            _textChangedByLoad = false;
        }

        private void AddSymbol(string symbol) => LilypondText = LilypondText?.Insert(textCursorIndex, symbol);

        /// <summary>
        /// This occurs when the text in the textbox has changed. This can either be by loading or typing.
        /// </summary>
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            // If we were typing, we need to do things.
            if (!_textChangedByLoad)
            {
                _waitingForRender = true;
                _lastChange = DateTime.Now;

                _mainViewModel.CurrentState = "Rendering...";

                Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
                {
                    if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                    {
                        _waitingForRender = false;
                        UndoCommand.RaiseCanExecuteChanged();

                        _musicLoader.LoadLilypondIntoWpfStaffsAndMidi(LilypondText);
                        _mainViewModel.CurrentState = "";
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
            }
        });

        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            originator.State = LilypondText;
            redoStack.Push(originator.Save());
            var memento = undoStack.Pop();
            originator.Restore(memento);
            _text = originator.State;
            RaisePropertyChanged(() => LilypondText);
        }, () => undoStack.Any());

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            originator.State = LilypondText;
            undoStack.Push(originator.Save());
            var memento = redoStack.Pop();
            originator.Restore(memento);
            _text = originator.State;
            RaisePropertyChanged(() => LilypondText);
        }, () => redoStack.Any());

        public ICommand SelectionChangedCommand => new RelayCommand<RoutedEventArgs>(e =>
        {
            TextBox textBox = e.Source as TextBox;
            textCursorIndex = textBox.CaretIndex;
        });

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            // TODO: In the application a lot of classes know which filetypes are supported. Lots and lots of repeated code here...
            // TODO save file event? -> the same as the main view model save?
            // Can this be done better?
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    _musicLoader.SaveToMidi(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".ly"))
                {
                    _musicLoader.SaveToLilypond(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    _musicLoader.SaveToPDF(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
        });
        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
