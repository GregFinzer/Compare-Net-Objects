using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare two generic objects
    /// </summary>
    /// <typeparam name="T1">The type of the first object</typeparam>
    /// <typeparam name="T2">The type of the second object</typeparam>
    public class CustomComparer<T1, T2> : BaseTypeComparer
    {
        /// <summary>
        /// Method to evaluate the results, return true if two objects are equal
        /// </summary>
        public Func<T1, T2, bool> Compare = (t1, t2) => false;

        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public CustomComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Constructor that takes a default root comparer
        /// </summary>
        public CustomComparer() : this(RootComparerFactory.GetRootComparer())
        {
        }

        /// <summary>
        /// Constructor that takes a the predication with a default root comparer
        /// </summary>
        /// <param name="compare">A function to determine if two objects are equal</param>
        public CustomComparer(Func<T1, T2, bool> compare) : this(RootComparerFactory.GetRootComparer(), compare)
        {
        }

        /// <summary>
        /// Constructor that takes a the predication with a root comparer
        /// </summary>
        /// <param name="rootComparer">The root comparer</param>
        /// <param name="compare">Method to determine if two objects are equal</param>
        public CustomComparer(RootComparer rootComparer, Func<T1, T2, bool> compare) : this(rootComparer)
        {
            Compare = compare;
        }
        /// <summary>
        /// Compare two objects
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            if (!Compare((T1)parms.Object1, (T2)parms.Object2))
            {
                AddDifference(parms);
            }
        }
        /// <summary>
        /// Returns true if both objects match their types
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return type1 == typeof(T1) && type2 == typeof(T2);
        }
    }
}