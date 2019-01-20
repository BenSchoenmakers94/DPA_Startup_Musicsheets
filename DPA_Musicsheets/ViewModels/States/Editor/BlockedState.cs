using DPA_Musicsheets.Models.Events;

namespace DPA_Musicsheets.ViewModels.States.Editor
{
    public class BlockedState : EditorState
    {
        public override void GoInto(LilypondViewModel owner)
        {
            owner.CanEdit = false;
        }
    }
}
