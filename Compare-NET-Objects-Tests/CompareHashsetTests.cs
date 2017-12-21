using System;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareHashsetTests
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
        public void HashSetsDifferent()
        {
            HashSetWrapper hashSet1 = new HashSetWrapper
            {
                StatusId = 1,
                Name = "Paul"
            };
            HashSetWrapper hashSet2 = new HashSetWrapper
            {
                StatusId = 1,
                Name = "Paul"
            };

            HashSetClass secondClassObject1 = new HashSetClass
            {
                Id = 1
            };
            HashSetClass secondClassObject2 = new HashSetClass
            {
                Id = 2
            };

            hashSet1.HashSetCollection.Add(secondClassObject1);
            hashSet2.HashSetCollection.Add(secondClassObject2);

            Assert.IsFalse(_compare.Compare(hashSet1, hashSet2).AreEqual);
        }

        [Test]
        public void HashSetsSame()
        {
            HashSetWrapper hashSet1 = new HashSetWrapper
            {
                StatusId = 1,
                Name = "Paul"
            };
            HashSetWrapper hashSet2 = new HashSetWrapper
            {
                StatusId = 1,
                Name = "Paul"
            };

            HashSetClass secondClassObject1 = new HashSetClass
            {
                Id = 1
            };
            HashSetClass secondClassObject2 = new HashSetClass
            {
                Id = 1
            };

            hashSet1.HashSetCollection.Add(secondClassObject1);
            hashSet2.HashSetCollection.Add(secondClassObject2);

            ComparisonResult result = _compare.Compare(hashSet1, hashSet2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);

        }
        #endregion
    }
}
