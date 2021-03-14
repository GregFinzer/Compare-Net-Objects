using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class EnumerableWithIndexerValues
    {
        public IEnumerable<List<int>?> EnumerableOfLists { get; }

        public EnumerableWithIndexerValues(IEnumerable<List<int>?> enumerableOfLists)
        {
            EnumerableOfLists = enumerableOfLists.Select(x => x); // Ensure that type will be enumerable.
        }

        public static EnumerableWithIndexerValues Create()
        {
            var listOfLists = PrepareEnumerable();
            return new EnumerableWithIndexerValues(listOfLists);
        }

        private static IEnumerable<List<int>?> PrepareEnumerable()
        {
            var listOfLists = new List<List<int>?>(6);
            for (int i = 0; i < listOfLists.Capacity; ++i)
            {
                listOfLists.Add(i % 3 == 0 ? null : new List<int>(Enumerable.Range(0, i + 1)));
            }

            return listOfLists.Select(x => x); // Ensure that type will be enumerable.
        }

        public void ManualCompare(EnumerableWithIndexerValues? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }

            var values1 = EnumerableOfLists.ToArray();
            var values2 = other.EnumerableOfLists.ToArray();
            Assert.AreEqual(values1.Length, values2.Length);
            for (int i = 0; i < values1.Length; ++i)
            {
                List<int>? ch1 = values1[i];
                List<int>? ch2 = values2[i];
                CollectionAssert.AreEqual(ch1, ch2);
            }
        }

        public void CompareObjects(EnumerableWithIndexerValues? other)
        {
            CompareLogic compareLogic = new()
            {
                Config =
                {
                    ComparePrivateProperties = false,
                    IgnoreObjectTypes = false,
                    IgnoreCollectionOrder = true // Should be true
                }
            };
            var result = compareLogic.Compare(this, other);
            if (!result.AreEqual)
            {
                Assert.Fail(result.DifferencesString);
            }
        }
    }
}
