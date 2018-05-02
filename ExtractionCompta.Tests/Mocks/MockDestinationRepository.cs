using System.Collections.Generic;
using ExtractionCompta.Repositories;

namespace ExtractionCompta.Tests.Mocks
{
    public class MockDestinationRepository : IComptaDestinationRepository
    {
        public List<DestinationLine> LinesOD { get; }  = new List<DestinationLine>();
        public List<DestinationLine> LinesBanque { get; }  = new List<DestinationLine>();


        public void SaveOd(List<DestinationLine> lines)
        {
            LinesOD.AddRange(lines);
        }

        public void SaveBanque(List<DestinationLine> lines)
        {
            LinesBanque.AddRange(lines);
        }
    }
}