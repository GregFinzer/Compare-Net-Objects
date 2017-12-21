using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareStructTests
    {
        #region Class Variables
        private CompareLogic _compare;

        struct Truck
        {
            public Size size;
        }

        struct Boxes
        {
            public IList<Size> sizes;
        }
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

        #region Struct Tests

        [Test]
        public void TruckTest()
        {
            var t1 = new Truck();
            var t2 = new Truck();

            t1.size.Height = 2;
            t1.size.Width = 2;

            t2.size.Height = 3;
            t2.size.Width = 3;

            ComparisonResult result = _compare.Compare(t1, t2);
            Assert.IsFalse(result.AreEqual);

            var b1 = new Boxes();
            var b2 = new Boxes();

            b1.sizes = new List<Size>();
            Size s1 = new Size();
            s1.Height = 2;
            s1.Width = 2;
            b1.sizes.Add(s1);

            b2.sizes = new List<Size>();
            Size s2 = new Size();
            s2.Height = 3;
            s2.Width = 3;
            b1.sizes.Add(s2);

            result = _compare.Compare(b1, b2);
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void Identify()
        {

            Size size1 = new Size();
            size1.Width = 800;
            size1.Height = 600;

            Size size2 = new Size();
            size2.Width = 1024;
            size2.Height = 768;

            Console.WriteLine(size1.GetHashCode());

            Console.WriteLine(size2.GetHashCode());
        }

        [Test]
        public void TestStruct()
        {
            Size size1 = new Size();
            size1.Width = 800;
            size1.Height = 600;

            Size size2 = new Size();
            size2.Width = 1024;
            size2.Height = 768;

            List<Size> list1 = new List<Size>();
            list1.Add(size1);
            list1.Add(size2);

            List<Size> list2 = new List<Size>();
            list2.Add(size1);
            list2.Add(size2);

            ComparisonResult result = _compare.Compare(list1, list2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void TestStructNegative()
        {
            Size size1 = new Size();
            size1.Width = 800;
            size1.Height = 600;

            Size size2 = new Size();
            size2.Width = 1024;
            size2.Height = 768;

            List<Size> list1 = new List<Size>();
            list1.Add(size1);
            list1.Add(size2);

            List<Size> list2 = new List<Size>();
            list2.Add(size1);
            Size size3 = new Size();
            size3.Width = 1025;
            size3.Height = 768;
            list2.Add(size3);

            Assert.IsFalse(_compare.Compare(list1, list2).AreEqual);
        }

        [Test]
        public void TestStructWithNoPublicFields()
        {
            var point1 = new Point(1, 1);
            var point2 = new Point(1, 1);

            ComparisonResult result = _compare.Compare(point1, point2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void TestStructWithNoPublicFieldsNegative()
        {
            var point1 = new Point(1, 1);
            var point2 = new Point(2, 2);

            Assert.IsFalse(_compare.Compare(point1, point2).AreEqual);
        }

        [Test]
        public void TestStructWithPublicStaticPropOfSameType()
        {
            var point1 = StructWithStaticProperty.Origin;
            var point2 = StructWithStaticProperty.Origin;

            ComparisonResult result = _compare.Compare(point1, point2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void CompareStructWithProperty()
        {
            var item1 = new StructWithProperty();
            var item2 = new StructWithProperty();

            item1.Property1 = 1;
            item2.Property1 = 1;

            _compare.Config.IgnoreObjectTypes = true;
            ComparisonResult result = _compare.Compare(item1, item2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }
        #endregion
    }
}
