using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DPA_Musicsheets.Models.Commands;

namespace DPA_Musicsheets.ViewModels
{
    public class ShortcutHandler
    {
        private Dictionary<List<Key>, ActionOption> Keys;

        public ShortcutHandler()
        {
            //Create shortcuts
            Keys = new Dictionary<List<Key>, ActionOption>
            {
                {new List<Key>{ Key.LeftCtrl, Key.S }, ActionOption.SaveAsLilyPond},
                {new List<Key>{ Key.LeftCtrl, Key.S, Key.P }, ActionOption.SaveAsPdf},
                {new List<Key>{ Key.LeftAlt, Key.A, Key.LeftAlt, Key.P }, ActionOption.SaveAsPdf},
                {new List<Key>{ Key.LeftCtrl, Key.O }, ActionOption.OpenFile},
                {new List<Key>{ Key.LeftAlt, Key.C }, ActionOption.AddClefTreble},
                {new List<Key>{ Key.LeftAlt, Key.S }, ActionOption.AddTempo},
                {new List<Key>{ Key.LeftAlt, Key.B }, ActionOption.AddBarLines},
                {new List<Key>{ Key.LeftAlt, Key.T }, ActionOption.AddTime},
                {new List<Key>{ Key.LeftAlt, Key.T, Key.D3 }, ActionOption.AddTime},
                {new List<Key>{ Key.LeftAlt, Key.T, Key.D4 }, ActionOption.AddTime},
                {new List<Key>{ Key.LeftAlt, Key.T, Key.D6 }, ActionOption.AddTime}
            };
        }

        public bool HasShortCut(List<Key> shortcut)
        {
            foreach (List<Key> k in Keys.Keys)
            {
                if (k.SequenceEqual(shortcut))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPartialMatch(List<Key> shortcut)
        {
            foreach (List<Key> k in Keys.Keys)
            {
                bool inList = true;
                for (int i = 0; i < Math.Min(k.Count, shortcut.Count); i++)
                {
                    if (shortcut[i] != k[i])
                    {
                        inList = false;
                    }
                }
                if (inList) return true;
            }
            return false;
        }

        private ActionOption FindShortCutAction(List<Key> shortcut)
        {
            foreach (List<Key> k in Keys.Keys)
            {
                if (k.SequenceEqual(shortcut))
                {
                    return Keys[k];
                }
            }
            return ActionOption.Undefined;
        }

        public void HandleShortCut(List<Key> pressedKeys, Func<string> openPathCallBack, Func<string> savePathCallBack, out ActionOption act, out string param)
        {
            param = null;
            act = FindShortCutAction(pressedKeys);
            //TODO move popup stuff?
            if (act == ActionOption.OpenFile)
            {
                param = openPathCallBack.Invoke();
                if (param == null)
                {
                    act = ActionOption.Undefined;
                }
            }
            else if (act == ActionOption.SaveAsLilyPond || act == ActionOption.SaveAsPdf)
            {
                param = savePathCallBack.Invoke();
                if (param == null)
                {
                    act = ActionOption.Undefined;
                }
            }
            else if (act == ActionOption.AddTime)
            {
                if (pressedKeys.Count == 2 || pressedKeys[2] == Key.D4)
                {
                    param = "4/4";
                }
                else if (pressedKeys[2] == Key.D3)
                {
                    param = "3/4";
                }
                else
                {
                    param = "6/8";
                }
            }
        }
    }
}
