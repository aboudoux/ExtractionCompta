using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtractionCompta
{
    public class VersementsEntrees : Versements
    {
        public VersementsEntrees(List<SourceLine> lines) : base(lines)
        {
        }        

        protected override DestinationLine ExtractHt(SourceLine line)
        {
            return new DestinationLine(line.DateVersement.Value, line.Compte, line.Libelle,  null, line.MontantHt);
        }

        protected override DestinationLine ExtractTva(SourceLine line)
        {
            return new DestinationLine(line.DateVersement.Value, new Compte(Compte.TVA_COLLECTEE), line.Libelle, null, line.MontantTva);
        }

        protected override DestinationLine ExtractCompteCourant(SourceLine line)
        {
            return new DestinationLine(line.DateVersement.Value, new Compte(Compte.COMPTE_COURANT), line.Libelle, line.MontantHt + line.MontantTva, null);
        }

        protected override DestinationLine CreateBanque(DateTime dateVersement, decimal total)
        {
            var libelle = IsSingleLine ? _lines.First().Libelle : Libelle.Virement;
            return new DestinationLine(dateVersement, new Compte(Compte.BANQUE), libelle, total, null);
        }

        protected override DestinationLine CreateVirementCompteCourant(DateTime dateVersement, decimal total) 
        {
            return new DestinationLine(dateVersement, new Compte(Compte.COMPTE_COURANT), Libelle.Virement, null, total);
        }
    }
}