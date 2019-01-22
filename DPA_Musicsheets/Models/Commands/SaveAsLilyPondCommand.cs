using System;
using DPA_Musicsheets.IO;
using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class SaveAsLilyPondCommand : Command
    {
        public SaveAsLilyPondCommand(FileHandleFacade fileHandleFacade) : base(fileHandleFacade)
        {
            ActionOption = ActionOption.SaveAsLilyPond;
        }
        public override void Execute(ActionOption actionOption, Func<string, string> openPathCallBack, Func<string, string> savePathCallBack, string parameter = null, string parameter2 = null)
        {
            if (CanExecute(actionOption))
            {
                FileHandleFacade.SaveFile(savePathCallBack.Invoke("Lilypond|*.ly"), parameter2);
            }
            else
            {
                Next.Execute(actionOption, openPathCallBack, savePathCallBack, parameter, parameter2);
            }
        }
    }
}
