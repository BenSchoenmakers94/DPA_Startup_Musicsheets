using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class SaveAsLilyPondCommand : Command
    {
        public SaveAsLilyPondCommand(MusicLoader musicLoader) : base(musicLoader)
        {
            ActionOption = ActionOption.SaveAsLilyPond;
        }
        public override void Execute(ActionOption actionOption, string parameter = null)
        {
            if (CanExecute(actionOption))
            {
                // parameter is the file path
                MusicLoader.SaveToLilypond(parameter);
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
