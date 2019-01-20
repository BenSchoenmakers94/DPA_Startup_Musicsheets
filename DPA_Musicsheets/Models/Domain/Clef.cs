using DPA_Musicsheets.Models.Visitor;

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

        public string getClefName()
        {
            switch (type)
            {
                case ClefType.G:
                    return "treble";
                case ClefType.C:
                    return "alto";
                case ClefType.F:
                    return "bass";
                default:
                    return "treble";
            }
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}