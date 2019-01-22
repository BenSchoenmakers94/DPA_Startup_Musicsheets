using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.Creation.LilyPond;
using DPA_Musicsheets.Interpreters.LilyPond;
using DPA_Musicsheets.Interpreters.Midi;
using DPA_Musicsheets.Interpreters.WpfStaffs;
using DPA_Musicsheets.IO.LilyPond;
using DPA_Musicsheets.IO.Midi;
using DPA_Musicsheets.IO.Pdf;
using DPA_Musicsheets.Models.Domain;
using DPA_Musicsheets.Models.Events;
using PSAMControlLibrary;

namespace DPA_Musicsheets.IO
{
    public class FileHandleFacade
    {
        private readonly List<GenericHandler> handlers;

        public FileHandleFacade()
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
            OwnEventmanager.Manager.Subscribe("reRender", RegenerateBasedOnEditor);
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
            names += " files ";
            //OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };

            string list = canSaves.Aggregate("(", (current1, save) => save.possibleExtensions.Aggregate(current1, (current, ext) => current + "*" + ext + " "));
            list = list.TrimEnd(' ');
            list += ")|";

            string list2 = canSaves.Aggregate("", (current1, save) => save.possibleExtensions.Aggregate(current1, (current, ext) => current + "*" + ext + ";"));
            list2 = list2.TrimEnd(';');
            string res = names + list + list2;
            return res;
        }

        public bool IsValidFile(string filePath)
        {
            return handlers.Any(h => h.canHandle(filePath));
        }

        public void Load(string fileName)
        {
            var handler = handlers.First(h => h.canHandle(fileName));
            Score s = handler.loadFile(fileName);
            OwnEventmanager.Manager.DispatchEvent("setStaffs", s);
            OwnEventmanager.Manager.DispatchEvent("setLilyPondText", handler.LilypondText);
            var se = handlers.First(h => h.CanGenerateSequence).GenerateSequence(s);
            OwnEventmanager.Manager.DispatchEvent("setSequence", se);
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

        public List<MusicalSymbol> GetWpfMusicalSymbols(Score song)
        {
            return new WpfStaffInterpreter().Convert(song);
        }

        private void RegenerateBasedOnEditor(object obj)
        {
            string lilypond = (string) obj;
            GenericHandler handler = handlers.First(h => h.fileType == "LilyPond");
            var score = new LilyPondInterpreter(new LilyPondNoteFactory()).ConvertBack(lilypond);

            OwnEventmanager.Manager.DispatchEvent("setStaffs", score);

            var se = handlers.First(h => h.CanGenerateSequence).GenerateSequence(score);
            OwnEventmanager.Manager.DispatchEvent("setSequence", se);
        }
    }
}
