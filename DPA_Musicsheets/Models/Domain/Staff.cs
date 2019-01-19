using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Models.Domain
{
    public class Staff
    {
        public Staff()
        {
         //   Children = new List<StaffElement>();
        }

        public Staff Parent { get; set; }
       // public List<StaffElement> Children { get; private set; }
        public Tuple<int, int> Rhythm { get; set; }
        public int Bpm { get; set; }

        public double BarDuration => (double)Rhythm.Item1 / (double)Rhythm.Item2;

//        public override void Accept(IStaffElementVisitor visitor)
//        {
//            visitor.Visit(this);
//        }
    }
}