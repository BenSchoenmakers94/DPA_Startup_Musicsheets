using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Interpreters;
using DPA_Musicsheets.Models.Visitor;

namespace DPA_Musicsheets.Models.Domain
{
    public abstract class Symbol
    {
        public abstract void Accept(IVisitor visitor);
    }
}
