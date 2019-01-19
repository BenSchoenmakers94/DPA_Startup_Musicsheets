using System.Collections.Generic;

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
        public List<Note> notes { get; set; }

        public Bar(RepeatType repeatType)
        {
            this.type = repeatType;
        }
    }
}