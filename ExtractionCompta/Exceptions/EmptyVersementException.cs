namespace ExtractionCompta.Exceptions
{
    public class EmptyVersementException : VersementsException
    {
        public EmptyVersementException() : base("Aucune ligne n'a �t� d�fini pour cr�er le versement")
        {
        }
    }
}