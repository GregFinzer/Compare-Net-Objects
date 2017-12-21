using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareTimespan
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

        #region Test Timespan
        [Test]
        public void TestTimespan()
        {
            TimeSpan ts1 = DateTime.Now - DateTime.Now.AddMinutes(-61);
            TimeSpan ts2 = DateTime.Now - DateTime.Now.AddHours(-49);

            List<TimeSpan> list1 = new List<TimeSpan>();
            list1.Add(ts1);
            list1.Add(ts2);

            List<TimeSpan> list2 = new List<TimeSpan>();
            list2.Add(ts1);
            list2.Add(ts2);

            ComparisonResult result = _compare.Compare(list1, list2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void TestTimeSpanNegative()
        {
            TimeSpan ts1 = DateTime.Now - DateTime.Now.AddMinutes(-61);
            TimeSpan ts2 = DateTime.Now - DateTime.Now.AddHours(-49);
            TimeSpan ts3 = DateTime.Now - DateTime.Now.AddHours(-48);

            List<TimeSpan> list1 = new List<TimeSpan>();
            list1.Add(ts1);
            list1.Add(ts2);

            List<TimeSpan> list2 = new List<TimeSpan>();
            list2.Add(ts1);
            list2.Add(ts3);

            Assert.IsFalse(_compare.Compare(list1, list2).AreEqual);
        }
        #endregion
    }
}
