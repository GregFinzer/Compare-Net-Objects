using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CustomComparerTests
    {

        [Test]
        public void UseCustomComparer()
        {
            CompareLogic compareLogic = new CompareLogic();

            SpecificTenant tenant1 = new SpecificTenant();
            tenant1.Name = "wire";
            tenant1.Amount = 37;

            SpecificTenant tenant2 = new SpecificTenant();
            tenant2.Name = "wire";
            tenant2.Amount = 155;            

            //No Custom Comparer
            Assert.IsFalse(compareLogic.Compare(tenant1, tenant2).AreEqual);

            //specify custom selector
            compareLogic.Config.CustomComparers.Add(new MyCustomComparer(RootComparerFactory.GetRootComparer()));

            Assert.IsTrue(compareLogic.Compare(tenant1, tenant2).AreEqual);

            tenant2.Amount = 42;
            Assert.IsFalse(compareLogic.Compare(tenant1, tenant2).AreEqual);
        }


    }
}
