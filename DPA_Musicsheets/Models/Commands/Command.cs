using System.Collections.Generic;
using System.Windows.Input;

namespace DPA_Musicsheets.Models.Commands
{
    public abstract class Command
    {
        public virtual void Execute(List<Key> pressedKeys) => Next.Execute(pressedKeys);
        public virtual bool CanExecute(List<Key> pressedKeys) => false;
        protected Command Next { get; set; }

        protected Command(Command next) => Next = next;
    }
}
