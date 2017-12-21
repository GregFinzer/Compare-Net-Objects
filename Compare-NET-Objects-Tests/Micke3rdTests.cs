using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class Micke3rdTests
    {
        [Test]
        public void WhenACollectionHasSomeOtherPropertiesOntItBesidesTheNormalOnesItShouldCompareThem()
        {
            var dog = new Micke3rdDog("scooby doo");
            var collection1 = new Micke3rdCollection<Micke3rdDog>();
            collection1.Foo = false;
            collection1.Add(dog);

            var person1 = new Micke3rdPerson();
            person1.Pets = collection1;

            var collection2 = new Micke3rdCollection<Micke3rdDog>();
            collection2.Foo = true;
            collection2.Add(dog);

            var person2 = new Micke3rdPerson();
            person2.Pets = collection2;

            Assert.IsTrue(person1.Pets.Foo != person2.Pets.Foo, "Foo's are different");

            var compareLogic = new CompareLogic();
            var result = compareLogic.Compare(person1, person2);
            Assert.IsFalse(result.AreEqual, "Foo's are different");
        }


    }

    public class Micke3rdCollection<T> : List<T>
    {
        public bool Foo { get; set; }
    }


    public class Micke3rdPerson
    {
        public Micke3rdCollection<Micke3rdDog> Pets { get; set; }
    }

    public class Micke3rdDog
    {
        public Micke3rdDog(string name)
        {
            this.Name = name;
        }
        public string Name { get; protected set; }
    }
}