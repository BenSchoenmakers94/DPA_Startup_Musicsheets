namespace DPA_Musicsheets.Models.Domain
{
    public class TimeSignature
    {
        public TimeSignature(int beatsPerMeasure, Length lengthOfOneBeat)
        {
            this.beatsPerMeasure = beatsPerMeasure;
            this.lengthOfOneBeat = lengthOfOneBeat;
        }

        public int beatsPerMeasure { get; private set; }
        public Length  lengthOfOneBeat { get; private set; }
    }
}