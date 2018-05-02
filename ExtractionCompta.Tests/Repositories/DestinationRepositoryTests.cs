using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExtractionCompta.Repositories;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractionCompta.Tests
{
    [TestClass]
    public class DestinationRepositoryTests
    {
        private string OdFilePath => Path.Combine(CurrentDirectory, CsvComptaDestinationRepository.Od);
        private string BanqueFilePath => Path.Combine(CurrentDirectory, CsvComptaDestinationRepository.Banque);
        private string CurrentDirectory => Directory.GetCurrentDirectory();
        

        [TestMethod]
        public void TestSaveAllCsvFile()
        {            
            var lines = new List<DestinationLine>()
            {
                new DestinationLine(new DateTime(2017, 03, 06), new Compte(Compte.BANQUE), "TEST BANQUE", null, 10)
            };

            var destination = new CsvComptaDestinationRepository(CurrentDirectory);
            destination.SaveOd(lines);
            destination.SaveBanque(lines);

            File.Exists(OdFilePath).Should().BeTrue();
            File.Exists(BanqueFilePath).Should().BeTrue();

            var fileLines = File.ReadAllLines(OdFilePath, Encoding.GetEncoding("ISO-8859-1"));
            fileLines.Should().HaveCount(2);
            fileLines[0].Should().Be("Date;Pièce;Compte;Libelle;;Debit;Credit");
            fileLines[1].ShouldBeEquivalentTo("06/03/2017;;512000;TEST BANQUE;;;10");

            fileLines = File.ReadAllLines(BanqueFilePath, Encoding.GetEncoding("ISO-8859-1"));
            fileLines.Should().HaveCount(2);
            fileLines[0].Should().Be("Date;Pièce;Compte;Libelle;;Debit;Credit");
            fileLines[1].ShouldBeEquivalentTo("06/03/2017;;512000;TEST BANQUE;;;10");
        }

        [TestMethod]
        public void TestSavePrecision()
        {            
            var lines = new List<DestinationLine>()
            {
                new DestinationLine(new DateTime(2017, 03, 06), new Compte(Compte.BANQUE), "TEST BANQUE", null, 1.123m)
            };

            var destination = new CsvComptaDestinationRepository(CurrentDirectory);
            destination.SaveOd(lines);

            File.Exists(OdFilePath).Should().BeTrue();
            var fileLines = File.ReadAllLines(OdFilePath, Encoding.GetEncoding("ISO-8859-1"));
            fileLines.Should().HaveCount(2);
            fileLines[0].Should().Be("Date;Pièce;Compte;Libelle;;Debit;Credit");
            fileLines[1].ShouldBeEquivalentTo("06/03/2017;;512000;TEST BANQUE;;;1,12");
        }
    }
}
