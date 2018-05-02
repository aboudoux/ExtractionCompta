namespace ExtractionCompta.Exceptions.InputLine
{
    public class DateVersementNotDefinedException : InputLineException
    {
        public DateVersementNotDefinedException() : base("Aucune date de versement n'est défini alors qu'un identifiant de versement existe")
        {
        }
    }
}