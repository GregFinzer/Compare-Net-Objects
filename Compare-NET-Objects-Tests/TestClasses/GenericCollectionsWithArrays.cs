using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class GenericCollectionsWithArrays
    {
        public List<int>?[]? ArrayOfLists { get; set; }

        public GenericCollectionsWithArrays()
        {
        }

        // Some weird code to fill list.
        public void Prepare()
        {
            ArrayOfLists = new List<int>?[2];
            for (int i = 0; i < ArrayOfLists.Length; ++i)
            {
                ArrayOfLists[i] = i % 3 == 0 ? null : new List<int>(Enumerable.Range(0, i + 1));
            }
        }

        public void ManualCompare(GenericCollectionsWithArrays? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }


            if (ArrayOfLists is null || other.ArrayOfLists is null)
            {
                Assert.IsNull(ArrayOfLists);
                Assert.IsNull(other.ArrayOfLists);
            }
            else
            {
                Assert.AreEqual(ArrayOfLists.Length, other.ArrayOfLists.Length);
                for (int i = 0; i < ArrayOfLists.Length; i++)
                {
                    List<int>? ch1 = ArrayOfLists[i];
                    List<int>? ch2 = other.ArrayOfLists[i];
                    CollectionAssert.AreEqual(ch1, ch2);
                }
            }
        }

        public void CompareObjects(GenericCollectionsWithArrays? other)
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
            {
                Assert.Fail(result.DifferencesString);
            }
        }
    }
}