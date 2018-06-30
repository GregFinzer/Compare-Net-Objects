using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using KellermanSoftware.CompareNetObjectsTests.TestClasses.Bal;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class IgnoreOrderTests
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
        public void NestedObjectShouldCompare()
        {
            //Arrange
            var bookingOld = new Booking();
            bookingOld.Offers.Add(new Offer
            {
                Id = 1,
                Label = "Offer 1",
                Order = 1
            });
            bookingOld.Offers[0].Products.Add(new BookingProduct
            {
                Id = 1,
                ProductId = 1,
                OfferId = 1,
                Label = "Product 1",
                Amount = 10
            });
            bookingOld.Offers[0].Products.Add(new BookingProduct
            {
                Id = 2,
                ProductId = 2,
                OfferId = 1,
                Label = "Product 2",
                Amount = 15
            });

            var bookingNew = new Booking();
            bookingNew.Offers.Add(new Offer
            {
                Id = 1,
                Label = "Change Label",
                Order = 1
            });
            bookingNew.Offers[0].Products.Add(new BookingProduct
            {
                Id = 1,
                ProductId = 1,
                OfferId = 1,
                Label = "Product 1",
                Amount = 10
            });
            bookingNew.Offers[0].Products.Add(new BookingProduct
            {
                Id = 2,
                ProductId = 3,
                OfferId = 1,
                Label = "Product 2 New",
                Amount = 15
            });

            var spec = new Dictionary<Type, IEnumerable<string>>
            {
                {typeof(Offer), new[] {"Id"}},
                {typeof(BookingProduct), new[] {"Id"}},
            };

            //Act
            ComparisonConfig config = new ComparisonConfig { MaxDifferences = 100, IgnoreCollectionOrder = false, CollectionMatchingSpec = spec };

            CompareLogic logic = new CompareLogic(config);
            ComparisonResult result = logic.Compare(bookingOld, bookingNew);

            //Assert
            Console.WriteLine(result.DifferencesString);
            Assert.False(result.AreEqual);
            Difference offerDifference = result.Differences.FirstOrDefault(o => o.PropertyName == "Offers[0].Label");
            Assert.IsNotNull(offerDifference, "Could not find an offer difference");
        }

        [Test]
        public void CompareListsIgnoreOrderTwoDifferentTypes()
        {
            List<Person> list1 = new List<Person>();
            list1.Add(new Person() {ID = 1, Name = "Logan 5"});
            list1.Add(new Person() { ID = 2, Name = "Francis 7" });

            List<Officer> list2 = new List<Officer>();
            list2.Add(new Officer() { ID = 2, Name = "Francis 7" });
            list2.Add(new Officer() { ID = 1, Name = "Logan 5" });

            ComparisonConfig config = new ComparisonConfig();
            Dictionary<Type, IEnumerable<string>> collectionSpec = new Dictionary<Type, IEnumerable<string>>();
            collectionSpec.Add(typeof(Person), new string[] { "ID" });
            collectionSpec.Add(typeof(Officer), new string[] { "ID" });

            config.IgnoreObjectTypes = true;
            config.IgnoreCollectionOrder = true;
            config.CollectionMatchingSpec = collectionSpec;

            CompareLogic compareLogic = new CompareLogic(config);
            var result = compareLogic.Compare(list1, list2);
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void IgnoreOrderSecondListHigherCount()
        {
            List<Reservation> list1 = new List<Reservation>();
            List<Reservation> list2 = new List<Reservation>();
            list1.Add(new Reservation() { ReservationId = "1" });
            list1.Add(new Reservation() { ReservationId = "2" });
            list1.Add(new Reservation() { ReservationId = "3" });
            list1.Add(new Reservation() { ReservationId = "4" });
            list1.Add(new Reservation() { ReservationId = "5" });
            list1.Add(new Reservation() { ReservationId = "5" });
            list1.Add(new Reservation() { ReservationId = "6" });
            list1.Add(new Reservation() { ReservationId = "7" });
            list1.Add(new Reservation() { ReservationId = "8" });
            list1.Add(new Reservation() { ReservationId = "9" });
            list1.Add(new Reservation() { ReservationId = "10" });

            list2.Add(new Reservation() { ReservationId = "1" });
            list2.Add(new Reservation() { ReservationId = "2" });
            list2.Add(new Reservation() { ReservationId = "3" });
            list2.Add(new Reservation() { ReservationId = "4" });
            list2.Add(new Reservation() { ReservationId = "5" });
            list2.Add(new Reservation() { ReservationId = "5" });
            list2.Add(new Reservation() { ReservationId = "6" });
            list2.Add(new Reservation() { ReservationId = "7" });
            list2.Add(new Reservation() { ReservationId = "8" });
            list2.Add(new Reservation() { ReservationId = "9" });
            list2.Add(new Reservation() { ReservationId = "10" });
            list2.Add(new Reservation() { ReservationId = "11" });
            list2.Add(new Reservation() { ReservationId = "12" });

            Dictionary<Type, IEnumerable<string>> collectionSpec = new Dictionary<Type, IEnumerable<string>>();
            collectionSpec.Add(typeof(Reservation), new string[] { "ReservationId" });
            CompareLogic compareLogic = new CompareLogic(new ComparisonConfig()
            {
                IgnoreCollectionOrder = true,
                MaxDifferences = int.MaxValue,
                CompareChildren = true,
                CompareFields = false,
                TreatStringEmptyAndNullTheSame = true,
                CollectionMatchingSpec = collectionSpec,
            });
            ComparisonResult compareResult = compareLogic.Compare(list1, list2);
            
            Assert.IsFalse(compareResult.AreEqual);
            Assert.AreEqual(3, compareResult.Differences.Count);
            Console.WriteLine(compareResult.DifferencesString);
        }

        [Test]
        public void IgnoreOrderAndIgnoreGuidMember()
        {
            List<ClassWithGuid> list1 = new List<ClassWithGuid>();
            List<ClassWithGuid> list2 = new List<ClassWithGuid>();

            int count1 = 1;
            int count2 = 10;

            while (count1 <= 10)
            {
                ClassWithGuid object1 = new ClassWithGuid();
                object1.SomeInteger = count1;

                list1.Add(object1);

                ClassWithGuid object2 = new ClassWithGuid();
                object2.SomeInteger = count2;

                list2.Add(object2);

                count1++;
                count2--;
            }

            _compare.Config.MembersToIgnore.Add("MyGuid");
            _compare.Config.IgnoreCollectionOrder = true;
            _compare.Config.MaxDifferences = int.MaxValue;

            ComparisonResult result = _compare.Compare(list1, list2);

            if (!result.AreEqual)
                Assert.Fail(result.DifferencesString);
        }

        [Test]
        public void CollectionDisorderedWithSpecMatchAndOffByCount()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();

            entity1.Children.Add(new Entity
            {
                Description = "1",
                EntityLevel = Level.Company
            });
            entity1.Children.Add(new Entity
            {
                Description = "5",
                EntityLevel = Level.Division
            });
            entity1.Children.Add(new Entity
            {
                Description = "3",
                EntityLevel = Level.Department
            });


            entity2.Children.Add(new Entity
            {
                Description = "1",
                EntityLevel = Level.Department
            });

            entity2.Children.Add(new Entity
            {
                Description = "2",
                EntityLevel = Level.Department
            });

            entity2.Children.Add(new Entity
            {
                Description = "4",
                EntityLevel = Level.Department
            });

            entity2.Children.Add(new Entity
            {
                Description = "5",
                EntityLevel = Level.Division
            });

            entity2.Children.Add(new Entity
            {
                Description = "3",
                EntityLevel = Level.Division
            });

            var spec = new Dictionary<Type, IEnumerable<string>>();

            spec.Add(typeof(Entity), new string[] { "Description" });

            _compare.Config.IgnoreCollectionOrder = true;

            _compare.Config.MaxDifferences = int.MaxValue;

            _compare.Config.CollectionMatchingSpec = spec;

            var result = _compare.Compare(entity1, entity2);
            Console.WriteLine(result.DifferencesString);

            Assert.AreEqual(result.Differences.Where(d => d.Object1Value == Level.Company.ToString() && d.Object2Value == Level.Department.ToString()).Count(), 1);

            Assert.AreEqual(result.Differences.Where(d => d.ToString().Contains("Description:")).Count(), 4);

            Assert.AreEqual(result.Differences.Where(d => d.Object1Value == "(null)" || d.Object2Value == "(null)").Count(), 2);

        }



        [Test]
        public void CollectionWithSpecMatch()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();
            var len = 3;

            for (int i = 0; i < len; i++)
            {
                entity1.Children.Add(new Entity
                {
                    Description = i.ToString(),
                    EntityLevel = Level.Company
                });

                entity2.Children.Add(new Entity
                {
                    Description = (i).ToString(),
                    EntityLevel = Level.Department
                });
            }

            var spec = new Dictionary<Type, IEnumerable<string>>();

            spec.Add(typeof(Entity), new string[] { "Description" });

            _compare.Config.IgnoreCollectionOrder = true;

            _compare.Config.CollectionMatchingSpec = spec;

            _compare.Config.MaxDifferences = int.MaxValue;

            var result = _compare.Compare(entity1, entity2);

            Assert.AreEqual(result.Differences[0].Object1Value, Level.Company.ToString());

            Assert.AreEqual(result.Differences[0].Object2Value, Level.Department.ToString());

            Assert.IsTrue(result.Differences[0].ToString().Contains("Description:"));
        }

        [Test]
        public void CollectionWithSpecMatchMatchSpecObjectDifference()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();

            entity1.Children.Add(new Entity
            {
                Description = "Entity",
                EntityLevel = Level.Company
            });

            entity2.Children.Add(new Entity
            {
                Description = "Entity",
                EntityLevel = Level.Department
            });


            var spec = new Dictionary<Type, IEnumerable<string>>();

            spec.Add(typeof(Entity), new string[] { "Description" });

            _compare.Config.IgnoreCollectionOrder = true;

            _compare.Config.CollectionMatchingSpec = spec;

            var result = _compare.Compare(entity1, entity2);

            Assert.AreEqual(result.Differences[0].Object1Value, Level.Company.ToString());

            Assert.AreEqual(result.Differences[0].Object2Value, Level.Department.ToString());

            Assert.IsTrue(result.Differences[0].ToString().Contains("Description:"));
        }

        [Test]
        public void CollectionWithSpecNoMatch()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();
            var len = 3;

            for (int i = 0; i < len; i++)
            {
                entity1.Children.Add(new Entity
                {
                    Description = i.ToString(),
                    EntityLevel = Level.Company
                });

                entity2.Children.Add(new Entity
                {
                    Description = (i).ToString(),
                    EntityLevel = Level.Department
                });
            }

            var spec = new Dictionary<Type, IEnumerable<string>>();

            spec.Add(typeof(Entity), new string[] { "Description", "EntityLevel" });

            _compare.Config.IgnoreCollectionOrder = true;

            _compare.Config.CollectionMatchingSpec = spec;

            var result = _compare.Compare(entity1, entity2);

            Assert.AreEqual(result.Differences[0].Object1Value, typeof(Entity).FullName);

            Assert.AreEqual(result.Differences[0].Object2Value, "(null)");

        }

        [Test]
        public void GenericEntityListMultipleItemsOddOrderTest()
        {
            GenericEntity<IEntity> genericEntity = new GenericEntity<IEntity>();
            genericEntity.MyList = new List<IEntity>();

            GenericEntity<IEntity> genericEntityCopy = new GenericEntity<IEntity>();
            genericEntityCopy.MyList = new List<IEntity>();

            //Brave Sir Robin Security Company
            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;

            //Brave Sir Robin Security Company
            Entity top2 = new Entity();
            top2.Description = "Brave Sir Robin Security Company";
            top2.Parent = null;
            top2.EntityLevel = Level.Company;

            //Brave Sir Robin Security Company
            Entity top5 = new Entity();
            top5.Description = "Brave Sir Robin Security Company";
            top5.Parent = null;
            top5.EntityLevel = Level.Company;

            Entity top3 = new Entity();
            top3.Description = "May I obey all your commands with equal pleasure, Sire!";
            top3.Parent = null;
            top3.EntityLevel = Level.Department;

            Entity top4 = new Entity();
            top4.Description = "Overtaxed, overworked and paid off with a knife, a club or a rope.";
            top4.Parent = null;
            top4.EntityLevel = Level.Department;


            genericEntity.MyList.Add(top4);
            genericEntity.MyList.Add(top3);
            genericEntity.MyList.Add(top5);
            genericEntity.MyList.Add(top2);
            genericEntityCopy.MyList.Add(top2);
            genericEntityCopy.MyList.Add(top1);
            genericEntityCopy.MyList.Add(top3);
            genericEntityCopy.MyList.Add(top4);

            _compare.Config.IgnoreCollectionOrder = true;

            var result = _compare.Compare(genericEntity, genericEntityCopy);

            Console.WriteLine(result.DifferencesString);

            Assert.IsTrue(_compare.Compare(genericEntity, genericEntityCopy).AreEqual);

            genericEntity.MyList[2].Description = "When danger reared its ugly head Brave Sir Robin fled.";

            Assert.IsFalse(_compare.Compare(genericEntity, genericEntityCopy).AreEqual);
        }

        [Test]
        public void GenericEntityListMultipleItemsWithIgnoreCollectionOrderTest()
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


            genericEntity.MyList.Add(top3);
            genericEntity.MyList.Add(top1);
            genericEntityCopy.MyList.Add(top2);
            genericEntityCopy.MyList.Add(top3);

            _compare.Config.IgnoreCollectionOrder = true;

            Assert.IsTrue(_compare.Compare(genericEntity, genericEntityCopy).AreEqual);

            genericEntityCopy.MyList[0].Description = "When danger reared its ugly head Brave Sir Robin fled.";

            Assert.IsFalse(_compare.Compare(genericEntity, genericEntityCopy).AreEqual);
        }

        [Test]
        public void HashSetsMultipleItemsWithIgnoreCollectionOrderTest()
        {
            HashSetWrapper hashSet1 = new HashSetWrapper
            {
                StatusId = 1,
                Name = "Paul"
            };
            HashSetWrapper hashSet2 = new HashSetWrapper
            {
                StatusId = 1,
                Name = "Paul"
            };

            HashSetClass secondClassObject1 = new HashSetClass
            {
                Id = 1
            };
            HashSetClass secondClassObject2 = new HashSetClass
            {
                Id = 2
            };

            HashSetClass secondClassObject3 = new HashSetClass
            {
                Id = 3
            };
            HashSetClass secondClassObject4 = new HashSetClass
            {
                Id = 4
            };


            hashSet1.HashSetCollection.Add(secondClassObject3);
            hashSet1.HashSetCollection.Add(secondClassObject1);
            hashSet1.HashSetCollection.Add(secondClassObject2);
            hashSet1.HashSetCollection.Add(secondClassObject4);

            hashSet2.HashSetCollection.Add(secondClassObject2);
            hashSet2.HashSetCollection.Add(secondClassObject4);
            hashSet2.HashSetCollection.Add(secondClassObject3);
            hashSet2.HashSetCollection.Add(secondClassObject1);

            _compare.Config.IgnoreCollectionOrder = true;

            ComparisonResult result = _compare.Compare(hashSet1, hashSet2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void TestDictionaryWithIgnoreCollectionOrder()
        {
            var p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Owen";

            var p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            var p3 = new Person();
            p3.Name = "Wink";
            p3.DateCreated = DateTime.Now.AddDays(-2);

            var dict1 = new Dictionary<string, Person>();
            dict1.Add("1001", p1);
            dict1.Add("1002", p2);

            var dict2 = new Dictionary<string, Person>();
            dict2.Add("1002", p2);
            dict2.Add("1001", p1);

            _compare.Config.IgnoreCollectionOrder = true;

            ComparisonResult result = _compare.Compare(dict1, dict2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);

            result = _compare.Compare(dict2, dict1);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);


            dict1.Add("1003", p3);
            dict2.Add("1003", p3);

            result = _compare.Compare(dict1, dict2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);

            result = _compare.Compare(dict2, dict1);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void TestIndexerWithIgnoreCollectionOrder()
        {
            var jane = new Person { Name = "Jane" };
            var john = new Person { Name = "Mary" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, jack, mary };
            var nameList2 = new List<Person>() { jane, john, jack };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            _compare.Config.IgnoreCollectionOrder = true;

            Assert.IsTrue(_compare.Compare(class1, class2).AreEqual);
            Assert.IsTrue(_compare.Compare(class2, class1).AreEqual);
        }

        [Test]
        public void TestIndexerWithIgnoreCollectionOrderNegative()
        {
            var jane = new Person { Name = "Jane" };
            var john = new Person { Name = "Mary" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };
            var jo = new Person { Name = "Jo" };
            var sarah = new Person { Name = "Sarah" };

            var nameList1 = new List<Person>() { jane, jack, mary, jo };
            var nameList2 = new List<Person>() { jane, john, jack, sarah };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            _compare.Config.IgnoreCollectionOrder = true;

            Assert.IsFalse(_compare.Compare(class1, class2).AreEqual);
            Assert.IsFalse(_compare.Compare(class2, class1).AreEqual);
        }

        [Test]
        public void TestIndexerWithIgnoreCollectionLengthNegative()
        {
            var jane = new Person { Name = "Jane" };
            var john = new Person { Name = "John" };
            var mary = new Person { Name = "Mary" };
            var jack = new Person { Name = "Jack" };

            var nameList1 = new List<Person>() { jane, john, jack, mary };
            var nameList2 = new List<Person>() { jane, john, jack };

            var class1 = new ListClass<Person>(nameList1);
            var class2 = new ListClass<Person>(nameList2);

            var prior = _compare.Config.MaxDifferences;
            _compare.Config.MaxDifferences = int.MaxValue;
            _compare.Config.IgnoreCollectionOrder = true;

            var result = _compare.Compare(class1, class2);
            Console.WriteLine(result.DifferencesString);

            Assert.AreEqual(3, result.Differences.Count);

            result = _compare.Compare(class2, class1);
            Console.WriteLine(result.DifferencesString);
            Assert.AreEqual(3, result.Differences.Count);


            _compare.Config.MaxDifferences = prior;
        }

        [Test]
        public void CollectionWithSpecComplexMatch()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();
            var len = 3;

            var parent = new Entity
            {
                Description = "Me Papa",
                EntityLevel = Level.Company
            };

            //entity1.Parent = parent;
            //entity2.Parent = parent;

            for (int i = 0; i < len; i++)
            {
                entity1.Children.Add(new Entity
                {
                    Description = i.ToString(),
                    EntityLevel = Level.Division,
                    Parent = parent
                });


                entity2.Children.Add(new Entity
                {
                    Description = (i).ToString(),
                    EntityLevel = Level.Division,
                    Parent = parent
                });
            }

            var spec = new Dictionary<Type, IEnumerable<string>>();

            spec.Add(typeof(Entity), new string[] { "Parent", "Description" });

            _compare.Config.IgnoreCollectionOrder = true;

            _compare.Config.CollectionMatchingSpec = spec;
            _compare.Config.MaxDifferences = int.MaxValue;

            var result = _compare.Compare(entity1, entity2);

            Assert.That(result.AreEqual);
        }

        [Test]
        public void CollectionFirstBiggerThanSecondWithSpecMatchHasSpecMatchInResult()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();

            entity1.Children.Add(new Entity
            {
                Description = "Entity1",
                EntityLevel = Level.Company
            });
            entity1.Children.Add(new Entity
            {
                Description = "Entity2",
                EntityLevel = Level.Department
            });

            entity2.Children.Add(new Entity
            {
                Description = "Entity2",
                EntityLevel = Level.Department
            });


            var spec = new Dictionary<Type, IEnumerable<string>>();

            spec.Add(typeof(Entity), new string[] { "Description" });

            _compare.Config.IgnoreCollectionOrder = true;

            _compare.Config.MaxDifferences = int.MaxValue;

            _compare.Config.CollectionMatchingSpec = spec;

            var result = _compare.Compare(entity1, entity2);

            Assert.IsTrue(result.Differences.Where(d => d.Object1Value == "(null)" || d.Object2Value == "(null)").Count() > 0);
            Assert.IsTrue(result.Differences.Where(d => d.ToString().Contains("Description:")).Count() > 0);


        }

        [Test]
        public void CollectionWithSpecEmptyProps()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();
            var len = 3;

            for (int i = 0; i < len; i++)
            {
                entity1.Children.Add(new Entity
                {
                    Description = i.ToString(),
                    EntityLevel = Level.Company
                });

                entity2.Children.Add(new Entity
                {
                    Description = (i).ToString(),
                    EntityLevel = Level.Department
                });
            }

            var spec = new Dictionary<Type, IEnumerable<string>>();

            spec.Add(typeof(Entity), new string[] { });

            _compare.Config.IgnoreCollectionOrder = true;

            _compare.Config.CollectionMatchingSpec = spec;

            var result = _compare.Compare(entity1, entity2);


            Assert.AreEqual(result.Differences[0].Object1Value, Level.Company.ToString());

        }

        [Test]
        public void CollectionWithSpecEmptyPropsAndEquality()
        {
            var entity1 = new EntityWithEquality();
            var entity2 = new EntityWithEquality();
            var len = 3;

            for (int i = 0; i < len; i++)
            {
                entity1.Children.Add(new EntityWithEquality
                {
                    Description = i.ToString(),
                    EntityLevel = Level.Company
                });

                entity2.Children.Add(new EntityWithEquality
                {
                    Description = (i).ToString(),
                    EntityLevel = Level.Department
                });
            }

            var spec = new Dictionary<Type, IEnumerable<string>>();

            spec.Add(typeof(EntityWithEquality), new string[] { });

            _compare.Config.IgnoreCollectionOrder = true;

            _compare.Config.CollectionMatchingSpec = spec;

            var result = _compare.Compare(entity1, entity2);


            Assert.AreEqual(result.Differences[0].Object1Value, Level.Company.ToString());

            Assert.AreEqual(result.Differences[0].Object2Value, Level.Department.ToString());

            Assert.IsTrue(result.Differences[0].ToString().Contains("(Company,Department)"));

        }

        [Test]
        public void TestDictionaryWithIgnoreCollectionOrderOddOrder()
        {
            var p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Owen";

            var p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            var p3 = new Person();
            p3.Name = "Wink";
            p3.DateCreated = DateTime.Now.AddDays(-2);

            var p4 = new Person();
            p3.Name = "Larry";
            p3.DateCreated = DateTime.Now.AddDays(-3);

            var dict1 = new Dictionary<string, Person>();
            dict1.Add("1003", p3);
            dict1.Add("1001", p1);
            dict1.Add("1004", p4);
            dict1.Add("1002", p2);

            var dict2 = new Dictionary<string, Person>();
            dict2.Add("1002", p2);
            dict2.Add("1001", p1);
            dict2.Add("1003", p3);
            dict2.Add("1004", p4);

            Assert.IsFalse(_compare.Compare(dict1, dict2).AreEqual);
            Assert.IsFalse(_compare.Compare(dict2, dict1).AreEqual);
        }

        [Test]
        public void TestDictionaryWithIgnoreCollectionOrderNegative()
        {
            var p1 = new Person();
            p1.DateCreated = DateTime.Now;
            p1.Name = "Owen";

            var p2 = new Person();
            p2.Name = "Greg";
            p2.DateCreated = DateTime.Now.AddDays(-1);

            var p3 = new Person();
            p3.Name = "Wink";
            p3.DateCreated = DateTime.Now.AddDays(-2);

            var p4 = new Person();
            p3.Name = "Larry";
            p3.DateCreated = DateTime.Now.AddDays(-3);

            var dict1 = new Dictionary<string, Person>();
            dict1.Add("1001", p1);
            dict1.Add("1002", p2);
            dict1.Add("1003", p3);

            var dict2 = new Dictionary<string, Person>();
            dict2.Add("1002", p2);
            dict2.Add("1001", p1);
            dict2.Add("1003", p4);

            Assert.IsFalse(_compare.Compare(dict1, dict2).AreEqual);
        }

        [Test]
        public void HashSetsMultipleItemsWithIgnoreCollectionOrderNegativeTest()
        {
            HashSetWrapper hashSet1 = new HashSetWrapper
                                      {
                                          StatusId = 1,
                                          Name = "Paul"
                                      };
            HashSetWrapper hashSet2 = new HashSetWrapper
                                      {
                                          StatusId = 1,
                                          Name = "Paul"
                                      };

            HashSetClass secondClassObject1 = new HashSetClass
                                              {
                                                  Id = 1
                                              };
            HashSetClass secondClassObject2 = new HashSetClass
                                              {
                                                  Id = 2
                                              };

            HashSetClass secondClassObject3 = new HashSetClass
                                              {
                                                  Id = 3
                                              };
            HashSetClass secondClassObject4 = new HashSetClass
                                              {
                                                  Id = 4
                                              };


            hashSet1.HashSetCollection.Add(secondClassObject3);
            hashSet1.HashSetCollection.Add(secondClassObject1);
            hashSet1.HashSetCollection.Add(secondClassObject2);
            hashSet1.HashSetCollection.Add(secondClassObject4);

            hashSet2.HashSetCollection.Add(secondClassObject2);
            hashSet2.HashSetCollection.Add(secondClassObject4);
            hashSet2.HashSetCollection.Add(secondClassObject3);
            hashSet2.HashSetCollection.Add(secondClassObject1);

            _compare.Config.IgnoreCollectionOrder = false;


            Assert.IsFalse(_compare.Compare(hashSet1, hashSet2).AreEqual);
        }

        [Test]
        public void ComparerIgnoreOrderTest()
        {
            var elemA = new Element(1, 4);
            var elemB = Element.ReverseCopy(elemA);

            var comparer = new CompareLogic();
            comparer.Config.IgnoreCollectionOrder = true;

            ComparisonResult result = comparer.Compare(elemA, elemB);
            Assert.IsTrue(result.Differences.Count == 0); //difference count should be 0 but 1 difference is found
        }

        [Test]
        public void ComparerIgnoreOrderSimpleArraysTest()
        {
            var a = new String[] { "A", "E", "F" };
            var b = new String[] { "A", "c", "d", "F" };

            var comparer = new CompareLogic();
            comparer.Config.IgnoreCollectionOrder = true;
            comparer.Config.MaxDifferences = int.MaxValue;

            ComparisonResult result = comparer.Compare(a, b);
            Console.WriteLine(result.DifferencesString);
            Assert.IsTrue(result.Differences.Where(d => d.Object1Value == "E").Count() == 1);
            Assert.IsTrue(result.Differences.Where(d => d.Object2Value == "c").Count() == 1);
            Assert.IsTrue(result.Differences.Where(d => d.Object2Value == "d").Count() == 1);

        }

        [Test]
        public void CollectionDisorderedWithSpecMatch()
        {
            var entity1 = new Entity();
            var entity2 = new Entity();

            entity1.Children.Add(new Entity
            {
                Description = "1",
                EntityLevel = Level.Company
            });
            entity1.Children.Add(new Entity
            {
                Description = "2",
                EntityLevel = Level.Division
            });
            entity1.Children.Add(new Entity
            {
                Description = "3",
                EntityLevel = Level.Department
            });
            entity1.Children.Add(new Entity
            {
                Description = "4",
                EntityLevel = Level.Employee
            });

            entity2.Children.Add(new Entity
            {
                Description = "2",
                EntityLevel = Level.Department
            });

            entity2.Children.Add(new Entity
            {
                Description = "4",
                EntityLevel = Level.Department
            });

            entity2.Children.Add(new Entity
            {
                Description = "1",
                EntityLevel = Level.Department
            });

            entity2.Children.Add(new Entity
            {
                Description = "3",
                EntityLevel = Level.Division
            });

            var spec = new Dictionary<Type, IEnumerable<string>>();

            spec.Add(typeof(Entity), new string[] { "Description" });

            _compare.Config.IgnoreCollectionOrder = true;

            _compare.Config.MaxDifferences = int.MaxValue;

            _compare.Config.CollectionMatchingSpec = spec;

            var result = _compare.Compare(entity1, entity2);

            Console.WriteLine(result.DifferencesString);

            Assert.AreEqual(result.Differences[0].Object1Value, Level.Company.ToString());

            Assert.AreEqual(result.Differences[0].Object2Value, Level.Department.ToString());

            Assert.IsTrue(result.Differences[0].ToString().Contains("Description:1"));

            Assert.IsTrue(result.Differences.Where(d => d.Object1Value == "(null)" || d.Object2Value == "(null)").ToArray().Length == 0);

        }

        [Test]
        public void CollectionMatchingSpecBasedOnInterfaceTest()
        {
            IEntity entityToModify = new Entity {Id = 101, Description = "Desription 2"};
            IEntity entity1 = new Entity
            {
                Id = 1,
                Children = new List<IEntity>
                {
                    new Entity
                    {
                        Id = 10,
                        Children = new List<IEntity>
                        {
                            new Entity {Id = 100, Description = "Description 1"}
                        }
                    },
                    new Entity
                    {
                        Id = 11,
                        Children = new List<IEntity>
                        {
                            entityToModify,
                            new Entity {Id = 102, Description = "Description 3"}
                        }
                    }

                }
            };
            IEntity newlyAddedEntity = new Entity {Id = 103, Description = "Description 4"};
            IEntity modifiedEntity = new Entity {Id = 101, Description = "Desription 5"};
            IEntity entity2 = new Entity
            {
                Id = 1,
                Children = new List<IEntity>
                {
                    new Entity
                    {
                        Id = 10,
                        Children = new List<IEntity>
                        {
                            new Entity {Id = 100, Description = "Description 1"},
                            newlyAddedEntity
                        }
                    },
                    new Entity
                    {
                        Id = 11,
                        Children = new List<IEntity>
                        {
                            new Entity {Id = 102, Description = "Description 3"},
                            modifiedEntity
                        }
                    }

                }
            };

            ComparisonConfig config = new ComparisonConfig
            {
                IgnoreCollectionOrder = true,
                MaxDifferences = int.MaxValue,
                CollectionMatchingSpec = new Dictionary<Type, IEnumerable<string>> { { typeof(IEntity), new string[] { "Id" } } }
            };
            _compare.Config = config;

            var result = _compare.Compare(entity1, entity2);
            Assert.AreEqual(result.Differences.Count, 3);
            Assert.AreEqual(result.Differences[0].PropertyName, "Children[Id:10].Children");
            Assert.AreEqual(result.Differences[1].PropertyName, "Children[Id:10].Children[Id:103]");
            Assert.AreEqual(result.Differences[2].PropertyName, "Children[Id:11].Children[Id:101].Description");
        }

        [Test]
        public void CompareListsIgnoreOrderMatchingSpecValueInBaseClass()
        {
            List<DeriveFromOfficer> list1 = new List<DeriveFromOfficer>();
            list1.Add(new DeriveFromOfficer() { ID = 1, Name = "Logan 5" });
            list1.Add(new DeriveFromOfficer() { ID = 2, Name = "Francis 7" });

            List<DeriveFromOfficer> list2 = new List<DeriveFromOfficer>();
            list2.Add(new DeriveFromOfficer() { ID = 2, Name = "Francis 7" });
            list2.Add(new DeriveFromOfficer() { ID = 1, Name = "Logan 4" });

            ComparisonConfig config = new ComparisonConfig();
            Dictionary<Type, IEnumerable<string>> collectionSpec = new Dictionary<Type, IEnumerable<string>>();
            collectionSpec.Add(typeof(Officer), new string[] { "ID" });

            config.IgnoreCollectionOrder = true;
            config.CollectionMatchingSpec = collectionSpec;

            CompareLogic compareLogic = new CompareLogic(config);
            var result = compareLogic.Compare(list1, list2);
            Assert.IsFalse(result.AreEqual);
            Assert.AreNotEqual(result.Differences.First().Object2, null);
            Assert.AreEqual(result.Differences.First().Object1, "Logan 5");
            Assert.AreEqual(result.Differences.First().Object2, "Logan 4");
        }

        [Test]
        public void ClassTypeInListIgnorePositive()
        {
            var comparisonConfig = new ComparisonConfig()
            {
                MaxDifferences = int.MaxValue,
                MembersToIgnore = new List<string>() { "Type" },
                ClassTypesToIgnore = new List<Type>() { typeof(Officer) },
                CollectionMatchingSpec = new Dictionary<System.Type, IEnumerable<string>>()
                {
                    {
                        typeof(Officer),
                        new List<string>() { "ID" }
                    },
                    {
                        typeof(Person),
                        new List<string>() { "ID" }
                    }
                },
                TreatStringEmptyAndNullTheSame = true,
                IgnoreCollectionOrder = true
            };

            _compare.Config = comparisonConfig;

            var list1 = new List<object>();
            var list2 = new List<object>();

            Officer p1 = new Officer();
            p1.ID = 2;
            p1.Name = "Greg";
            p1.Type = Deck.AstroPhysics;

            Officer p2 = new Officer();
            p2.ID = 1;
            p2.Name = "Leyla";
            p2.Type = Deck.Engineering;

            Person p3 = new Person();
            p3.ID = 3;
            p3.Name = "Henk";
            p3.DateCreated = DateTime.Now;

            Person p4 = new Person();
            p4.ID = 3;
            p4.Name = "Henk";
            p4.DateCreated = p3.DateCreated;

            list1.Add(p1);
            list1.Add(p3);
            list2.Add(p2);
            list2.Add(p4);

            var result = _compare.Compare(list1, list2);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [Test]
        public void CompareListsIgnoreOrderTypeAndNull()
        {
            List<Person> list1 = new List<Person>();
            list1.Add(new Person() { ID = 1, Name = "Logan 5" });
            list1.Add(new Person() { ID = 2, Name = "Francis 7" });
            list1.Add(new Person() { ID = 3, Name = null });

            List<Officer> list2 = new List<Officer>();
            list2.Add(new Officer() { ID = 2, Name = "Francis 7" });
            list2.Add(new Officer() { ID = 1, Name = "Logan 5" });
            list2.Add(new Officer() { ID = 3, Name = string.Empty });

            ComparisonConfig config = new ComparisonConfig();
            Dictionary<Type, IEnumerable<string>> collectionSpec = new Dictionary<Type, IEnumerable<string>>();
            collectionSpec.Add(typeof(Person), new string[] { "ID", "Name" });
            collectionSpec.Add(typeof(Officer), new string[] { "ID", "Name" });

            config.IgnoreObjectTypes = true;
            config.IgnoreCollectionOrder = true;
            config.TreatStringEmptyAndNullTheSame = true;
            config.CollectionMatchingSpec = collectionSpec;

            CompareLogic compareLogic = new CompareLogic(config);
            var result = compareLogic.Compare(list1, list2);
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void CompareListsIgnoreOrderAndNull()
        {
            List<Person> list1 = new List<Person>();
            list1.Add(new Person() { ID = 1, Name = "Logan 5" });
            list1.Add(new Person() { ID = 2, Name = "Francis 7" });
            list1.Add(new Person() { ID = 3, Name = null });

            List<Person> list2 = new List<Person>();
            list2.Add(new Person() { ID = 2, Name = "Francis 7" });
            list2.Add(new Person() { ID = 1, Name = "Logan 5" });
            list2.Add(new Person() { ID = 3, Name = string.Empty });

            ComparisonConfig config = new ComparisonConfig();

            config.IgnoreCollectionOrder = true;
            config.TreatStringEmptyAndNullTheSame = true;

            CompareLogic compareLogic = new CompareLogic(config);
            var result = compareLogic.Compare(list1, list2);
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void CompareListsInOrderAndIgnoreNull()
        {
            List<Person> list1 = new List<Person>();
            list1.Add(new Person() { ID = 1, Name = "Logan 5" });
            list1.Add(new Person() { ID = 2, Name = "Francis 7" });
            list1.Add(new Person() { ID = 3, Name = null });

            List<Person> list2 = new List<Person>();
            list2.Add(new Person() { ID = 1, Name = "Logan 5" });
            list2.Add(new Person() { ID = 2, Name = "Francis 7" });
            list2.Add(new Person() { ID = 3, Name = string.Empty });

            ComparisonConfig config = new ComparisonConfig();

            config.IgnoreCollectionOrder = true;
            config.TreatStringEmptyAndNullTheSame = true;

            CompareLogic compareLogic = new CompareLogic(config);
            var result = compareLogic.Compare(list1, list2);
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void CompareListsIgnoreOrderAndCase()
        {
            List<Person> list1 = new List<Person>();
            list1.Add(new Person() { ID = 1, Name = "Logan 5" });
            list1.Add(new Person() { ID = 2, Name = "Francis 7" });

            List<Person> list2 = new List<Person>();
            list2.Add(new Person() { ID = 2, Name = "francis 7" });
            list2.Add(new Person() { ID = 1, Name = "logan 5" });

            ComparisonConfig config = new ComparisonConfig();

            config.IgnoreCollectionOrder = true;
            config.CaseSensitive = false;

            CompareLogic compareLogic = new CompareLogic(config);
            var result = compareLogic.Compare(list1, list2);
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void CompareListsInOrderAndIgnorCase()
        {
            List<Person> list1 = new List<Person>();
            list1.Add(new Person() { ID = 1, Name = "Logan 5" });
            list1.Add(new Person() { ID = 2, Name = "Francis 7" });

            List<Person> list2 = new List<Person>();
            list2.Add(new Person() { ID = 1, Name = "logan 5" });
            list2.Add(new Person() { ID = 2, Name = "francis 7" });
            

            ComparisonConfig config = new ComparisonConfig();

            config.IgnoreCollectionOrder = true;
            config.CaseSensitive = false;

            CompareLogic compareLogic = new CompareLogic(config);
            var result = compareLogic.Compare(list1, list2);
            Assert.IsTrue(result.AreEqual);
        }
        #endregion
    }
}
