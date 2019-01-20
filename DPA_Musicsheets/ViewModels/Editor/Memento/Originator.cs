using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.ViewModels.Editor.Memento
{
    public class Originator
    {
        public string State { get; set; }

        public Memento Save()
        {
            return new Memento(State);
        }

        public void Restore(Memento memento)
        {
            State = memento.State;
        }
    }
}
