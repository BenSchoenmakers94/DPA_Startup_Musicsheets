using System;
using System.Linq;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Creation.LilyPond
{
    public class LilyPondSymbolFactory : AbstractSymbolFactory<string>
    {
        public override Symbol create(string noteSpecifier)
        {
            int higherPitch =  noteSpecifier.Count(f => f == '\'');
            int lowerPitch = noteSpecifier.Count(f => f == ',');
            var totalPitch = 4 + higherPitch + lowerPitch;
            var tone = getTone(noteSpecifier);
            var intonation = getIntonation(noteSpecifier);
            Length length = calculateDuration(noteSpecifier);
            return new Note(tone, totalPitch, intonation) {dot = noteSpecifier.EndsWith("."), length = length};
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