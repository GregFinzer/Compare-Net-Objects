using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
			var config = new ComparisonConfig();
			config.MembersToIgnore.Add("Person.Name");

			var person1 = new Person {Name = "Darth Vader"};
			var person2 = new Person {Name = "Anakin Skywalker"};

			var compare = new CompareLogic(config);

			var result = compare.Compare(person1, person2);
			Assert.IsTrue(result.AreEqual);
		}

		[Test]
		public void IgnoreTypeMemberUsingIgnoreProperty()
		{
			var config = new ComparisonConfig();
			config.IgnoreProperty<Person>(x => x.Name);

			var person1 = new Person { Name = "Darth Vader" };
			var person2 = new Person { Name = "Anakin Skywalker" };

			var compare = new CompareLogic(config);

			var result = compare.Compare(person1, person2);
			Assert.IsTrue(result.AreEqual);
		}

		[Test]
		public void IgnoreUnaryTypeMemberUsingIgnoreProperty()
		{
			var config = new ComparisonConfig();
			config.IgnoreProperty<Person>(x => x.DateModified);

			var person1 = new Person { DateModified = new DateTime(2019, 5, 23) };
			var person2 = new Person { DateModified = new DateTime(2015, 1, 2)};

			var compare = new CompareLogic(config);

			var result = compare.Compare(person1, person2);
			Assert.IsTrue(result.AreEqual);
		}

		[Test]
		public void UsingIgnorePropertyThrowsWithField()
		{
			var config = new ComparisonConfig();
			Assert.Throws<ArgumentException>(() =>
				config.IgnoreProperty<Person>(x => x.DateCreated));
		}

		[Test]
		public void UsingIgnorePropertyThrowsWithMethod()
		{
			var config = new ComparisonConfig();
			Assert.Throws<ArgumentException>(() =>
				config.IgnoreProperty<Person>(x => x.GetAge()));
		}

		[Test]
		public void ExpandoObject()
		{
			var a = new DynamicContainer
			{
				expando = JsonConvert.DeserializeObject<ExpandoObject>("{\"test\": \"1\"}")
			};

			var b = new DynamicContainer
			{
				expando = JsonConvert.DeserializeObject<ExpandoObject>("{\"test\": \"2\"}")
			};

			var compareLogic = new CompareLogic();
			compareLogic.Config.MembersToIgnore.Add("test");
			compareLogic.Config.CustomComparers.Add(new StringMightBeFloat(RootComparerFactory.GetRootComparer()));
			compareLogic.Config.MaxDifferences = 50;

			var result = compareLogic.Compare(a, b);

			Console.WriteLine(result.DifferencesString);
			Assert.IsTrue(result.AreEqual);
		}

		[Test]
		public void IgnoreBaseClassPropertyUsingIgnoreProperty()
		{
			var config = new ComparisonConfig();
			config.IgnoreProperty<Officer>(x => x.ID);

			var deriveFromOfficer1 = new DeriveFromOfficer {HomeAddress = "Address", ID = 1, Name = "John", Type = Deck.Engineering};
			var deriveFromOfficer2 = new DeriveFromOfficer {HomeAddress = "Address", ID = 2, Name = "John", Type = Deck.Engineering};

			var derive2FromOfficer1 = new Derive2FromOfficer { Email = "a@a.com", ID = 3, Name = "John", Type = Deck.Engineering };
			var derive2FromOfficer2 = new Derive2FromOfficer { Email = "a@a.com", ID = 4, Name = "John", Type = Deck.Engineering };

			var compare = new CompareLogic(config);

			var result = compare.Compare(deriveFromOfficer1, deriveFromOfficer2);
			Assert.IsTrue(result.AreEqual);

			result = compare.Compare(derive2FromOfficer1, derive2FromOfficer2);
			Assert.IsTrue(result.AreEqual);
		}

		[Test]
		public void IgnoreDerivedClassPropertyUsingIgnoreProperty()
		{
			var config = new ComparisonConfig();
			config.IgnoreProperty<DeriveFromOfficer>(x => x.ID);

			var deriveFromOfficer1 = new DeriveFromOfficer { HomeAddress = "Address", ID = 1, Name = "John", Type = Deck.Engineering };
			var deriveFromOfficer2 = new DeriveFromOfficer { HomeAddress = "Address", ID = 2, Name = "John", Type = Deck.Engineering };

			var derive2FromOfficer1 = new Derive2FromOfficer { Email = "a@a.com", ID = 3, Name = "John", Type = Deck.Engineering };
			var derive2FromOfficer2 = new Derive2FromOfficer { Email = "a@a.com", ID = 4, Name = "John", Type = Deck.Engineering };

			var compare = new CompareLogic(config);

			var result = compare.Compare((Officer)deriveFromOfficer1, (Officer)deriveFromOfficer2);
			Assert.IsTrue(result.AreEqual);

			result = compare.Compare(derive2FromOfficer1, derive2FromOfficer2);
			Assert.IsFalse(result.AreEqual);
		}


        [Test]
        public void IgnoreReadonlyCollectionCount()
        {
            var coll1 = new List<int>
            {
                1, 2, 3, 4, 5
            }.AsReadOnly();
            var coll2 = new List<int>
            {
                1, 2, 3, 4, 5, 6
            }.AsReadOnly();
            var compareLogic = new CompareLogic
            {
                Config = new ComparisonConfig
                {
                    MaxDifferences = int.MaxValue,
                    CompareReadOnly = false
                }
            };

            var result = compareLogic.Compare(coll1, coll2);
            Assert.IsFalse(result.Differences.Any(difference => difference.ChildPropertyName == "Count"));
            Assert.AreEqual(result.Differences.Count, 0);

            var obj1 = new
            {
                Name = "test",
                Collection = coll1
            };
            var obj2 = new
            {
                Name = "test",
                Collection = coll2
            };

            result = compareLogic.Compare(obj1, obj2);
            Assert.IsFalse(result.Differences.Any(difference => difference.ChildPropertyName == "Count"));
            Assert.AreEqual(result.Differences.Count, 0);

            compareLogic = new CompareLogic
            {
                Config = new ComparisonConfig
                {
                    MaxDifferences = int.MaxValue,
                    CompareReadOnly = true
                }
            };

            result = compareLogic.Compare(obj1, obj2);
            Assert.IsTrue(result.Differences.Any(difference => difference.ChildPropertyName == "Count"));
            Assert.AreEqual(result.Differences.Count, 1);
        }
    }
}