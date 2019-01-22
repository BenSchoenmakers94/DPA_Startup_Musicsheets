using System.Collections.Generic;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Creation.Midi
{
    public class MidiNoteFactory : AbstractNoteFactory<int>
    {
        public override Note create(int noteSpecifier)
        {
            var tone = getTone(noteSpecifier);
            var intonation = getIntonation(noteSpecifier);
            var pitch = noteSpecifier / 12 - 1;
            return new Note(tone, pitch, intonation);
        }

        protected override Tones getTone(int toneSpecifier)
        {
            switch (toneSpecifier % 12)
            {
                case 0:
                    return Tones.C;
                case 1:
                    return Tones.C;
                case 2:
                    return Tones.D;
                case 3:
                    return Tones.D;
                case 4:
                    return Tones.E;
                case 5:
                    return Tones.F;
                case 6:
                    return Tones.F;
                case 7:
                    return Tones.G;
                case 8:
                    return Tones.G;
                case 9:
                    return Tones.A;
                case 10:
                    return Tones.A;
                case 11:
                    return Tones.B;
                default:
                    return Tones.NO_TONE;
            }
        }

        protected override Intonation getIntonation(int intonationSpecifier)
        {
            var listOfSharps = new List<int> {1, 3, 6, 8, 10};
            return listOfSharps.Contains(intonationSpecifier % 12) ? Intonation.Sharp : Intonation.None;
        }
    }
}