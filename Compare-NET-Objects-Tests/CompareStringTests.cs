using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareStringTests
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
        public void CompareStringEmptyAndNullDifferent()
        {
            Assert.IsFalse(_compare.Compare(string.Empty, null).AreEqual);
        }

        [Test]
        public void CompareStringEmptyAndNullEqual()
        {
            _compare.Config.TreatStringEmptyAndNullTheSame = true;
            Assert.IsTrue(_compare.Compare(string.Empty, null).AreEqual);
            _compare.Config.Reset();
        }
        #endregion
    }
}
