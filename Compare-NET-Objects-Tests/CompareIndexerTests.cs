using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareIndexerTests
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
        public void TestIndexerPositive()
        {
            var jane = new Person { Name = "Jane" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, jack, mary };
            var nameList2 = new List<Person>() { jane, jack, mary };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            Assert.IsTrue(_compare.Compare(class1, class2).AreEqual);
        }

        [Test]
        public void TestIndexerNegative()
        {
            var jane = new Person { Name = "Jane" };
            var john = new Person { Name = "John" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, jack, mary };
            var nameList2 = new List<Person>() { jane, john, jack };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            Assert.IsFalse(_compare.Compare(class1, class2).AreEqual);
        }



        [Test]
        public void TestIndexerLengthNegative()
        {
            var jane = new Person { Name = "Jane" };
            var john = new Person { Name = "John" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, john, jack, mary };
            var nameList2 = new List<Person>() { jane, john, jack };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            var prior = _compare.Config.MaxDifferences;
            _compare.Config.MaxDifferences = int.MaxValue;

            ComparisonResult result = _compare.Compare(class1, class2);
            Assert.AreEqual(result.Differences.Count, 3);

            result = _compare.Compare(class2, class1);
            Assert.AreEqual(result.Differences.Count, 3);

            _compare.Config.MaxDifferences = prior;
        }
        #endregion
    }
}
