using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Models.Domain
{
    public class Staff
    {
        public Staff()
        {
            bars = new List<Bar>();
        }

        public Staff parent { get; set; }
        public List<Bar> bars { get; private set; }
        public Tuple<int, int> rhythm { get; set; }
        public int bpm { get; set; }

        public double BarDuration => (double)rhythm.Item1 / (double)rhythm.Item2;
    }
}