namespace ExtractionCompta.Exceptions.InputLine
{
    public class LineIdNotDefinedException : InputLineException
    {
        public LineIdNotDefinedException() : base("Identifiant de ligne non défini")
        {
        }
    }
}