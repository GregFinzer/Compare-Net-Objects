using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;
using System;
using System.Linq;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class MissingMemberTests
    {
        private static readonly PersonSummary PersonSummary = new PersonSummary
        {
            ID = 1234,
            Name = "Humpty",
            LastName = "Dumpty"
        };

        private static readonly Person Person = new Person
        {
            ID = 1234,
            Name = "Humpty",
            LastName = "Dumpty",
            Age = 101,
            DateCreated = DateTime.Now,
            DateModified = DateTime.Now
        };

        [Test]
        public void ShouldIncludeMissingMembersInDifferencesWhenConfigured()
        {
            var testInstance = new CompareLogic();
            testInstance.Config.IgnoreObjectTypes = true;
            testInstance.Config.IgnoreMissingProperties = false;
            testInstance.Config.IgnoreMissingFields = false;
            testInstance.Config.MaxDifferences = 100;

            var result = testInstance.Compare(
                Person,
                PersonSummary);

            Assert.IsFalse(result.AreEqual);
            Assert
                .IsTrue(result.Differences.Select(x => x.PropertyName)
                .Contains(nameof(Person.Age)),
                "Differences contains the missing Age property");
            Assert
                .IsTrue(result.Differences.Select(x => x.PropertyName)
                .Contains(nameof(Person.DateModified)),
                "Differences contains the missing DateModified property");
            Assert
                .IsTrue(result.Differences.Select(x => x.PropertyName)
                .Contains(nameof(Person.DateCreated)),
                "Differences contains the missing DateCreated property");
        }

        [Test]
        public void ShouldNotIncludeMissingMembersInDifferencesWhenNotConfigured()
        {
            var testInstance = new CompareLogic();
            testInstance.Config.IgnoreObjectTypes = true;
            testInstance.Config.MaxDifferences = 100;

            var result = testInstance.Compare(
                Person,
                PersonSummary);

            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void ShouldIncludeMissingMembersInDifferencesUnlessExplicitlyIgnored()
        {
            var testInstance = new CompareLogic();
            testInstance.Config.IgnoreObjectTypes = true;
            testInstance.Config.IgnoreMissingProperties = false;
            testInstance.Config.IgnoreMissingFields = false;
            testInstance.Config.IgnoreProperty<Person>(x => x.DateModified);
            testInstance.Config.MaxDifferences = 100;

            var result = testInstance.Compare(
                Person,
                PersonSummary);

            Assert.IsFalse(result.AreEqual);
            Assert
                .IsTrue(result.Differences.Select(x => x.PropertyName)
                .Contains(nameof(Person.Age)),
                "Differences contains the missing Age property");
            Assert
                .IsFalse(result.Differences.Select(x => x.PropertyName)
                .Contains(nameof(Person.DateModified)),
                "Differences does not contain the missing DateModified property as it is explicitly ignored");
            Assert
                .IsTrue(result.Differences.Select(x => x.PropertyName)
                .Contains(nameof(Person.DateCreated)),
                "Differences contains the missing DateCreated property");
        }
    }
}
