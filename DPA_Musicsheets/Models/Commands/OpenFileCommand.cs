using System.Collections.Generic;
using System.Windows.Input;
using DPA_Musicsheets.IO;
using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class OpenFileCommand : Command
    {
        public OpenFileCommand(FileHandleFacade fileHandleFacade) : base(fileHandleFacade)
        {
            ActionOption = ActionOption.OpenFile;
        }

        public override void Execute(ActionOption actionOption, string parameter = null)
        {
            if (CanExecute(actionOption))
            {
                //Parameter is the path to the file
                FileHandleFacade.Load(parameter);
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
