using System.Linq;
using System.Text;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Interpreters.LilyPond
{
    public class LilyPondInterpreter : GenericInterpreter<string>
    {
        public override string Convert(Score song)
        {
            var sb = new StringBuilder();
            sb.AppendLine("\\relative c' { ");
            sb.AppendLine("\\clef treble ");
            sb.AppendLine($"\\time {song.staffsInScore.FirstOrDefault()?.timeSignature.beatsPerMeasure}/{song.staffsInScore.FirstOrDefault()?.timeSignature.lengthOfOneBeat} ");
            var list = song.staffsInScore.FirstOrDefault()?.metronome.getBeatsPerMinute();
            sb.AppendLine(list?.Count > 1
                ? $"\\tempo {song.staffsInScore.FirstOrDefault()?.metronome.tempoIndication}={list?[0]} - {list?[1]} "
                : $"\\tempo {song.staffsInScore.FirstOrDefault()?.metronome.tempoIndication}={list?[0]} ");


            foreach (var staff in song.staffsInScore)
            {
                foreach (var bar in staff.bars)
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
            }

            return sb.ToString();
        }

        public override Score ConvertBack(string transformable)
        {
            var score = new Score();
            string[] lilyPondText = transformable.Split().Where(item => item.Length > 0).ToArray();

//            for (int i = 0; i < lilyPondText.Length; i++)
//            {
//                switch (lilyPondText[i])
//                {
//                    case "\\relative":
//                        // New relative section start with a startPitch and an ocatveChange
//                        RelativeExpression relativeExpression = new RelativeExpression(lilyPondText[i + 1][0],
//                            string.Concat(lilyPondText[i + 1].Skip(1)));
//
//                        sections.Push(relativeExpression);
//
//                        if (rootSection == null)
//                            rootSection = relativeExpression;
//
//                        i += 2;
//                        break;
//                    case "\\repeat":
//                        RepeatExpression repeatExpression = new RepeatExpression();
//
//                        sections.Peek()?.ChildExpressions.Add(repeatExpression);
//                        sections.Push(repeatExpression);
//
//                        i += 3;
//                        break;
//                    case "\\alternative":
//                        AlternativeExpression alternativeExpression = new AlternativeExpression();
//
//                        sections.Peek()?.ChildExpressions.Add(alternativeExpression);
//                        sections.Push(alternativeExpression);
//
//                        context.InAlternative = true;
//                        i++;
//                        break;
//                    case "\\clef":
//                        sections.Peek().ChildExpressions.Add(new ClefExpression(lilypondText[i + 1]));
//                        i++;
//                        break;
//                    case "\\tempo":
//                        sections.Peek().ChildExpressions.Add(new TempoExpression(lilypondText[i + 1]));
//                        i += 1;
//                        break;
//                    case "\\time":
//                        sections.Peek().ChildExpressions.Add(new TimeSignatureExpression(lilypondText[i + 1]));
//                        i++;
//                        break;
//                    case "{":
//                        if (context.InAlternative)
//                        {
//                            // There is a new alternative group in the current alternative block
//                            AlternativeGroupExpression alternativeGroup = new AlternativeGroupExpression();
//
//                            sections.Peek()?.ChildExpressions.Add(alternativeGroup);
//                            sections.Push(alternativeGroup);
//
//                            context.InAlternativeGroup = true;
//                        }
//                        else
//                        {
//                            LilypondSection lilypondSection = new LilypondSection();
//
//                            if (sections.Any())
//                            {
//                                sections.Peek()?.ChildExpressions.Add(lilypondSection);
//                                sections.Push(lilypondSection);
//                            }
//                        }
//                        break;
//                    case "}": // Section has ended. It is no longer the current section, so pop it from the stack
//                        if (context.InRepeat)
//                        {
//                            if (lilypondText[i + 1] != "\\alternative")
//                            {
//                                sections.Peek().ChildExpressions.Add(new BarlineExpression(true));
//                            }
//
//                            context.InRepeat = false;
//                        }
//                        if (sections.Any()) sections.Pop();
//                        break;
//                    case "|": sections.Peek().ChildExpressions.Add(new BarlineExpression()); break;
//                    // It is a note or an unknown token
//                    default:
//                        try
//                        {
//                            sections.Peek().ChildExpressions.Add(new NoteExpression(lilypondText[i]));
//                            break;
//                        }
//                        catch (Exception)
//                        {
//                            /* It is an unknown token, skip it. */
//                            break;
//                        }
//                }
//            }

            return score;
        }
    }
}