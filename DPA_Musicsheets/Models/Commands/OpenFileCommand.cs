using System;
using DPA_Musicsheets.IO;
using DPA_Musicsheets.Models.Events;

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
                string path;
                if (parameter == null)
                {
                    path = openPathCallBack.Invoke(null);
                }
                else
                {
                    path = parameter;
                }
                OwnEventmanager.Manager.DispatchEvent("changeFilePath", path);
                FileHandleFacade.Load(path);
            }
            else
            {
                Next.Execute(actionOption, openPathCallBack, savePathCallBack, parameter, parameter2);
            }
        }
    }
}
