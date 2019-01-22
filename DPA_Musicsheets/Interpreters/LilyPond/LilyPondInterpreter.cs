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
        private StringBuilder stringBuilder_;

        public LilyPondInterpreter(LilyPondNoteFactory factory)
        {
            factory_ = factory;
            CanGenerateSequence = false;
        }

        public override string Convert(Score song)
        {
            stringBuilder_ = new StringBuilder();
            stringBuilder_.AppendLine("\\relative c' { ");

            foreach (var staff in song.staffsInScore)
            {
                foreach (var bar in staff.bars)
                {
                    foreach (var note in bar.notes)
                    {
                        note.Accept(this);
                    }
                    bar.Accept(this);
                }
            }
            return stringBuilder_.ToString();
        }

        public override Score ConvertBack(string transformable)
        {
            var score = new Score();
            score.addStaff(new Staff());
            score.staffsInScore.First().addBar(new Bar(RepeatType.NoRepeat));

            var tokens = getTokensFromLilypond(transformable);

            var current = tokens.First.Value;

            factory_.Octave = -1;

            while (current != null)
            {
                switch (current.TokenKind)
                {
                    case LilypondTokenKind.Note:
                        score.staffsInScore.Last().bars.Last().notes.Add(factory_.create(current.Value));
                        break;
                    case LilypondTokenKind.Tempo:
                        var tempoSections = current.NextToken.Value.Split('=');
                        Length lengthOfBeat = (Length)Int32.Parse(tempoSections.First());
                        var rangeMeasures = tempoSections.Last().Split('-');
                        if (rangeMeasures.Length > 1)
                        {
                            score.staffsInScore.Last().bars.Last().notes.Add(new Metronome(lengthOfBeat,
                                new Tuple<int, int>(Int32.Parse(rangeMeasures.First()),
                                    Int32.Parse(rangeMeasures.Last()))));
                        }
                        else
                        {
                            score.staffsInScore.Last().bars.Last().notes.Add(new Metronome(lengthOfBeat, Int32.Parse(rangeMeasures.First())));
                        }
                        break;
                    case LilypondTokenKind.Time:
                        var timeSignatureSections = current.NextToken.Value.Split('/');
                        score.staffsInScore.Last().bars.Last().notes.Add(new TimeSignature(Int32.Parse(timeSignatureSections.First()), (Length)Int32.Parse(timeSignatureSections.Last())));
                        break;
                    case LilypondTokenKind.Bar:
                        score.staffsInScore.Last().addBar(new Bar(RepeatType.NoRepeat));
                        break;
                    case LilypondTokenKind.Clef:
                        var clefString = current.NextToken.Value;
                        Clef newClef;
                        switch (clefString)
                        {
                            case "treble":
                                newClef = new Clef(ClefType.GClef);
                                break;
                            case "alto":
                                newClef = new Clef(ClefType.CClef);
                                break;
                            case "bass":
                                newClef = new Clef(ClefType.FClef);
                                break;
                            default:
                                newClef = new Clef(ClefType.GClef);
                                break;
                        }
                        score.staffsInScore.Last().bars.Last().notes.Add(newClef);
                        break;
                    case LilypondTokenKind.Rest:
                        Rest rest = new Rest();
                        rest.length = (Length) int.Parse(Regex.Match(current.Value, @"\d+").Value);
                        score.staffsInScore.Last().bars.Last().notes.Add(rest);
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
                    case LilypondTokenKind.Unknown when new Regex(@"[a-g][,'eis]*[0-9]+[~]?[.]*", RegexOptions.IgnoreCase).IsMatch(token.Value):
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
            stringBuilder_.Append("r");
            stringBuilder_.Append((int) rest.length + " ");
        }

        public void Visit(Note note)
        {
            stringBuilder_.Append(note.tone == Tones.NO_TONE ? "r" : note.tone.ToString());
            if (note.pitch == 3) stringBuilder_.Append('\'');
            if (note.pitch == 5) stringBuilder_.Append(',');
            if (note.intonation == Intonation.Flat) stringBuilder_.Append('\'');
            if (note.intonation == Intonation.Sharp) stringBuilder_.Append('\'');
            stringBuilder_.Append((int) note.length);
            if (note.dot) stringBuilder_.Append('.');
            if (note.connected) stringBuilder_.Append("~");
            stringBuilder_.Append(" ");
        }

        public void Visit(Bar bar)
        {
            stringBuilder_.Append("| \n");
        }

        public void Visit(Clef clef)
        {
            stringBuilder_.AppendLine($"\\clef {clef.getClefName()} ");
        }

        public void Visit(Metronome metronome)
        {
            var list = metronome.getBeatsPerMinute();
            stringBuilder_.AppendLine(list?.Count > 1
                ? $"\\tempo {(int) metronome.tempoIndication}={list?[0]}-{list?[1]} "
                : $"\\tempo {(int) metronome.tempoIndication}={list?[0]} ");
        }

        public void Visit(TimeSignature timeSignature)
        {
            stringBuilder_.AppendLine($"\\time {timeSignature.beatsPerMeasure}/{(int)timeSignature.lengthOfOneBeat} ");
        }
    }
}