using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class GenericCollectionWithArrays
    {
        public List<int>?[] ArrayOfLists { get; }

        public GenericCollectionWithArrays(List<int>?[] arrayOfLists)
        {
            ArrayOfLists = arrayOfLists;
        }

        public static GenericCollectionWithArrays Create()
        {
            var arrayOfLists = PrepareArray();
            return new GenericCollectionWithArrays(arrayOfLists);
        }

        private static List<int>?[] PrepareArray()
        {
            var arrayOfLists = new List<int>?[2];
            for (int i = 0; i < arrayOfLists.Length; ++i)
            {
                arrayOfLists[i] = i % 3 == 0 ? null : new List<int>(Enumerable.Range(0, i + 1));
            }

            return arrayOfLists;
        }

        public void ManualCompare(GenericCollectionWithArrays? other)
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

        public void CompareObjects(GenericCollectionWithArrays? other)
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