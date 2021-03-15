using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.Indexers
{
    internal sealed class EnumerableOfSomething
    {
        public IEnumerable<IEnumerable<int>?> Value { get; }

        public EnumerableOfSomething(IEnumerable<IEnumerable<int>?> value)
        {
            Value = value.ToTrueEnumerable();
        }

        public static EnumerableOfSomething CreateWithLists()
        {
            var enumerableOfLists = PrepareEnumerableWithLists();
            return new EnumerableOfSomething(enumerableOfLists);
        }

        public static EnumerableOfSomething CreateWithArrays()
        {
            var enumerableOfArrays = PrepareEnumerableWithLists()
                .Select(x => x?.ToArray())
                .ToArray();
            return new EnumerableOfSomething(enumerableOfArrays);
        }

        public static EnumerableOfSomething CreateWithEnumerables()
        {
            var enumerableOfArrays = PrepareEnumerableWithLists()
                .Select(x => x?.ToArray().ToTrueEnumerable())
                .ToArray();
            return new EnumerableOfSomething(enumerableOfArrays);
        }

        private static IEnumerable<IEnumerable<int>?> PrepareEnumerableWithLists()
        {
            var listOfLists = new List<List<int>?>(6);
            for (int i = 0; i < listOfLists.Capacity; ++i)
            {
                listOfLists.Add(i % 3 == 0 ? null : new List<int>(Enumerable.Range(0, i + 1)));
            }

            return listOfLists;
        }

        public void ManualCompare(EnumerableOfSomething? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }

            var values1 = Value.ToArray();
            var values2 = other.Value.ToArray();
            Assert.AreEqual(values1.Length, values2.Length);
            for (int i = 0; i < values1.Length; ++i)
            {
                var ch1 = values1[i];
                var ch2 = values2[i];
                CollectionAssert.AreEqual(ch1, ch2);
            }
        }

        public void CompareObjects(EnumerableOfSomething? other, bool expected)
        {
            CompareLogic compareLogic = new CompareLogic
            {
                Config =
                {
                    ComparePrivateProperties = false,
                    IgnoreObjectTypes = false,
                    IgnoreCollectionOrder = true // Should be true
                }
            };
            var result = compareLogic.Compare(this, other);
            if (expected && !result.AreEqual)
            {
                Assert.Fail(result.DifferencesString);
            }
            else if (!expected && result.AreEqual)
            {
                Assert.Fail("Expected that objects are not equal.");
            }
        }
    }
}
