using System;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareDateTimeTests
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
        public void CompareDateTimeAFewMillisecondsOff()
        {
            DateTime date1 = DateTime.Now;
            DateTime date2 = date1.AddMilliseconds(2);

            ComparisonResult result = _compare.Compare(date1, date2);

            Assert.IsFalse(result.AreEqual);
        }


        [Test]
        public void CompareDateTimeAFewMillisecondsOffIgnore()
        {
            DateTime date1 = DateTime.Now;
            DateTime date2 = date1.AddMilliseconds(2);

            _compare.Config.MaxMillisecondsDateDifference = 100;
            ComparisonResult result = _compare.Compare(date1, date2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }
        #endregion
    }
}
