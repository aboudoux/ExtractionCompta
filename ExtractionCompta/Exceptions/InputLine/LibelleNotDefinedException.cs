namespace ExtractionCompta.Exceptions.InputLine
{
    public class LibelleNotDefinedException : InputLineException
    {
        public LibelleNotDefinedException(int lineId) : base("Aucun libelle n'a �t� d�fini pour la ligne " + lineId)
        {
        }
    }
}