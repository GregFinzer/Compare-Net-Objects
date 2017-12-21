using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class ComparisonResultTests
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
        public void ParentPropertyNameArrayTest()
        {
            Difference diff = new Difference();
            diff.PropertyName = ".Password[4]";
            Assert.AreEqual(".Password", diff.ParentPropertyName);
        }

        [Test]
        public void ParentPropertyNameChildTest()
        {
            Difference diff = new Difference();
            diff.PropertyName = ".Customer.FirstName";
            Assert.AreEqual(".Customer", diff.ParentPropertyName);
        }
        #endregion

    }
}
