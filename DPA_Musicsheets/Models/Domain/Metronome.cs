using System;
using System.Collections.Generic;

namespace DPA_Musicsheets.Models.Domain
{
    public class Metronome
    {
        public Metronome(Length tempoIndication, int beatsPerMinute)
        {
            this.tempoIndication = tempoIndication;
            this.beatsPerMinute = beatsPerMinute;
        }

        public Metronome(Length tempoIndication, Tuple<int, int> beatsPerMinuteInRange)
        {
            this.tempoIndication = tempoIndication;
            this.beatsPerMinuteInRange = beatsPerMinuteInRange;
        }

        public List<int> getBeatsPerMinute()
        {
            var list = new List<int>();
            if (beatsPerMinuteInRange == null) { list.Add(beatsPerMinute); }
            else { list.Add(beatsPerMinuteInRange.Item1); list.Add(beatsPerMinuteInRange.Item2); } 
            return list;
        }

        public Length tempoIndication { get; private set; }
        private int beatsPerMinute { get; set; }
        private Tuple<int, int> beatsPerMinuteInRange { get; set; }
    }
}