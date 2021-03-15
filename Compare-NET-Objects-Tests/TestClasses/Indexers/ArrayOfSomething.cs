using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.Indexers
{
    internal sealed class ArrayOfSomething
    {
        public IEnumerable<int>?[] Value { get; }

        public ArrayOfSomething(IEnumerable<int>?[] value)
        {
            Value = value;
        }

        public static ArrayOfSomething CreateWithLists()
        {
            var arrayOfLists = PrepareArrayWithLists();
            return new ArrayOfSomething(arrayOfLists);
        }

        public static ArrayOfSomething CreateWithArrays()
        {
            var arrayOfArrays = PrepareArrayWithLists()
                .Select(x => x?.ToArray())
                .ToArray();
            return new ArrayOfSomething(arrayOfArrays);
        }

        public static ArrayOfSomething CreateWithEnumerables()
        {
            var arrayOfArrays = PrepareArrayWithLists()
                .Select(x => x?.ToArray().ToTrueEnumerable())
                .ToArray();
            return new ArrayOfSomething(arrayOfArrays);
        }

        private static IEnumerable<int>?[] PrepareArrayWithLists()
        {
            var arrayOfLists = new List<int>?[6];
            for (int i = 0; i < arrayOfLists.Length; ++i)
            {
                arrayOfLists[i] = i % 3 == 0 ? null : new List<int>(Enumerable.Range(0, i + 1));
            }

            return arrayOfLists;
        }

        public void ManualCompare(ArrayOfSomething? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }


            if (Value is null || other.Value is null)
            {
                Assert.IsNull(Value);
                Assert.IsNull(other.Value);
            }
            else
            {
                Assert.AreEqual(Value.Length, other.Value.Length);
                for (int i = 0; i < Value.Length; ++i)
                {
                    var ch1 = Value[i];
                    var ch2 = other.Value[i];
                    CollectionAssert.AreEqual(ch1, ch2);
                }
            }
        }

        public void CompareObjects(ArrayOfSomething? other, bool expected)
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
