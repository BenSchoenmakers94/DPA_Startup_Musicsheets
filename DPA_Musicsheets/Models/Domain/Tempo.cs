namespace DPA_Musicsheets.Models.Domain
{
    public class Tempo : Symbol
    {
        public Tempo(int barBeats, int beatUnit)
        {
            this.barBeats = barBeats;
            this.beatUnit = beatUnit;
        }

        public int barBeats { get; private set; }
        public int beatUnit { get; private set; }
    }
}