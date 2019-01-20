using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.Creation.LilyPond;
using DPA_Musicsheets.Interpreters.LilyPond;
using DPA_Musicsheets.Interpreters.Midi;
using DPA_Musicsheets.IO.LilyPond;
using DPA_Musicsheets.IO.Midi;
using DPA_Musicsheets.IO.Pdf;

namespace DPA_Musicsheets.IO
{
    public class FileChoiceHandler
    {
        private readonly List<GenericHandler> handlers;

        public FileChoiceHandler()
        {
            var lilyPondFileHandler = new LilyPondFileHandler(new LilyPondInterpreter(new LilyPondNoteFactory()));
            var midiFileHandler = new MidiFileHandler(new MidiInterpreter());
            var pdfFileHandler = new PdfFileHandler();
            handlers = new List<GenericHandler>
            {
                lilyPondFileHandler,
                midiFileHandler,
                pdfFileHandler
            };
        }

        public string GetSupportedSaveTypes()
        {
            List<GenericHandler> canSaves = handlers.Where(h => h.CanSave).ToList();
            string list = canSaves.Aggregate("", (current1, save) => save.possibleExtensions.Aggregate(current1, (current, ext) => current + save.fileType + "|*" + ext + "|"));
            list = list.TrimEnd('|');
            return list;
        }

        public string GetSupportedLoadTypes()
        {
            List<GenericHandler> canSaves = handlers.Where(h => h.CanLoad).ToList();
            string names = canSaves.Aggregate("", (current, handler) => current + handler.fileType + " or ");
            names = names.TrimEnd(' ', 'r', 'o');
            names += " files";

            string list = canSaves.Aggregate("(", (current1, save) => save.possibleExtensions.Aggregate(current1, (current, ext) => current + "*" + ext + " "));
            list = list.TrimEnd(' ');
            list += ")|";

            string list2 = canSaves.Aggregate("(", (current1, save) => save.possibleExtensions.Aggregate(current1, (current, ext) => current + "*" + ext + ";"));
            list2 = list2.TrimEnd(';');
            return names + list + list2;
        }

        public bool IsValidFile(string filePath)
        {
            return handlers.Any(h => h.canHandle(filePath));
        }

        public void SaveFile(string path, string content)
        {
            foreach (GenericHandler handler in handlers)
            {
                if (handler.canHandle(path))
                {
                    handler.saveToFile(path, content);
                    return;
                }
            }
        }
    }
}
