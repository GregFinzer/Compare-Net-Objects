using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class DictionaryWithIndexerKeysAndValues
    {
        public Dictionary<List<int>, List<int>?> DictOfLists { get; }

        public DictionaryWithIndexerKeysAndValues(Dictionary<List<int>, List<int>?> dictOfLists)
        {
            DictOfLists = dictOfLists;
        }

        public static DictionaryWithIndexerKeysAndValues Create()
        {
            var dictOfLists = PrepareDictionary();
            return new DictionaryWithIndexerKeysAndValues(dictOfLists);
        }

        private static Dictionary<List<int>, List<int>?> PrepareDictionary()
        {
            int dicOfDicsCapacity = 6;
            var keyDictionaryComparer = new KeyListComparer();
            var dicOfDics = new Dictionary<List<int>, List<int>?>(dicOfDicsCapacity, keyDictionaryComparer);

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

        public void ManualCompare(DictionaryWithIndexerKeysAndValues? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }

            Assert.AreEqual(DictOfLists.Count, other.DictOfLists.Count);
            foreach (var key in other.DictOfLists.Keys)
            {
                var v2 = other.DictOfLists[key];
                Assert.IsTrue(DictOfLists.TryGetValue(key, out var v1));
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

        public void CompareObjects(DictionaryWithIndexerKeysAndValues? other)
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

        private sealed class KeyListComparer : IEqualityComparer<List<int>>
        {
            public KeyListComparer()
            {
            }

            public bool Equals(List<int>? x, List<int>? y)
            {
                if (x is null)
                {
                    return y is null;
                }

                if (y is null)
                {
                    return false;
                }

                if (x.Count != y.Count)
                {
                    return false;
                }

                return x.SequenceEqual(y);
            }

            public int GetHashCode(List<int>? obj)
            {
                if (obj is null)
                {
                    return 0;
                }

                unchecked
                {
                    int hash = 269;
                    hash = (hash * 47) ^ obj.Count;
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
