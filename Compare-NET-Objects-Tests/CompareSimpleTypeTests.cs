using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareSimpleTypeTests
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

        #region GUID Tests
        [Test]
        public void TestGuid()
        {
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            List<Guid> list1 = new List<Guid>();
            list1.Add(guid1);
            list1.Add(guid2);

            List<Guid> list2 = new List<Guid>();
            list2.Add(guid1);
            list2.Add(guid2);

            ComparisonResult result = _compare.Compare(list1, list2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void TestGuidNegative()
        {
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            Guid guid3 = Guid.NewGuid();

            List<Guid> list1 = new List<Guid>();
            list1.Add(guid1);
            list1.Add(guid2);

            List<Guid> list2 = new List<Guid>();
            list2.Add(guid1);
            list2.Add(guid3);

            Assert.IsFalse(_compare.Compare(list1, list2).AreEqual);
        }

        #endregion

        #region Property Tests
        [Test]
        public void PropertyAndFieldTest()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Greg";
            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = p1.DateCreated;

            ComparisonResult result = _compare.Compare(p1, p2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void TickTest()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Greg";
            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = p1.DateCreated.AddTicks(1);

            Assert.IsFalse(_compare.Compare(p1, p2).AreEqual);
        }


        #endregion
    }
}
