namespace ExtractionCompta.Exceptions.InputLine
{
    public class CompteNotDefinedException : InputLineException
    {
        public CompteNotDefinedException() : base("Le numéro de compte n'a pas été défini")
        {
        }
    }
}