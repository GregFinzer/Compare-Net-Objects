using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using KellermanSoftware.CompareNetObjects;

namespace Compare_NET_Objects_Core.Tests
{
    [TestFixture]
    public class TypeHelperTests
    {
        [Test]
        public void IsByteArray()
        {
            var enumerable = new byte[] { 3, 5, 7, 8, 9 };
            var type = enumerable.GetType();
            Assert.That(TypeHelper.IsByteArray(type));
        }

        [Test]
        public void IsEnumerable()
        {
            var enumerable = new List<int> { 3, 5, 7, 8, 9}.Where(i => i > 4);
            var type = enumerable.GetType();
            Assert.That(TypeHelper.IsEnumerable(type));
        }
    }
}
