namespace DPA_Musicsheets.Models.Domain
{
    public enum Tones
    {
        C,
        D,
        E,
        F,
        G,
        A,
        B
    }

    public enum Length
    {
        Breve,
        Semibreve,
        Minim,
        Crotchet,
        Quaver,
        Semiquaver,
        Demisemiquaver,
        Hemidemisemiquaver
    }

    public enum Modifier
    {
        None = 0,
        Sharp = 1,
        Flat = -1
    }

    public class Note : Symbol
    {
        public Note(Tones tone, Length length, bool dot, int pitch, Modifier modifier)
        {
            this.tone = tone;
            this.length = length;
            this.dot = dot;
            this.pitch = pitch;
            this.modifier = modifier;
        }

        public Tones tone { get; private set; }
        public Length length { get; private set; }
        public bool dot { get; private set; }
        public int pitch { get; private set; }
        public Modifier modifier { get; private set; }


    }
}