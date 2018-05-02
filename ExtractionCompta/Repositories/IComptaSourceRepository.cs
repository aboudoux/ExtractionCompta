using System.Collections.Generic;

namespace ExtractionCompta.Repositories
{
    public interface IComptaSourceRepository
    {
        IEnumerable<SourceLine> GetSorties();
        IEnumerable<SourceLine> GetEntrees();
    }
}