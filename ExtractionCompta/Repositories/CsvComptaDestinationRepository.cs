using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.String;

namespace ExtractionCompta.Repositories
{
    public class CsvComptaDestinationRepository : IComptaDestinationRepository
    {
        private readonly string _outputDirectory;

        private const string Header = "Date;Pièce;Compte;Libelle;;Debit;Credit\r\n";
        public const string Banque = "Banque.csv";
        public const string Od = "OD.csv";

        public CsvComptaDestinationRepository(string outputDirectory)
        {            
            if (IsNullOrWhiteSpace(outputDirectory))
                throw new ArgumentNullException(nameof(outputDirectory));

            Directory.CreateDirectory(outputDirectory);
            _outputDirectory = outputDirectory;
        }

        public void SaveOd(List<DestinationLine> lines) => Save(lines, Od);

        public void SaveBanque(List<DestinationLine> lines) => Save(lines, Banque);

        private void Save(List<DestinationLine> lines, string fileName)
        {
            if (lines == null) throw new ArgumentNullException(nameof(lines));
            var builder = new StringBuilder(Header);

            foreach (var line in lines)
                builder.AppendLine($"{line.Date.ToString("dd/MM/yyyy")};;{line.Compte};{line.Libelle};;{line.Debit?.ToString("0.##")};{line.Credit?.ToString("0.##")}");

            File.WriteAllText(Path.Combine(_outputDirectory,fileName), builder.ToString(), Encoding.GetEncoding("ISO-8859-1"));
        }          
    }
}