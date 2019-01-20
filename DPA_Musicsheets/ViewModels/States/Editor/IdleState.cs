using DPA_Musicsheets.Models.Events;

namespace DPA_Musicsheets.ViewModels.States.Editor
{
    public class IdleState : EditorState
    {
        public IdleState()
        {
            showText = "";
        }

        public override void GoInto(LilypondViewModel owner)
        {
            owner.CanEdit = true;
            OwnEventmanager.Manager.DispatchEvent("changeInformativeText", showText);
        }
    }
}
