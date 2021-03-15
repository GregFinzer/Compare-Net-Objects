using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareIndexerTests
    {
        #region Tests

        [Test]
        public void TestIndexerPositive()
        {
            // Arrange.
            var jane = new Person { Name = "Jane" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, jack, mary };
            var nameList2 = new List<Person>() { jane, jack, mary };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            var compare = new CompareLogic();

            // Act.
            ComparisonResult comparisonResult = compare.Compare(class1, class2);

            // Assert.
            Assert.IsTrue(comparisonResult.AreEqual);
        }

        [Test]
        public void TestIndexerNegative()
        {
            // Arrange.
            var jane = new Person { Name = "Jane" };
            var john = new Person { Name = "John" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, jack, mary };
            var nameList2 = new List<Person>() { jane, john, jack };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            var compare = new CompareLogic();

            // Act.
            ComparisonResult comparisonResult = compare.Compare(class1, class2);

            // Assert.
            Assert.IsFalse(comparisonResult.AreEqual);
        }

        [Test]
        public void TestIndexerLengthNegative()
        {
            // Arrange.
            var jane = new Person { Name = "Jane" };
            var john = new Person { Name = "John" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, john, jack, mary };
            var nameList2 = new List<Person>() { jane, john, jack };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            var compare = new CompareLogic();
            compare.Config.MaxDifferences = int.MaxValue;

            // Act.
            ComparisonResult result12 = compare.Compare(class1, class2);
            ComparisonResult result21 = compare.Compare(class2, class1);

            // Assert.
            Assert.AreEqual(result12.Differences.Count, 3);
            Assert.AreEqual(result21.Differences.Count, 3);
        }

        [Test]
        public void ObjectWithIndexerShouldCompare()
        {
            // Arrange.
            var values = new[] { 1, 2, 3, 4, 5 };
            var indexedObject = new IndexerObject(values);

            // Act.
            var anotherIndexedObject = new IndexerObject(values);

            // Assert.
            CompareLogic compareLogic = new CompareLogic
            {
                Config =
                {
                    ComparePrivateProperties = false,
                    IgnoreObjectTypes = false,
                    IgnoreCollectionOrder = true
                }
            };
            var result = compareLogic.Compare(indexedObject, anotherIndexedObject);
            if (!result.AreEqual)
            {
                Assert.Fail(result.DifferencesString);
            }
        }

        #endregion
    }
}
