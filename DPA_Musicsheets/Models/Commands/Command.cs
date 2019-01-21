using DPA_Musicsheets.IO;

namespace DPA_Musicsheets.Models.Commands
{
    public class Command
    {
        public virtual void Execute(ActionOption actionOption, string parameter = null) {}
        protected bool CanExecute(ActionOption actionOption) => actionOption == ActionOption;
        public Command Next { get; set; }
        protected ActionOption ActionOption;
        protected readonly FileHandleFacade FileHandleFacade;
        public Command(FileHandleFacade fileHandleFacade)
        {
            FileHandleFacade = fileHandleFacade;
        }
    }
}
