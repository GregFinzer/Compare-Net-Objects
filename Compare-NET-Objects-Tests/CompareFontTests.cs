using System;
using System.Drawing;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareFontTests
    {
        #region Class Variables
        private CompareLogic _compare;
        #endregion

        #region Setup/Teardown

        /// <summary>
        /// Code that is run once for a suite of tests
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {

        }

        /// <summary>
        /// Code that is run once after a suite of tests has finished executing
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {

        }

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
        public void FontPositiveTest()
        {
            Font font1 = new Font("Arial", 12);
            Font font2 = new Font("Arial", 12);

            ComparisonResult result = _compare.Compare(font1, font2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void FontNegativeTest()
        {
            Font font1 = new Font("Arial", 12);
            Font font2 = new Font("Arial", 14);

            ComparisonResult result = _compare.Compare(font1, font2);
            Assert.IsFalse(result.AreEqual);
        }


        #endregion
    }
}
