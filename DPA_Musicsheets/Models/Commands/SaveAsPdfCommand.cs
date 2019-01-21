using DPA_Musicsheets.IO;
using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class SaveAsPdfCommand : Command
    {
        public SaveAsPdfCommand(FileHandleFacade fileHandleFacade) : base(fileHandleFacade)
        {
            ActionOption = ActionOption.SaveAsPdf;
        }

        public override void Execute(ActionOption actionOption, string parameter = null, string parameter2 = null)
        {
            if (CanExecute(actionOption))
            {
                // parameter is the file path
                FileHandleFacade.SaveFile(parameter, parameter2);
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
