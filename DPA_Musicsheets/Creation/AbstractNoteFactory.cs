

using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Creation
{
    public abstract class AbstractNoteFactory<T>
    {
        public abstract Note create(T noteSpecifier);

        protected abstract Tones getTone(T toneSpecifier);

        protected abstract Intonation getIntonation(T intonationSpecifier);
    }
}