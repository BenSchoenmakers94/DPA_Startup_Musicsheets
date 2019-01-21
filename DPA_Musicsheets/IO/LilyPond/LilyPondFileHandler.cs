using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DPA_Musicsheets.Interpreters.LilyPond;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.IO.LilyPond
{
    class LilyPondFileHandler : GenericHandler
    {
        private LilyPondInterpreter interpreter;

        public LilyPondFileHandler(LilyPondInterpreter interpreter)
        {
            this.interpreter = interpreter;
            this.fileType = "LilyPond";
            this.possibleExtensions = new List<string> { ".ly" };
            CanSave = true;
            CanLoad = true;
        }

        public static List<string> GetExtensions()
        {
            return new List<string> { ".ly" };
        }

        public string GetSupportedFileTypeString()
        {
            return this.buildSupportedFileTypeString();
        }

        protected override Score load(string filename)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var line in File.ReadAllLines(filename))
            {
                sb.AppendLine(line);
            }

            LilypondText = sb.ToString();
            return interpreter.ConvertBack(sb.ToString());
        }

        protected override bool save(string filename, Score staff)
        {
            var lily = interpreter.Convert(staff);
            if (lily == null) return false;

            try
            {
                System.IO.File.WriteAllText(@filename, lily);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public override void saveToFile(string fileLocation, string text)
        {
            using (StreamWriter outputFile = new StreamWriter(fileLocation))
            {
                outputFile.Write(text);
                outputFile.Close();
            }
        }
    }
}