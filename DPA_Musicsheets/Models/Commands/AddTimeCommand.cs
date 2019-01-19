using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class AddTimeCommand : Command
    {
        public AddTimeCommand(Command next, MusicLoader musicLoader) : base(next, musicLoader)
        {
            ActionOption = ActionOption.AddTime;
        }
        public override void Execute(ActionOption actionOption, string parameter = null)
        {
            // parameter is the optional additional time
            if (CanExecute(actionOption))
            {
                //TODO move elsewhere, let the command just pass on the param to a function
                if(string.IsNullOrEmpty(parameter)) parameter = "4/4";
                // T = 4/4, T+4 = 4/4, T+3 = 3/4, T+6 = 6/8
                //TODO add time, depending on parameter
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
