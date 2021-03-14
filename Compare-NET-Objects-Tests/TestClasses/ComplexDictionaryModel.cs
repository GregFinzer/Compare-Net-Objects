using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

using KeyDictionary = System.Collections.Generic.Dictionary<short, int>;
using ValueDictionary = System.Collections.Generic.Dictionary<string, ushort?>;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    // Define this type here to use types above.
    using ComplexDictionary = Dictionary<KeyDictionary, ValueDictionary?>;

    internal sealed class ComplexDictionaryModel

    {
        public ComplexDictionary DictOfDicts { get; }

        public ComplexDictionaryModel(ComplexDictionary dictOfDicts)
        {
            DictOfDicts = dictOfDicts;
        }

        public static ComplexDictionaryModel Create()
        {
            var dictOfDicts = PrepareComplexDictionary();
            return new ComplexDictionaryModel(dictOfDicts);
        }

        private static ComplexDictionary PrepareComplexDictionary()
        {
            int dicOfDicsCapacity = 6; // 0 or 1 does not show the error.
            var keyDictionaryComparer = new KeyDictionaryComparer();
            var dicOfDics = new ComplexDictionary(dicOfDicsCapacity, keyDictionaryComparer);

            int keyCounter = 1;
            for (int i = 0; i < dicOfDicsCapacity; ++i)
            {
                var key = new KeyDictionary();
                int[] keyData = { keyCounter++, keyCounter++ };
                for (int k = 0; k < keyData.Length; ++k)
                {
                    key[unchecked((short) (keyData[k] * -1))] = keyData[k];
                }

                ValueDictionary? value = null;
                if (i % 3 != 0)
                {
                    value = new ValueDictionary();
                    string[] keys = (new[] { keyCounter++.ToString(), keyCounter++.ToString() }).Where(k => k != null).ToArray()!;
                    ushort?[] values = Enumerable.Range(1, keys.Length).Select(x => (ushort?)x).ToArray();
                    for (int k = 0; k < keys.Length; ++k)
                    {
                        value[keys[k]] = values[k];
                    }
                }

                dicOfDics[key] = value;
            }

            return dicOfDics;
        }

        public void ManualCompare(ComplexDictionaryModel? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }

            Assert.AreEqual(DictOfDicts.Count, other.DictOfDicts.Count);
            foreach (KeyDictionary key in other.DictOfDicts.Keys)
            {
                ValueDictionary? v2 = other.DictOfDicts[key];
                Assert.IsTrue(DictOfDicts.TryGetValue(key, out var v1));
                if (v1 is null || v2 is null)
                {
                    Assert.IsNull(v1);
                    Assert.IsNull(v2);
                }
                else
                {
                    Assert.AreEqual(v1.Count, v2.Count);
                    foreach (string kk1 in v1.Keys)
                    {
                        ushort? vv1 = v1[kk1];
                        Assert.IsTrue(v2.TryGetValue(kk1, out var vv2));
                        Assert.AreEqual(vv1, vv2);
                    }
                }
            }
        }

        public void CompareObjects(ComplexDictionaryModel? other)
        {
            var compareLogic = new CompareLogic()
            {
                Config =
                {
                    ComparePrivateProperties = false,
                    IgnoreObjectTypes = false,
                    IgnoreCollectionOrder = false // true does not change the result :(
                }
            };
            var result = compareLogic.Compare(this, other);
            if (!result.AreEqual)
            {
                Assert.Fail(result.DifferencesString);
            }
        }

        #region Internals

        private sealed class KeyDictionaryComparer : IEqualityComparer<KeyDictionary>
        {
            public KeyDictionaryComparer()
            {
            }

            public bool Equals(KeyDictionary? x, KeyDictionary? y)
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

                foreach (var key in x.Keys)
                {
                    var v1 = x[key];
                    if (!y.TryGetValue(key, out var v2))
                    {
                        return false;
                    }

                    if (v1 != v2)
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(KeyDictionary? obj)
            {
                if (obj is null)
                {
                    return 0;
                }

                unchecked
                {
                    int hash = 269;
                    hash = (hash * 47) ^ obj.Count;
                    foreach (var key in obj.Keys)
                    {
                        hash = (hash * 47) ^ key;
                        hash = (hash * 47) ^ obj[key];
                    }

                    return hash;
                }
            }
        }

        #endregion
    }
}