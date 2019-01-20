using System.Collections.Generic;
using DPA_Musicsheets.Models.Visitor;

namespace DPA_Musicsheets.Models.Domain
{
    public enum RepeatType
    {
        NoRepeat = 0,
        ForwardRepeat = 1,
        BackwardRepeat = 2
    }
    public class Bar : Symbol
    {
        public RepeatType type { get; private set; }
        public List<Symbol> notes { get; set; }

        public Bar(RepeatType repeatType)
        {
            this.type = repeatType;
            notes = new List<Symbol>();
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}