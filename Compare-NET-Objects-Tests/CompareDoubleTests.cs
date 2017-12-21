using System;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareDoubleTests
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
        public void CompareDoubleSlightlyOff()
        {
            Double double1 = 1.09;
            Double double2 = 1.009;
            
            _compare.Config.DoublePrecision = .01;
            ComparisonResult result = _compare.Compare(double1, double2);

            Assert.IsFalse(result.AreEqual);
        }


        [Test]
        public void CompareDoubleSlightlyOffIgnore()
        {
            Double double1 = 1.09;
            Double double2 = 1.009;

            _compare.Config.DoublePrecision = .1;
            ComparisonResult result = _compare.Compare(double1, double2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }
        #endregion
    }
}
