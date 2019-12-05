using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    internal class TestClass
    {
        public decimal? DecimalValue { get; set; }
    }

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

        [TestCase("1.1", "1.10", 0, true)]
        [TestCase("1.11", "1.10", 0, false)]
        [TestCase("1.11", "1.10", 0.1, true)]
        [TestCase("1.2", "1.1", 0.1, false)]
        [TestCase("1.10", "1.1", 0.01, true)]
        [TestCase("1.10", "1.11", 0.001, false)]
        [TestCase("1.100", "1.110", 0.001, false)]
        [TestCase("1.110", "1.11", 0.001, true)]
        public void CompareDecimalNullableInComplexCollectionTest(decimal? value1, decimal? value2, decimal precision, bool expectedResult)
        {
            if (precision > 0)
            {
                _compare.Config.IgnoreCollectionOrder = true;
                _compare.Config.DecimalPrecision = precision;
            }

            var test1 = new[] { new TestClass { DecimalValue = value1 } };
            var test2 = new[] { new TestClass { DecimalValue = value2 } };

            var result = _compare.Compare(test1, test2);
            Assert.That(result.AreEqual, Is.EqualTo(expectedResult));
        }
    }
}