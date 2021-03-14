using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class GenericCollectionsWithLists
    {
        public List<List<int>?>? ListOfLists { get; set; }

        public GenericCollectionsWithLists()
        {
        }

        // Some weird code to fill list.
        public void Prepare()
        {
            ListOfLists = new List<List<int>?>(2);
            for (int i = 0; i < ListOfLists.Capacity; ++i)
                ListOfLists.Add(i % 3 == 0 ? null : new List<int>(Enumerable.Range(0, i + 1)));
        }

        public void ManualCompare(GenericCollectionsWithLists? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }


            if (ListOfLists is null || other.ListOfLists is null)
            {
                Assert.IsNull(ListOfLists);
                Assert.IsNull(other.ListOfLists);
            }
            else
            {
                Assert.AreEqual(ListOfLists.Count, other.ListOfLists.Count);
                for (int i = 0; i < ListOfLists.Count; i++)
                {
                    List<int>? ch1 = ListOfLists[i];
                    List<int>? ch2 = other.ListOfLists[i];
                    CollectionAssert.AreEqual(ch1, ch2);
                }
            }
        }

        public void CompareObjects(GenericCollectionsWithLists? other)
        {
            CompareLogic compareLogic = new()
            {
                Config =
                {
                    ComparePrivateProperties = false,
                    IgnoreObjectTypes = false,
                    IgnoreCollectionOrder = true // true does not change the result :(
                }
            };
            var result = compareLogic.Compare(this, other);
            if (!result.AreEqual)
                Assert.Fail(result.DifferencesString);
        }
    }
}