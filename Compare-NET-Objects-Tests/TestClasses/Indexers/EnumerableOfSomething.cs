using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class EnumerableOfSomething
    {
        public IEnumerable<IEnumerable<int>?> Value { get; }

        public EnumerableOfSomething(IEnumerable<IEnumerable<int>?> enumerableOfLists)
        {
            Value = enumerableOfLists.Select(x => x); // Ensure that type will be enumerable.
        }

        public static EnumerableOfSomething CreateWithLists()
        {
            var listOfLists = PrepareEnumerableWithLists();
            return new EnumerableOfSomething(listOfLists);
        }

        private static IEnumerable<IEnumerable<int>?> PrepareEnumerableWithLists()
        {
            var listOfLists = new List<List<int>?>(6);
            for (int i = 0; i < listOfLists.Capacity; ++i)
            {
                listOfLists.Add(i % 3 == 0 ? null : new List<int>(Enumerable.Range(0, i + 1)));
            }

            return listOfLists.Select(x => x); // Ensure that type will be enumerable.
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

        public void CompareObjects(EnumerableOfSomething? other)
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
