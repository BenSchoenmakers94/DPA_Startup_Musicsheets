using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi
{
    public class MidiInterpreter : GenericInterpreter<Sequence>
    {
        public override Sequence Convert(Score song)
        {
            var newSequence = new Sequence();
            return newSequence;

        }

        public override Score ConvertBack(Sequence transformable)
        {
            throw new System.NotImplementedException();
        }
    }
}