using System;
using System.Collections.Generic;
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
        }

        public string getSupportedFileTypeString()
        {
            return this.buildSupportedFileTypeString();
        }

        protected override Score load(string fileName)
        {
            var midiSequence = new Sequence();
            midiSequence.Load(fileName);

            return interpreter.ConvertBack(midiSequence);
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
    }
}