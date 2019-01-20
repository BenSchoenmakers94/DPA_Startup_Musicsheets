using DPA_Musicsheets.Models.Visitor;

namespace DPA_Musicsheets.Models.Domain
{
    public class TimeSignature : Symbol
    {
        public TimeSignature(int beatsPerMeasure, Length lengthOfOneBeat)
        {
            this.beatsPerMeasure = beatsPerMeasure;
            this.lengthOfOneBeat = lengthOfOneBeat;
        }

        public int beatsPerMeasure { get; private set; }
        public Length  lengthOfOneBeat { get; private set; }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}