using System;
using System.Collections;
using System.Linq;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compares System.Collections.Immutable.ImmutableArray
    /// </summary>
    public class ImmutableArrayComparer : BaseTypeComparer
    {
        private readonly ListComparer _listComparer;

        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public ImmutableArrayComparer(RootComparer rootComparer) : base(rootComparer)
        {
            _listComparer = new ListComparer(rootComparer);
        }

        /// <summary>
        /// Returns true if both are immutable arrays with same generic argument type
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsImmutableArray(type1) && TypeHelper.IsImmutableArray(type2);
        }

        /// <summary>
        /// Compares two immutable arrays
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            //This should never happen, null check happens one level up
            if (parms.Object1 == null || parms.Object2 == null)
                return;

            parms.Object1 = ((IList) parms.Object1).Cast<object>().ToArray();
            parms.Object2 = ((IList) parms.Object2).Cast<object>().ToArray();

            _listComparer.CompareType(parms);
        }
    }
}
