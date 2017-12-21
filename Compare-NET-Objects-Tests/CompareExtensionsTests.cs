using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareExtensionsTests
    {
        [Test]
        public void ShouldCompare_When_Not_Equal_Should_Throw_An_Exception()
        {
            //Arrange
            string errorMessage = "Groups should be equal";
            var people1 = new List<Person>() { new Person() { Name = "Joe" } };
            var people2 = new List<Person>() { new Person() { Name = "Sue" } };
            var group1 = new KeyValuePair<string, List<Person>>("People", people1);
            var group2 = new KeyValuePair<string, List<Person>>("People", people2);

            try
            {
                //Act
                group1.ShouldCompare(group2, errorMessage);
            }
            catch (CompareException ex)
            {
                string result = ex.ToString();
                Console.WriteLine(result);

                //Assert
                Assert.IsFalse(ex.Result.AreEqual);
                Assert.AreEqual(1, ex.Result.Differences.Count);
                Assert.IsTrue(result.Contains(errorMessage));
            }
        }

        [Test]
        public void ShouldCompare_When_Equal_Should__Not_Throw_An_Exception()
        {
            //Arrange
            string errorMessage = "Groups should be equal";
            var people1 = new List<Person>() { new Person() { Name = "Joe" } };
            var people2 = new List<Person>() { new Person() { Name = "Joe" } };
            var group1 = new KeyValuePair<string, List<Person>>("People", people1);
            var group2 = new KeyValuePair<string, List<Person>>("People", people2);

            //Act
            group1.ShouldCompare(group2, errorMessage);
        }

        [Test]
        public void ShouldNotCompare_When_Equal_Should_Throw_An_Exception()
        {
            //Arrange
            string errorMessage = "Groups should NOT be equal";
            var people1 = new List<Person>() { new Person() { Name = "Joe" } };
            var people2 = new List<Person>() { new Person() { Name = "Joe" } };
            var group1 = new KeyValuePair<string, List<Person>>("People", people1);
            var group2 = new KeyValuePair<string, List<Person>>("People", people2);

            try
            {
                //Act
                group1.ShouldNotCompare(group2, errorMessage);
            }
            catch (CompareException ex)
            {
                string result = ex.ToString();
                Console.WriteLine(result);

                //Assert
                Assert.IsTrue(ex.Result.AreEqual);
                Assert.AreEqual(0, ex.Result.Differences.Count);
                Assert.IsTrue(result.Contains(errorMessage));
            }
        }

        [Test]
        public void ShouldNotCompare_When_Not_Equal_Should__Not_Throw_An_Exception()
        {
            //Arrange
            string errorMessage = "Groups should be equal";
            var people1 = new List<Person>() { new Person() { Name = "Joe" } };
            var people2 = new List<Person>() { new Person() { Name = "Sue" } };
            var group1 = new KeyValuePair<string, List<Person>>("People", people1);
            var group2 = new KeyValuePair<string, List<Person>>("People", people2);

            //Act
            group1.ShouldNotCompare(group2, errorMessage);
        }
    }
}
