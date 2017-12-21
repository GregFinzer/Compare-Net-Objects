using System;
using System.Collections.Generic;
using System.Diagnostics;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
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

        [Test, Ignore("Inconsistent")]
        public void CachingTest()
        {
            List<Person> list1 = new List<Person>();
            List<Person> list2 = new List<Person>();

            for (int i = 1; i <= 10000; i++)
            {
                Person person = new Person();
                person.DateCreated = DateTime.Now;
                person.Name = "Robot " + i;
                list1.Add(person);
                list2.Add(Common.CloneWithSerialization(person));
            }

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
