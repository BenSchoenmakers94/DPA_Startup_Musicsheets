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
        public override void Execute(ActionOption actionOption, string parameter = null)
        {
            if (CanExecute(actionOption))
            {
                // parameter is the file path
                //TODO fix this so it has the content
                //FileHandleFacade.SaveFile(parameter);
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
