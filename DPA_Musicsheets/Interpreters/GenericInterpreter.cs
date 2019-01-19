using System;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Interpreters
{
    public abstract class GenericInterpreter<T>
    {
        public abstract T Convert(Staff song);

        public abstract Staff ConvertBack(T transformable);
    }
}