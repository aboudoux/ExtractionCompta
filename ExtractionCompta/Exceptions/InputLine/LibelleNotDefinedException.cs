namespace ExtractionCompta.Exceptions.InputLine
{
    public class LibelleNotDefinedException : InputLineException
    {
        public LibelleNotDefinedException(int lineId) : base("Aucun libelle n'a été défini pour la ligne " + lineId)
        {
        }
    }
}