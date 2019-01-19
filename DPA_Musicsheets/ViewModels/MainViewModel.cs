using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DPA_Musicsheets.Models.Commands;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly List<Key> downKeyQueue;
        private readonly List<Key> upKeyQueue;
        private string _fileName;
        private readonly Command firstCommand;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        /// <summary>
        /// The current state can be used to display some text.
        /// "Rendering..." is a text that will be displayed for example.
        /// </summary>
        private string _currentState;
        public string CurrentState
        {
            get => _currentState;
            set { _currentState = value; RaisePropertyChanged(() => CurrentState); }
        }

        private MusicLoader _musicLoader;

        public MainViewModel(MusicLoader musicLoader)
        {
            // TODO: Can we use some sort of eventing system so the managers layer doesn't have to know the viewmodel layer?
            _musicLoader = musicLoader;
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";
            downKeyQueue = new List<Key>();
            upKeyQueue = new List<Key>();
            CommandBuilder cb = new CommandBuilder();
            firstCommand = cb.BuildCommands(musicLoader);
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            FileName = OpenOpenFileDialog("Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly");
        });

        public ICommand LoadCommand => new RelayCommand(() =>
        {
            firstCommand.Execute(ActionOption.OpenFile, FileName);
        });

        #region Focus and key commands, these can be used for implementing hotkeys
        public ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
            Console.WriteLine(@"Maingrid Lost focus");
        });

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>(e =>
        {
            if (!downKeyQueue.Contains(e.Key))
            {
                Key key = e.Key == Key.System ? e.SystemKey : e.Key;
                downKeyQueue.Add(key);
                e.Handled = true;
            }
            Console.WriteLine($@"Key down: {e.Key}");
        });

        public ICommand OnKeyUpCommand => new RelayCommand<KeyEventArgs>(e =>
       {
           Key key = e.Key == Key.System ? e.SystemKey : e.Key;
           upKeyQueue.Add(key);
           if (downKeyQueue.Contains(key))
           {
               downKeyQueue.Remove(key);
           }
           if (!downKeyQueue.Any() && upKeyQueue.Count > 1)
           {
               if (upKeyQueue[0] != Key.LeftAlt && upKeyQueue[0] != Key.RightAlt)
               {
                   upKeyQueue.Reverse();
               }
               HandleCommand(upKeyQueue);
               upKeyQueue.Clear();
           }
           Console.WriteLine($@"Key Up: {key}");
       });

        private string OpenOpenFileDialog(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = filter };
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        private string OpenSaveFileDialog(string filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = filter };
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }

        private void ShowErrorDialog(string error)
        {
            MessageBox.Show(error, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void HandleCommand(IReadOnlyList<Key> keyCombo)
        {
            if (!keyCombo.Any() || keyCombo.Count < 2 || keyCombo.Count > 3)
            {
                //Not a known command based on metadata, so it's just a symbol add in the box (presumably)
                return;
            }
            ActionOption action = ActionOption.Undefined;
            string param = null;
            if (keyCombo[0] == Key.LeftCtrl || keyCombo[0] == Key.RightCtrl)
            {
                string filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly";
                if (keyCombo[1] == Key.O)
                {
                    param = OpenOpenFileDialog(filter);
                    if (param != null)
                    {
                        // Dialog has been cancelled or has failed, abort
                        action = ActionOption.OpenFile;
                        FileName = param;
                    }
                }
                else if (keyCombo[1] == Key.S)
                {
                    if (keyCombo.Count == 2)
                    {
                        action = ActionOption.SaveAsLilyPond;
                        filter = "Lilypond files (*.ly)|*.ly";
                    }
                    else if (keyCombo[3] == Key.P)
                    {
                        action = ActionOption.SaveAsPdf;
                        filter = "Pdf files (*.pdf)|.pdf";
                    }
                    param = OpenSaveFileDialog(filter);
                    if (param == null)
                    {
                        // Dialog has been cancelled or has failed, abort
                        action = ActionOption.Undefined;
                    }
                }
            }
            else if (keyCombo[0] == Key.LeftAlt || keyCombo[0] == Key.RightAlt)
            {
                switch (keyCombo[1])
                {
                    case Key.C:
                        action = ActionOption.AddClefTreble;
                        break;
                    case Key.S:
                        action = ActionOption.AddTempo;
                        break;
                    case Key.B:
                        action = ActionOption.AddBarLines;
                        break;
                    case Key.T:
                        action = ActionOption.AddTime;
                        if (keyCombo.Count == 2 || keyCombo[2] == Key.D4) param = "4/4";
                        else if (keyCombo[2] == Key.D3) param = "3/4";
                        else if (keyCombo[2] == Key.D6) param = "6/8";
                        else
                        {
                            // Bad combo, abort
                            action = ActionOption.Undefined;
                        }
                        break;
                }
            }
            if (action != ActionOption.Undefined)
            {
                firstCommand.Execute(action, param);
            }
            //else
            //{
            //    ShowErrorDialog("Something went wrong. \nPlease try again.");
            //}
        }

        public ICommand OnWindowClosingCommand => new RelayCommand(() =>
        {
            //TODO check if there are unsaved changes and if so, prompt user
            ViewModelLocator.Cleanup();
        });
        #endregion Focus and key commands, these can be used for implementing hotkeys
    }
}
