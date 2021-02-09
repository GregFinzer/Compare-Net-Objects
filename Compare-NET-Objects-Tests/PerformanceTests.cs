using System;
using System.Collections.Generic;
using System.Diagnostics;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using KellermanSoftware.CompareNetObjectsTests.TestClasses.Performance;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class PerformanceTests
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

        #region Tests

        [Test]
        public void CompareTypicalObjectSet()
        {
            int max = 10000;

            List<PerformanceOrder> orders = new List<PerformanceOrder>();

            for (int i = 0; i < max; i++)
            {
                PerformanceOrder order = new PerformanceOrder();
                order.OrderId = i;
                order.Email = "john.whorfin@buckaroobanzai.com";
                order.FirstName = "John";
                order.LastName = "Whorfin";
                order.Phone = "(555) 123-4567";

                order.BillingAddress = new PerformanceAddress();
                order.BillingAddress.AddressId = i + max + 1;
                order.BillingAddress.FirstName = "John";
                order.BillingAddress.LastName = "Whorfin";
                order.BillingAddress.AddressLine1 = "100 John Lithgow Lane";
                order.BillingAddress.AddressLine2 = "Suite 8";
                order.BillingAddress.City = "Los Angeles";
                order.BillingAddress.State = "Shock"; //You got me in the state of Shock
                order.BillingAddress.Zip = "88888";
                order.BillingAddress.Phone = "(555) 123-4567";

                order.ShippingAddress = new PerformanceAddress();
                order.ShippingAddress.AddressId = i + max + max + 1;
                order.ShippingAddress.FirstName = "John";
                order.ShippingAddress.LastName = "Whorfin";
                order.ShippingAddress.AddressLine1 = "100 John Lithgow Lane";
                order.ShippingAddress.AddressLine2 = "Suite 8";
                order.ShippingAddress.City = "Los Angeles";
                order.ShippingAddress.State = "Shock"; //You got me in the state of Shock
                order.ShippingAddress.Zip = "88888";
                order.ShippingAddress.Phone = "(555) 123-4567";

                order.OrderDetails = new List<PerformanceOrderDetail>();

                var detail = new PerformanceOrderDetail();
                detail.OrderDetailId = i;
                detail.ProductName = "Space Ship";
                detail.Price = 888888888.88M;
                detail.Quantity = 8;
                detail.SKU = "BB8";
                order.OrderDetails.Add(detail);
                orders.Add(order);
            }

            List<PerformanceOrder> orders2 = Common.CloneWithSerialization(orders);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            ComparisonResult result = _compare.Compare(orders, orders2);
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        [Test]
        public void CompareGiantLists()
        {
            int max = 10000;
            List<Person> persons1 = new List<Person>();

            for (int i = 0; i < max; i++)
            {
                Person person = new Person();
                person.Name = "Greg";
                persons1.Add(person);
            }

            List<Person> persons2 = Common.CloneWithSerialization(persons1);
           
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ComparisonResult result = _compare.Compare(persons1, persons2);
            watch.Stop();
            
            Console.WriteLine(watch.ElapsedMilliseconds);
        }


        [Test, Ignore("Takes 10 seconds to run, too slow")]
        public void ComparerIgnoreOrderHugeNumericArraysTest()
        {
            const int len = 100000;
            var a = new Int32[len];
            var b = new Int32[len];
            for (var i = 0; i < len; i++)
            {
                a[i] = i;
                b[len - 1 - i] = i;
            }

            var comparer = new CompareLogic();
            comparer.Config.IgnoreCollectionOrder = true;
            comparer.Config.MaxDifferences = 1;

            ComparisonResult result = comparer.Compare(a, b);
            Console.WriteLine(result.DifferencesString);
            Assert.IsTrue(result.AreEqual);

            Console.WriteLine(result.ElapsedMilliseconds);
        }

        [Test]
        public void CachingTest()
        {
            int max = 10000;

            List<PerformanceOrder> list1 = new List<PerformanceOrder>();

            for (int i = 0; i < max; i++)
            {
                PerformanceOrder order = new PerformanceOrder();
                order.OrderId = i;
                order.Email = "john.whorfin@buckaroobanzai.com";
                order.FirstName = "John";
                order.LastName = "Whorfin";
                order.Phone = "(555) 123-4567";

                order.BillingAddress = new PerformanceAddress();
                order.BillingAddress.AddressId = i + max + 1;
                order.BillingAddress.FirstName = "John";
                order.BillingAddress.LastName = "Whorfin";
                order.BillingAddress.AddressLine1 = "100 John Lithgow Lane";
                order.BillingAddress.AddressLine2 = "Suite 8";
                order.BillingAddress.City = "Los Angeles";
                order.BillingAddress.State = "Shock"; //You got me in the state of Shock
                order.BillingAddress.Zip = "88888";
                order.BillingAddress.Phone = "(555) 123-4567";

                order.ShippingAddress = new PerformanceAddress();
                order.ShippingAddress.AddressId = i + max + max + 1;
                order.ShippingAddress.FirstName = "John";
                order.ShippingAddress.LastName = "Whorfin";
                order.ShippingAddress.AddressLine1 = "100 John Lithgow Lane";
                order.ShippingAddress.AddressLine2 = "Suite 8";
                order.ShippingAddress.City = "Los Angeles";
                order.ShippingAddress.State = "Shock"; //You got me in the state of Shock
                order.ShippingAddress.Zip = "88888";
                order.ShippingAddress.Phone = "(555) 123-4567";

                order.OrderDetails = new List<PerformanceOrderDetail>();

                var detail = new PerformanceOrderDetail();
                detail.OrderDetailId = i;
                detail.ProductName = "Space Ship";
                detail.Price = 888888888.88M;
                detail.Quantity = 8;
                detail.SKU = "BB8";
                order.OrderDetails.Add(detail);
                list1.Add(order);
            }

            List<PerformanceOrder> list2 = Common.CloneWithSerialization(list1);

            _compare.Config.Caching = false;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Assert.IsTrue(_compare.Compare(list1, list2).AreEqual);
            watch.Stop();
            long timeWithNoCaching = watch.ElapsedMilliseconds;
            Console.WriteLine("Compare 10000 objects no caching: {0} milliseconds", timeWithNoCaching);

            _compare.Config.Caching = true;
            watch.Reset();
            watch.Start();
            Assert.IsTrue(_compare.Compare(list1, list2).AreEqual);
            watch.Stop();
            long timeWithCaching = watch.ElapsedMilliseconds;
            Console.WriteLine("Compare 10000 objects with caching: {0} milliseconds", timeWithCaching);

            Assert.IsTrue(timeWithCaching < timeWithNoCaching);
        }
        #endregion

    }
}
