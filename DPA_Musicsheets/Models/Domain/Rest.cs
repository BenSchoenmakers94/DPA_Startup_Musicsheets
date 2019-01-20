using DPA_Musicsheets.Models.Visitor;

namespace DPA_Musicsheets.Models.Domain
{
    public class Rest : Symbol
    {
        public Length length { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}