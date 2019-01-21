using DPA_Musicsheets.IO;
using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class SaveAsLilyPondCommand : Command
    {
        public SaveAsLilyPondCommand(FileHandleFacade fileHandleFacade) : base(fileHandleFacade)
        {
            ActionOption = ActionOption.SaveAsLilyPond;
        }
        public override void Execute(ActionOption actionOption, string parameter = null, string parameter2 = null)
        {
            if (CanExecute(actionOption))
            {
                // parameter is the file path, parameter2 is content
                FileHandleFacade.SaveFile(parameter, parameter2);
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
