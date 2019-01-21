using System;
using System.Collections.Generic;
using DPA_Musicsheets.Creation.LilyPond;
using DPA_Musicsheets.Interpreters.LilyPond;
using DPA_Musicsheets.Interpreters.Midi;
using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.IO.Midi
{
    public class MidiFileHandler : GenericHandler
    {
        private MidiInterpreter interpreter;

        public MidiFileHandler(MidiInterpreter interpreter)
        {
            this.interpreter = interpreter;
            this.fileType = "Midi";
            this.possibleExtensions = new List<string> { ".midi", ".mid" };
            CanSave = true;
            CanLoad = true;
        }

        public string getSupportedFileTypeString()
        {
            return this.buildSupportedFileTypeString();
        }

        protected override Score load(string fileName)
        {
            var midiSequence = new Sequence();
            midiSequence.Load(fileName);

            Score res = interpreter.ConvertBack(midiSequence);

            LilypondText = new LilyPondInterpreter(new LilyPondNoteFactory()).Convert(res);
            return res;
        }

        protected override bool save(string fileName, Score staff)
        {
            var sequence = interpreter.Convert(staff);
            if (sequence == null) return false;
            try
            {
                sequence.Save(fileName);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public override void saveToFile(string fileLocation, string text)
        {
            throw new NotImplementedException();
        }
    }
}