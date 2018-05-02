namespace ExtractionCompta.Exceptions
{
    public class EmptyVersementException : VersementsException
    {
        public EmptyVersementException() : base("Aucune ligne n'a été défini pour créer le versement")
        {
        }
    }
}