using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Models.Events
{
    public class Event<T>
    {
        private readonly List<Action<string>> actions;

        public Event(List<Action<string>> actions)
        {
            this.actions = actions;
        }

        public Event()
        {
            actions = new List<Action<string>>();
        }

        public void Dispatch(string arg)
        {
            actions.ForEach(a => a.Invoke(arg));
        }

        public void Subscribe(Action<string> newAction)
        {
            actions.Add(newAction);
        }
    }
}
