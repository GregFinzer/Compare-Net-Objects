using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsIntegrationTests
{
    [TestFixture]
    public class BugTestsIntegration
    {
#if !NETSTANDARD
        [Test]
        public void SaveAndLoadConfig()
        {
            //This is the comparison class
            CompareLogic compareLogic = new CompareLogic();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            compareLogic.SaveConfiguration(filePath);
            compareLogic.LoadConfiguration(filePath);


            //Create a couple objects to compare
            Person person1 = new Person();
            person1.DateCreated = DateTime.Now;
            person1.Name = "Greg";

            Person person2 = new Person();
            person2.Name = "John";
            person2.DateCreated = person1.DateCreated;

            ComparisonResult result = compareLogic.Compare(person1, person2);

            //These will be different, write out the differences
            if (!result.AreEqual)
                Console.WriteLine(result.DifferencesString);
            else
                Console.WriteLine("Objects are the same");
        }

        [Test]
        public void TypesToIgnoreShouldSerializeAndDeserialize()
        {
            //This is the comparison class
            CompareLogic compareLogic = new CompareLogic();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SerializationException.json");

            compareLogic.Config.TypesToIgnore.Add(typeof(Guid));
            compareLogic.SaveConfiguration(filePath);
            compareLogic.LoadConfiguration(filePath);
            Assert.IsTrue(compareLogic.Config.TypesToIgnore.Contains(typeof(Guid)));

            //Create a couple objects to compare
            Person person1 = new Person();
            person1.DateCreated = DateTime.Now;
            person1.Name = "Greg";

            Person person2 = new Person();
            person2.Name = "John";
            person2.DateCreated = person1.DateCreated;

            ComparisonResult result = compareLogic.Compare(person1, person2);

            //These will be different, write out the differences
            if (!result.AreEqual)
                Console.WriteLine(result.DifferencesString);
            else
                Console.WriteLine("Objects are the same");
        }

        [Test]
        public void HashsetStringsShouldSerializeAndDeserialize()
        {
            //This is the comparison class
            CompareLogic compareLogic = new CompareLogic();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HashsetStringTest.json");

            compareLogic.Config.MembersToIgnore.Add("CreateDate");
            compareLogic.Config.MembersToInclude.Add("OrderId");
            compareLogic.SaveConfiguration(filePath);
            compareLogic.LoadConfiguration(filePath);

            Assert.IsTrue(compareLogic.Config.MembersToIgnore.Contains(("CreateDate")));
            Assert.IsTrue(compareLogic.Config.MembersToInclude.Contains(("OrderId")));
        }
#endif
    }
}
