using System.Collections.Generic;
using System.Windows.Input;

namespace DPA_Musicsheets.Models.Commands
{
    public class AddTimeCommand : Command
    {
        public AddTimeCommand(Command next) : base(next)
        {
        }
        public override void Execute(List<Key> pressedKeys)
        {
            if (CanExecute(pressedKeys))
            {
                //TODO add time, depending on pressed keys
                // T = 4/4, T+4 = 4/4, T+3 = 3/4, T+6 = 6/8
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
