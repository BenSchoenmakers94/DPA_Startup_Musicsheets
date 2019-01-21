using DPA_Musicsheets.Models.Visitor;

namespace DPA_Musicsheets.Models.Domain
{

    public enum ClefType
    {
        CClef = 3,
        FClef = 4,
        GClef = 2
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
                case ClefType.GClef:
                    return "treble";
                case ClefType.CClef:
                    return "alto";
                case ClefType.FClef:
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