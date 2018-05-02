using System;
using ExtractionCompta.Exceptions.OutputLine;
using static System.String;

namespace ExtractionCompta
{
    public class DestinationLine
    {
        public DestinationLine(DateTime date, Compte compte, string libelle, decimal? debit, decimal? credit)
        {            
            if( date == null || date == DateTime.MinValue )
                throw new DateNotDefinedException();
            if (compte == null)
                throw new NumeroCompteNotDefinedException();
            if (IsNullOrWhiteSpace(libelle))
                throw new LibelleNotDefinedException();
            if (!debit.HasValue && !credit.HasValue)
                throw new DebitAndCreditNotDefinedException();
            if (debit.HasValue && credit.HasValue)
                throw new DebitAndCreditAreBothDefinedException();

            Date = date;
            Compte = compte;
            Libelle = libelle;
            Debit = debit;
            Credit = credit;        
        }

        public DateTime Date { get; }
        public Compte Compte { get; }
        public string Libelle { get; }
        public decimal? Debit { get; }
        public decimal? Credit {get; }

        public bool LineComeFromMultipleVersement { get; private set; }

        public DestinationLine FromMultipleVersement(bool value)
        {
            LineComeFromMultipleVersement = value;
            return this;
        }
    }
}