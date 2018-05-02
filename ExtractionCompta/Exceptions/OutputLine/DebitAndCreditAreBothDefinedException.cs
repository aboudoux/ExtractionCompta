namespace ExtractionCompta.Exceptions.OutputLine
{
    public class DebitAndCreditAreBothDefinedException : OutputLineException
    {
        public DebitAndCreditAreBothDefinedException() : base("Le d�bit et le cr�di ont �t� tous les deux d�fini. Seul un des deux doit l'�tre.")
        {
        }
    }
}