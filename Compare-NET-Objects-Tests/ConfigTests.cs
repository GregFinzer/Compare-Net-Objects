﻿using System;
using System.IO;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.Attributes;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class ConfigTests
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

        #region Callback Tests
        [Test]
        public void CallbackNotCalledTest()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Greg";
            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = p1.DateCreated;

            var called = false;

            _compare.Config.DifferenceCallback = d =>
            {
                called = true;
            };

            _compare.Compare(p1, p2);

            Assert.IsFalse(called);
        }

        [Test]
        public void CallbackCalledTest()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Greg";
            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = p1.DateCreated.AddSeconds(1);

            var called = false;

            _compare.Config.DifferenceCallback = d =>
            {
                called = true;
            };

            _compare.Compare(p1, p2);

            Assert.IsTrue(called);
        }
        #endregion

        #region Ignore Object Types Tests

        [Test]
        public void IgnoreObjectTypesPositive()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            IndianRecipeDetail detail2 = new IndianRecipeDetail(true, "Toffee");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.Config.IgnoreObjectTypes = true;
            var result = _compare.Compare(detail1, detail2);

            Assert.IsTrue(result.AreEqual, result.DifferencesString);
            _compare.Config.IgnoreObjectTypes = false;
        }

        [Test]
        public void IgnoreObjectTypesNegative()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            IndianRecipeDetail detail2 = new IndianRecipeDetail(true, "Toffee");
            detail2.Ingredient = "Curry sauce";

            _compare.Config.IgnoreObjectTypes = true;
            var result = _compare.Compare(detail1, detail2);

            Console.WriteLine(result.DifferencesString);
            Assert.IsFalse(result.AreEqual, result.DifferencesString);
            _compare.Config.IgnoreObjectTypes = false;
        }



        #endregion

        #region IgnoreByAttributeTest
        [Test]
        public void IgnorePropertyAttribute()
        {
            Movie movie1 = new Movie();
            movie1.Name = "Mission Impossible 13, Ethan Gets Unlucky";
            movie1.PaymentForTomCruise = 20000000;

            Movie movie2 = new Movie();
            movie2.Name = "Mission Impossible 13, Ethan Gets Unlucky";
            movie2.PaymentForTomCruise = 20000001;

            //The difference for PaymentForTomCruise will be ignored becuase it is marked with the ExcludeFromEquality
            _compare.Config.AttributesToIgnore.Add(typeof(ExcludeFromEquality));
            Assert.IsTrue(_compare.Compare(movie1, movie2).AreEqual);

            _compare.Config.AttributesToIgnore.Clear();
        }

        [Test]
        public void IgnorePropertyAttributeDifferent()
        {
            Movie movie1 = new Movie();
            movie1.Name = "Mission Impossible 13, Ethan Gets Unlucky";
            movie1.PaymentForTomCruise = 20000000;

            Movie movie2 = new Movie();
            movie2.Name = "Mission Impossible 14, Ethan Gets Unlucky";
            movie2.PaymentForTomCruise = 20000001;

            //The difference for PaymentForTomCruise will be ignored becuase it is marked with the ExcludeFromEquality
            _compare.Config.AttributesToIgnore.Add(typeof(ExcludeFromEquality));
            Assert.IsFalse(_compare.Compare(movie1, movie2).AreEqual);

            _compare.Config.AttributesToIgnore.Clear();
        }
        #endregion

        #region Ignore Read Only Tests
        [Test]
        public void IgnoreReadOnlyPositive()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(false, "Toffee");
            detail2.Ingredient = "Crunchy Chocolate";

            _compare.Config.CompareReadOnly = false;
            Assert.IsTrue(_compare.Compare(detail1, detail2).AreEqual);
            _compare.Config.CompareReadOnly = true;
        }

        [Test]
        public void IgnoreReadOnlyNegative()
        {
            RecipeDetail detail1 = new RecipeDetail(true, "Toffee");
            detail1.Ingredient = "Crunchy Chocolate";

            RecipeDetail detail2 = new RecipeDetail(false, "Toffee");
            detail2.Ingredient = "Crunchy Chocolate";

            Assert.IsFalse(_compare.Compare(detail1, detail2).AreEqual);
        }
        #endregion

        #region Class Type Include Tests
        [Test]
        public void ClassTypeIncludePositive()
        {
            _compare.Config.ClassTypesToInclude.Add(typeof(Person));

            Person p1 = new Person();
            p1.Name = "Greg";
            p1.DateCreated = DateTime.Now;

            Person p2 = new Person();
            p2.Name = "Leyla";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            var result = _compare.Compare(p1, p2);
            Assert.IsFalse(result.AreEqual, result.DifferencesString);
        }

        [Test]
        public void ClassTypeIncludeNegative()
        {
            _compare.Config.ClassTypesToInclude.Add(typeof(Entity));

            Person p1 = new Person();
            p1.Name = "Greg";
            p1.DateCreated = DateTime.Now;

            Person p2 = new Person();
            p2.Name = "Leyla";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            var result = _compare.Compare(p1, p2);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }
        #endregion

        #region Class Type Ignore Tests
        [Test]
        public void ClassTypeIgnorePositive()
        {
            _compare.Config.ClassTypesToIgnore.Add(typeof(Person));

            Person p1 = new Person();
            p1.Name = "Greg";
            p1.DateCreated = DateTime.Now;

            Person p2 = new Person();
            p2.Name = "Leyla";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            var result = _compare.Compare(p1, p2);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [Test]
        public void ClassTypeIgnoreNegative()
        {
            _compare.Config.ClassTypesToIgnore.Add(typeof(Entity));

            Person p1 = new Person();
            p1.Name = "Greg";
            p1.DateCreated = DateTime.Now;

            Person p2 = new Person();
            p2.Name = "Leyla";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            var result = _compare.Compare(p1, p2);
            Assert.IsFalse(result.AreEqual, result.DifferencesString);
        }
        #endregion

        #region Case Insensitive Tests

        [Test]
        public void CaseSensitiveTest()
        {
            //Arrange
            _compare.Config.CaseSensitive = true;

            //Act
            var result = _compare.Compare("The quick brown fox jumps over the lazy dog.", "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG.");

            //Assert
            Assert.IsFalse(result.AreEqual, result.DifferencesString);
        }

        [Test]
        public void CaseInSensitiveTest()
        {
            //Arrange
            _compare.Config.CaseSensitive = false;

            //Act
            var result = _compare.Compare("The quick brown fox jumps over the lazy dog.", "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG.");

            //Assert
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }
        #endregion



    }
}
