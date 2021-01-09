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

            var result = _compare.Compare(dict1, dict2);

            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void TestNullDictionary()
        {
            _compare.Config.MaxDifferences = 10;

            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Owen";

            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            Dictionary<string, Person> dict1 = null;

            Dictionary<string, Person> dict2 = new Dictionary<string, Person>();
            dict2.Add("1001", p1);
            dict2.Add("1002", p1);
            dict2.Add("1003", p1);

            ComparisonResult result1 = _compare.Compare(dict1, dict2);
            ComparisonResult result2 = _compare.Compare(dict2, dict1);

            Console.WriteLine(result1.DifferencesString);
            Assert.IsFalse(result1.AreEqual);
            Assert.AreEqual(4, result1.Differences.Count);

            Console.WriteLine(result2.DifferencesString);
            Assert.IsFalse(result2.AreEqual);
            Assert.AreEqual(4, result2.Differences.Count);
        }

        [Test]
        public void TestDictionaryKeysNotAddedInOrder()
        {
            _compare.Config.MaxDifferences = 10;

            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = DateTime.Now.AddDays(-1);
            Person p3 = new Person();
            p3.Name = "Martha";
            p3.DateCreated = DateTime.Now.AddDays(-1);

            Dictionary<string, Person> dict1 = new Dictionary<string, Person>();
            Dictionary<string, Person> dict2 = new Dictionary<string, Person>();

            dict1.Add("1005", p2);

            dict1.Add("1006", p3);
            dict2.Add("1006", p3);

            dict2.Add("1005", p2);

            ComparisonResult result = _compare.Compare(dict1, dict2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }
        #endregion
    }
}
