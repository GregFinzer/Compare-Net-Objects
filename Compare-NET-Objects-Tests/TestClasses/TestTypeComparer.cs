using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using System;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal class TestTypeComparer : BaseTypeComparer
    {
        public bool CompareTypeCalled { get; set; }
        public bool IsTypeMatchCalled { get; set; }

        public TestTypeComparer(RootComparer rootComparer) 
            : base(rootComparer)
        {
            CompareTypeCalled = false;
            IsTypeMatchCalled = false;
        }


        public override void CompareType(CompareParms parms)
        {
            CompareTypeCalled = true;
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            IsTypeMatchCalled = true;
            return false;
        }

        public void Reset()
        {
            CompareTypeCalled = false;
            IsTypeMatchCalled = false;
        }
    }
}
