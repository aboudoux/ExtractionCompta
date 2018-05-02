namespace ExtractionCompta.Exceptions.InputLine
{
    public class DateVersementNotDefinedException : InputLineException
    {
        public DateVersementNotDefinedException() : base("Aucune date de versement n'est d�fini alors qu'un identifiant de versement existe")
        {
        }
    }
}