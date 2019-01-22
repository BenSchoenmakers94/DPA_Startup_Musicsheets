using System;
using DPA_Musicsheets.Creation.Midi;
using DPA_Musicsheets.Interpreters.Midi.MidiMessaging;
using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi
{
    public class MidiInterpreter : GenericInterpreter<Sequence>
    {
        public MidiInterpreter()
        {
            midiNoteFactory = new MidiNoteFactory();
            midiMessagingService = new MidiMessagingService();
        }

        public Sequence Sequence { get; set; }
        public MidiEvent MidiEvent { get; set; }
        public int SequenceCount { get; set; }
        public int PreviousNoteAbsoluteTicks { get; set; }
        public bool StartedNoteIsClosed { get; set; }
        public Note CurrentNote { get; set; }
        public MidiNoteFactory midiNoteFactory { get; }
        public MidiMessagingService midiMessagingService { get; }
        public Track MetaTrack { get; set; }
        public Track InstrumentTrack { get; set; }
        public Staff Staff { get; set; }
        public Tuple<int, int> Rhythm { get; set; }

        public override Sequence Convert(Score song)
        {
            var newSequence = new Sequence();
            
            return newSequence;

        }

        public override Score ConvertBack(Sequence transformable)
        {
            var score = new Score();
            StartedNoteIsClosed = true;
            PreviousNoteAbsoluteTicks = 0;

            var track = Sequence[0];
            track.Merge(Sequence[1]);

            foreach (var midiEvent in track.Iterator())
            {
                MidiEvent = midiEvent;
                var midiMessage = midiEvent.MidiMessage;

                if (midiMessage.MessageType == MessageType.Channel)
                {
                   var worker = midiMessagingService.GetChannelMessageWorker(midiMessage);
                   worker.handleMessage((ChannelMessage)midiMessage, this, score);
                } else if (midiMessage.MessageType == MessageType.Meta)
                {
                   var worker = midiMessagingService.GetMetaMessageWorker(midiMessage);
                    worker.handleMessage((MetaMessage)midiMessage, score);
                }
            }
            return score;
        }
    }
}