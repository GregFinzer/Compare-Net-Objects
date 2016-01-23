using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class ComparisonResultTests
    {
        #region Class Variables
        private CompareLogic _compare;
        #endregion

        #region Setup/Teardown

        /// <summary>
        /// Code that is run once for a suite of tests
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {

        }

        /// <summary>
        /// Code that is run once after a suite of tests has finished executing
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {

        }

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
        public void WeakReferenceNegativeTest()
        {
            var jane = new Person { Name = "Jane" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, jack, mary };
            var nameList2 = new List<Person>() { jane, mary, jack };

            var result = _compare.Compare(nameList1, nameList2);
            _compare.Config.MaxDifferences = int.MaxValue;

            Console.WriteLine(result.DifferencesString);

            Assert.IsTrue(result.Differences[0].Object1.IsAlive);
            Assert.IsTrue(result.Differences[0].Object2.IsAlive);

            Assert.AreNotEqual(result.Differences[0].Object1.Target, result.Differences[0].Object2.Target);
        }

        [Test]
        public void ParentPropertyNameArrayTest()
        {
            Difference diff = new Difference();
            diff.PropertyName = ".Password[4]";
            Assert.AreEqual(".Password", diff.ParentPropertyName);
        }

        [Test]
        public void ParentPropertyNameChildTest()
        {
            Difference diff = new Difference();
            diff.PropertyName = ".Customer.FirstName";
            Assert.AreEqual(".Customer", diff.ParentPropertyName);
        }
        #endregion

    }
}
