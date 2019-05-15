using System;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.IgnoreExample
{
    class StringMightBeFloat : BaseTypeComparer
    {
        public StringMightBeFloat(RootComparer rootComparer) : base(rootComparer)
        {
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return type1 == typeof(string) && type2 == typeof(string);
        }

        public override void CompareType(CompareParms parms)
        {
            {

                var st1 = (string)parms.Object1;
                var st2 = (string)parms.Object2;

                if (st1.Equals(st2))
                {
                    return;
                }

                float flt1;
                float flt2;

                if (float.TryParse(st1, out flt1) && float.TryParse(st2, out flt2))
                {
                    if (flt1.Equals(flt2))
                    {
                        return;
                    }
                }

                AddDifference(parms);
            }
        }
    }
}
