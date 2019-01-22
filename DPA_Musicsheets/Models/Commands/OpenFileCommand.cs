using System;
using DPA_Musicsheets.IO;

namespace DPA_Musicsheets.Models.Commands
{
    public class OpenFileCommand : Command
    {
        public OpenFileCommand(FileHandleFacade fileHandleFacade) : base(fileHandleFacade)
        {
            ActionOption = ActionOption.OpenFile;
        }

        public override void Execute(ActionOption actionOption, Func<string, string> openPathCallBack, Func<string, string> savePathCallBack, string parameter = null, string parameter2 = null)
        {
            if (CanExecute(actionOption))
            {
                FileHandleFacade.Load(parameter ?? openPathCallBack.Invoke(null));
            }
            else
            {
                Next.Execute(actionOption, openPathCallBack, savePathCallBack, parameter, parameter2);
            }
        }
    }
}
