using DPA_Musicsheets.Models.Events;

namespace DPA_Musicsheets.ViewModels.States.Editor
{
    public class BlockedState : IEditorState
    {
        public BlockedState()
        {
            showText = "Playing...";
        }
        public override void GoInto(LilypondViewModel owner)
        {
            owner.CanEdit = false;
            OwnEventmanager.Manager.DispatchEvent("changeInformativeText", showText);
        }
    }
}
