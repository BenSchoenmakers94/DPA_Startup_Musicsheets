using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Models.Events
{
    public class OwnEventmanager
    {
        private static OwnEventmanager instance;
        private Dictionary<string, Event<string>> events;

        private OwnEventmanager()
        {
            events = new Dictionary<string, Event<string>>
            {
                {"addLilyPondToken", new Event<string>() }  ,
                {"changeInformativeText", new Event<string>() },
                {"changeEditorState", new Event<string>() },
                {"changePlayerState", new Event<string>() },
                {"onClose", new Event<string>() }
            };
        }

        public void DispatchEvent(string name, string argument)
        {
            events[name].Dispatch(argument);
        }

        public void Subscribe(string name, Action<string> callback)
        {
            events[name].Subscribe(callback);
        }

        public static OwnEventmanager Manager
        {
            get
            {
                if (instance == null)
                {
                    instance = new OwnEventmanager();
                }
                return instance;
            }
        }
    }
}
