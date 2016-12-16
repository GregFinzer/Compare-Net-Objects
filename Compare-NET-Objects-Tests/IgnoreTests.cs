using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class IgnoreTests
    {
        [Test]
        public void IgnoreTypeMember()
        {
            ComparisonConfig config = new ComparisonConfig();
            config.MembersToIgnore.Add("Person.Name");

            Person person1 = new Person() {Name="Darth Vader"};
            Person person2 = new Person() { Name = "Anakin Skywalker" };

            CompareLogic compare = new CompareLogic(config);

            var result = compare.Compare(person1,person2);
            Assert.IsTrue(result.AreEqual);

        }
    }
}
