using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Logic to compare to pointers
    /// </summary>
    public class PointerComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public PointerComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both types are a pointer
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsPointer(type1) && TypeHelper.IsPointer(type2);
        }

        /// <summary>
        /// Compare two pointers
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            if ((parms.Object1 is IntPtr && parms.Object2 is IntPtr && ((IntPtr)parms.Object1) != ((IntPtr)parms.Object2))
                || (parms.Object1 is UIntPtr && parms.Object2 is UIntPtr && ((UIntPtr)parms.Object1) != ((UIntPtr)parms.Object2)))
            {
                Difference difference = new Difference
                {
                    ParentObject1 = new WeakReference(parms.ParentObject1),
                    ParentObject2 = new WeakReference(parms.ParentObject2),
                    PropertyName = parms.BreadCrumb,
                    Object1 = new WeakReference(parms.Object1),
                    Object2 = new WeakReference(parms.Object2)
                };

                AddDifference(parms.Result, difference);
            }
        }
    }
}
