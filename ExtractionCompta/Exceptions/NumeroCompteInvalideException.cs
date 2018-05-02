using System;

namespace ExtractionCompta.Exceptions
{
    public class NumeroCompteInvalideException : Exception
    {
        public NumeroCompteInvalideException(string valeur) : base($"Numéro de compte {valeur} invalide. Celui ci doit être composé de 6 chiffres.")
        {
            
        }
    }
}
