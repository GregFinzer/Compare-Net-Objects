using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Logic to compare two runtime types
    /// </summary>
    public class RuntimeTypeComparer : BaseTypeComparer 
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public RuntimeTypeComparer(RootComparer rootComparer)
            : base(rootComparer)
        {}


        /// <summary>
        /// Returns true if both types are of type runtme type
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsRuntimeType(type1) && TypeHelper.IsRuntimeType(type2);
        }

        /// <summary>
        /// Compare two runtime types
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            Type t1 = (Type)parms.Object1;
            Type t2 = (Type)parms.Object2;

            if (t1.FullName != t2.FullName)
            {
                Difference difference = new Difference
                {
                    ParentObject1 =  new WeakReference(parms.ParentObject1),
                    ParentObject2 =  new WeakReference(parms.ParentObject2),
                    PropertyName = parms.BreadCrumb,
                    Object1Value = t1.FullName,
                    Object2Value = t2.FullName,
                    ChildPropertyName = "FullName",
                    Object1 = new WeakReference(parms.Object1),
                    Object2 = new WeakReference(parms.Object2)
                };

                AddDifference(parms.Result, difference);
            }
        }
    }
}
