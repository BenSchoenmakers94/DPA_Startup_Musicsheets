using System.Collections.Generic;
using System.IO;
using System.Text;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.IO
{
    public abstract class GenericHandler
    {
        public List<string> possibleExtensions { get; protected set; }
        public string fileType { get; protected set; }
        public bool CanSave { get; set; }
        public bool CanLoad { get; set; }
        public Score loadFile(string fileName)
        {
            return canHandle(fileName) ? load(fileName) : null;
        }

        public bool saveFile(string fileName, Score staff)
        {
            return canHandle(fileName) && save(fileName, staff);
        }

        protected abstract Score load(string filename);
        protected abstract bool save(string filename, Score staff);

        public abstract void saveToFile(string fileLocation, string text);

        public bool canHandle(string filename)
        {
            return possibleExtensions.Contains(Path.GetExtension(filename));
        }

        protected string buildSupportedFileTypeString()
        {
            var sb = new StringBuilder();
            sb.Append($"{fileType} (");
            foreach (var extension in possibleExtensions)
            {
                sb.Append($"*{extension} ");
            }

            sb.Append(")|");
            foreach (var extension in possibleExtensions)
            {
                sb.Append($"*{extension}");
                if (extension != possibleExtensions[possibleExtensions.Count - 1]) sb.Append(";");
            }

            return sb.ToString();
        }

        public string LilypondText { get; set; }
    }
}