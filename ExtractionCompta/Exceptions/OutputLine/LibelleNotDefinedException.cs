namespace ExtractionCompta.Exceptions.OutputLine
{
    public class LibelleNotDefinedException : OutputLineException
    {
        public LibelleNotDefinedException() : base("Le libelle n'a pas été défini")
        {
        }
    }
}