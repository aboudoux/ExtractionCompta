using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtractionCompta
{
    public class VersementsSorties : Versements
    {
        public VersementsSorties(List<SourceLine> lines) : base(lines)
        {
        }

        protected override DestinationLine ExtractHt(SourceLine line) {
            return new DestinationLine(line.DateVersement.Value, line.Compte, line.Libelle, line.MontantHt, null);
        }

        protected override DestinationLine ExtractTva(SourceLine line) {
            return new DestinationLine(line.DateVersement.Value, new Compte(Compte.TVA), line.Libelle, line.MontantTva, null);
        }

        protected override DestinationLine ExtractCompteCourant(SourceLine line) {
            return new DestinationLine(line.DateVersement.Value, new Compte(Compte.COMPTE_COURANT), line.Libelle, null, line.MontantHt + line.MontantTva);
        }

        protected override DestinationLine CreateBanque(DateTime dateVersement, decimal total) {
            var libelle = (IsSingleLine && !CompteCourant) ? _lines.First().Libelle : Libelle.Virement;
            return new DestinationLine(dateVersement, new Compte(Compte.BANQUE), libelle, null, total);
        }

        protected override DestinationLine CreateVirementCompteCourant(DateTime dateVersement, decimal total) {
            return new DestinationLine(dateVersement, new Compte(Compte.COMPTE_COURANT), Libelle.Virement, total, null);
        }
    }
}