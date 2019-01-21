using System.Collections.Generic;
using DPA_Musicsheets.Interpreters.Midi.MidiMessaging.Channel;
using DPA_Musicsheets.Interpreters.Midi.MidiMessaging.Meta;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi.MidiMessaging
{
    public class MidiMessagingService
    {
        private Dictionary<MetaType, IMetaMessageWorker> metaWorkers;
        private Dictionary<ChannelCommand, IChannelMessageWorker> channelWorkers;

        public MidiMessagingService()
        {
            // Meta
            metaWorkers = new Dictionary<MetaType, IMetaMessageWorker>
            {
                {MetaType.Tempo, new MetronomeMessage()}, {MetaType.TimeSignature, new TimeSignatureMessage()}
            };

            // Channel
            channelWorkers = new Dictionary<ChannelCommand, IChannelMessageWorker>
            {
                {ChannelCommand.NoteOn, new NoteOnMessage()}
            };
        }

        public IMetaMessageWorker GetMetaMessageWorker(IMidiMessage midiMessage)
        {
            if (midiMessage is MetaMessage message && metaWorkers.ContainsKey(message.MetaType))
            {
                return metaWorkers[message.MetaType];
            }
            return null;
        }

        public IChannelMessageWorker GetChannelMessageWorker(IMidiMessage midiMessage)
        {
            if (midiMessage is ChannelMessage message && channelWorkers.ContainsKey(message.Command))
            {
                return channelWorkers[message.Command];
            }
            return null;
        }
    }
}