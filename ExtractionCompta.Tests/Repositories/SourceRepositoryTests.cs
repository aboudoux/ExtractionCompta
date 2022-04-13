using System;
using System.IO;
using System.Linq;
using ExtractionCompta.Repositories;
using FluentAssertions;
using LinqToExcel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractionCompta.Tests.Repositories
{
    [TestClass]
    public class SourceRepositoryTests
    {
        [TestMethod]
        public void TestQueryReturnSomeResult()
        {
            var excel = new ExcelQueryFactory(Path.Combine(TestDirectory.RessourcesDirectory, "test_linq.xlsx"));
            var query = excel.Worksheet("Sorties").ToList();
            query.Should().NotBeEmpty();
        }

        [TestMethod]
        public void TestTableMaterialisation() {
            var excel = new ExcelQueryFactory(Path.Combine(TestDirectory.RessourcesDirectory, "test_linq.xlsx"));
            var query = excel.Worksheet<TableauExcel>("Sorties").ToList();
            query.Should().NotBeEmpty();

            var firstLine = query.First();
            firstLine.Id.ShouldBeEquivalentTo(1);
            firstLine.DateEnregistrement.ShouldBeEquivalentTo(new DateTime(2016,10,5));
            firstLine.MontantHt.Should().Be(100);
            firstLine.Compte.Should().Be("631300");
            firstLine.DateVersement.Should().BeNull();
            firstLine.IdVersement.Should().BeNull();
        }

        [TestMethod]
        public void TestReadSortieFromRepository()
        {
            var repository = new ExcelComptaSourceRepository(Path.Combine(TestDirectory.RessourcesDirectory, "test_linq.xlsx"));
            var lines = repository.GetSorties();
            lines.Should().HaveCount(11);
        }
    }
}
