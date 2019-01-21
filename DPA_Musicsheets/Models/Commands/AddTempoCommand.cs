using DPA_Musicsheets.IO;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models.Events;

namespace DPA_Musicsheets.Models.Commands
{
    public class AddTempoCommand : Command
    {
        public AddTempoCommand(FileHandleFacade fileHandleFacade) : base(fileHandleFacade)
        {
            ActionOption = ActionOption.AddTempo;
        }
        public override void Execute(ActionOption actionOption, string parameter = null, string parameter2 = null)
        {
            // parameter will be empty
            if (CanExecute(actionOption))
            {
                OwnEventmanager.Manager.DispatchEvent("addLilyPondToken", "\\tempo4=120");
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
