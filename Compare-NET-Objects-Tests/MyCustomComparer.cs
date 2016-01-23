using System;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;

namespace KellermanSoftware.CompareNetObjectsTests
{
    public class MyCustomComparer : BaseTypeComparer
    {
        public MyCustomComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return type1 == typeof (SpecificTenant);
        }

        public override void CompareType(CompareParms parms)
        {
            var st1 = (SpecificTenant)parms.Object1;
            var st2 = (SpecificTenant)parms.Object2;

            if (st1.Name != st2.Name || st1.Amount > 100 || st2.Amount < 100)
            {
                AddDifference(parms);
            }
        }
    }
}
