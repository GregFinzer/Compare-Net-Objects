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
        #region Class Variables
        private CompareLogic _compare;
        #endregion

        #region Setup/Teardown


        /// <summary>
        /// Code that is run before each test
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            _compare = new CompareLogic();
        }

        /// <summary>
        /// Code that is run after each test
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            _compare = null;
        }
        #endregion

        #region UseCustomComparerTest

        [Test]
        public void UseCustomComparer()
        {
            SpecificTenant tenant1 = new SpecificTenant();
            tenant1.Name = "wire";
            tenant1.Amount = 37;

            SpecificTenant tenant2 = new SpecificTenant();
            tenant2.Name = "wire";
            tenant2.Amount = 155;            

            //No Custom Comparer
            Assert.IsFalse(_compare.Compare(tenant1, tenant2).AreEqual);

            //specify custom selector
            _compare.Config.CustomComparers.Add(new MyCustomComparer(RootComparerFactory.GetRootComparer()));

            Assert.IsTrue(_compare.Compare(tenant1, tenant2).AreEqual);

            tenant2.Amount = 42;
            Assert.IsFalse(_compare.Compare(tenant1, tenant2).AreEqual);

            //restore
            _compare.Config.CustomComparers = new List<BaseTypeComparer>();
        }

        #endregion
    }
}
