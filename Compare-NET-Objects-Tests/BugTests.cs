using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.Attributes;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;
using System.Drawing;
using System.Linq;
using System.Reflection;
using KellermanSoftware.CompareNetObjects.Reports;
using Point = System.Drawing.Point;

#if !NETSTANDARD
using System.Drawing.Drawing2D;
#endif

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
        public void TimespanShouldCompareWhenCompareChildrenIsFalse()
        {
            //Arrange
            var object1 = new ClassWithTimespan() { MyTimeSpan = new TimeSpan(6, 0, 0) };
            var object2 = new ClassWithTimespan { MyTimeSpan = new TimeSpan(7, 0, 0) };

            var comparerConfig = new ComparisonConfig();
            comparerConfig.CompareChildren = false;
            var comparer = new CompareLogic(comparerConfig);

            //Act
            var result = comparer.Compare(object1, object2);

            //Assert
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void GetPrivatePropertiesNetStandard()
        {
            //Arrange
            Type type = typeof(ClassWithPrivateProperties);

            //Act
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //Assert
            Assert.IsTrue(props.Length > 0);
        }

        /// <summary>
        /// https://github.com/GregFinzer/Compare-Net-Objects/issues/77
        /// </summary>
        [Test]
        public void ComparisonsOfTypesWithPrivateFieldsAreAccurate()
        {
            var compareLogic = new CompareLogic(new ComparisonConfig { ComparePrivateFields = true });
            var result = compareLogic.Compare(new SomethingWithPrivateField(123), new SomethingWithPrivateField(456));
            Assert.IsFalse(result.AreEqual);
        }

        private class SomethingWithPrivateField
        {
            private readonly int _key;
            public SomethingWithPrivateField(int key) { _key = key; }
        }

        /// <summary>
        /// https://github.com/GregFinzer/Compare-Net-Objects/issues/77
        /// </summary>
        [Test]
        public void ComparisonsOfTypesWithPrivatePropertiesAreAccurate()
        {
            var compareLogic = new CompareLogic(new ComparisonConfig { ComparePrivateProperties = true });
            var result = compareLogic.Compare(new SomethingWithPrivateProperty(123), new SomethingWithPrivateProperty(456));
            Assert.IsFalse(result.AreEqual);
        }

        private class SomethingWithPrivateProperty
        {
            public SomethingWithPrivateProperty(int key) { Key = key; }
            private int Key { get; }
        }

        /// <summary>
        /// https://github.com/GregFinzer/Compare-Net-Objects/issues/110
        /// </summary>
        [Test]
        public void CsvReportWithCommaTest()
        {
            // set up data
            Person person1 = new Person();
            person1.Name = "Greg";
            person1.LastName = "Miller";
            person1.Age = 42;

            Person person2 = new Person();
            person2.Name = "Greg";
            person2.LastName = "Miller";
            person2.Age = 17;

            // compare
            var left = new List<Person> { person1 };
            var right = new List<Person> { person2 };

            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.IgnoreCollectionOrder = true;
            compareLogic.Config.CollectionMatchingSpec.Add(
                typeof(Person),
                new string[] { "Name", "LastName" });   // specify two indexes

            ComparisonResult result = compareLogic.Compare(left, right);

            // write to csv
            var csv = new CsvReport();
            string output = csv.OutputString(result.Differences);
            Console.WriteLine(output);
            Assert.IsTrue(output.Contains("\"[Name:Greg,LastName:Miller].Age\""));
        }

        [Test]
        public void DifferentNullableDecimalFieldsShouldNotBeEqualWhenCompareChildrenIsFalse()
        {
            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.CompareChildren = false;

            PrimitiveFieldsNullable object1 = new PrimitiveFieldsNullable();
            object1.DecimalField = 0;

            PrimitiveFieldsNullable object2 = new PrimitiveFieldsNullable();
            object2.DecimalField = 3.0M;

            Assert.IsFalse(compareLogic.Compare(object1, object2).AreEqual);
        }

        [Test]
        public void DifferentDecimalFieldsShouldNotBeEqualWhenCompareChildrenIsFalse()
        {
            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.CompareChildren = false;

            PrimitiveFields object1 = new PrimitiveFields();
            object1.DecimalField = 0;

            PrimitiveFields object2 = new PrimitiveFields();
            object2.DecimalField = 3.0M;

            Assert.IsFalse(compareLogic.Compare(object1, object2).AreEqual);
        }

        [Test]
        public void DifferentIntegersShouldNotBeEqualInsideAnAnonymousType()
        {
            CompareLogic compareLogic = new CompareLogic();

            // try with integers
            var int1 = new { MyNumber = (int)0 };
            var int2 = new { MyNumber = (int)3 };

            // test with CompareChildren = false
            compareLogic.Config.CompareChildren = false;
            ComparisonResult test3 = compareLogic.Compare(int1, int2);
            Assert.IsFalse(test3.AreEqual, "int Test - CompareChildren = false");

            // test with CompareChildren = true
            compareLogic.Config.CompareChildren = true;
            ComparisonResult test4 = compareLogic.Compare(int1, int2);
            Assert.AreEqual(1, test4.Differences.Count);
            Assert.IsFalse(test4.AreEqual, "int Test - CompareChildren = true");
        }

        [Test]
        public void DifferentDecimalsShouldNotBeEqualInsideAnAnonymousType()
        {
            CompareLogic compareLogic = new CompareLogic();

            // try with decimals
            var dec1 = new { MyNumber = (decimal)0 };
            var dec2 = new { MyNumber = (decimal)3.0 };

            // test with CompareChildren = false
            compareLogic.Config.CompareChildren = false;
            ComparisonResult test1 = compareLogic.Compare(dec1, dec2);
            Assert.IsFalse(test1.AreEqual, "Decimal Test - CompareChildren = false");

            // test with CompareChildren = true
            compareLogic.Config.CompareChildren = true;
            ComparisonResult test2 = compareLogic.Compare(dec1, dec2);
            Assert.IsFalse(test2.AreEqual, "Decimal Test - CompareChildren = true");
        }

        [Test]
        public void NullableDecimalWithCompareChildrenFalseShouldSendADifferenceCallback()
        {
            List<Difference> differences = new List<Difference>();

            SpecialFields specialFields1 = new SpecialFields();
            specialFields1.NullableDecimalProperty = 1000;

            SpecialFields specialFields2 = new SpecialFields();
            specialFields2.NullableDecimalProperty = 2000;

            _compare.Config = new ComparisonConfig()
            {
                CompareChildren = false,
                DifferenceCallback = difference => { differences.Add(difference); }
            };

            _compare.Compare(specialFields1, specialFields2);
            Assert.That(differences.FirstOrDefault(), Is.Not.Null);
        }

        [Test]
        public void PropertyNameShouldNotHaveAPeriodInFrontOfIt()
        {
            Person person1 = new Person() {Name = "Luke Skywalker", DateCreated = DateTime.Today, ID = 1};
            Person person2 = new Person() { Name = "Leia Skywalker", DateCreated = DateTime.Today, ID = 1 };

            var result = _compare.Compare(person1, person2);

            Assert.IsFalse(result.AreEqual, "Expected to be different");
            Assert.IsFalse(result.Differences[0].PropertyName.StartsWith("."), "Expected not to start with period");

        }

        [Test]
        public void ShowBreadCrumbTest()
        {
            var people1 = new List<Person>() { new Person() { Name = "Joe" } };
            var people2 = new List<Person>() { new Person() { Name = "Joe" } };
            var group1 = new KeyValuePair<string, List<Person>>("People", people1);
            var group2 = new KeyValuePair<string, List<Person>>("People", people2);
            _compare.Config.ShowBreadcrumb = true;
            var result = _compare.Compare(group1, group2);
            Assert.IsTrue(result.AreEqual);
        }
        [Test]
        public void ListOfDictionariesWithIgnoreOrder()
        {
            var bar1 = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    {"a", "b"},
                    {"c", "d"},
                    {"e", "f"},
                    {"g", "h"},
                }
            };

            var bar2 = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    {"e", "f"},
                    {"g", "h"},
                    {"c", "d"},
                    {"a", "b"},
                }
            };

            var comparer = new CompareLogic { Config = { IgnoreCollectionOrder = true } };
            var res = comparer.Compare(bar1, bar2);
            Assert.IsTrue(res.AreEqual,res.DifferencesString);
        }

        [Test]
        public void DbNullTest()
        {
            CompareLogic compareLogic = new CompareLogic();

            ComparisonResult result = compareLogic.Compare(DBNull.Value, DBNull.Value);
            if (!result.AreEqual)
                Console.WriteLine(result.DifferencesString);
        }

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

        #if !NETSTANDARD

        [Test]
        public void LinearGradient()
        {
            LinearGradientBrush brush1 = new LinearGradientBrush(new Point(), new Point(0, 10), Color.Red, Color.Red);
            LinearGradientBrush brush2 = new LinearGradientBrush(new Point(), new Point(0, 10), Color.Red, Color.Blue);

            Assert.IsFalse(_compare.Compare(brush1, brush2).AreEqual);
        }

        #endif

        [Test]
        public void DecimalCollectionWhenOrderIgnored()
        {
            var compare = new CompareLogic(new ComparisonConfig
            {
                IgnoreCollectionOrder = true
            });
            Assert.IsTrue(compare.Compare(new decimal[] { 10, 1 }, new [] { 10.0m, 1.0m }).AreEqual);
        }

        #endregion
    }
}