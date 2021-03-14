using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class DictionaryOfSomething
    {
        public IReadOnlyDictionary<IEnumerable<int>, IEnumerable<int>?> Value { get; }

        public DictionaryOfSomething(IReadOnlyDictionary<IEnumerable<int>, IEnumerable<int>?> dictOfLists)
        {
            Value = dictOfLists;
        }

        public static DictionaryOfSomething CreateWithLists()
        {
            var dictOfLists = PrepareDictionaryWithLists();
            return new DictionaryOfSomething(dictOfLists);
        }

        private static IReadOnlyDictionary<IEnumerable<int>, IEnumerable<int>?> PrepareDictionaryWithLists()
        {
            int dicOfDicsCapacity = 6;
            var keyDictionaryComparer = new KeyListComparer();
            var dicOfDics = new Dictionary<IEnumerable<int>, IEnumerable<int>?>(
                dicOfDicsCapacity, keyDictionaryComparer
            );

            int keyCounter = 1;
            for (int i = 0; i < dicOfDicsCapacity; ++i)
            {
                var key = new List<int>();
                int[] keyData = { keyCounter++, keyCounter++ };
                key.AddRange(keyData);

                List<int>? value = null;
                if (i % 3 != 0)
                {
                    var values = Enumerable.Range(1, i);
                    value = new List<int>(values);
                }

                dicOfDics[key] = value;
            }

            return dicOfDics;
        }

        public void ManualCompare(DictionaryOfSomething? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }

            Assert.AreEqual(Value.Count, other.Value.Count);
            foreach (var key in other.Value.Keys)
            {
                var v2 = other.Value[key];
                Assert.IsTrue(Value.TryGetValue(key, out var v1));
                if (v1 is null || v2 is null)
                {
                    Assert.IsNull(v1);
                    Assert.IsNull(v2);
                }
                else
                {
                    CollectionAssert.AreEqual(v1, v2);
                }
            }
        }

        public void CompareObjects(DictionaryOfSomething? other)
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

        #region Internals

        private sealed class KeyListComparer : IEqualityComparer<IEnumerable<int>>
        {
            public KeyListComparer()
            {
            }

            public bool Equals(IEnumerable<int>? x, IEnumerable<int>? y)
            {
                if (x is null)
                {
                    return y is null;
                }

                if (y is null)
                {
                    return false;
                }

                return x.SequenceEqual(y);
            }

            public int GetHashCode(IEnumerable<int>? obj)
            {
                if (obj is null)
                {
                    return 0;
                }

                unchecked
                {
                    int hash = 269;
                    foreach (var key in obj)
                    {
                        hash = (hash * 47) ^ key;
                    }

                    return hash;
                }
            }
        }

        #endregion

    }
}
