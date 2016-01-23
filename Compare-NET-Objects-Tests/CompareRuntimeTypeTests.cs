using System;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareRuntimeTypeTests
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
        public void TypeOfTypePositiveTest()
        {
            Table table1 = new Table();
            table1.TableName = "Person";
            table1.ClassType = typeof(Person);

            Table table2 = new Table();
            table2.TableName = "Person";
            table2.ClassType = typeof(Person);

            ComparisonResult result = _compare.Compare(table1, table2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);

        }

        [Test]
        public void TypeOfTypeNegativeTest()
        {
            Table table1 = new Table();
            table1.TableName = "Person";
            table1.ClassType = typeof(Person);

            Table table2 = new Table();
            table2.TableName = "Person";
            table2.ClassType = typeof(RecipeDetail);

            Assert.IsFalse(_compare.Compare(table1, table2).AreEqual);
        }
        #endregion
    }
}
