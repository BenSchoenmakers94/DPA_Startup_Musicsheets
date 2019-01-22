using System;
using System.Linq;
using DPA_Musicsheets.Creation.Midi;
using DPA_Musicsheets.Interpreters.Midi.MidiMessaging;
using DPA_Musicsheets.Models.Domain;
using DPA_Musicsheets.Models.Visitor;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi
{
    public class MidiInterpreter : GenericInterpreter<Sequence>, IVisitor
    {
        public MidiInterpreter()
        {
            midiMessagingService = new MidiMessagingService();
        }

        public Sequence Sequence { get; set; }
        public MidiEvent MidiEvent { get; set; }
        public int SequenceCount { get; set; }
        public int PreviousNoteAbsoluteTicks { get; set; }
        public bool StartedNoteIsClosed { get; set; }
        public Note CurrentNote { get; set; }
        public MidiMessagingService midiMessagingService { get; }
        public Track MetaTrack { get; set; }
        public Track InstrumentTrack { get; set; }
        public Staff Staff { get; set; }
        public Tuple<int, int> Rhythm { get; set; }

        public override Sequence Convert(Score song)
        {
            Sequence = new Sequence();
            PreviousNoteAbsoluteTicks = 0;
            MetaTrack = new Track();
            InstrumentTrack = new Track();
            Sequence.Add(MetaTrack);
            Sequence.Add(InstrumentTrack);

            foreach (var staff in song.staffsInScore)
            {
                foreach (var bar in staff.bars)
                {
                    bar.Accept(this);
                }
            }

            MetaTrack.Insert(PreviousNoteAbsoluteTicks, MetaMessage.EndOfTrackMessage);
            InstrumentTrack.Insert(PreviousNoteAbsoluteTicks, MetaMessage.EndOfTrackMessage);

            return Sequence;

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

        private void InsertTempo(Metronome metronome)
        {
            // Calculate tempo
            int speed = (60000000 / metronome.getBeatsPerMinute().Last());
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            MetaTrack.Insert(PreviousNoteAbsoluteTicks, new MetaMessage(MetaType.Tempo, tempo));
        }

        private void InsertTimeSignature(TimeSignature timeSignature)
        {
            byte[] timeSignatureBytes = new byte[4];
            timeSignatureBytes[0] = (byte)timeSignature.beatsPerMeasure;
            timeSignatureBytes[1] = (byte)timeSignature.lengthOfOneBeat;
            MetaTrack.Insert(PreviousNoteAbsoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignatureBytes));
        }

        public void Visit(Rest rest)
        {
            throw new NotImplementedException();
        }

        public void Visit(Note note)
        {
            throw new NotImplementedException();
        }

        public void Visit(Bar bar)
        {
            throw new NotImplementedException();
        }

        public void Visit(Clef clef)
        {
            throw new NotImplementedException();
        }

        public void Visit(Metronome metronome)
        {
            throw new NotImplementedException();
        }

        public void Visit(TimeSignature timeSignature)
        {
            throw new NotImplementedException();
        }
    }
}