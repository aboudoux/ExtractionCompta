namespace ExtractionCompta.Exceptions.OutputLine
{
    public class DebitAndCreditAreBothDefinedException : OutputLineException
    {
        public DebitAndCreditAreBothDefinedException() : base("Le débit et le crédi ont été tous les deux défini. Seul un des deux doit l'être.")
        {
        }
    }
}