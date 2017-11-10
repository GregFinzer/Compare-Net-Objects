#region Includes
using System;
using System.Collections.Generic;
using System.Diagnostics;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;
#endregion

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture(Description = "Tests for CompareLogic"), Category("CompareLogic")]
    public class CompareLogicTests
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

        #region Documentation Tests
        [Test]
        public void DocumentationTest()
        {
            //This is the comparison class
            CompareLogic compareLogic = new CompareLogic();

            //Create a couple objects to compare
            Person person1 = new Person();
            person1.DateCreated = DateTime.Now;
            person1.Name = "Greg";

            Person person2 = new Person();
            person2.Name = "John";
            person2.DateCreated = person1.DateCreated;

            //These will be different, write out the differences
            ComparisonResult result = compareLogic.Compare(person1, person2);
            if (!result.AreEqual)
                Console.WriteLine(result.DifferencesString);
        }

        #endregion












    }
}
