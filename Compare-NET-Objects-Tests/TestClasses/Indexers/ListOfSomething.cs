using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class ListOfSomething
    {
        public IReadOnlyList<IEnumerable<int>?> Value { get; }

        public ListOfSomething(IReadOnlyList<IEnumerable<int>?> listOfLists)
        {
            Value = listOfLists;
        }

        public static ListOfSomething CreateWithLists()
        {
            var listOfLists = PrepareListWithLists();
            return new ListOfSomething(listOfLists);
        }

        private static IReadOnlyList<IEnumerable<int>?> PrepareListWithLists()
        {
            var listOfLists = new List<List<int>?>(6);
            for (int i = 0; i < listOfLists.Capacity; ++i)
            {
                listOfLists.Add(i % 3 == 0 ? null : new List<int>(Enumerable.Range(0, i + 1)));
            }

            return listOfLists;
        }

        public void ManualCompare(ListOfSomething? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }

            Assert.AreEqual(Value.Count, other.Value.Count);
            for (int i = 0; i < Value.Count; ++i)
            {
                var ch1 = Value[i];
                var ch2 = other.Value[i];
                CollectionAssert.AreEqual(ch1, ch2);
            }
        }

        public void CompareObjects(ListOfSomething? other)
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
