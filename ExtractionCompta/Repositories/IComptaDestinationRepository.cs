using System.Collections.Generic;

namespace ExtractionCompta.Repositories
{
    public interface IComptaDestinationRepository
    {
        void SaveOd(List<DestinationLine> lines);
        void SaveBanque(List<DestinationLine> lines);
    }
}