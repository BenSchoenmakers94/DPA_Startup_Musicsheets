using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi.MidiMessaging.Channel
{
    public class NoteOnMessage : IChannelMessageWorker
    { 
        public void handleMessage(ChannelMessage channelMessage, MidiInterpreter midiInterpreter)
        {
            throw new System.NotImplementedException();
        }

        public void handleMessage(ChannelMessage channelMessage, Score score)
        {
            throw new System.NotImplementedException();
        }
    }
}