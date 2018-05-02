using System;
using ExtractionCompta.Exceptions;
using ExtractionCompta.Exceptions.InputLine;
using static System.String;

namespace ExtractionCompta
{
    public class SourceLine
    {
        public SourceLine(int? lineId, string libelle, decimal montantHt, decimal montantTva, Compte compte, DateTime? dateVersement, int? versementId)
        {
            if (!lineId.HasValue)
                throw new LineIdNotDefinedException();
            if (compte == null)
                throw new CompteNotDefinedException();
            if (IsNullOrWhiteSpace(libelle))
                throw new LibelleNotDefinedException(lineId.Value);
            if (versementId.HasValue && !dateVersement.HasValue)
                throw new DateVersementNotDefinedException();
            if (dateVersement.HasValue && !versementId.HasValue)
                throw new VersementIdNotDefinedExpcetion();

            LineId = lineId;
            Libelle = libelle;
            MontantHt = montantHt;
            MontantTva = montantTva;
            Compte = compte;
            DateVersement = dateVersement;
            VersementId = versementId;
        }

        public int? LineId { get; }
        public string Libelle { get; }
        public decimal MontantHt { get; }
        public decimal MontantTva { get; }
        public Compte Compte { get; }
        public DateTime? DateVersement { get; }
        public int? VersementId { get; }
    }
}