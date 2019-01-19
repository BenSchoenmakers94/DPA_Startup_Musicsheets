using System;
using System.Collections.Generic;
using System.Windows.Input;
using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class Command
    {
        public virtual void Execute(ActionOption actionOption, string parameter = null) {}
        protected bool CanExecute(ActionOption actionOption) => actionOption == ActionOption;
        public Command Next { get; set; }
        protected ActionOption ActionOption;
        protected readonly MusicLoader MusicLoader;
        public Command(Command next, MusicLoader musicLoader)
        {
            Next = next;
            MusicLoader = musicLoader;
        }
    }
}
