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

            Assert.IsTrue(result.Differences[0].ParentObject1.GetType() == typeof(Person));
            Assert.IsTrue(result.Differences[0].ParentObject2.GetType() == typeof(Person));

            Assert.AreEqual("Batman", ((Person)result.Differences[0].ParentObject1).Name);
            Assert.AreEqual("Robin", ((Person)result.Differences[0].ParentObject2).Name);
        }
        #endregion

    }
}
