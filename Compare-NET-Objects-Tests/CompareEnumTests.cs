using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareEnumTests
    {
        #region Class Variables
        private CompareLogic _compare;
        #endregion

        #region Setup/Teardown



        /// <summary>
        /// Code that is run before each test
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            _compare = new CompareLogic();
        }

        /// <summary>
        /// Code that is run after each test
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            _compare = null;
        }
        #endregion

        #region Tests

        [Test]
        public void EnumTypeShownTest()
        {
            Officer officer1 = new Officer();
            officer1.Name = "Greg";
            officer1.Type = Deck.Engineering;

            Officer officer2 = new Officer();
            officer2.Name = "John";
            officer2.Type = Deck.AstroPhysics;

            _compare.Config.MaxDifferences = 2;

            ComparisonResult result = _compare.Compare(officer1, officer2);
            if (!result.AreEqual)
            {
                Console.WriteLine(result.DifferencesString);
            }

            Assert.IsTrue(result.DifferencesString.Contains("Deck"));
        }

        [Test]
        public void TestEnumeration()
        {
            List<Deck> list1 = new List<Deck>();
            list1.Add(Deck.Engineering);
            list1.Add(Deck.SickBay);

            List<Deck> list2 = new List<Deck>();
            list2.Add(Deck.Engineering);
            list2.Add(Deck.SickBay);

            ComparisonResult result = _compare.Compare(list1, list2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void TestEnumerationNegative()
        {
            List<Deck> list1 = new List<Deck>();
            list1.Add(Deck.Engineering);
            list1.Add(Deck.SickBay);

            List<Deck> list2 = new List<Deck>();
            list2.Add(Deck.Engineering);
            list2.Add(Deck.AstroPhysics);

            ComparisonResult result = _compare.Compare(list1, list2);
            Assert.IsFalse(result.AreEqual);
            Console.WriteLine(result.DifferencesString);
        }

        [Test]
        public void TestNullableEnumeration()
        {
            List<Deck?> list1 = new List<Deck?>();
            list1.Add(Deck.Engineering);
            list1.Add(null);

            List<Deck?> list2 = new List<Deck?>();
            list2.Add(Deck.Engineering);
            list2.Add(null);

            ComparisonResult result = _compare.Compare(list1, list2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void TestNullableEnumerationNegative()
        {
            List<Deck?> list1 = new List<Deck?>();
            list1.Add(Deck.Engineering);
            list1.Add(null);

            List<Deck?> list2 = new List<Deck?>();
            list2.Add(Deck.Engineering);
            list2.Add(Deck.AstroPhysics);

            Assert.IsFalse(_compare.Compare(list1, list2).AreEqual);
        }
        #endregion
    }
}
