using System;
using System.Collections.Generic;
using DPA_Musicsheets.Models.Domain;
using DPA_Musicsheets.Models.Visitor;
using PSAMControlLibrary;
using Clef = DPA_Musicsheets.Models.Domain.Clef;
using Note = DPA_Musicsheets.Models.Domain.Note;
using Rest = DPA_Musicsheets.Models.Domain.Rest;
using TimeSignature = DPA_Musicsheets.Models.Domain.TimeSignature;

namespace DPA_Musicsheets.Interpreters.WpfStaffs
{
    public class WpfStaffInterpreter : IVisitor
    {
        private List<MusicalSymbol> musicSymbols;

        public WpfStaffInterpreter()
        {
            musicSymbols = new List<MusicalSymbol>();
        }
        public List<MusicalSymbol> Convert(Score song)
        {
            musicSymbols.Clear();
            foreach (var staff in song.staffsInScore)
            {
                foreach (var bar in staff.bars)
                {
                    foreach (var note in bar.notes)
                    {
                        note.Accept(this);
                    }
                    bar.Accept(this);
                }
            }
            return musicSymbols;
        }

        public void Visit(Rest rest)
        {
            musicSymbols.Add(new PSAMControlLibrary.Rest((MusicalSymbolDuration) rest.length));
        }

        public void Visit(Note note)
        {
            string not = ((char)note.tone).ToString().ToUpper();
            var notNote = new PSAMControlLibrary.Note(not, (int) note.intonation, note.pitch,
                (PSAMControlLibrary.MusicalSymbolDuration) note.length, NoteStemDirection.Up, NoteTieType.None,
                new List<NoteBeamType> {NoteBeamType.Single});
            if (note.dot) notNote.NumberOfDots = 1;
            musicSymbols.Add(notNote);
        }

        public void Visit(Bar bar)
        {
            musicSymbols.Add(new PSAMControlLibrary.Barline());
        }

        public void Visit(Clef clef)
        {
            var clefType = (PSAMControlLibrary.ClefType)clef.type;
            musicSymbols.Add(new PSAMControlLibrary.Clef(PSAMControlLibrary.ClefType.GClef, (int)clef.type));
        }

        public void Visit(Metronome metronome)
        {
            // not supported apparently
        }

        public void Visit(TimeSignature timeSignature)
        {
            musicSymbols.Add(new PSAMControlLibrary.TimeSignature(TimeSignatureType.Numbers, (uint)timeSignature.beatsPerMeasure, (uint) timeSignature.lengthOfOneBeat));
        }
    }
}
