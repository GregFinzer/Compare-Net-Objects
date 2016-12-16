using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace Compare_NET_Objects_Core_Tests
{
    [TestFixture]
    public class CoreTests
    {
        [Test]
        public void Compare_IndexerCompareAndPropertyComparePositive()
        {
            var jane = new Person { Name = "Jane" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, jack, mary };
            var nameList2 = new List<Person>() { jane, jack, mary };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            var compare = new CompareLogic();
            Assert.IsTrue(compare.Compare(class1, class2).AreEqual);
        }
    }
}
