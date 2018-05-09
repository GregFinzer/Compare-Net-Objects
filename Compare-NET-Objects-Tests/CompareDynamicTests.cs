using System;
using System.Dynamic;
using KellermanSoftware.CompareNetObjects;
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
            human1.Interests = new[] { "Swimming", "Books", "Biking" };
            human1.Stuff = new ExpandoObject();
            human1.Stuff.Foo = "bar";
            human1.Stuff.Hmm = 42;

            dynamic human2 = new ExpandoObject();
            human2.Name = "Jane";
            human2.Surname = "Doe";
            human2.Interests = new[] { "Swimming", "Books", "Biking" };
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
    }
}
