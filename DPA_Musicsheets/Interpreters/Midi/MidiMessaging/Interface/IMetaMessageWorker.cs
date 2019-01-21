using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi.MidiMessaging
{
    public interface IMetaMessageWorker : IGenericMidiMessage
    {
        void handleMessage(MetaMessage metaMessage, Score score);
    }
}