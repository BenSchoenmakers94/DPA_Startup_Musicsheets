using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Creation.LilyPond
{
    public class LilyPondNoteFactory : AbstractNoteFactory<string>
    {
        private Dictionary<Tones, int> octaveKeeper_;

        public LilyPondNoteFactory()
        {
            this.octaveKeeper_ = new Dictionary<Tones, int>
            {
                { Tones.A, 0 },
                { Tones.B, 0 },
                { Tones.C, 0 },
                { Tones.D, 0 },
                { Tones.E, 0 },
                { Tones.F, 0 },
                { Tones.G, 0 },
            };
        }

        public override Note create(string noteSpecifier)
        {
            bool connected = noteSpecifier.Contains("~");
            var tone = getTone(noteSpecifier);
            var intonation = getIntonation(noteSpecifier);
            Length length = calculateDuration(noteSpecifier);

            int higherPitch =  noteSpecifier.Count(f => f == '\'');
            int lowerPitch = noteSpecifier.Count(f => f == ',');
            octaveKeeper_[tone] += higherPitch + lowerPitch;
            var totalPitch = 4 + octaveKeeper_[tone];
           
            return new Note(tone, totalPitch, intonation) {dot = noteSpecifier.EndsWith("."), length = length, connected = connected};
        }

        protected override Tones getTone(string toneSpecifier)
        {
            var tone = (Tones) toneSpecifier.First();
            return tone != null ? tone : Tones.NO_TONE;
        }

        protected override Intonation getIntonation(string intonationSpecifier)
        {
            if (intonationSpecifier.Contains("fi"))
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