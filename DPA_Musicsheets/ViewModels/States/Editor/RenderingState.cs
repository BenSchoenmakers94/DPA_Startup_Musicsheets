using System;
using System.Threading.Tasks;
using DPA_Musicsheets.Models.Events;

namespace DPA_Musicsheets.ViewModels.States.Editor
{
    public class RenderingState : EditorState
    {
        private DateTime lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        public RenderingState()
        {
            showText = "Rendering...";
        }
        public override void GoInto(LilypondViewModel owner)
        {
            lastChange = DateTime.Now;
            OwnEventmanager.Manager.DispatchEvent("changeInformativeText", showText);
            Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith(task =>
            {
                if ((DateTime.Now - lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                {
                    owner.UndoCommand.RaiseCanExecuteChanged();
                    OwnEventmanager.Manager.DispatchEvent("changeEditorState", "Idle");
                }
            }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
        }
    }
}
