using System;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Interpreters
{
    public abstract class GenericInterpreter<T>
    {
        public abstract T Convert(Score song);

        public abstract Score ConvertBack(T transformable);

        public bool CanGenerateSequence { get; set; }
    }
}