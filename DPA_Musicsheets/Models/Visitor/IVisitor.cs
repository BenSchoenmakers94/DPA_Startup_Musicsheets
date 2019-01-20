using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Models.Visitor
{
    public interface IVisitor
    {
        void Visit(Rest rest);
        void Visit(Note note);
        void Visit(Bar bar);
        void Visit(Clef clef);
        void Visit(Metronome metronome);
        void Visit(TimeSignature timeSignature);
    }
}