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
    public class ParentObjectTests
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
        public void ParentIsAClassTest()
        {
            Person person1 = new Person();
            person1.Name = "Batman";

            Person person2 = new Person();
            person2.Name = "Robin";

            ComparisonResult result = _compare.Compare(person1, person2);

            Assert.IsTrue(result.Differences[0].ParentObject1 != null);
            Assert.IsTrue(result.Differences[0].ParentObject2 != null);

            Assert.IsTrue(result.Differences[0].ParentObject1.Target.GetType() == typeof(Person));
            Assert.IsTrue(result.Differences[0].ParentObject2.Target.GetType() == typeof(Person));

            Assert.AreEqual("Batman", ((Person)result.Differences[0].ParentObject1.Target).Name);
            Assert.AreEqual("Robin", ((Person)result.Differences[0].ParentObject2.Target).Name);
        }
        #endregion

    }
}
