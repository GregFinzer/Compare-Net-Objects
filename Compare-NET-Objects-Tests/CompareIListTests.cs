using System;
using System.Collections;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareIListTests
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

        #region Array Tests
        [Test]
        public void ByteArrayTestPositive()
        {
            byte[] b1 = new byte[256];
            byte[] b2 = new byte[256];
            for (int i = 0; i <= 255; i++)
                b1[i] = (byte)i;

            b1.CopyTo(b2, 0);

            ComparisonResult result = _compare.Compare(b1, b2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void ByteArrayTestNegative()
        {
            byte[] b1 = new byte[256];
            byte[] b2 = new byte[256];
            for (int i = 0; i <= 255; i++)
                b1[i] = (byte)i;

            b1.CopyTo(b2, 0);
            b2[5] = 77;

            ComparisonResult result = _compare.Compare(b1, b2);

            if (result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void ByteListTestNegative()
        {
            byte[] b1 = new byte[256];
            byte[] b2 = new byte[256];
            for (int i = 0; i <= 255; i++)
                b1[i] = (byte)i;

            b1.CopyTo(b2, 0);
            b2[5] = 77;

            List<byte> bList1 = new List<byte>(b1);
            List<byte> bList2 = new List<byte>(b2);
            ComparisonResult result = _compare.Compare(bList1, bList2);

            if (result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void ArrayTest()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Greg";

            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = p1.DateCreated;

            Person[] array1 = new Person[2];
            Person[] array2 = new Person[2];

            array1[0] = p1;
            array1[1] = p2;

            array2[0] = Common.CloneWithSerialization(p1);
            array2[1] = Common.CloneWithSerialization(p2);

            ComparisonResult result = _compare.Compare(array1, array2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);

        }

        [Test]
        public void ArrayTestNegative()
        {
            Person p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Greg";

            Person p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = p1.DateCreated;

            Person[] array1 = new Person[2];
            Person[] array2 = new Person[2];

            array1[0] = p1;
            array1[1] = p2;

            array2[0] = Common.CloneWithSerialization(p1);
            array2[1] = Common.CloneWithSerialization(p2);
            array2[1].Name = "Bob";

            Assert.IsFalse(_compare.Compare(array1, array2).AreEqual);
        }

        [Test]
        public void MultiDimensionalByteArrayTest()
        {
            byte[,] bytes1 = new byte[3, 2];
            byte[,] bytes2 = new byte[3, 2];

            bytes1[0, 0] = 3;
            bytes1[1, 0] = 35;
            bytes1[2, 0] = 6;
            bytes1[0, 1] = 3;
            bytes1[1, 1] = 35;
            bytes1[2, 1] = 6;

            bytes2[0, 0] = 3;
            bytes2[1, 0] = 35;
            bytes2[2, 0] = 6;
            bytes2[0, 1] = 3;
            bytes2[1, 1] = 35;
            bytes2[2, 1] = 6;

            ComparisonResult result = _compare.Compare(bytes1, bytes2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void MultiDimensionalByteArrayNegative()
        {
            byte[,] bytes1 = new byte[3, 2];
            byte[,] bytes2 = new byte[3, 2];

            bytes1[0, 0] = 3;
            bytes1[1, 0] = 35;
            bytes1[2, 0] = 6;
            bytes1[0, 1] = 3;
            bytes1[1, 1] = 35;
            bytes1[2, 1] = 6;

            bytes2[0, 0] = 3;
            bytes2[1, 0] = 35;
            bytes2[2, 0] = 6;
            bytes2[0, 1] = 3;
            bytes2[1, 1] = 36;
            bytes2[2, 1] = 6;

            Assert.IsFalse(_compare.Compare(bytes1, bytes2).AreEqual);
        }

        #endregion

        #region Ignore Children Tests
        [Test]
        public void IgnoreChildDifferencesPositiveTest()
        {
            List<Entity> entityTree = new List<Entity>();

            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            entityTree.Add(top1);

            Entity div1 = new Entity();
            div1.Description = "Minstrils";
            div1.EntityLevel = Level.Division;
            div1.Parent = top1;
            top1.Children.Add(div1);

            List<Entity> entityTreeCopy = Common.CloneWithSerialization(entityTree);

            entityTreeCopy[0].Children[0].EntityLevel = Level.Department;

            _compare.Config.CompareChildren = false;
            Assert.IsTrue(_compare.Compare(entityTree, entityTreeCopy).AreEqual);
            _compare.Config.CompareChildren = true;
        }

        [Test]
        public void IgnoreChildDifferencesNegativeTest()
        {
            List<Entity> entityTree = new List<Entity>();

            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            entityTree.Add(top1);

            Entity div1 = new Entity();
            div1.Description = "Minstrils";
            div1.EntityLevel = Level.Division;
            div1.Parent = top1;
            top1.Children.Add(div1);

            List<Entity> entityTreeCopy = Common.CloneWithSerialization(entityTree);

            entityTreeCopy[0].Children[0].EntityLevel = Level.Department;

            _compare.Config.CompareChildren = true;
            Assert.IsFalse(_compare.Compare(entityTree, entityTreeCopy).AreEqual);
        }

        #endregion

        #region Generic Entity List Test
        [Test]
        public void GenericEntityListTest()
        {
            GenericEntity<IEntity> genericEntity = new GenericEntity<IEntity>();
            genericEntity.MyList = new List<IEntity>();

            //Brave Sir Robin Security Company
            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            genericEntity.MyList.Add(top1);

            GenericEntity<IEntity> genericEntityCopy = new GenericEntity<IEntity>();
            genericEntityCopy.MyList = new List<IEntity>();

            //Brave Sir Robin Security Company
            Entity top2 = new Entity();
            top2.Description = "Brave Sir Robin Security Company";
            top2.Parent = null;
            top2.EntityLevel = Level.Company;
            genericEntityCopy.MyList.Add(top2);

            Assert.IsTrue(_compare.Compare(genericEntity, genericEntityCopy).AreEqual);

            genericEntityCopy.MyList[0].Description = "When danger reared its ugly head Brave Sir Robin fled.";

            Assert.IsFalse(_compare.Compare(genericEntity, genericEntityCopy).AreEqual);
        }

        [Test]
        public void GenericEntityListMultipleItemsTest()
        {
            GenericEntity<IEntity> genericEntity = new GenericEntity<IEntity>();
            genericEntity.MyList = new List<IEntity>();

            //Brave Sir Robin Security Company
            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;

            GenericEntity<IEntity> genericEntityCopy = new GenericEntity<IEntity>();
            genericEntityCopy.MyList = new List<IEntity>();

            //Brave Sir Robin Security Company
            Entity top2 = new Entity();
            top2.Description = "Brave Sir Robin Security Company";
            top2.Parent = null;
            top2.EntityLevel = Level.Company;


            Entity top3 = new Entity();
            top3.Description = "May I obey all your commands with equal pleasure, Sire!";
            top3.Parent = null;
            top3.EntityLevel = Level.Department;


            genericEntity.MyList.Add(top1);
            genericEntity.MyList.Add(top3);
            genericEntityCopy.MyList.Add(top2);
            genericEntityCopy.MyList.Add(top3);

            Assert.IsTrue(_compare.Compare(genericEntity, genericEntityCopy).AreEqual);

            genericEntityCopy.MyList[0].Description = "When danger reared its ugly head Brave Sir Robin fled.";

            Assert.IsFalse(_compare.Compare(genericEntity, genericEntityCopy).AreEqual);
        }





        #endregion

        #region Entity Tree Tests
        [Test]
        public void TestEntityTree()
        {
            List<Entity> entityTree = new List<Entity>();

            //Brave Sir Robin Security Company
            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            entityTree.Add(top1);

            Entity div1 = new Entity();
            div1.Description = "Minstrils";
            div1.EntityLevel = Level.Division;
            div1.Parent = top1;
            top1.Children.Add(div1);

            Entity div2 = new Entity();
            div2.Description = "Sub Contracted Fighting";
            div2.EntityLevel = Level.Division;
            div2.Parent = top1;
            top1.Children.Add(div2);

            Entity dep2 = new Entity();
            dep2.Description = "Trojan Rabbit Department";
            dep2.EntityLevel = Level.Department;
            dep2.Parent = div2;
            div2.Children.Add(dep2);

            //Roger the Shrubber's Fine Shrubberies
            Entity top1b = new Entity();
            top1b.Description = "Roger the Shrubber's Fine Shrubberies";
            top1b.Parent = null;
            top1b.EntityLevel = Level.Company;
            entityTree.Add(top1b);

            Entity div1b = new Entity();
            div1b.Description = "Manufacturing";
            div1b.EntityLevel = Level.Division;
            div1b.Parent = top1;
            top1b.Children.Add(div1);

            Entity dep1b = new Entity();
            dep1b.Description = "Design Department";
            dep1b.EntityLevel = Level.Department;
            dep1b.Parent = div1b;
            div1b.Children.Add(dep1b);

            Entity dep2b = new Entity();
            dep2b.Description = "Arranging Department";
            dep2b.EntityLevel = Level.Department;
            dep2b.Parent = div1b;
            div1b.Children.Add(dep2b);

            Entity div2b = new Entity();
            div2b.Description = "Sales";
            div2b.EntityLevel = Level.Division;
            div2b.Parent = top1;
            top1b.Children.Add(div2b);

            List<Entity> entityTreeCopy = Common.CloneWithSerialization(entityTree);

            ComparisonResult result = _compare.Compare(entityTree, entityTreeCopy);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);

        }

        [Test]
        public void TestEntityTreeNegative()
        {
            List<Entity> entityTree = new List<Entity>();

            //Brave Sir Robin Security Company
            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            entityTree.Add(top1);

            Entity div1 = new Entity();
            div1.Description = "Minstrils";
            div1.EntityLevel = Level.Division;
            div1.Parent = top1;
            top1.Children.Add(div1);

            Entity div2 = new Entity();
            div2.Description = "Sub Contracted Fighting";
            div2.EntityLevel = Level.Division;
            div2.Parent = top1;
            top1.Children.Add(div2);

            Entity dep2 = new Entity();
            dep2.Description = "Trojan Rabbit Department";
            dep2.EntityLevel = Level.Department;
            dep2.Parent = div2;
            div2.Children.Add(dep2);

            //Roger the Shrubber's Fine Shrubberies
            Entity top1b = new Entity();
            top1b.Description = "Roger the Shrubber's Fine Shrubberies";
            top1b.Parent = null;
            top1b.EntityLevel = Level.Company;
            entityTree.Add(top1b);

            Entity div1b = new Entity();
            div1b.Description = "Manufacturing";
            div1b.EntityLevel = Level.Division;
            div1b.Parent = top1;
            top1b.Children.Add(div1);

            Entity dep1b = new Entity();
            dep1b.Description = "Design Department";
            dep1b.EntityLevel = Level.Department;
            dep1b.Parent = div1b;
            div1b.Children.Add(dep1b);

            Entity dep2b = new Entity();
            dep2b.Description = "Arranging Department";
            dep2b.EntityLevel = Level.Department;
            dep2b.Parent = div1b;
            div1b.Children.Add(dep2b);

            Entity div2b = new Entity();
            div2b.Description = "Sales";
            div2b.EntityLevel = Level.Division;
            div2b.Parent = top1;
            top1b.Children.Add(div2b);

            List<Entity> entityTreeCopy = Common.CloneWithSerialization(entityTree);

            entityTreeCopy[1].Children[1].Description = "Retail";

            ComparisonResult result = _compare.Compare(entityTree, entityTreeCopy);
            Console.WriteLine(result.DifferencesString);
            Assert.IsFalse(result.AreEqual);

            #if !NETSTANDARD
            Console.WriteLine(result.ElapsedMilliseconds);
            #endif
        }

        #endregion

        #region List Tests

        [Test]
        public void CompareListsTwoDifferentTypes()
        {
            List<Person> list1 = new List<Person>();
            list1.Add(new Person(){Name="Logan 5"});
            list1.Add(new Person(){Name="Francis 7"});

            List<Officer> list2 = new List<Officer>();
            list2.Add(new Officer() { Name = "Logan 5" });
            list2.Add(new Officer() { Name = "Francis 7" });

            ComparisonConfig config = new ComparisonConfig();
            config.IgnoreObjectTypes = true;

            CompareLogic compareLogic = new CompareLogic(config);
            var result = compareLogic.Compare(list1, list2);
            Assert.IsTrue(result.AreEqual);

        }

        [Test]
        public void CompareListsOfStringBytePairs()
        {
            var list1 = new List<KeyValuePair<string, byte>> { new KeyValuePair<string, byte>("hello", 1) };
            var list2 = new List<KeyValuePair<string, byte>> { new KeyValuePair<string, byte>("world", 2) };

            ComparisonConfig config = new ComparisonConfig();
            config.IgnoreObjectTypes = true;

            CompareLogic compareLogic = new CompareLogic(config);
            var result = compareLogic.Compare(list1, list2);
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void ListOfNullObjects()
        {
            Person p1 = null;
            Person p2 = null;

            List<Person> list1 = new List<Person>();
            list1.Add(p1);
            list1.Add(p2);

            List<Person> list2 = new List<Person>();
            list2.Add(p1);
            list2.Add(p2);

            ComparisonResult result = _compare.Compare(list1, list2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void CompareListsTwoDifferentListTypes()
        {
            List<Person> list1 = new List<Person>();
            list1.Add(new Person(){Name="Logan 5"});
            list1.Add(new Person(){Name="Francis 7"});

            NewList<Person> list2 = new NewList<Person>(list1);

            ComparisonConfig config = new ComparisonConfig();
            config.IgnoreObjectTypes = true;

            CompareLogic compareLogic = new CompareLogic(config);
            var result = compareLogic.Compare(list1, list2);
            Assert.IsTrue(result.AreEqual);

            list2.Add(new Person(){Name="Roger 4"});

            result = compareLogic.Compare(list1, list2);
            Assert.IsFalse(result.AreEqual);
        }

        private class NewList<T> : IList<T>
        {
            private readonly IList<T> _wrappedList;

            public NewList(IEnumerable<T> list)
            {
                _wrappedList = new List<T>(list);;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _wrappedList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _wrappedList.GetEnumerator();
            }

            public void Add(T item)
            {
                _wrappedList.Add(item);
            }

            public void Clear()
            {
                _wrappedList.Clear();
            }

            public bool Contains(T item)
            {
                return _wrappedList.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                _wrappedList.CopyTo(array, arrayIndex);
            }

            public bool Remove(T item)
            {
                return _wrappedList.Remove(item);
            }

            public int Count => _wrappedList.Count;

            public bool IsReadOnly => _wrappedList.IsReadOnly;

            public int IndexOf(T item)
            {
                return _wrappedList.IndexOf(item);
            }

            public void Insert(int index, T item)
            {
                _wrappedList.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                _wrappedList.RemoveAt(index);
            }

            public T this[int index]
            {
                get => _wrappedList[index];
                set => _wrappedList[index] = value;
            }
        }

        #endregion
    }
}
