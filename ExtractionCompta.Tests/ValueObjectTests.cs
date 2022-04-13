

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExtractionCompta.Exceptions;
using ExtractionCompta.Exceptions.InputLine;
using ExtractionCompta.Exceptions.OutputLine;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractionCompta.Tests
{
    [TestClass]
    public class ValueObjectTests
    {
       
        [DataTestMethod]
        [DataRow("")]
        [DataRow("1")]
        [DataRow("12")]
        [DataRow("123")]
        [DataRow("1234")]
        [DataRow("12345")]
        [DataRow("1234567")]
        [DataRow("123456789")]
        [DataRow("12345A")]
        [ExpectedException(typeof(NumeroCompteInvalideException))]
        public void NumeroCompteShouldFail(string valeur)
        {
            var c = new Compte(valeur);
        }

        [DataTestMethod]
        [DataRow("111111")]
        [DataRow("123456")]
        [DataRow("999999")]
        public void NumerCompteShouldBeOk(string valeur)
        {
            var c = new Compte(valeur);
            Assert.IsNotNull(c);
        }

        [DataTestMethod]
        [DataRow(null,"",0,0,null,null,0)]
        [DataRow(null, "TEST", 0, 0, "123456", null, 0)]
        [DataRow(1, "", 0, 0, "123456", null, 0)]
        [DataRow(1, "TEST", 0, 0, "", null, 0)]
        [DataRow(1, "TEST", 2, 1, "123456", "01/07/1980", null)]
        [DataRow(1, "TEST", 2, 1, "123456", null, 3)]
        [ExpectedException(typeof(InputLineException), AllowDerivedTypes = true)]
        public void InputLineShouldFail(int? lineId, string libelle, double montantHt, double montantTva, string compte, string dateVersement, int? versementId )
        {
            DateTime? lineDate = null;
            if (!string.IsNullOrWhiteSpace(dateVersement))
                lineDate = DateTime.Parse(dateVersement);

            Compte numeroCompte = null;
            if( !string.IsNullOrWhiteSpace(compte) )
                numeroCompte = new Compte(compte);

            var line = new SourceLine(lineId, libelle, (decimal)montantHt, (decimal)montantTva, numeroCompte, lineDate, versementId, false);
        }

        [DataTestMethod]
        [DataRow(1, "TEST", 0D, 0D, "123456", null, null)]
        [DataRow(1, "TEST", 25D, 30D, "123456", "01/07/1980", 10)]
        [DataRow(0, "TEST", 25D, 30D, "123456", "01/07/1980", 10)]
        public void InputLineShouldBeOk(int lineId, string libelle, double montantHt, double montantTva, string compte, string dateVersement, int? versementId)
        {
            DateTime? lineDate = null;
            if (!string.IsNullOrWhiteSpace(dateVersement))
                lineDate = DateTime.Parse(dateVersement);

            Compte numeroCompte = null;
            if (!string.IsNullOrWhiteSpace(compte))
                numeroCompte = new Compte(compte);

            var line = new SourceLine(lineId, libelle, (decimal)montantHt, (decimal)montantTva, numeroCompte, lineDate, versementId, false);
            Assert.IsNotNull(line);
        }

        [DataTestMethod]
        [DataRow("","","",null, null)]
        [DataRow("01/07/1980","","",null, null)]
        [DataRow("01/07/1980","123456","",null, null)]
        [DataRow("01/07/1980","123456","TEST",10D, 10D)]
        [DataRow("01/07/1980","123456","",null, 10D)]
        [DataRow("01/07/1980","","TEST",null, 10D)]
        [DataRow("","123456","TEST",null, 10D)]
        [ExpectedException(typeof(OutputLineException), AllowDerivedTypes = true)]
        public void OutputLineShouldFail(string date, string compte, string libelle, double? debit, double? credit)
        {
            var lineDate = DateTime.MinValue;
            if(!string.IsNullOrWhiteSpace(date))
                lineDate = DateTime.Parse(date);

            Compte numeroCompte = null;
            if (!string.IsNullOrWhiteSpace(compte))
                numeroCompte = new Compte(compte);

            var line = new DestinationLine(lineDate, numeroCompte, libelle, (decimal?)debit, (decimal?)credit);
        }

        [DataTestMethod]
        [DataRow("01/07/1980", "123456", "TEST", null, 10D)]
        [DataRow("01/07/1980", "123456", "TEST", 10D, null)]
        public void OutputLineShouldBOk(string date, string compte, string libelle, double? debit, double? credit)
        {
            var lineDate = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(date))
                lineDate = DateTime.Parse(date);

            Compte numeroCompte = null;
            if (!string.IsNullOrWhiteSpace(compte))
                numeroCompte = new Compte(compte);

            var line = new DestinationLine(lineDate, numeroCompte, libelle, (decimal?)debit, (decimal?)credit);
            Assert.IsNotNull(line);
        }       
    }
}
