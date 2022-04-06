using System.Collections.Generic;
using System.IO;
using System.Linq;
using LinqToExcel;

namespace ExtractionCompta.Repositories
{
    public class ExcelComptaSourceRepository : IComptaSourceRepository
    {
        private readonly string _excelFilePath;

        public ExcelComptaSourceRepository(string excelFilePath)
        {            
            if( !File.Exists(excelFilePath) )
                throw new FileNotFoundException("Impossible d'ouvrir la compta excel", excelFilePath);

            _excelFilePath = excelFilePath;
        }

        public IEnumerable<SourceLine> GetSorties()
        {
            return ReadExcelSheet("Sorties");
        }

        public IEnumerable<SourceLine> GetEntrees()
        {
            return ReadExcelSheet("Entrées");
        }

        private IEnumerable<SourceLine> ReadExcelSheet(string sheetName)
        {
            var excel = new ExcelQueryFactory(_excelFilePath);
            var query = excel.Worksheet<TableauExcel>(sheetName).ToList();

            return query.Select(a => new SourceLine(a.Id, a.Libelle, a.MontantHt, a.MontantTtc, new Compte(a.Compte), a.DateVersement, a.IdVersement, !string.IsNullOrWhiteSpace(a.Cca))).ToList();
        }
    }
}