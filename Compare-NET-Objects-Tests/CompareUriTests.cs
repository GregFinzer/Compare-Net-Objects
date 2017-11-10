using System;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareUriTests
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
        public void UriPostive()
        {
            Uri uri1 = new Uri("http://www.kellermansoftware.com");
            Uri uri2 = new Uri("http://www.kellermansoftware.com");

            ComparisonResult result = _compare.Compare(uri1, uri2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);

        }

        [Test]
        public void UriNegative()
        {
            Uri uri1 = new Uri("http://www.kellermansoftware.com");
            Uri uri2 = new Uri("http://www.codeplex.com");
            Assert.IsFalse(_compare.Compare(uri1, uri2).AreEqual);
        }
        #endregion
    }
}
