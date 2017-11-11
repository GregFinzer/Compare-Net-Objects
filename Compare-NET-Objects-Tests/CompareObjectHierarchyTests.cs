using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses.ObjectHierarchy;
using NUnit.Framework;
using System;
using System.Linq;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareObjectHierarchyTests
    {
        CompareLogic sut;

        [SetUp]
        public void SetUp()
        {
            var config = new ComparisonConfig();
            config.MaxDifferences = 20;
            config.AutoClearCache = true;
            config.MembersToIgnore.Add("Id");
            config.IgnoreCollectionOrder = true;
            config.CollectionMatchingSpec = new System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.IEnumerable<string>>
            {
                {typeof(Identifier), new [] {"Type", "Code"}},
                {typeof(TestClasses.ObjectHierarchy.Attribute), new [] {"Name"}},
                {typeof(HoldingsReport), new [] {"Holdings"}},
                {typeof(Holding), new [] {"Identifiers", "Attributes", "Name", "Description"}},
                {typeof(Bond), new [] {"Identifiers", "Attributes", "Name", "Description", "IssueSnPRating"}},
                {typeof(FundShare), new [] {"Identifiers", "Attributes", "Name", "Description", "FundLegalName"}},
            };
        
            sut = new CompareLogic(config);
        }

        [Test]
        public void SameOrderExpectingEqualTest()
        {
            // Arrange
            var reference = TestObjectHierarchyFactory.CreateHoldingsReport(2);
            var difference = TestObjectHierarchyFactory.CreateHoldingsReport(2);

            // Act
            var actual = sut.Compare(reference, difference);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Differences, Is.Not.Null);
            actual.Differences.ForEach(x => {
                Console.WriteLine($"Path: {x.PropertyName}");
                Console.WriteLine($"    Expected: {x.Object1Value}");
                Console.WriteLine($"    Received: {x.Object2Value}");
            });
            Assert.That(actual.AreEqual, Is.True);
            Assert.That(actual.Differences.Count, Is.EqualTo(0));
        }

        [Test]
        public void ChangedHighLevelOrderExpectingEqualTest()
        {
            // Arrange
            var reference = TestObjectHierarchyFactory.CreateHoldingsReport(2);
            var difference = TestObjectHierarchyFactory.CreateHoldingsReport(2);
            var rand = new Random();
            difference.Holdings = difference.Holdings.OrderBy(x => rand.Next()).ToList();

            // Act
            var actual = sut.Compare(reference, difference);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.AreEqual, Is.True);
            Assert.That(actual.Differences, Is.Not.Null);
            Assert.That(actual.Differences.Count, Is.EqualTo(0));
        }


        [Test]
        public void ChangedHighAndLowLevelOrderExpectingEqualTest()
        {
            // Arrange
            var reference = TestObjectHierarchyFactory.CreateHoldingsReport(2);
            var difference = TestObjectHierarchyFactory.CreateHoldingsReport(2);
            var rand = new Random();
            difference.Holdings = difference.Holdings.OrderBy(x => rand.Next()).ToList();
            difference.Holdings.ForEach(h => {
                h.Identifiers = h.Identifiers.OrderBy(y => rand.Next()).ToList();
                h.Attributes = h.Attributes.OrderBy(y => rand.Next()).ToList();
            });

            // Act
            var actual = sut.Compare(reference, difference);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.AreEqual, Is.True);
            Assert.That(actual.Differences, Is.Not.Null);
            Assert.That(actual.Differences.Count, Is.EqualTo(0));
        }

        [Test]
        public void ChangedHighAndLowLevelOrderWithKeyChangeExpectingNotEqualTest()
        {
            // Arrange
            var reference = TestObjectHierarchyFactory.CreateHoldingsReport(2);
            var difference = TestObjectHierarchyFactory.CreateHoldingsReport(2);
            var rand = new Random();
            difference.Holdings = difference.Holdings.OrderBy(x => rand.Next()).ToList();
            difference.Holdings.ForEach(h => {
                h.Identifiers = h.Identifiers.OrderBy(y => rand.Next()).ToList();
                h.Attributes = h.Attributes.OrderBy(y => rand.Next()).ToList();
            });
            var mutantSubject = difference.Holdings.OfType<Bond>().First();
            var expectedDescription = mutantSubject.Description;
            mutantSubject.Description = "Mutated value";

            // Act
            var actual = sut.Compare(reference, difference);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.AreEqual, Is.False);
            Assert.That(actual.Differences, Is.Not.Null);
            Assert.That(actual.Differences.Count, Is.EqualTo(2));

            var first = actual.Differences.First();
            Assert.That(first, Is.Not.Null);
            Assert.That(first.Object2, Is.Null);

            var last = actual.Differences.Last();
            Assert.That(last, Is.Not.Null);
            Assert.That(last.Object1, Is.Null);
        }

        [Test]
        public void ChangedHighAndLowLevelOrderWithTinyValueChangeExpectingNotEqualTest()
        {
            // Arrange
            var reference = TestObjectHierarchyFactory.CreateHoldingsReport(2);
            var difference = TestObjectHierarchyFactory.CreateHoldingsReport(2);
            var rand = new Random();
            difference.Holdings = difference.Holdings.OrderBy(x => rand.Next()).ToList();
            difference.Holdings.ForEach(h => {
                h.Identifiers = h.Identifiers.OrderBy(y => rand.Next()).ToList();
                h.Attributes = h.Attributes.OrderBy(y => rand.Next()).ToList();
            });
            var mutantSubject = difference.Holdings.OfType<Bond>().First();
            var expectedMaturity = mutantSubject.Maturity;
            mutantSubject.Maturity = DateTime.Today.AddDays(1);

            // Act
            var actual = sut.Compare(reference, difference);

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.AreEqual, Is.False);
            Assert.That(actual.Differences, Is.Not.Null);
            Assert.That(actual.Differences.Count, Is.EqualTo(1));

            var first = actual.Differences.First();
            Assert.That(first, Is.Not.Null);
            Assert.That(first.Object1Value, Is.EqualTo(expectedMaturity.ToString()));
            Assert.That(first.Object2Value, Is.EqualTo(mutantSubject.Maturity.ToString()));
        }
    }
}
