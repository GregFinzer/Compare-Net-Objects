using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.Attributes;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class AlPeTests
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

        public class ClassWithIndexer
        {
            public ClassWithIndexer()
            {
                _list.Add(new Content { Text = "Item0" });
                _list.Add(new Content { Text = "Item1" });
                _list.Add(new Content { Text = "Item2" });
                _list.Add(new Content { Text = "Item3" });
            }

            private List<Content> _list = new List<Content>();

            public Content this[int indexer]
            {
                get { return _list[indexer]; }
            }
        }

        public class Content
        {
            public string Text { get; set; }
        }

        #region Tests
        [Test]
        public void PropertyComparerFailsWithTargetParameterCountException()
        {
            //This is the comparison class
            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.SkipInvalidIndexers = true;

            var class1 = new ClassWithIndexer();
            var class2 = new ClassWithIndexer();

            //These will be different, write out the differences
            ComparisonResult result = compareLogic.Compare(class1, class2);
            if (!result.AreEqual)
                Console.WriteLine(result.DifferencesString);
        }
        #endregion
    }
}