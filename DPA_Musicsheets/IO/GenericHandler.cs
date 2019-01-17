using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DPA_Musicsheets.IO
{
    public abstract class GenericHandler
    {
        public abstract List<string> Extensions { get; }
        public abstract string FileType { get; }
        public abstract string FileTypeString { get; }

        public Staff LoadFile(string fileName)
        {
            if (HandlesFile(fileName))
            {
                return Load(fileName);
            }
            if (Next != null)
            {
                return Next.LoadFile(fileName);
            }
            return null;
        }

        public bool SaveFile(string fileName, Staff staff)
        {
            if (HandlesFile(fileName))
            {
                return Save(fileName, staff);
            }
            if (Next != null)
            {
                return Next.SaveFile(fileName, staff);
            }
            return false;
        }

        protected abstract Staff Load(string filename);
        protected abstract bool Save(string filename, Staff staff);

        public bool HandlesFile(string filename)
        {
            return Extensions.Contains(Path.GetExtension(filename));
        }

        public static string BuildSupportedFileTypeString(string fileType, List<string> extensions)
        {
            var builder = new StringBuilder();
            builder.Append($"{fileType} (");
            foreach (var extension in extensions)
            {
                builder.Append($"*{extension} ");
            }
            builder.Append(")|");
            foreach (var extension in extensions)
            {
                builder.Append($"*{extension}");
                if (extension != extensions[extensions.Count - 1]) builder.Append(";");
            }
            return builder.ToString();
        }

        public List<string> GetSupportedFileTypeStrings()
        {
            var ext = new List<string> { FileTypeString };

            if (Next != null)
            {
                ext.AddRange(Next.GetSupportedFileTypeStrings());
            }
            return ext;
        }

    }
}