using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using Newtonsoft.Json;
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
            CompareLogic compareLogic = new()
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

#nullable enable

        // New feature: comparison of collections with indexers objects.
        //https://github.com/GregFinzer/Compare-Net-Objects/issues/223
        [Test]
        public void ListWithIndexerValuesShouldCompare()
        {
            // Arrange.
            var originalGraph = ListWithIndexerValues.Create();

            // Act.
            var serializedLists = JsonConvert.SerializeObject(originalGraph.ListOfLists.ToArray());

            var lists = JsonConvert.DeserializeObject<List<int>?[]>(serializedLists);

            var listOfLists = lists.ToList();
            var deserializedGraph = new ListWithIndexerValues(listOfLists);

            // Assert.
            Console.WriteLine("Manual Compare");
            originalGraph.ManualCompare(deserializedGraph);
            Console.WriteLine("Compare .NET Objects");
            originalGraph.CompareObjects(deserializedGraph);
        }

        // New feature: comparison of collections with indexers objects.
        //https://github.com/GregFinzer/Compare-Net-Objects/issues/223
        [Test]
        public void ArrayWithIndexerValuesShouldCompare()
        {
            // Arrange.
            var originalGraph = ArraysWithIndexerValues.Create();

            // Act.
            var serializedLists = JsonConvert.SerializeObject(originalGraph.ArrayOfLists.ToArray());

            var lists = JsonConvert.DeserializeObject<List<int>?[]>(serializedLists);

            var arrayOfLists = lists.ToArray();
            var deserializedGraph = new ArraysWithIndexerValues(arrayOfLists);

            // Assert.
            Console.WriteLine("Manual Compare");
            originalGraph.ManualCompare(deserializedGraph);
            Console.WriteLine("Compare .NET Objects");
            originalGraph.CompareObjects(deserializedGraph);
        }

        // New feature: comparison of collections with indexers objects.
        //https://github.com/GregFinzer/Compare-Net-Objects/issues/223
        [Test]
        public void DictionaryWithIndexerKeysAndValuesShouldCompare()
        {
            // Arrange.
            var originalGraph = DictionaryWithIndexerKeysAndValues.Create();

            // Act.
            var serializedKeys = JsonConvert.SerializeObject(originalGraph.DictOfLists.Keys.ToArray());
            var serializedValues = JsonConvert.SerializeObject(originalGraph.DictOfLists.Values.ToArray());

            var keys = JsonConvert.DeserializeObject<List<int>[]>(serializedKeys);
            var values = JsonConvert.DeserializeObject<List<int>[]>(serializedValues);

            var dictOfLists = keys.Zip(values, (x, y) => (x, y)).ToDictionary(pair => pair.x, pair => pair.y);
            var deserializedGraph = new DictionaryWithIndexerKeysAndValues(dictOfLists!);

            // Assert.
            Console.WriteLine("Manual Compare");
            //originalGraph.ManualCompare(deserializedGraph);
            Console.WriteLine("Compare .NET Objects");
            originalGraph.CompareObjects(deserializedGraph);
        }

#nullable disable

        #endregion
    }
}
