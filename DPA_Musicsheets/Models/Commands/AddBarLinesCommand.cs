using System.Collections.Generic;
using System.Windows.Input;
using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class AddBarLinesCommand : Command
    {
        public AddBarLinesCommand(Command next, MusicLoader musicLoader) : base(next, musicLoader)
        {
            ActionOption = ActionOption.AddBarLines;
        }
        public override void Execute(ActionOption actionOption, string parameter = null)
        {
            // parameter will be empty
            if (CanExecute(actionOption))
            {
                //TODO add missing bar lines
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
