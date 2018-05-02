namespace ExtractionCompta.Exceptions
{
    public class InvalidVersementIdException : VersementsException
    {
        public InvalidVersementIdException(int versementId) : base("Erreur : l'une des lignes en entrée ne possède pas l'identifiant de versement " + versementId)
        {
        }
    }
}