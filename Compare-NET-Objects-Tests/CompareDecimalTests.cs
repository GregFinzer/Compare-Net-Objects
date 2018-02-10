using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareDecimalTests
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

        [TestCase(10, 10, 0, true)]
        [TestCase(10, 20, 0, false)]
        [TestCase(10, 10.01, 0, false)]
        [TestCase(1, 1.09, 0.1, true)]
        [TestCase(1.08, 1.09, 0.01, true)]
        [TestCase(1.08, 1.09, 0.001, false)]
        public void CompareTest(decimal value1, decimal value2, decimal precision, bool expectedResult)
        {
            if (precision > 0)
            {
                _compare.Config.DecimalPrecision = precision;
            }

            var result = _compare.Compare(value1, value2);
            Assert.AreEqual(expectedResult, result.AreEqual);
        }
    }
}
