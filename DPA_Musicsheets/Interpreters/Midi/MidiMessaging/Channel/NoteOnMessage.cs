using System.Linq;
using DPA_Musicsheets.Creation.Midi;
using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi.MidiMessaging.Channel
{
    public class NoteOnMessage : IChannelMessageWorker
    {
        public void handleMessage(ChannelMessage channelMessage, MidiInterpreter midiInterpreter, Score score)
        {
            if (channelMessage.Data2 > 0)
            {
                midiInterpreter.CurrentNote = new MidiNoteFactory().create(channelMessage.Data1);

                midiInterpreter.StartedNoteIsClosed = false;
            }
            else if (!midiInterpreter.StartedNoteIsClosed)
            {
                midiInterpreter.CurrentNote.length = GetNoteLength(
                        midiInterpreter.PreviousNoteAbsoluteTicks, 
                        midiInterpreter.MidiEvent.AbsoluteTicks, 
                        midiInterpreter.Sequence.Division, 
                        midiInterpreter.Rhythm.Item1, 
                        midiInterpreter.Rhythm.Item2, out bool dot);

                midiInterpreter.CurrentNote.dot = dot;

                midiInterpreter.PreviousNoteAbsoluteTicks = midiInterpreter.MidiEvent.AbsoluteTicks;

                midiInterpreter.StartedNoteIsClosed = true;

                score.staffsInScore.Last().bars.Last().notes.Add(midiInterpreter.CurrentNote);
                midiInterpreter.CurrentNote = null;
            }
            else
            {
                score.staffsInScore.Last().bars.Last().notes.Add(new Rest() { length = midiInterpreter.CurrentNote.length });
                midiInterpreter.StartedNoteIsClosed = false;
            }
        }

        private Length GetNoteLength(int absoluteTicks, int nextNoteAbsoluteTicks, int division, int beatNote, int beatsPerBar, out bool hasDot)
        {
            hasDot = false;

            double deltaTicks = nextNoteAbsoluteTicks - absoluteTicks;

            if (deltaTicks <= 0)
            {
                return 0;
            }

            double percentageOfBeatNote = deltaTicks / division;

            if (!(percentageOfBeatNote == 4 || percentageOfBeatNote == 2 || percentageOfBeatNote == 1 || percentageOfBeatNote == 0.5 || percentageOfBeatNote == 0.25 || percentageOfBeatNote == 0.125))
            {
                hasDot = true;
                percentageOfBeatNote = percentageOfBeatNote / 3 * 2;
            }
            percentageOfBeatNote = percentageOfBeatNote / 4;

            return (Length)(int)percentageOfBeatNote;
        }
    }
}