using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareDictionaryTests
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
        public void TestDictionary()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Owen";
            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            Dictionary<string, Person> dict1 = new Dictionary<string, Person>();
            dict1.Add("1001", p1);
            dict1.Add("1002", p2);

            Dictionary<string, Person> dict2 = Common.CloneWithSerialization(dict1);

            ComparisonResult result = _compare.Compare(dict1, dict2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);

        }





        [Test]
        public void TestDictionaryNegative()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Owen";
            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            Dictionary<string, Person> dict1 = new Dictionary<string, Person>();
            dict1.Add("1001", p1);
            dict1.Add("1002", p2);

            Dictionary<string, Person> dict2 = Common.CloneWithSerialization(dict1);

            dict2["1002"].DateCreated = DateTime.Now.AddDays(1);

            Assert.IsFalse(_compare.Compare(dict1, dict2).AreEqual);

        }
        #endregion
    }
}
