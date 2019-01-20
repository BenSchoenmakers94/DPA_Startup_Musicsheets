﻿namespace DPA_Musicsheets.ViewModels.States.Editor
{
    public abstract class EditorState
    {
        protected string showText;

        public virtual void GoInto(LilypondViewModel owner)
        {
        }
    }
}
