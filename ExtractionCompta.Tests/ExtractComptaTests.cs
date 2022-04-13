using System;
using ExtractionCompta.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractionCompta.Tests
{
    [TestClass]
    public class ExtractComptaTests
    {
        [TestMethod]
        public void UnSeulVersementEnSortieGenereUnVirementBancaire() 
        {
            var source = new MockSourceRepository();
            var output = new MockDestinationRepository();

            source.Sorties.Add(new SourceLine(1,"TEST", 10, 2, new Compte("123456"), new DateTime(2017,04,03), 1, false));

            new ExtractCompta(source, output).Execute();

            output.LinesBanque.Should().HaveCount(3)
                .And.Contain(a => a.Compte.Valeur == "123456" && a.Debit == 10 && a.Libelle == "TEST")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 2 && a.Libelle == "TEST")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 12 && a.Libelle == "TEST");

            output.LinesOD.Should().BeEmpty();
        }

        [TestMethod]
        public void UnSeulVersementEnSortieMarqueEnCcaGenereUnVirementCca() 
        {
	        var source = new MockSourceRepository();
	        var output = new MockDestinationRepository();


	        source.Sorties.Add(new SourceLine(1, "TEST", 10, 2, new Compte("123456"), new DateTime(2017, 04, 03), 1, true));

	        new ExtractCompta(source, output).Execute();

	        output.LinesBanque.Should().HaveCount(2)
		        .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Debit == 12 && a.Libelle == Libelle.Virement)
		        .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 12 && a.Libelle == Libelle.Virement);

	        output.LinesOD.Should().HaveCount(3)
		        .And.Contain(a => a.Compte.Valeur == "123456" && a.Debit == 10 && a.Libelle == "TEST")
		        .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 2 && a.Libelle == "TEST")
		        .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Credit == 12 && a.Libelle == "TEST");
        }

        [TestMethod]
        public void PlusieurVersementEnSortieDontUnMarqueCcaGeneresCompteCourantPLusUnVirement()
        {
            var source = new MockSourceRepository();
            var output = new MockDestinationRepository();

            source.Sorties.Add(new SourceLine(1, "TEST 1", 10, 2, new Compte("111111"), new DateTime(2017, 04, 05), 1, true));
            source.Sorties.Add(new SourceLine(1, "TEST 2", 5, 1, new Compte("222222"), new DateTime(2017, 04, 05), 1, false));

            new ExtractCompta(source, output).Execute();

            output.LinesOD.Should().HaveCount(6)
                .And.Contain(a => a.Compte.Valeur == "111111" && a.Debit == 10 && a.Libelle == "TEST 1")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 2 && a.Libelle == "TEST 1")
                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Credit == 12 && a.Libelle == "TEST 1")

                .And.Contain(a => a.Compte.Valeur == "222222" && a.Debit == 5 && a.Libelle == "TEST 2")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 1 && a.Libelle == "TEST 2")
                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Credit == 6 && a.Libelle == "TEST 2");

            output.LinesBanque.Should().HaveCount(2)
                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Debit== 18 && a.Libelle == Libelle.Virement)
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 18 && a.Libelle == Libelle.Virement);
        }

        [TestMethod]
        public void PlusieurVersementEnSortieDontGenerespPlusieursUnVirement() {
	        var source = new MockSourceRepository();
	        var output = new MockDestinationRepository();

	        source.Sorties.Add(new SourceLine(1, "TEST 1", 10, 2, new Compte("111111"), new DateTime(2017, 04, 05), 1, false));
	        source.Sorties.Add(new SourceLine(1, "TEST 2", 5, 1, new Compte("222222"), new DateTime(2017, 04, 05), 1, false));

	        new ExtractCompta(source, output).Execute();

	        output.LinesOD.Should().BeEmpty();

            output.LinesBanque.Should().HaveCount(5)
		        .And.Contain(a => a.Compte.Valeur == "111111" && a.Debit == 10 && a.Libelle == "TEST 1")
		        .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 2 && a.Libelle == "TEST 1")

		        .And.Contain(a => a.Compte.Valeur == "222222" && a.Debit == 5 && a.Libelle == "TEST 2")
		        .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 1 && a.Libelle == "TEST 2")

		        .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 18 && a.Libelle == Libelle.Virement);
        }

        [TestMethod]
        public void VersementSimpleEtVersementMultipleAvecCca()
        {
            var source = new MockSourceRepository();
            var destination = new MockDestinationRepository();

            source.Sorties.Add(new SourceLine(1, "TEST 1", 1, 1, new Compte("111111"), new DateTime(2017, 04, 05), 1, false));
            source.Sorties.Add(new SourceLine(2, "TEST 2", 2, 2, new Compte("222222"), new DateTime(2017, 04, 05), 2, true));
            source.Sorties.Add(new SourceLine(3, "TEST 3", 3, 3, new Compte("333333"), new DateTime(2017, 04, 05), 2, false));
            source.Sorties.Add(new SourceLine(4, "TEST 4", 4, 4, new Compte("444444"), new DateTime(2017, 04, 05), 3, true));
            source.Sorties.Add(new SourceLine(5, "TEST 5", 5, 5, new Compte("555555"), new DateTime(2017, 04, 05), 3, false));
            source.Sorties.Add(new SourceLine(6, "TEST 6", 6, 6, new Compte("666666"), new DateTime(2017, 04, 05), 4, false));
            source.Sorties.Add(new SourceLine(7, "TEST 7", 7, 0, new Compte("777777"), new DateTime(2017, 04, 05), 5, false));

            new ExtractCompta(source, destination).Execute();
            
            destination.LinesBanque.Should().HaveCount(12)
                .And.Contain(a => a.Compte.Valeur == "111111" && a.Debit == 1 && a.Libelle == "TEST 1")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 1 && a.Libelle == "TEST 1")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 2 && a.Libelle == "TEST 1")                                

                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Debit == 10 && a.Libelle == Libelle.Virement)
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 10 && a.Libelle == Libelle.Virement)

                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Debit == 18 && a.Libelle == Libelle.Virement)
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 18 && a.Libelle == Libelle.Virement)

                .And.Contain(a => a.Compte.Valeur == "666666" && a.Debit == 6 && a.Libelle == "TEST 6")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 6 && a.Libelle == "TEST 6")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 12 && a.Libelle == "TEST 6")

                .And.Contain(a => a.Compte.Valeur == "777777" && a.Debit == 7 && a.Libelle == "TEST 7")
	            .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 7 && a.Libelle == "TEST 7");

            destination.LinesOD.Should().HaveCount(12)
                .And.Contain(a => a.Compte.Valeur == "222222" && a.Debit == 2 && a.Libelle == "TEST 2")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 2 && a.Libelle == "TEST 2")
                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Credit == 4 && a.Libelle == "TEST 2")
                .And.Contain(a => a.Compte.Valeur == "333333" && a.Debit == 3 && a.Libelle == "TEST 3")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 3 && a.Libelle == "TEST 3")
                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Credit == 6 && a.Libelle == "TEST 3")

                .And.Contain(a => a.Compte.Valeur == "444444" && a.Debit == 4 && a.Libelle == "TEST 4")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 4 && a.Libelle == "TEST 4")
                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Credit == 8 && a.Libelle == "TEST 4")
                .And.Contain(a => a.Compte.Valeur == "555555" && a.Debit == 5 && a.Libelle == "TEST 5")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 5 && a.Libelle == "TEST 5")
                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Credit == 10 && a.Libelle == "TEST 5");
        }

        [TestMethod]
        public void VersementSimpleEtVersementMultipleSansCca() {
            var source = new MockSourceRepository();
            var destination = new MockDestinationRepository();

            source.Sorties.Add(new SourceLine(1, "TEST 1", 1, 1, new Compte("111111"), new DateTime(2017, 04, 05), 1, false));
            source.Sorties.Add(new SourceLine(2, "TEST 2", 2, 2, new Compte("222222"), new DateTime(2017, 04, 05), 2, false));
            source.Sorties.Add(new SourceLine(3, "TEST 3", 3, 3, new Compte("333333"), new DateTime(2017, 04, 05), 2, false));
            source.Sorties.Add(new SourceLine(4, "TEST 4", 4, 4, new Compte("444444"), new DateTime(2017, 04, 05), 3, false));
            source.Sorties.Add(new SourceLine(5, "TEST 5", 5, 5, new Compte("555555"), new DateTime(2017, 04, 05), 3, false));
            source.Sorties.Add(new SourceLine(6, "TEST 6", 6, 6, new Compte("666666"), new DateTime(2017, 04, 05), 4, false));
            source.Sorties.Add(new SourceLine(7, "TEST 7", 7, 0, new Compte("777777"), new DateTime(2017, 04, 05), 5, false));

            new ExtractCompta(source, destination).Execute();

            destination.LinesBanque.Should().HaveCount(18)
	            .And.Contain(a => a.Compte.Valeur == "111111" && a.Debit == 1 && a.Libelle == "TEST 1")
	            .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 1 && a.Libelle == "TEST 1")
	            .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 2 && a.Libelle == "TEST 1")

	            .And.Contain(a => a.Compte.Valeur == "222222" && a.Debit == 2 && a.Libelle == "TEST 2")
	            .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 2 && a.Libelle == "TEST 2")
	            .And.Contain(a => a.Compte.Valeur == "333333" && a.Debit == 3 && a.Libelle == "TEST 3")
	            .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 3 && a.Libelle == "TEST 3")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 10 && a.Libelle == Libelle.Virement)

	            .And.Contain(a => a.Compte.Valeur == "444444" && a.Debit == 4 && a.Libelle == "TEST 4")
	            .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 4 && a.Libelle == "TEST 4")
	            .And.Contain(a => a.Compte.Valeur == "555555" && a.Debit == 5 && a.Libelle == "TEST 5")
	            .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 5 && a.Libelle == "TEST 5")
	            .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 18 && a.Libelle == Libelle.Virement)

	            .And.Contain(a => a.Compte.Valeur == "666666" && a.Debit == 6 && a.Libelle == "TEST 6")
	            .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 6 && a.Libelle == "TEST 6")
	            .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 12 && a.Libelle == "TEST 6")

	            .And.Contain(a => a.Compte.Valeur == "777777" && a.Debit == 7 && a.Libelle == "TEST 7")
	            .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 7 && a.Libelle == "TEST 7");
        }

        [TestMethod]

        public void VersementEntreeSimple()
        {
            var source = new MockSourceRepository();
            var destination = new MockDestinationRepository();


            source.Entrees.Add(new SourceLine(1, "Capital social", 1000, 0, new Compte("101200"), new DateTime(2016, 10, 07), 1, false));
            source.Entrees.Add(new SourceLine(2, "Facture apollo octobre 2016", 4680, 936, new Compte("622600"), new DateTime(2016, 12, 02), 2, false));

            new ExtractCompta(source, destination).Execute();

            destination.LinesBanque.Should().HaveCount(5)
                .And.Contain(a => a.Compte.Valeur == "101200" && a.Credit == 1000 && a.Libelle == "Capital social")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Debit == 1000 && a.Libelle == "Capital social")

                .And.Contain(a => a.Compte.Valeur == "622600" && a.Credit == 4680 && a.Libelle == "Facture apollo octobre 2016")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA_COLLECTEE && a.Credit == 936 && a.Libelle == "Facture apollo octobre 2016")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Debit == 5616 && a.Libelle == "Facture apollo octobre 2016");
        }

        [TestMethod]
        public void VersementEntreePlusSortiesAvecCca()
        {
            var source = new MockSourceRepository();
            var destination = new MockDestinationRepository();

            source.Sorties.Add(new SourceLine(1, "TEST 1", 1, 1, new Compte("111111"), new DateTime(2016, 11, 01), 1, false));
            source.Sorties.Add(new SourceLine(2, "TEST 2", 2, 2, new Compte("222222"), new DateTime(2017, 04, 05), 2, true));
            source.Sorties.Add(new SourceLine(3, "TEST 3", 3, 3, new Compte("333333"), new DateTime(2017, 04, 05), 2, false));

            source.Entrees.Add(new SourceLine(1, "Capital social", 1000, 0, new Compte("101200"), new DateTime(2016, 10, 07), 1, false));
            source.Entrees.Add(new SourceLine(2, "Facture apollo octobre 2016", 4680, 936, new Compte("622600"), new DateTime(2017, 01, 01), 2, false));

            new ExtractCompta(source, destination).Execute();

            destination.LinesBanque.Should().HaveCount(10)
                .And.Contain(a => a.Compte.Valeur == "111111" && a.Debit == 1 && a.Libelle == "TEST 1")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 1 && a.Libelle == "TEST 1")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 2 && a.Libelle == "TEST 1")                

                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Debit == 10 && a.Libelle == Libelle.Virement)
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 10 && a.Libelle == Libelle.Virement)

                .And.Contain(a => a.Compte.Valeur == "101200" && a.Credit == 1000 && a.Libelle == "Capital social")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Debit == 1000 && a.Libelle == "Capital social")

                .And.Contain(a => a.Compte.Valeur == "622600" && a.Credit == 4680 && a.Libelle == "Facture apollo octobre 2016")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA_COLLECTEE && a.Credit == 936 && a.Libelle == "Facture apollo octobre 2016")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Debit == 5616 && a.Libelle == "Facture apollo octobre 2016");

            destination.LinesOD.Should().HaveCount(6)
                .And.Contain(a => a.Compte.Valeur == "222222" && a.Debit == 2 && a.Libelle == "TEST 2")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 2 && a.Libelle == "TEST 2")
                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Credit == 4 && a.Libelle == "TEST 2")
                .And.Contain(a => a.Compte.Valeur == "333333" && a.Debit == 3 && a.Libelle == "TEST 3")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 3 && a.Libelle == "TEST 3")
                .And.Contain(a => a.Compte.Valeur == Compte.COMPTE_COURANT && a.Credit == 6 && a.Libelle == "TEST 3");

            destination.LinesOD[0].Libelle.Should().Be("TEST 2");
            destination.LinesOD[1].Libelle.Should().Be("TEST 2");
            destination.LinesOD[2].Libelle.Should().Be("TEST 2");
            destination.LinesOD[3].Libelle.Should().Be("TEST 3");
            destination.LinesOD[4].Libelle.Should().Be("TEST 3");
            destination.LinesOD[5].Libelle.Should().Be("TEST 3");

            destination.LinesBanque[0].Libelle.Should().Be("Capital social");
            destination.LinesBanque[1].Libelle.Should().Be("Capital social");

            destination.LinesBanque[2].Libelle.Should().Be("TEST 1");
            destination.LinesBanque[3].Libelle.Should().Be("TEST 1");
            destination.LinesBanque[4].Libelle.Should().Be("TEST 1");

            destination.LinesBanque[5].Libelle.Should().Be("Facture apollo octobre 2016");
            destination.LinesBanque[6].Libelle.Should().Be("Facture apollo octobre 2016");
            destination.LinesBanque[7].Libelle.Should().Be("Facture apollo octobre 2016");

            
            destination.LinesBanque[8].Libelle.Should().Be(Libelle.Virement);
            destination.LinesBanque[9].Libelle.Should().Be(Libelle.Virement);
        }

        [TestMethod]
        public void VersementEntreePlusSortiesSansCca() {
            var source = new MockSourceRepository();
            var destination = new MockDestinationRepository();

            source.Sorties.Add(new SourceLine(1, "TEST 1", 1, 1, new Compte("111111"), new DateTime(2016, 11, 01), 1, false));
            source.Sorties.Add(new SourceLine(2, "TEST 2", 2, 2, new Compte("222222"), new DateTime(2017, 04, 05), 2, false));
            source.Sorties.Add(new SourceLine(3, "TEST 3", 3, 3, new Compte("333333"), new DateTime(2017, 04, 05), 2, false));

            source.Entrees.Add(new SourceLine(1, "Capital social", 1000, 0, new Compte("101200"), new DateTime(2016, 10, 07), 1, false));
            source.Entrees.Add(new SourceLine(2, "Facture apollo octobre 2016", 4680, 936, new Compte("622600"), new DateTime(2017, 01, 01), 2, false));

            new ExtractCompta(source, destination).Execute();

            destination.LinesBanque.Should().HaveCount(13)
                .And.Contain(a => a.Compte.Valeur == "111111" && a.Debit == 1 && a.Libelle == "TEST 1")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 1 && a.Libelle == "TEST 1")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 2 && a.Libelle == "TEST 1")

                .And.Contain(a => a.Compte.Valeur == "222222" && a.Debit == 2 && a.Libelle == "TEST 2")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 2 && a.Libelle == "TEST 2")
                .And.Contain(a => a.Compte.Valeur == "333333" && a.Debit == 3 && a.Libelle == "TEST 3")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA && a.Debit == 3 && a.Libelle == "TEST 3")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Credit == 10 && a.Libelle == Libelle.Virement)

                .And.Contain(a => a.Compte.Valeur == "101200" && a.Credit == 1000 && a.Libelle == "Capital social")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Debit == 1000 && a.Libelle == "Capital social")

                .And.Contain(a => a.Compte.Valeur == "622600" && a.Credit == 4680 && a.Libelle == "Facture apollo octobre 2016")
                .And.Contain(a => a.Compte.Valeur == Compte.TVA_COLLECTEE && a.Credit == 936 && a.Libelle == "Facture apollo octobre 2016")
                .And.Contain(a => a.Compte.Valeur == Compte.BANQUE && a.Debit == 5616 && a.Libelle == "Facture apollo octobre 2016");

            
            destination.LinesBanque[0].Libelle.Should().Be("Capital social");
            destination.LinesBanque[1].Libelle.Should().Be("Capital social");

            destination.LinesBanque[2].Libelle.Should().Be("TEST 1");
            destination.LinesBanque[3].Libelle.Should().Be("TEST 1");
            destination.LinesBanque[4].Libelle.Should().Be("TEST 1");

            destination.LinesBanque[5].Libelle.Should().Be("Facture apollo octobre 2016");
            destination.LinesBanque[6].Libelle.Should().Be("Facture apollo octobre 2016");
            destination.LinesBanque[7].Libelle.Should().Be("Facture apollo octobre 2016");
        }
    }
}
