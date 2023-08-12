#if NET6_0

using System;
using KellermanSoftware.CompareNetObjects;
using Microsoft.OData.Edm;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareDateOnlyTests
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
        public void CompareDateOnly_SameDates_ReturnsTrue()
        {
            var date1 = new DateOnly(2021, 1, 1);
            var date2 = new DateOnly(2021, 1, 1);

            var result = _compare.Compare(date1, date2);

            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void CompareDateOnly_DifferentDates_ReturnsFalse()
        {
            DateOnly date1 = new DateOnly(2021, 1, 1);
            DateOnly date2 = new DateOnly(2021, 1, 2);

            var result = _compare.Compare(date1, date2);

            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void CompareDateOnly_DifferentTypes_ShouldBeDifferent()
        {
            var date = new DateOnly(2021, 1, 1);
            var dateTime = date.ToDateTime(new());
            var result = _compare.Compare(date, dateTime);

            Assert.IsFalse(result.AreEqual);
            Assert.IsTrue(result.DifferencesString.Contains("Different Types"));

        }
#endregion
    }
}

#endif