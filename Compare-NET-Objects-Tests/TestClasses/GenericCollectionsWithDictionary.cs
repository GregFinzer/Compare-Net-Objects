using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

using KeyDictionary = System.Collections.Generic.Dictionary<short, int>;
using ValueDictionary = System.Collections.Generic.Dictionary<string, ushort?>;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    // Define this type here to use types above.
    using ComplexDictionary = Dictionary<KeyDictionary, ValueDictionary?>;

    internal sealed class GenericCollectionsWithDictionary

    {
        public ComplexDictionary? DicOfDics { get; set; }

        public GenericCollectionsWithDictionary()
        {
        }

        // Some weird code to fill dictionary.
        public void Prepare()
        {
            int dicOfDicsCapacity = 2; // 0 or 1 does not show the error.
            var keyDictionaryComparer = new KeyDictionaryComparer();
            DicOfDics = new ComplexDictionary(dicOfDicsCapacity, keyDictionaryComparer);

            int keyCounter = 1;
            for (int i = 0; i < dicOfDicsCapacity; ++i)
            {
                var key = new KeyDictionary();
                {
                    int[] keyData = { keyCounter++, keyCounter++ };
                    for (int k = 0; k < keyData.Length; ++k)
                    {
                        key[unchecked((short) (keyData[k] * -1))] = keyData[k];
                    }
                }

                ValueDictionary? value = null;
                if (i % 3 != 0)
                {
                    value = new ValueDictionary();
                    string[] keys = (new[] { (keyCounter++).ToString(), keyCounter++.ToString() }).Where(k => k != null).ToArray()!;
                    ushort?[] values = Enumerable.Range(1, keys.Length).Select(x => (ushort?)x).ToArray();
                    for (int k = 0; k < keys.Length; ++k)
                    {
                        value[keys[k]] = values[k];
                    }
                }

                DicOfDics[key] = value;
            }
        }

        public void ManualCompare(GenericCollectionsWithDictionary? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }

            if (DicOfDics is null || other.DicOfDics is null)
            {
                Assert.IsNull(DicOfDics);
                Assert.IsNull(other.DicOfDics);
            }
            else
            {
                Assert.AreEqual(DicOfDics.Count, other.DicOfDics.Count);
                foreach (KeyDictionary key in other.DicOfDics.Keys)
                {
                    ValueDictionary? v2 = other.DicOfDics[key];
                    Assert.IsTrue(DicOfDics.TryGetValue(key, out var v1));
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
        }

        public void CompareObjects(GenericCollectionsWithDictionary? other)
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

        public sealed class KeyDictionaryComparer : IEqualityComparer<KeyDictionary>
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