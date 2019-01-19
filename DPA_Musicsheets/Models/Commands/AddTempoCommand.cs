using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class AddTempoCommand : Command
    {
        public AddTempoCommand(Command next, MusicLoader musicLoader) : base(next, musicLoader)
        {
            ActionOption = ActionOption.AddTempo;
        }
        public override void Execute(ActionOption actionOption, string parameter = null)
        {
            // paramter will be empty
            if (CanExecute(actionOption))
            {
                //TODO add tempo (speed) 4=120
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
