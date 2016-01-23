using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.Attributes;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Point = System.Drawing.Point;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class BugTests
    {
        #region Class Variables
        private CompareLogic _compare;
        #endregion

        #region Setup/Teardown

        /// <summary>
        /// Code that is run once for a suite of tests
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {

        }

        /// <summary>
        /// Code that is run once after a suite of tests has finished executing
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {

        }

        /// <summary>
        /// Code that is run before each test
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            _compare = new CompareLogic();
        }

        /// <summary>
        /// Code that is run after each test
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            _compare = null;
        }
        #endregion

        #region Tests

        [Test]
        public void PropertyComparerFailsWithObjectNullException()
        {
            //This is the comparison class
            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.SkipInvalidIndexers = true;

            //Create a couple objects to compare
            Person2 person1 = new Person2();
            person1.DateCreated = DateTime.Now;
            person1.Name = "Greg";

            Person2 person2 = new Person2();
            person2.Name = "John";
            person2.DateCreated = person1.DateCreated;

            //These will be different, write out the differences
            ComparisonResult result = compareLogic.Compare(person1, person2);
            if (!result.AreEqual)
                Console.WriteLine(result.DifferencesString);
        }

        [Test]
        public void GermanUmlautsAndAccents()
        {
            string string1 = "straße";
            string string2 = "strasse";

            ComparisonResult result = _compare.Compare(string1, string2);
            Console.WriteLine(result.DifferencesString);
            Assert.IsFalse(result.AreEqual);
        }

        public class Foo
        {
        }

        public class Bar
        {
        }

        [Test]
        public void GenericEnumerable()
        {
            List<Foo> fooList = new List<Foo>();
            IEnumerable<Bar> barEnumerable = fooList.Select(f => new Bar());
            List<Bar> barList = new List<Bar>();

            CompareLogic logic = new CompareLogic();
            logic.Config.IgnoreObjectTypes = true;

            ComparisonResult result = logic.Compare(barEnumerable, barList);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [Test]
        public void ObjectTypeObjectTest()
        {
            ObjectTypeClass objectClass1 = new ObjectTypeClass();
            objectClass1.FieldObject = new object();
            objectClass1.PropertyObject = new object();
            ObjectTypeClass.StaticObject = new object();

            ObjectTypeClass objectClass2 = new ObjectTypeClass();
            objectClass2.FieldObject = new object();
            objectClass2.PropertyObject = new object();

            ComparisonResult result = _compare.Compare(objectClass1, objectClass2);

            if (!result.AreEqual)
                Assert.Fail(result.DifferencesString);
        }

        [Test]
        public void IgnoreTypesTest()
        {
            ExampleDto1 dto1 = new ExampleDto1();
            dto1.Name = "Greg";

            ExampleDto2 dto2 = new ExampleDto2();
            dto2.Name = "Greg";

            ComparisonResult result = _compare.Compare(dto1, dto2);

            //These will be different because the types are different
            Assert.IsFalse(result.AreEqual);
            Console.WriteLine(result.DifferencesString);

            _compare.Config.IgnoreObjectTypes = true;

            result = _compare.Compare(dto1, dto2);

            //Ignore types so they will be equal
            Assert.IsTrue(result.AreEqual);
            
            _compare.Config.Reset();
            
        }






        [Test]
        public void TankElementsToIncludeTest()
        {
            _compare.Config.MaxDifferences = 10;

            _compare.Config.MembersToInclude.Add("TankPerson");
            _compare.Config.MembersToInclude.Add("TankName");
            _compare.Config.MembersToInclude.Add("Name");
            _compare.Config.MembersToInclude.Add("FamilyName");
            _compare.Config.MembersToInclude.Add("GivenName");

            //Create a couple objects to compare
            TankPerson person1 = new TankPerson()
            {
                Id = 1,
                DateCreated = DateTime.Now,
                Name = new TankName { FamilyName = "Huston", GivenName = "Greg" },
                Address = "Address1"
            };
            TankPerson person2 = new TankPerson()
            {
                Id = 2,
                Name = new TankName { FamilyName = "McClane", GivenName = "John" },
                DateCreated = DateTime.UtcNow,
                Address = "Address2"
            };

            ComparisonResult result = _compare.Compare(person1, person2);
            Assert.IsFalse(result.AreEqual);

            //These will be different, write out the differences
            if (!result.AreEqual)
            {
                Console.WriteLine("------");

                Console.WriteLine("###################");
                result.Differences.ForEach(d => Console.WriteLine(d.ToString()));
            }

            _compare.Config.MembersToInclude.Clear();
            _compare.Config.MaxDifferences = 1;
        }

        private Shipment CreateShipment()
        {
            return new Shipment { Customer = "ADEG", IdentCode = 12934871928374, InsertDate = new DateTime(2012, 06, 12) };
        }

        [Test]
        public void IgnoreByAttribute_test_should_fail_difference_should_be_customer()
        {
            // Arrange
            Shipment shipment1 = CreateShipment();
            Shipment shipment2 = CreateShipment();
            shipment2.InsertDate = DateTime.Now; // InsertDate has the CompareIgnoreAttribute on it
            shipment2.Customer = "Andritz";

            _compare.Config.AttributesToIgnore.Add(typeof(CompareIgnoreAttribute));
            _compare.Config.MaxDifferences = int.MaxValue;

            // Act
            var result = _compare.Compare(shipment1, shipment2);

            // Assert
            Assert.IsFalse(result.AreEqual);
            Assert.AreEqual(1, result.Differences.Count);
            Console.WriteLine(result.DifferencesString);
            Assert.AreEqual("ADEG", result.Differences[0].Object1Value);
            Assert.AreEqual("Andritz", result.Differences[0].Object2Value);

            _compare.Config.AttributesToIgnore.Clear();
        }

        [Test]
        public void WilliamCWarnerTest()
        {
            ILabTest labTest = new LabTest();
            labTest.AlternateContainerDescription = "Test 1";
            labTest.TestName = "Test The Audit";

            ILabTest origLabLest = new LabTest();//this would be in session
            origLabLest.TestName = "Original Test Name";
            origLabLest.AlternateContainerDescription = "Test 2";

            _compare.Config.MaxDifferences = 500;
            var result = _compare.Compare(labTest, origLabLest);

            Assert.IsFalse(result.AreEqual);
            Assert.IsTrue(result.Differences.Count > 0);
            Console.WriteLine(result.DifferencesString);
        }

        [Test]
        public void LinearGradient()
        {
            LinearGradientBrush brush1 = new LinearGradientBrush(new Point(), new Point(0, 10), Color.Red, Color.Red);
            LinearGradientBrush brush2 = new LinearGradientBrush(new Point(), new Point(0, 10), Color.Red, Color.Blue);

            Assert.IsFalse(_compare.Compare(brush1, brush2).AreEqual);
        }

        #endregion
    }
}