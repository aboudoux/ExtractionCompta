using System.Collections.Generic;
using ExtractionCompta.Repositories;

namespace ExtractionCompta.Tests.Mocks
{
    public class MockSourceRepository : IComptaSourceRepository
    {
        public List<SourceLine> Sorties { get; }  = new List<SourceLine>();
        public List<SourceLine> Entrees { get; }  = new List<SourceLine>();

        public IEnumerable<SourceLine> GetSorties()
        {
            return Sorties;
        }

        public IEnumerable<SourceLine> GetEntrees()
        {
            return Entrees;
        }
    }
}