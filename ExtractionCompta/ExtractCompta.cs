using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using ExtractionCompta.Repositories;

namespace ExtractionCompta
{
    public class ExtractCompta
    {
        private readonly IComptaSourceRepository _input;
        private readonly IComptaDestinationRepository _output;

        public ExtractCompta(IComptaSourceRepository input, IComptaDestinationRepository output)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            _input = input;
            _output = output;
        }

        public void Execute()
        {
            var outputLines = new List<DestinationLine>();

            foreach (var versements in GetAllVersements())
                outputLines.AddRange(versements.ToOutputLines());

            SaveLines(outputLines.OrderBy(a => a.Date).ToList());
        }

        private void SaveLines(List<DestinationLine> lines)
        {
            var odLines = new List<DestinationLine>();
            var banqueLines = new List<DestinationLine>();

            foreach (var line in lines) {
                if (line.LineComeFromMultipleVersement && line.Libelle != Libelle.Virement)
                    odLines.Add(line);
                else
                    banqueLines.Add(line);                
            }

            _output.SaveOd(odLines);
            _output.SaveBanque(banqueLines);
        }

        private List<Versements> GetAllVersements() 
        {
            var result = new List<Versements>();

            foreach (var grouping in _input.GetSorties().Where(a => a.DateVersement.HasValue).GroupBy(a=>a.VersementId) )
                result.Add(new VersementsSorties(grouping.ToList()));

            foreach (var grouping in _input.GetEntrees().Where(a => a.DateVersement.HasValue).GroupBy(a => a.VersementId))
                result.Add(new VersementsEntrees(grouping.ToList()));

            return result;
        }        
    }
}