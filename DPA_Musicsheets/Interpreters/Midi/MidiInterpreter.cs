using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi
{
    public class MidiInterpreter : GenericInterpreter<Sequence>
    {
        public override Sequence Convert(Staff song)
        {
            throw new System.NotImplementedException();
        }

        public override Staff ConvertBack(Sequence transformable)
        {
            throw new System.NotImplementedException();
        }
    }
}