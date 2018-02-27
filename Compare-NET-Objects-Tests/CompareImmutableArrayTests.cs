using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace KellermanSoftware.CompareNetObjectsTests
{
    using System;

    [TestFixture]
    public class CompareImmutableArrayTests
    {
        [Test]
        public void CompareSameArrays()
        {
            var comparer = new CompareLogic();
            var a1 = new[] { 1, 2, 3, 4, 6 }.ToImmutableArray();
            var a2 = new[] { 1, 2, 3, 5, 6 }.ToImmutableArray();

            var result = comparer.Compare(a1, a2);

            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void CompareDiferentArrays()
        {
            var comparer = new CompareLogic();
            var a1 = new[] { 1, 2, 3 }.ToImmutableArray();
            var a2 = new[] { 1, 2, 3 }.ToImmutableArray();

            var result = comparer.Compare(a1, a2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void CompareArraysOfDiferentOrder()
        {
            var comparer = new CompareLogic { Config = new ComparisonConfig { IgnoreCollectionOrder = true } };
            var a1 = new[] { 1, 2 }.ToImmutableArray();
            var a2 = new[] { 2, 1 }.ToImmutableArray();

            var result = comparer.Compare(a1, a2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void CompareSameArraysOnLevel2()
        {
            var comparer = new CompareLogic();
            var e1 = new TestEnvelope(new[] { "John", "Jane" });
            var e2 = new TestEnvelope(new[] { "John", "Jane" });

            var result = comparer.Compare(e1, e2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void CompareDiferentArraysOnLevel2()
        {
            var comparer = new CompareLogic();
            var e1 = new TestEnvelope(new[] { "John", "Jane" });
            var e2 = new TestEnvelope(new[] { "John", "Mary" });

            var result = comparer.Compare(e1, e2);

            Assert.IsFalse(result.AreEqual);
        }
    }

    public class TestEnvelope
    {
        public TestEnvelope(IEnumerable<string> names)
        {
            Names = names.ToImmutableArray();
        }

        public ImmutableArray<string> Names { get; }
    }
}
