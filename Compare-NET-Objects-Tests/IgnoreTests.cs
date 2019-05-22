using System;
using System.Dynamic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using KellermanSoftware.CompareNetObjectsTests.TestClasses.IgnoreExample;
using Newtonsoft.Json;
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

            Person person1 = new Person() {Name = "Darth Vader"};
            Person person2 = new Person() {Name = "Anakin Skywalker"};

            CompareLogic compare = new CompareLogic(config);

            var result = compare.Compare(person1, person2);
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void ExpandoObject()
        {
            DynamicContainer a = new DynamicContainer()
            {
                expando = JsonConvert.DeserializeObject<ExpandoObject>("{\"test\": \"1\"}")
            };

            DynamicContainer b = new DynamicContainer()
            {
                expando = JsonConvert.DeserializeObject<ExpandoObject>("{\"test\": \"2\"}")
            };

            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MembersToIgnore.Add("test");
            compareLogic.Config.CustomComparers.Add(new StringMightBeFloat(RootComparerFactory.GetRootComparer()));
            compareLogic.Config.MaxDifferences = 50;

            ComparisonResult result = compareLogic.Compare(a, b);

            Console.WriteLine(result.DifferencesString);
            Assert.IsTrue(result.AreEqual);
        }
    }
}