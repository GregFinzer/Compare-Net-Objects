using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Common functionality for all Type Comparers
    /// </summary>
    public abstract class BaseTypeComparer : BaseComparer
    {        
        /// <summary>
        /// A reference to the root comparer as newed up by the RootComparerFactory
        /// </summary>
        public RootComparer RootComparer { get; set; }

        /// <summary>
        /// Protected constructor that references the root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        protected BaseTypeComparer(RootComparer rootComparer)
        {
            RootComparer = rootComparer;
        }


        /// <summary>
        /// If true the type comparer will handle the comparison for the type
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public abstract bool IsTypeMatch(Type type1, Type type2);

        /// <summary>
        /// Compare the two objects
        /// </summary>
        public abstract void CompareType(CompareParms parms);


    }
}
