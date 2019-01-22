using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.IO.Pdf
{
    public class PdfFileHandler : GenericHandler
    {
        public PdfFileHandler()
        {
            CanSave = true;
            CanLoad = false;
            this.fileType = "Pdf";
            this.possibleExtensions = new List<string> { ".pdf"};
            CanGenerateSequence = false;
        }
        protected override Score load(string filename)
        {
            throw new NotImplementedException();
        }

        protected override bool save(string fileName, Score staff)
        {
            throw new NotImplementedException();
        }

        public override void saveToFile(string fileLocation, string text)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(fileLocation);
            string tmpFileName = $"{fileLocation}-tmp.ly";

            using (StreamWriter outputFile = new StreamWriter(fileLocation))
            {
                outputFile.Write(text);
                outputFile.Close();
            }

            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Path.GetDirectoryName(tmpFileName);
            string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
            string targetFolder = Path.GetDirectoryName(fileLocation);
            string targetFileName = Path.GetFileNameWithoutExtension(fileLocation);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = string.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited)
            { /* Wait for exit */
            }
            if (sourceFolder != targetFolder || sourceFileName != targetFileName)
            {
                File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                File.Delete(tmpFileName);
            }
        }
    }
}
