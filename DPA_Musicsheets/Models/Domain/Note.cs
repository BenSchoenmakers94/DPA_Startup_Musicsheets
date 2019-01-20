using System;

namespace DPA_Musicsheets.Models.Domain
{
    public enum Tones
    {
        NO_TONE = 0,
        C = 'C',
        D = 'D',
        E = 'E',
        F = 'F',
        G = 'G',
        A = 'A',
        B = 'B'
    }

    public enum Length
    {
        Semibreve = 1,
        Minim = 2,
        Crotchet = 4,
        Quaver = 8,
        Semiquaver = 16,
        Demisemiquaver = 32,
        Hemidemisemiquaver = 64
    }

    public enum Intonation
    {
        None = 0,
        Sharp = 1,
        Flat = -1
    }

    public class Note : Symbol
    {
        public Note(Tones tone, int pitch, Intonation intonation)
        {
            this.tone = tone;
            this.pitch = pitch;
            this.intonation = intonation;
            this.connected = false;
        }

        public Tones tone { get; private set; }
        public Length length { get; set; }
        public bool dot { get; set; }
        public int pitch { get; private set; }
        public bool connected { get; set; }
        public Intonation intonation { get; private set; }


    }
}