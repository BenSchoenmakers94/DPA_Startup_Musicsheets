using System.Text;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Interpreters.LilyPond
{
    public class LilyPondInterpreter : GenericInterpreter<string>
    {
        public override string Convert(Staff song)
        {
            var sb = new StringBuilder();
            sb.AppendLine("\\relative c' { ");
            sb.AppendLine("\\clef treble ");
            sb.AppendLine($"\\time {song.rhythm.Item1}/{song.rhythm.Item2} ");
            sb.AppendLine($"\\tempo 4={song.bpm} ");

            foreach (var bar in song.bars)
            {
                foreach (var note in bar.notes)
                {
                    sb.Append(note.tone == Tones.NO_TONE ? "r" : note.tone.ToString());
                    if (note.pitch == 3) sb.Append('\'');
                    if (note.pitch == 5) sb.Append(',');
                    if (note.intonation == Intonation.Flat) sb.Append('\'');
                    if (note.intonation == Intonation.Sharp) sb.Append('\'');
                    sb.Append(note.length);
                    if (note.dot) sb.Append('.');
                    sb.Append(" ");
                }

                sb.Append("| \n");
            }

            return sb.ToString();
        }

        public override Staff ConvertBack(string transformable)
        {
            throw new System.NotImplementedException();
        }
    }
}