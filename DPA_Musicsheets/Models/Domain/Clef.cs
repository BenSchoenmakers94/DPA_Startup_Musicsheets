namespace DPA_Musicsheets.Models.Domain
{

    public enum ClefType
    {
        G = 0,
        C = 1,
        F = 2
    }
    public class Clef : Symbol
    {
        public ClefType type { get; private set; }

        public Clef(ClefType clefType)
        {
            type = clefType;
        }
    }
}