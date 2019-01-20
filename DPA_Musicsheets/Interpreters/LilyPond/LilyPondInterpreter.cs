using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Creation.LilyPond;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.Domain;
using DPA_Musicsheets.Models.Visitor;

namespace DPA_Musicsheets.Interpreters.LilyPond
{
    public class LilyPondInterpreter : GenericInterpreter<string>, IVisitor
    {
        private LilyPondNoteFactory factory_;
        private string currentAppendedText_;

        public LilyPondInterpreter(LilyPondNoteFactory factory)
        {
            factory_ = factory;
        }

        public override string Convert(Score song)
        {
            var sb = new StringBuilder();
            sb.AppendLine("\\relative c' { ");
            sb.AppendLine($"\\clef {song.staffsInScore.FirstOrDefault()?.clef.getClefName()} ");
            sb.AppendLine($"\\time {song.staffsInScore.FirstOrDefault()?.timeSignature.beatsPerMeasure}/{song.staffsInScore.FirstOrDefault()?.timeSignature.lengthOfOneBeat} ");
            var list = song.staffsInScore.FirstOrDefault()?.metronome.getBeatsPerMinute();
            sb.AppendLine(list?.Count > 1
                ? $"\\tempo {song.staffsInScore.FirstOrDefault()?.metronome.tempoIndication}={list?[0]}-{list?[1]} "
                : $"\\tempo {song.staffsInScore.FirstOrDefault()?.metronome.tempoIndication}={list?[0]} ");


            foreach (var staff in song.staffsInScore)
            {
                foreach (var bar in staff.bars)
                {
                    foreach (var note in bar.notes)
                    {
                        if (note is Note)
                        {
                            note =  note;
                            sb.Append(note.tone == Tones.NO_TONE ? "r" : note.tone.ToString());
                            if (note.pitch == 3) sb.Append('\'');
                            if (note.pitch == 5) sb.Append(',');
                            if (note.intonation == Intonation.Flat) sb.Append('\'');
                            if (note.intonation == Intonation.Sharp) sb.Append('\'');
                            sb.Append(note.length);
                            if (note.dot) sb.Append('.');
                            sb.Append(" ");
                        }
                    }

                    sb.Append("| \n");
                }
            }

            return sb.ToString();
        }

        public override Score ConvertBack(string transformable)
        {
            var score = new Score();
            score.addStaff(new Staff());

            var tokens = getTokensFromLilypond(transformable);

            var current = tokens.First.Value;

            while (current != null)
            {
                switch (current.TokenKind)
                {
                    case LilypondTokenKind.Note:
                        score.staffsInScore.Last().bars.Last().notes.Add(factory_.create(current.Value));
                        break;
                    case LilypondTokenKind.Tempo:
                        var tempoSections = current.NextToken.Value.Split('=');
                        Length lengthOfBeat = (Length) Int32.Parse(tempoSections.First());
                        var rangeMeasures = tempoSections.Last().Split('-');
                        if (rangeMeasures.Length > 1)
                        {
                            score.staffsInScore.Last().metronome = new Metronome(lengthOfBeat,
                                new Tuple<int, int>(Int32.Parse(rangeMeasures.First()),
                                    Int32.Parse(rangeMeasures.Last())));
                        }
                        else
                        {
                            score.staffsInScore.Last().metronome = new Metronome(lengthOfBeat, Int32.Parse(rangeMeasures.First()));
                        }
                        break;
                    case LilypondTokenKind.Time:
                        var timeSignatureSections = current.NextToken.Value.Split('=');
                        score.staffsInScore.Last().timeSignature = new TimeSignature(Int32.Parse(timeSignatureSections.First()), (Length)Int32.Parse(timeSignatureSections.Last()));
                        break;
                    case LilypondTokenKind.Bar:
                        score.staffsInScore.Last().addBar(new Bar(RepeatType.NoRepeat));
                        break;
                    case LilypondTokenKind.Clef:
                        var clefString = current.NextToken.Value;
                        Clef newClef = null;
                        switch (clefString)
                        {
                            case "treble":
                                newClef = new Clef(ClefType.G);
                                break;
                            case "alto":
                                newClef = new Clef(ClefType.C);
                                break;
                            case "bass":
                                newClef = new Clef(ClefType.F);
                                break;
                            default:
                                newClef = new Clef(ClefType.G);
                                break;
                        }
                        score.staffsInScore.Last().clef = newClef;
                        break;
                }
                current = current.NextToken;
            }
            return score;
        }
        private LinkedList<LilypondToken> getTokensFromLilypond(string lilyPondText)
        {
            var tokens = new LinkedList<LilypondToken>();

            foreach (string substring in lilyPondText.Split(' ').Where(item => item.Length > 0))
            {
                var sanitizedString = substring.Replace("\r\n", string.Empty);
                LilypondToken token = new LilypondToken
                {
                    Value = sanitizedString
                };

                switch (sanitizedString)
                {
                    case "\\relative": token.TokenKind = LilypondTokenKind.Staff; break;
                    case "\\clef": token.TokenKind = LilypondTokenKind.Clef; break;
                    case "\\time": token.TokenKind = LilypondTokenKind.Time; break;
                    case "\\tempo": token.TokenKind = LilypondTokenKind.Tempo; break;
                    case "\\repeat": token.TokenKind = LilypondTokenKind.Repeat; break;
                    case "\\alternative": token.TokenKind = LilypondTokenKind.Alternative; break;
                    case "{": token.TokenKind = LilypondTokenKind.SectionStart; break;
                    case "}": token.TokenKind = LilypondTokenKind.SectionEnd; break;
                    case "|": token.TokenKind = LilypondTokenKind.Bar; break;
                    default: token.TokenKind = LilypondTokenKind.Unknown; break;
                }

                switch (token.TokenKind)
                {
                    case LilypondTokenKind.Unknown when new Regex(@"[~]?[a-g][,'eis]*[0-9]+[.]*", RegexOptions.IgnoreCase).IsMatch(token.Value):
                        token.TokenKind = LilypondTokenKind.Note;
                        break;
                    case LilypondTokenKind.Unknown when new Regex(@"r.*?[0-9][.]*").IsMatch(substring):
                        token.TokenKind = LilypondTokenKind.Rest;
                        break;
                }

                if (tokens.Last != null)
                {
                    tokens.Last.Value.NextToken = token;
                    token.PreviousToken = tokens.Last.Value;
                }

                tokens.AddLast(token);
            }

            return tokens;
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
    }
}