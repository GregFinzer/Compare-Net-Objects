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
            Assert.IsTrue(compare.Compare(value1, value1).AreEqual);
        }
    }
}
