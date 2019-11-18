using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using KellermanSoftware.CompareNetObjectsTests.TestClasses.OnlyIce;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CustomComparerTests
    {
        [Test]
        public void UseCustomComparer()
        {
            CompareLogic compareLogic = new CompareLogic();

            SpecificTenant tenant1 = new SpecificTenant();
            tenant1.Name = "wire";
            tenant1.Amount = 37;

            SpecificTenant tenant2 = new SpecificTenant();
            tenant2.Name = "wire";
            tenant2.Amount = 155;

            //No Custom Comparer
            Assert.IsFalse(compareLogic.Compare(tenant1, tenant2).AreEqual);

            //specify custom selector
            compareLogic.Config.CustomComparers.Add(new MyCustomComparer(RootComparerFactory.GetRootComparer()));

            Assert.IsTrue(compareLogic.Compare(tenant1, tenant2).AreEqual);

            tenant2.Amount = 42;
            Assert.IsFalse(compareLogic.Compare(tenant1, tenant2).AreEqual);
        }

        [Test]
        public void UseFuncCustomComparer()
        {

            CompareLogic compareLogic = new CompareLogic();

            SpecificTenant tenant1 = new SpecificTenant();
            tenant1.Name = "wire";
            tenant1.Amount = 37;

            SpecificTenant tenant2 = new SpecificTenant();
            tenant2.Name = "wire";
            tenant2.Amount = 155;

            //No Custom Comparer
            Assert.IsFalse(compareLogic.Compare(tenant1, tenant2).AreEqual);

            //specify custom selector
            //using the same rule as MyCustomComparer
            compareLogic.Config.CustomComparers.Add(new CustomComparer<SpecificTenant, SpecificTenant>(
                (st1, st2) => !(st1.Name != st2.Name || st1.Amount > 100 || st2.Amount < 100)
            ));

            Assert.IsTrue(compareLogic.Compare(tenant1, tenant2).AreEqual);

            tenant2.Amount = 42;
            Assert.IsFalse(compareLogic.Compare(tenant1, tenant2).AreEqual);

            //Change rule at runtime
            var comparerer = compareLogic.Config.CustomComparers[0] as CustomComparer<SpecificTenant, SpecificTenant>;
            comparerer.Compare = (st1, st2) => !(st1.Name != st2.Name || st1.Amount > 100 || st2.Amount < 40);
            Assert.IsTrue(compareLogic.Compare(tenant1, tenant2).AreEqual);
        }

        [Test]
        public void CustomBaseClassPropertyComparerForEnum()
        {
            var config = new ComparisonConfig();
            config.CustomPropertyComparer<DtoMetaCampo>(metaCampo => metaCampo.Obrigatorio, new CustomComparer<EnumSimNao, EnumSimNao>((i, i1) => i == EnumSimNao.NAO));

            var dtoMetaCampo1 = new DtoMetaCampo { Obrigatorio = EnumSimNao.NAO, Id = 1 };
            var dtoMetaCampo2 = new DtoMetaCampo { Obrigatorio = EnumSimNao.SIM, Id = 1 };

            var compare = new CompareLogic(config);

            var result = compare.Compare(dtoMetaCampo1, dtoMetaCampo2);
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void CustomBaseClassPropertyComparer()
        {
            var config = new ComparisonConfig();
            config.CustomPropertyComparer<Officer>(officer => officer.ID,
                new CustomComparer<int, int>((i, i1) => i % 2 == 1));

            var deriveFromOfficer1 = new Officer { ID = 1, Name = "John", Type = Deck.Engineering };
            var deriveFromOfficer2 = new Officer { ID = 2, Name = "John", Type = Deck.Engineering };

            var derive2FromOfficer1 = new Derive2FromOfficer { Email = "a@a.com", ID = 3, Name = "John", Type = Deck.Engineering };
            var derive2FromOfficer2 = new Derive2FromOfficer { Email = "a@a.com", ID = 4, Name = "John", Type = Deck.Engineering };

            var compare = new CompareLogic(config);

            var result = compare.Compare(deriveFromOfficer1, deriveFromOfficer2);
            Assert.IsTrue(result.AreEqual);

            result = compare.Compare(derive2FromOfficer1, derive2FromOfficer2);
            Assert.IsTrue(result.AreEqual);
        }
    }
}
