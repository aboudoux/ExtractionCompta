namespace ExtractionCompta.Exceptions.OutputLine
{
    public class DateNotDefinedException : OutputLineException
    {
        public DateNotDefinedException() : base("La date n'a pas été défini")
        {
        }
    }
}