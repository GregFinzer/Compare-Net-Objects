using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class ArrayOfSomething
    {
        public IEnumerable<int>?[] Value { get; }

        public ArrayOfSomething(IEnumerable<int>?[] arrayOfLists)
        {
            Value = arrayOfLists;
        }

        public static ArrayOfSomething CreateWithLists()
        {
            var arrayOfLists = PrepareArrayWithLists();
            return new ArrayOfSomething(arrayOfLists);
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

        public void CompareObjects(ArrayOfSomething? other)
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
