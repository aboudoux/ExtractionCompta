using System;

namespace ExtractionCompta.Exceptions.InputLine
{
    public abstract class InputLineException : Exception
    {
        protected InputLineException(string message) : base(message)
        {
            
        }
    }
}