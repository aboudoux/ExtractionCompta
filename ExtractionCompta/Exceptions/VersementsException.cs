using System;

namespace ExtractionCompta.Exceptions
{
    public abstract class VersementsException : Exception
    {
        protected VersementsException(string message)  : base(message)
        {
            
        }
    }
}