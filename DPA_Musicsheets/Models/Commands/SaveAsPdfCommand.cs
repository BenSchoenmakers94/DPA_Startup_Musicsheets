﻿using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class SaveAsPdfCommand : Command
    {
        public SaveAsPdfCommand(MusicLoader musicLoader) : base(musicLoader)
        {
            ActionOption = ActionOption.SaveAsPdf;
        }

        public override void Execute(ActionOption actionOption, string parameter = null)
        {
            if (CanExecute(actionOption))
            {
                // parameter is the file path
                MusicLoader.SaveToPDF(parameter);
            }
            else
            {
                Next.Execute(actionOption, parameter);
            }
        }
    }
}
