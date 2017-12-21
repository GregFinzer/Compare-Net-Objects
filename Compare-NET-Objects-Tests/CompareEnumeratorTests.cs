using System;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareEnumeratorTests
    {

        #region Linq Enumerator Tests
        [Test]
        public void WhenSecondEntityHasLinqEnumeratorPropertyThenDetectsDifferences()
        {
            var a = new EnumerableTestEntity
            {
                Items = new[] { 1, 2, 3 }
            };
            var b = new EnumerableTestEntity
            {
                Items = new[] { 1, 2, 4 }.Where(i => i < 5)
            };

            var oc = new CompareLogic();
            oc.Config.IgnoreObjectTypes = true;
            oc.Config.CompareChildren = true;

            var result = oc.Compare(a, b);
            Console.WriteLine(result.DifferencesString);
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void WhenFirstEntityHasLinqEnumeratorPropertyThenDetectsDifferences()
        {
            var a = new EnumerableTestEntity
            {
                Items = new[] { 1, 2, 3 }.Where(i => i < 5)
            };
            var b = new EnumerableTestEntity
            {
                Items = new[] { 1, 2, 4 }
            };

            var oc = new CompareLogic();
            oc.Config.IgnoreObjectTypes = true;
            oc.Config.CompareChildren = true;

            var result = oc.Compare(a, b);
            Console.WriteLine(result.DifferencesString);
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void WhenBothEntitiesHaveLinqEnumeratorPropertyThenDetectsDifferences()
        {
            var a = new EnumerableTestEntity
            {
                Items = new[] { 1, 2, 3 }.Where(i => i < 5)
            };
            var b = new EnumerableTestEntity
            {
                Items = new[] { 1, 2, 4 }.Where(i => i < 5)
            };

            var oc = new CompareLogic();
            oc.Config.IgnoreObjectTypes = true;
            oc.Config.CompareChildren = true;

            var result = oc.Compare(a, b);
            Console.WriteLine(result.DifferencesString);
            Assert.IsFalse(result.AreEqual);
        }
        #endregion
    }
}
