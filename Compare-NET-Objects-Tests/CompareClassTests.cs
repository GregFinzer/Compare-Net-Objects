using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareClassTests
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

        #region Null Tests
        [Test]
        public void NullObjects()
        {
            Person p1 = null;
            Person p2 = null;

            ComparisonResult result = _compare.Compare(p1, p2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);

        }

        [Test]
        public void OneObjectNull()
        {
            Person p1 = null;
            Person p2 = new Person();
            Assert.IsFalse(_compare.Compare(p1, p2).AreEqual);
            Assert.IsFalse(_compare.Compare(p2, p1).AreEqual);
        }

        [Test]
        public void SecondObjectNull()
        {
            Person p1 = new Person();
            Person p2 = null;
            Assert.IsFalse(_compare.Compare(p1, p2).AreEqual);
            Assert.IsFalse(_compare.Compare(p2, p1).AreEqual);
        }

        #endregion

        #region Deep Tests
        [Test]
        public void DeepNotEqual()
        {
            var people1 = new List<Person>() { new Person() { Name = "Joe" } };
            var people2 = new List<Person>() { new Person() { Name = "Sue" } };
            var group1 = new KeyValuePair<string, List<Person>>("People", people1);
            var group2 = new KeyValuePair<string, List<Person>>("People", people2);
            var result = _compare.Compare(group1, group2);
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void DeepEqual()
        {
            var people1 = new List<Person>() { new Person() { Name = "Joe" } };
            var people2 = new List<Person>() { new Person() { Name = "Joe" } };
            var group1 = new KeyValuePair<string, List<Person>>("People", people1);
            var group2 = new KeyValuePair<string, List<Person>>("People", people2);
            var result = _compare.Compare(group1, group2);
            Assert.IsTrue(result.AreEqual);
        }

        #endregion

        #region Shallow Tests
        [Test]
        public void ShallowWithNullNoChanges()
        {
            PrimitivePropertiesNullable p1 = new PrimitivePropertiesNullable();
            PrimitivePropertiesNullable p2 = new PrimitivePropertiesNullable();
            _compare.Config.CompareChildren = false;

            ComparisonResult result = _compare.Compare(p1, p2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void ShallowWithNullWithChanges()
        {
            PrimitivePropertiesNullable p1 = new PrimitivePropertiesNullable();
            PrimitivePropertiesNullable p2 = new PrimitivePropertiesNullable();
            p2.BooleanProperty = true;
            _compare.Config.CompareChildren = false;
            Assert.IsFalse(_compare.Compare(p1, p2).AreEqual);
        }

        #endregion

        #if !NETSTANDARD

        #region Private Property Tests
        [Test]
        public void PrivatePropertyPositive()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(true, "Toffee");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.Config.ComparePrivateProperties = true;
            Assert.IsTrue(_compare.Compare(detail1, detail2).AreEqual);
            _compare.Config.ComparePrivateProperties = false;
        }

        [Test]
        public void PrivatePropertyNegative()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(true, "Crunchy Frogs");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.Config.ComparePrivateProperties = true;
            Assert.IsFalse(_compare.Compare(detail1, detail2).AreEqual);
            _compare.Config.ComparePrivateProperties = false;
        }
        #endregion

        #region Private Field Tests
        [Test]
        public void PrivateFieldPositive()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(true, "Toffee");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.Config.ComparePrivateFields = true;
            Assert.IsTrue(_compare.Compare(detail1, detail2).AreEqual);
            _compare.Config.ComparePrivateFields = false;
        }

        [Test]
        public void PrivateFieldNegative()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(true, "Crunchy Frogs");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.Config.ComparePrivateFields = true;
            Assert.IsFalse(_compare.Compare(detail1, detail2).AreEqual);
            _compare.Config.ComparePrivateFields = false;
        }

        [Test]
        public void PrivateFieldInBaseClassPositive()
        {
            Movie movie1 = new Movie();
            movie1.Name = "Jaws";
            movie1.SetPrivateString("Boca Grande");

            Movie movie2 = new Movie();
            movie2.Name = "Jaws";
            movie2.SetPrivateString("Boca Grande");

            _compare.Config.ComparePrivateFields = true;
            Assert.IsTrue(_compare.Compare(movie1, movie2).AreEqual);
            _compare.Config.ComparePrivateFields = false;
        }

        [Test]
        public void PrivateFieldInBaseClassNegative()
        {
            Movie movie1 = new Movie();
            movie1.Name = "Jaws";
            movie1.SetPrivateString("Boca Grande");

            Movie movie2 = new Movie();
            movie2.Name = "Jaws";
            movie2.SetPrivateString("Boca Pequeno");

            _compare.Config.ComparePrivateFields = true;
            Assert.IsFalse(_compare.Compare(movie1, movie2).AreEqual);
            _compare.Config.ComparePrivateFields = false;
        }
        #endregion

        #endif

        #region InterfaceMembers
        [Test]
        public void CompareInterfaceMembers()
        {
            ComparisonConfig config = new ComparisonConfig();
            config.InterfaceMembers.Add(typeof(IName));

            _compare = new CompareLogic(config);

            Person person1 = new Person();
            person1.Name = "Greg";
            person1.DateCreated = DateTime.Now;

            Person person2 = new Person();
            person2.Name = "Greg";
            person2.DateCreated = DateTime.Now.AddDays(-1);

            var result = _compare.Compare(person1, person2);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);

        }
        #endregion

        #region Set-only Property Tests

        [Test]
        public void WriteOnlyPropertyTests()
        {
            WriteOnlyPropertyClass c1 = new WriteOnlyPropertyClass
            {
                Name = "Jim",
                WriteOnly = 1
            };

            WriteOnlyPropertyClass c2 = new WriteOnlyPropertyClass
            {
                Name = "Jim",
                WriteOnly = 2
            };

            Assert.IsTrue(_compare.Compare(c1, c2).AreEqual);            
        }

        #endregion
    }
}
