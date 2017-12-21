using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace Compare_NET_Objects_Core_Tests
{
    [TestFixture]
    public class CoreTests
    {
        [Test]
        public void CompareSimple()
        {
            int value1 = 7;
            int value2 = 7;
            var compare = new CompareLogic();
            Assert.IsTrue(compare.Compare(value1, value2).AreEqual);
        }

        [Test]
        public void DecimalCollectionWhenOrderIgnored()
        {
            var compare = new CompareLogic(new ComparisonConfig
            {
                IgnoreCollectionOrder = true
            });
            Assert.IsTrue(compare.Compare(new decimal[] { 10, 1 }, new[] { 10.0m, 1.0m }).AreEqual);
        }
    }
}
