#if NET6_0_OR_GREATER

using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare dates
    /// </summary>
    public sealed class TimeOnlyComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public TimeOnlyComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both types are TimeOnly
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsTimeOnly(type1) && TypeHelper.IsTimeOnly(type2);
        }

        /// <summary>
        /// Compare two DateOnly variables
        /// </summary>
        /// <param name="parms"></param>
        public override void CompareType(CompareParms parms)
        {
            //This should never happen, null check happens one level up
            if (parms.Object1 == null || parms.Object2 == null)
                return;

            var time1 = (TimeOnly)parms.Object1;
            var time2 = (TimeOnly)parms.Object2;

            if (!time1.Equals(time2))
            {
                AddDifference(parms);
            }
        }
    }
}

#endif