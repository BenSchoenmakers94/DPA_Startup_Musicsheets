using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Creation.LilyPond
{
    public class LilyPondNoteFactory : AbstractNoteFactory<string>
    {
        public int Octave { get; set; }

        public LilyPondNoteFactory()
        {
            Octave = -1;
        }

        public override Note create(string noteSpecifier)
        {
            bool connected = noteSpecifier.Contains("~");
            var intonation = getIntonation(noteSpecifier);
            Length length = calculateDuration(noteSpecifier);

            int higherPitch =  noteSpecifier.Count(f => f == '\'');
            int lowerPitch = noteSpecifier.Count(f => f == ',');

            Tones tone = getTone(noteSpecifier);
            Octave += higherPitch;
            Octave -= lowerPitch;
            var totalPitch = 4 + Octave; 
            return new Note(tone, totalPitch, intonation) {dot = noteSpecifier.EndsWith("."), length = length, connected = connected};
        }

        protected override Tones getTone(string toneSpecifier)
        {
            var tone = (Tones) toneSpecifier.First();
            return tone != null ? tone : Tones.NO_TONE;
        }

        protected override Intonation getIntonation(string intonationSpecifier)
        {
            if (intonationSpecifier.Contains("fi "))
                return Intonation.Flat;
            else if (intonationSpecifier.Contains("gi"))
                return Intonation.Sharp;

            return Intonation.None;
        }

        private Length calculateDuration(string durationSpecifier)
        {
            var number = Regex.Match(durationSpecifier, @"\d+").Value;

            return (Length) Int32.Parse(number);
        }
    }
}