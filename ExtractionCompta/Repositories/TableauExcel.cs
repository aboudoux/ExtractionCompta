using System;
using LinqToExcel.Attributes;

namespace ExtractionCompta.Repositories
{
    public class TableauExcel
    {
        [ExcelColumn("Id")]
        public int Id { get; set; }
        [ExcelColumn("Date enregistrement")]
        public DateTime DateEnregistrement { get; set; }
        [ExcelColumn("Libellé")]
        public string Libelle { get; set; }
        [ExcelColumn("Montant HT")]
        public decimal MontantHt { get; set; }
        [ExcelColumn("Montant TVA")]
        public decimal MontantTtc { get; set; }
        [ExcelColumn("Total TTC")]
        public string TotalTtc { get; set; }
        [ExcelColumn("Compte")]
        public string Compte { get; set; }
        [ExcelColumn("Date versement")]
        public DateTime? DateVersement { get; set; }
        [ExcelColumn("Id versement")]
        public int? IdVersement { get; set; }
    }
}