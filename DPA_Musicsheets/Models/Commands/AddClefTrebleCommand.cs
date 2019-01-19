﻿using System.Collections.Generic;
using System.Windows.Input;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models.Events;

namespace DPA_Musicsheets.Models.Commands
{
    public class AddClefTrebleCommand : Command
    {
        public AddClefTrebleCommand(MusicLoader musicLoader) : base(musicLoader)
        {
            ActionOption = ActionOption.AddClefTreble;
        }
        public override void Execute(ActionOption actionOption, string parameter = null)
        {
            //parameter will be empty
            if (CanExecute(actionOption))
            {
                OwnEventmanager.Manager.DispatchEvent("addLilyPondToken", "\\clef treble");
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
