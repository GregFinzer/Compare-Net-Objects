using System;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class PointComparerTests
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
        public void TestIntPtr()
        {
            Assert.IsTrue(_compare.Compare((IntPtr)1, (IntPtr)1).AreEqual);
            Assert.IsFalse(_compare.Compare((IntPtr)1, (IntPtr)2).AreEqual);
        }

        [Test]
        public void TestUIntPtr()
        {
            Assert.IsTrue(_compare.Compare((UIntPtr)1, (UIntPtr)1).AreEqual);
            Assert.IsFalse(_compare.Compare((UIntPtr)1, (UIntPtr)2).AreEqual);
        }
        #endregion
    }
}
