using System;
using System.Linq;
using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi.MidiMessaging.Meta
{
    public class TimeSignatureMessage : IMetaMessageWorker
    { 
        public void handleMessage(MetaMessage metaMessage, MidiInterpreter midi, Score score)
        {
            byte[] timeSignatureBytes = metaMessage.GetBytes();
            var timeSignature = new TimeSignature(timeSignatureBytes[0], (Length)(int) Math.Pow(2, timeSignatureBytes[1]));
            midi.beatsPerMessage = timeSignature.beatsPerMeasure;
            midi.timeSignatureLengthNote = timeSignature.lengthOfOneBeat;
            score.staffsInScore.Last().bars.Last().notes.Add(timeSignature);
        }
    }
}