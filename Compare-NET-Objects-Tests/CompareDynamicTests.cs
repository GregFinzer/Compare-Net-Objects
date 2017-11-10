using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareDynamicTests
    {
        [Test]
        public void DifferentDynamicObjectsShouldNotBeEqual()
        {
            dynamic human1 = new System.Dynamic.ExpandoObject();
            human1.Name = "John";
            human1.Surname = "Doe";
            human1.Interests = new string[] {"Swimming", "Books", "Biking"};
            human1.Stuff = new System.Dynamic.ExpandoObject();
            human1.Stuff.Foo = "bar";
            human1.Stuff.Hmm = 42;

            dynamic human2 = new System.Dynamic.ExpandoObject();
            human2.Name = "Jane";
            human2.Surname = "Doe";
            human2.Interests = new string[] { "Swimming", "Books", "Biking" };
            human2.Stuff = new System.Dynamic.ExpandoObject();
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
