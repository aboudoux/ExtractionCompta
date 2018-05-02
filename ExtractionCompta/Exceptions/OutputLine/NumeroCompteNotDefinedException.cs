namespace ExtractionCompta.Exceptions.OutputLine
{
    public class NumeroCompteNotDefinedException : OutputLineException
    {
        public NumeroCompteNotDefinedException() : base("Le numéro de compte n'a pas été défini")
        {
        }
    }
}