using System.Collections.Generic;
using System.Windows.Input;

namespace DPA_Musicsheets.Models.Commands
{
    public class SaveAsPdfCommand : Command
    {
        public SaveAsPdfCommand(Command next) : base(next)
        {}

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
    }
}
