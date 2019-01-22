using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DPA_Musicsheets.IO;
using DPA_Musicsheets.Models.Commands;
using DPA_Musicsheets.Models.Events;
using DPA_Musicsheets.ViewModels.Converters;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly List<Key> downKeyQueue;
        private readonly List<Key> upKeyQueue;
        private string _fileName;
        private readonly Command firstCommand;
        private readonly ShortcutHandler shortcutHandler;
        private readonly FileHandleFacade _fileHandleFacade;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        private string lilyPondText;        

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

        public MainViewModel(FileHandleFacade fileHandleFacade)
        {
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";
            downKeyQueue = new List<Key>();
            upKeyQueue = new List<Key>();
            CommandBuilder cb = new CommandBuilder();
            shortcutHandler = new ShortcutHandler();
            firstCommand = cb.BuildCommands(fileHandleFacade);
            OwnEventmanager.Manager.Subscribe("changeInformativeText", ChangeInformativeMessage);
            OwnEventmanager.Manager.Subscribe("changedLilyPond", SetLilyPondText);
            _fileHandleFacade = new FileHandleFacade();
        }

        private void SetLilyPondText(object obj)
        {
            lilyPondText = (string) obj;
        }

        private void ChangeInformativeMessage(object obj)
        {
            CurrentState = (string)obj;
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            FileName = OpenOpenFileDialog(null);
        });

        public ICommand LoadCommand => new RelayCommand(() =>
        {
            firstCommand.Execute(ActionOption.OpenFile, OpenOpenFileDialog, OpenSaveFileDialog, FileName);
        });

        public ICommand HandleButtonCommand => new RelayCommand<string>(input =>
        {
            ActionOption command;
            command = (ActionOption) new ActionOptionIntConverter().Convert(int.Parse(input), null, null, null);
            firstCommand.Execute(command, OpenOpenFileDialog, OpenSaveFileDialog, null, lilyPondText);
        });


        #region Focus and key commands, these can be used for implementing hotkeys

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>(e =>
        {
            Key key = e.Key == Key.System ? e.SystemKey : e.Key;
            if (!downKeyQueue.Any() || downKeyQueue[downKeyQueue.Count - 1] != key)
            {
                downKeyQueue.Add(key);
                if (!shortcutHandler.IsPartialMatch(downKeyQueue) && !shortcutHandler.IsPartialMatch(upKeyQueue))
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
        });

        public ICommand OnKeyUpCommand => new RelayCommand<KeyEventArgs>(e =>
       {
           // Get key or system key (in case of using ALT)
           Key key = e.Key == Key.System ? e.SystemKey : e.Key;
           if (!upKeyQueue.Any() || upKeyQueue[upKeyQueue.Count - 1] != key)
           {
               upKeyQueue.Add(key);
           }
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

        private string OpenOpenFileDialog(string filter)
        {
            if (filter == null)
            {
                filter = _fileHandleFacade.GetSupportedLoadTypes(); 

            }
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = filter };
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        private string OpenSaveFileDialog(string filter)
        {
            if (filter == null)
            {
                filter = _fileHandleFacade.GetSupportedSaveTypes();

            }
            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = filter };
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }

        private void HandleCommand(List<Key> keyCombo)
        {
            ActionOption action;
            string param;
            shortcutHandler.HandleShortCut(keyCombo, out action, out param);
            if (action != ActionOption.Undefined)
            {
                firstCommand.Execute(action, OpenOpenFileDialog, OpenSaveFileDialog, param, lilyPondText);
            }
        }

        public ICommand OnWindowClosingCommand => new RelayCommand(() =>
        {
            OwnEventmanager.Manager.DispatchEvent("onClose", null);
            ViewModelLocator.Cleanup();
        });
        #endregion Focus and key commands, these can be used for implementing hotkeys
    }
}
