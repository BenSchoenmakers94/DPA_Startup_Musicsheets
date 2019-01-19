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
        private readonly ShortcutHandler shortcutHandler;
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
            shortcutHandler = new ShortcutHandler();
            firstCommand = cb.BuildCommands(musicLoader);
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            //TODO fix "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly"
            FileName = OpenOpenFileDialog();
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
            Key key = e.Key == Key.System ? e.SystemKey : e.Key;
            if (!downKeyQueue.Any() || downKeyQueue[downKeyQueue.Count - 1] != key)
            {
                downKeyQueue.Add(key);
                if (!shortcutHandler.IsPartialMatch(upKeyQueue))
                {
                    // Empty queue if the current items don't match up.
                    downKeyQueue.Clear();
                }
                else
                {
                    // This prevents windows from beeping at you when you use ALT + combo.
                    // Only set to true if it's not a match, otherwise normal keyboard input will be considered handled when it's not.
                    e.Handled = true;
                }
            }
            else
            {
                Console.WriteLine($@"Key down: {e.Key}");
            }
        });

        public ICommand OnKeyUpCommand => new RelayCommand<KeyEventArgs>(e =>
       {
           // Get key or system key (in case of using ALT)
           Key key = e.Key == Key.System ? e.SystemKey : e.Key;
           upKeyQueue.Add(key);
           if (downKeyQueue.Contains(key))
           {
               downKeyQueue.Remove(key);
           }
           if (!downKeyQueue.Any() && upKeyQueue.Count > 1)
           {
               //TODO fix random reversal when using alt

               if (upKeyQueue[0] != Key.LeftAlt && upKeyQueue[0] != Key.RightAlt)
               {
                   upKeyQueue.Reverse();
               }
               if (upKeyQueue[0] == Key.LeftAlt && upKeyQueue[upKeyQueue.Count - 1] == Key.LeftAlt)
               {
                   int magic = upKeyQueue.Count - 2;
                   var keep = upKeyQueue[magic];
                   upKeyQueue[magic] = upKeyQueue[magic + 1];
                   upKeyQueue[magic + 1] = keep;
               }
               if (shortcutHandler.HasShortCut(upKeyQueue))
               {
                   HandleCommand(upKeyQueue);
                   upKeyQueue.Clear();
               }
               if (!shortcutHandler.IsPartialMatch(upKeyQueue))
               {
                   upKeyQueue.Clear();
               }
           }
           Console.WriteLine($@"Key Up: {key}");
       });

        private string OpenOpenFileDialog()
        {
            //TODO central place to store available types
            string filter = "";
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = filter };
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        private string OpenSaveFileDialog()
        {
            //TODO central place to store available types
            string filter = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = filter };
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }

        private void ShowErrorDialog(string error)
        {
            MessageBox.Show(error, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void HandleCommand(List<Key> keyCombo)
        {
            ActionOption action;
            string param;
            shortcutHandler.HandleShortCut(keyCombo, OpenOpenFileDialog, OpenSaveFileDialog, out action, out param);
            if (action != ActionOption.Undefined)
            {
                firstCommand.Execute(action, param);
            }
        }

        public ICommand OnWindowClosingCommand => new RelayCommand(() =>
        {
            //TODO check if there are unsaved changes and if so, prompt user
            ViewModelLocator.Cleanup();
        });
        #endregion Focus and key commands, these can be used for implementing hotkeys
    }
}
