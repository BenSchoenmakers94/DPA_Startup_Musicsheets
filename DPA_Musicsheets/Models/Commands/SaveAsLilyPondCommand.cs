using System.Collections.Generic;
using System.Windows.Input;

namespace DPA_Musicsheets.Models.Commands
{
    public class SaveAsLilyPondCommand : Command
    {
        public override void Execute(List<Key> pressedKeys)
        {
            if (CanExecute(pressedKeys))
            {
                //TODO save
            }
            else
            {
                Next.Execute(pressedKeys);
            }
        }

        public override bool CanExecute(List<Key> pressedKeys)
        {
            //TODO check if can execute
            return false;
        }

        public SaveAsLilyPondCommand(Command next) : base(next)
        {}
    }
}
