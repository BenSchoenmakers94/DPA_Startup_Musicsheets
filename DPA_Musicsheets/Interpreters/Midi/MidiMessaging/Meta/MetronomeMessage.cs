using System.Linq;
using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Interpreters.Midi.MidiMessaging.Meta
{
    public class MetronomeMessage : IMetaMessageWorker
    {
        public void handleMessage(MetaMessage metaMessage, Score score)
        {
            byte[] tempoBytes = metaMessage.GetBytes();
            int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
            var bpm = 60000000 / tempo;
            var metronome = new Metronome(Length.Quarter, bpm);
            score.staffsInScore.Last().metronome = metronome;
        }
    }
}