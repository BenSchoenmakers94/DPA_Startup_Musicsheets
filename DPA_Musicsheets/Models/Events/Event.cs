using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Models.Events
{
    public class Event<T>
    {
        private readonly List<Action<T>> actions;

        public Event(List<Action<T>> actions)
        {
            this.actions = actions;
        }

        public Event()
        {
            actions = new List<Action<T>>();
        }

        public void Dispatch(T arg)
        {
            actions.ForEach(a => a.Invoke(arg));
        }

        public void Subscribe(Action<T> newAction)
        {
            actions.Add(newAction);
        }
    }
}
