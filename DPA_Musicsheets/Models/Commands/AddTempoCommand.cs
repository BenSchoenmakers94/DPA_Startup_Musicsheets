using System.Collections.Generic;
using System.Windows.Input;

namespace DPA_Musicsheets.Models.Commands
{
    public class AddTempoCommand : Command
    {
        public AddTempoCommand(Command next) : base(next)
        {
        }
        public override void Execute(List<Key> pressedKeys)
        {
            if (CanExecute(pressedKeys))
            {
                //TODO add tempo (speed) 4=120
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
    }
}
