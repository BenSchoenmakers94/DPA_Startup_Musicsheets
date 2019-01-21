using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi.MidiMessaging
{
    public interface IChannelMessageWorker : IGenericMidiMessage
    {
        void handleMessage(ChannelMessage channelMessage, Score score);
    }
}