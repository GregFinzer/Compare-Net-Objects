using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareCollectionsTests
    {
        [Test]
        public void CollectionsSame()
        {
            var coll1 = new List<KeyValuePair<string, byte[]>>
            {
                new KeyValuePair<string, byte[]>("Hello", new byte[] { 1, 2, 3 })
            };
            var coll2 = new Dictionary<string, byte[]>
            {
                {"Hello", new byte[] { 1, 2, 3 } }
            };
            var compareLogic = new CompareLogic
            {
                Config = new ComparisonConfig
                {
                    IgnoreObjectTypes = true,
                    IgnoreCollectionOrder = true
                }
            };

            Assert.IsTrue(compareLogic.Compare(coll1, coll2).AreEqual);
        }
    }
}
