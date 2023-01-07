#if NET6_0_OR_GREATER

using System;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareTimeOnlyTests
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
        public void CompareTimeOnly_SameTimes_ReturnsTrue()
        {
            var time1 = new TimeOnly(12, 0, 0);
            var time2 = new TimeOnly(12, 0, 0);

            var result = _compare.Compare(time1, time2);

            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void CompareTimeOnly_DifferentTimes_ReturnsFalse()
        {
            TimeOnly time1 = new TimeOnly(12, 0, 0);
            TimeOnly time2 = new TimeOnly(12, 0, 1);

            var result = _compare.Compare(time1, time2);

            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void CompareTimeOnly_DifferentTypes_ThrowsException()
        {
            var time = new TimeOnly(12, 0, 0);
            var dateTime = DateTime.Now;

            Assert.Throws
            (
                typeof(NotSupportedException),
                () => _compare.Compare(time, dateTime),
                $"Cannot compare object of type {nameof(DateTime)}"
            );
        }
        #endregion
    }
}

#endif