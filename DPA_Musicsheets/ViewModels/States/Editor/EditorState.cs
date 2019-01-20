namespace DPA_Musicsheets.ViewModels.States.Editor
{
    public abstract class IEditorState
    {
        protected string showText;

        public virtual void GoInto(LilypondViewModel owner)
        {
        }
    }
}
