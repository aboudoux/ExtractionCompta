namespace ExtractionCompta.Exceptions.InputLine
{
    public class VersementIdNotDefinedExpcetion : InputLineException
    {
        public VersementIdNotDefinedExpcetion() : base("L'identifiant de versement n'est pas d�fini alors qu'une date de versement existe")
        {
        }
    }
}