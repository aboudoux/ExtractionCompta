using System;
using System.Text;
using System.Collections.Generic;
using ExtractionCompta.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractionCompta.Tests
{
    [TestClass]
    public class VersementsTests
    {
        [TestMethod]
        [ExpectedException(typeof(EmptyVersementException))]
        public void VersementsWithEmptyListShouldFail() {
            var versements = new VersementsSorties(new List<SourceLine>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidVersementIdException))]
        public void MulptipleLinesWithDifferentVersementIdShouldFail() {
            var inputs = new List<SourceLine>()
            {
                new SourceLine(1, "TEST", 1, 1, new Compte("123456"), DateTime.Now, 1, false),
                new SourceLine(2, "TEST", 1, 1, new Compte("123456"), DateTime.Now, 2, false),
            };
            var versements = new VersementsSorties(inputs);
        }

        [TestMethod]
        public void MulptipleLinesWithSameVersementIdShouldBeOk() {
            var inputs = new List<SourceLine>()
            {
                new SourceLine(1, "TEST", 1, 1, new Compte("123456"), DateTime.Now, 1, false),
                new SourceLine(2, "TEST", 1, 1, new Compte("123456"), DateTime.Now, 1, false),
                new SourceLine(3, "TEST", 1, 1, new Compte("123456"), DateTime.Now, 1, false),
            };
            var versements = new VersementsSorties(inputs);
            versements.Should().NotBeNull();
                        
        }       
    }
}
