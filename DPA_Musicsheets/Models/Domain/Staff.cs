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

        public List<Bar> bars { get; set; }
        public TimeSignature timeSignature { get; set; }
        public Metronome metronome { get; set; }
        public Clef clef { get; set; }

        public void addBar(Bar bar)
        {
            bars.Add(bar);
        }

        public double BarDuration => (double)timeSignature.beatsPerMeasure / (double)timeSignature.lengthOfOneBeat;
    }
}