using System;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareDateTimeOffsets
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
        public void DateTimeOffsetEqual()
        {
            DateTimeOffset offset1 = new DateTimeOffset(DateTime.Now);
            DateTimeOffset offset2 = new DateTimeOffset(offset1.DateTime);

            ComparisonResult result = _compare.Compare(offset1, offset2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void DateTimeOffsetNotEqual()
        {
            DateTimeOffset offset1 = new DateTimeOffset(DateTime.Now);
            DateTimeOffset offset2 = new DateTimeOffset(DateTime.Now.AddSeconds(10));

            ComparisonResult result = _compare.Compare(offset1, offset2);
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void CompareDateTimeAFewMillisecondsOff()
        {
            DateTimeOffset date1 = new DateTimeOffset(DateTime.Now);
            DateTimeOffset date2 = new DateTimeOffset(DateTime.Now.AddMilliseconds(2));

            ComparisonResult result = _compare.Compare(date1, date2);

            Assert.IsFalse(result.AreEqual);
        }


        [Test]
        public void CompareDateTimeAFewMillisecondsOffIgnore()
        {
            DateTimeOffset date1 = new DateTimeOffset(DateTime.Now);
            DateTimeOffset date2 = new DateTimeOffset(DateTime.Now.AddMilliseconds(2));

            _compare.Config.MaxMillisecondsDateDifference = 100;
            ComparisonResult result = _compare.Compare(date1, date2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }
        #endregion
    }
}
