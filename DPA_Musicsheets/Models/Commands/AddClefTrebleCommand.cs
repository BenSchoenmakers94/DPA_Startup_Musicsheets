using System.Collections.Generic;
using System.Windows.Input;
using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class AddClefTrebleCommand : Command
    {
        public AddClefTrebleCommand(Command next, MusicLoader musicLoader) : base(next, musicLoader)
        {
            ActionOption = ActionOption.AddClefTreble;
        }
        public override void Execute(ActionOption actionOption, string parameter = null)
        {
            //parameter will be empty
            if (CanExecute(actionOption))
            {
                //TODO add clef treble
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
