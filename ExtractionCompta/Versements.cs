using System;
using System.Collections.Generic;
using System.Linq;
using ExtractionCompta.Exceptions;

namespace ExtractionCompta
{
    public abstract class Versements
    {
        protected readonly List<SourceLine> _lines;
        protected bool IsSingleLine { get; }
        protected bool CompteCourant { get; }

        protected Versements(List<SourceLine> lines)
        {
            if (lines == null) throw new ArgumentNullException(nameof(lines));

            if( !lines.Any() )
                throw new EmptyVersementException();

            var versementId = lines.First().VersementId;
            if(!versementId.HasValue || lines.Any(a=>a.VersementId != versementId))
                throw new InvalidVersementIdException(versementId.Value);

            _lines = lines;
            IsSingleLine = _lines.Count == 1;
            CompteCourant = lines.Any(a => a.Cca);
        }

        public List<DestinationLine> ToOutputLines()
        {
            var result = new List<DestinationLine>();

            foreach (var line in _lines)
            {
                result.Add(ExtractHt(line).FromMultipleVersement(!IsSingleLine).FromCompteCourant(CompteCourant));
                if( line.MontantTva > 0 )
                    result.Add(ExtractTva(line).FromMultipleVersement(!IsSingleLine).FromCompteCourant(CompteCourant));
                if( CompteCourant)
                    result.Add(ExtractCompteCourant(line).FromMultipleVersement(!IsSingleLine).FromCompteCourant(CompteCourant));
            }

            var dateVersement = _lines.First().DateVersement.Value;
            var montantTotalVersements = _lines.Sum(a => a.MontantTva + a.MontantHt);

            if (CompteCourant)
                result.Add(CreateVirementCompteCourant(dateVersement, montantTotalVersements).FromMultipleVersement(!IsSingleLine).FromCompteCourant(CompteCourant));

            result.Add(CreateBanque(dateVersement, montantTotalVersements).FromMultipleVersement(!IsSingleLine).FromCompteCourant(CompteCourant));

            return result;
        }

        protected abstract DestinationLine CreateVirementCompteCourant(DateTime dateVersement, decimal total);

        protected abstract DestinationLine ExtractHt(SourceLine line);

        protected abstract DestinationLine ExtractTva(SourceLine line);

        protected abstract DestinationLine ExtractCompteCourant(SourceLine line);

        protected abstract DestinationLine CreateBanque(DateTime dateVersement, decimal total);
    }
}