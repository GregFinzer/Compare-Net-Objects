// LogicEqualityComparerTests: Initial contribution by David Rieman

using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class LogicEqualityComparerTests
    {
        private LogicEqualityComparer _objectComparer;
        private LogicEqualityComparer<KeyValuePair<string, List<Person>>> _typedComparer;
        private Person _abe, _joe1, _joe2, _sue;

        /// <summary>Code that is run before each test.</summary>
        [SetUp]
        public void Initialize()
        {
            _objectComparer = new LogicEqualityComparer();
            _typedComparer = new LogicEqualityComparer<KeyValuePair<string, List<Person>>>();
            _abe = new Person() { Name = "Abe" };
            _joe1 = new Person() { Name = "Joe" };
            _joe2 = new Person() { Name = "Joe" };
            _sue = new Person() { Name = "Sue" };
        }

        [Test]
        public void LogicEqualityComparerDeepEqual()
        {
            var group1 = new KeyValuePair<string, List<Person>>("People", new List<Person>() { _joe1 });
            var group2 = new KeyValuePair<string, List<Person>>("People", new List<Person>() { _joe2 });
            Assert.IsTrue(_objectComparer.Equals(group1, group2));
            Assert.IsTrue(_typedComparer.Equals(group1, group2));
        }

        [Test]
        public void LogicEqualityComparerDeepNotEqual()
        {
            var group1 = new KeyValuePair<string, List<Person>>("People", new List<Person>() { _abe });
            var group2 = new KeyValuePair<string, List<Person>>("People", new List<Person>() { _sue });
            Assert.IsFalse(_objectComparer.Equals(group1, group2));
            Assert.IsFalse(_typedComparer.Equals(group1, group2));
        }

        [Test]
        public void LogicEqualityComparerDeepLinqExcept()
        {
            var group1 = new KeyValuePair<string, List<Person>>("People", new List<Person>() { _abe });
            var group2 = new KeyValuePair<string, List<Person>>("People", new List<Person>() { _sue });
            var groups = new List<KeyValuePair<string, List<Person>>>() { group1, group2 };
            var groupsWithJoe = new List<KeyValuePair<string, List<Person>>>() { group1 };
            var groupsWithoutJoe = groups.Except(groupsWithJoe, _typedComparer);
            Assert.IsTrue(groupsWithoutJoe.Count() == 1);
            Assert.IsTrue(groupsWithoutJoe.First().Value.First().Name == "Sue");
        }

        [Test]
        public void LogicEqualityComparerLinqExcept()
        {
            var people = new List<Person>() { _abe, _joe1, _sue, _joe2 };
            var peopleWhoAreJoe = new List<Person>() { _joe1 };
            var comparer = new LogicEqualityComparer<Person>();
            var peopleWhoAreNotJoe = people.Except(peopleWhoAreJoe, comparer);
            Assert.IsTrue(peopleWhoAreNotJoe.Count() == 2);
            Assert.IsFalse(peopleWhoAreNotJoe.Where(p => p.Name == "Joe").Any());
        }

        [Test]
        public void LogicEqualityComparerIgnoresInstanceSensitiveHashes()
        {
            Assert.IsTrue(_objectComparer.GetHashCode(_joe1) == _objectComparer.GetHashCode(_joe2));
        }

        [Test]
        public void LogicEqualityComparerUsesObjectHashesIfConfiguredTo()
        {
            _objectComparer.UseObjectHashes = true;
            Assert.IsTrue(_objectComparer.GetHashCode(_joe1) != _objectComparer.GetHashCode(_joe2));
        }
    }
}