namespace ExtractionCompta.Exceptions.OutputLine
{
    public class DebitAndCreditNotDefinedException : OutputLineException
    {
        public DebitAndCreditNotDefinedException() : base("Ni le débit ni le crédit n'ont été défini pour cette ligne")
        {
        }
    }
}