using System;
using System.Dynamic;
using System.Web.OData;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using Newtonsoft.Json;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareDynamicTests
    {
        private sealed class ExpandoObjectChild
        {
            public ExpandoObject Expando { get; set; }
        }

        [Test]
        public void CompareLogicShouldHandleExpandoChildObjects()
        {
            dynamic childX = new ExpandoObject();
            childX.A = 1;
            dynamic childY = new ExpandoObject();
            childY.B = 2;

            var x = new ExpandoObjectChild
            {
                Expando = childX as ExpandoObject,
            };
            var y = new ExpandoObjectChild
            {
                Expando = childY as ExpandoObject,
            };

            var compareLogic = new CompareLogic();
            var result = compareLogic.Compare(x, y);
            Assert.False(result.AreEqual);
        }

        [Test]
        public void CompareLogicShouldHandleExpandoObjects()
        {
            dynamic x = new ExpandoObject();
            x.A = 1;
            dynamic y = new ExpandoObject();
            x.B = 2;

            var compareLogic = new CompareLogic();
            var result = compareLogic.Compare(x as ExpandoObject, y as ExpandoObject);
            Assert.False(result.AreEqual);
        }

        [Test]
        public void CompareLogicShouldHandleDeserializedJsonExpandoObjects()
        {
            var x = JsonConvert.DeserializeObject<ExpandoObject>("{ \"A\" : 1 }");
            var y = JsonConvert.DeserializeObject<ExpandoObject>("{ \"B\" : 1 }");

            var compareLogic = new CompareLogic();
            var result = compareLogic.Compare(x, y);
            Assert.False(result.AreEqual);
        }

        [Test]
        public void DifferentDynamicObjectsShouldNotBeEqual()
        {
            dynamic human1 = new ExpandoObject();
            human1.Name = "John";
            human1.Surname = "Doe";
            human1.Interests = new[] {"Swimming", "Books", "Biking"};
            human1.Stuff = new ExpandoObject();
            human1.Stuff.Foo = "bar";
            human1.Stuff.Hmm = 42;

            dynamic human2 = new ExpandoObject();
            human2.Name = "Jane";
            human2.Surname = "Doe";
            human2.Interests = new[] {"Swimming", "Books", "Biking"};
            human2.Stuff = new ExpandoObject();
            human2.Stuff.Foo = "bar";
            human2.Stuff.Hmm = 42;

            var oc = new CompareLogic();
            oc.Config.IgnoreObjectTypes = true;
            oc.Config.CompareChildren = true;

            var result = oc.Compare(human1, human2);
            Console.WriteLine(result.DifferencesString);
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void CompareDynamicObjectWithUnderlyingType()
        {
            SimpleEntity x = new SimpleEntity();

            dynamic dynamicX = x;
            dynamicX.Name = nameof(SimpleEntity);
            dynamicX.Id = typeof(SimpleEntity).GUID;

            SimpleEntity y = new SimpleEntity();
            dynamic dynamicY = y;
            dynamicY.Name = nameof(SimpleEntity);
            dynamicY.Id = typeof(SimpleEntity).GUID;

            CompareLogic comparer = new CompareLogic();
            ComparisonResult result = comparer.Compare(dynamicX, dynamicY);

            Assert.True(result.AreEqual);
        }

        [Test]
        public void CompareDynamicObjectWithUnderlyingTypeNegative()
        {
            SimpleEntity x = new SimpleEntity();

            dynamic dynamicX = x;
            dynamicX.Name = nameof(SimpleEntity);
            dynamicX.Id = typeof(SimpleEntity).GUID;

            SimpleEntity y = new SimpleEntity();
            dynamic dynamicY = y;
            dynamicY.Name = nameof(SimpleEntity);
            dynamicY.Id = Guid.NewGuid();

            CompareLogic comparer = new CompareLogic();
            ComparisonResult result = comparer.Compare(dynamicX, dynamicY);

            Assert.False(result.AreEqual);
        }

        [Test]
        public void CompareAnnonymousType()
        {
            dynamic myDynamic1 = new {PropertyOne = true, PropertyTwo = false};
            dynamic myDynamic2 = new {PropertyOne = true, PropertyTwo = false};

            CompareLogic comparer = new CompareLogic();
            ComparisonResult result = comparer.Compare(myDynamic1, myDynamic2);

            Assert.True(result.AreEqual);
        }

        [Test]
        public void CompareAnnonymousTypeNegative()
        {
            dynamic myDynamic1 = new {PropertyOne = true, PropertyTwo = false};
            dynamic myDynamic2 = new {PropertyOne = true, PropertyTwo = true};

            CompareLogic comparer = new CompareLogic();
            ComparisonResult result = comparer.Compare(myDynamic1, myDynamic2);

            Assert.False(result.AreEqual);
        }

        [Test]
        public void CompareDynamicOData()
        {
            Delta<SimpleEntity> x = new Delta<SimpleEntity>();

            dynamic dynamicX = x;
            dynamicX.Name = nameof(SimpleEntity);
            dynamicX.Id = typeof(SimpleEntity).GUID;

            Delta<SimpleEntity> y = new Delta<SimpleEntity>();

            dynamic dynamicY = y;
            dynamicY.Name = nameof(SimpleEntity);
            dynamicY.Id = typeof(SimpleEntity).GUID;

            CompareLogic comparer = new CompareLogic();
            ComparisonResult result = comparer.Compare(y, x);

            Assert.True(result.AreEqual);
        }

        [Test]
        public void CompareDynamicObjectTest()
        {
            dynamic contact1 = new DynamicXMLNode("contact1s");
            contact1.Name = "Patrick Hines";
            contact1.Phone = "206-555-0144";
            contact1.Address = new DynamicXMLNode();
            contact1.Address.Street = "123 Main St";
            contact1.Address.City = "Mercer Island";
            contact1.Address.State = "WA";
            contact1.Address.Postal = "68402";

            dynamic contact2 = new DynamicXMLNode("contact2s");
            contact2.Name = "Patrick Hines";
            contact2.Phone = "206-555-0144";
            contact2.Address = new DynamicXMLNode();
            contact2.Address.Street = "123 Main St";
            contact2.Address.City = "Mercer Island";
            contact2.Address.State = "WA";
            contact2.Address.Postal = "68402";

            CompareLogic comparer = new CompareLogic();
            ComparisonResult result = comparer.Compare(contact1, contact2);

            Assert.True(result.AreEqual);
        }

        [Test]
        public void CompareDynamicObjectTestNegative()
        {
            dynamic contact1 = new DynamicXMLNode("contact1s");
            contact1.Name = "Patrick Hines";
            contact1.Phone = "206-555-0144";
            contact1.Address = new DynamicXMLNode();
            contact1.Address.Street = "123 Main St";
            contact1.Address.City = "Mercer Island";
            contact1.Address.State = "WA";
            contact1.Address.Postal = "68402";

            dynamic contact2 = new DynamicXMLNode("contact2s");
            contact2.Name = "Patrick Hines";
            contact2.Phone = "206-555-0144";
            contact2.Address = new DynamicXMLNode();
            contact2.Address.Street = "123 Main St";
            contact2.Address.City = "Beverly Hills";
            contact2.Address.State = "CA";
            contact2.Address.Postal = "90210";

            CompareLogic comparer = new CompareLogic();
            ComparisonResult result = comparer.Compare(contact1, contact2);

            Assert.False(result.AreEqual);
        }
    }
}
