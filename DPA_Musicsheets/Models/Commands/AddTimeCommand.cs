using DPA_Musicsheets.IO;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models.Events;

namespace DPA_Musicsheets.Models.Commands
{
    public class AddTimeCommand : Command
    {
        public AddTimeCommand(FileHandleFacade fileHandleFacade) : base(fileHandleFacade)
        {
            ActionOption = ActionOption.AddTime;
        }
        public override void Execute(ActionOption actionOption, string parameter = null)
        {
            // parameter is the optional additional time
            if (CanExecute(actionOption))
            {
                OwnEventmanager.Manager.DispatchEvent("addLilyPondToken", "\\time " + parameter);
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
