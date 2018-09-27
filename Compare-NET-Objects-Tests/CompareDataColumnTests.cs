#if !NETSTANDARD

using System.Data;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareDataColumnTests
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
        public void DataColumnPositiveTest()
        {
            var dc1 = new DataColumn("Flavor", typeof(string));
            var dc2 = new DataColumn("Flavor", typeof(string));
            Assert.IsTrue(_compare.Compare(dc1, dc2).AreEqual);
        }

        [Test]
        public void DataColumnMismatchedNameTest()
        {
            var dc1 = new DataColumn("Flavor", typeof(string));
            var dc2 = new DataColumn("Flavour", typeof(string));
            var result = _compare.Compare(dc1, dc2);
            Assert.IsFalse(result.AreEqual);

            Assert.AreEqual(1, result.Differences.Count);

            Assert.AreEqual("Flavor", result.Differences[0].Object1Value);
            Assert.AreEqual("Flavour", result.Differences[0].Object2Value);
        }

        [Test]
        public void DataColumnMismatchedTypeTest()
        {
            var dc1 = new DataColumn("Price", typeof(string));
            var dc2 = new DataColumn("Price", typeof(decimal));
            var result = _compare.Compare(dc1, dc2);
            Assert.IsFalse(result.AreEqual);

            Assert.AreEqual(1, result.Differences.Count);

            Assert.AreEqual("System.String", result.Differences[0].Object1Value);
            Assert.AreEqual("System.Decimal", result.Differences[0].Object2Value);
        }
        #endregion
    }
}

#endif
