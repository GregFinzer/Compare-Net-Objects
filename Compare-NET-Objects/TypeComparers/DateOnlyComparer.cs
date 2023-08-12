#if NET6_0_OR_GREATER

using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare dates
    /// </summary>
    public sealed class DateOnlyComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public DateOnlyComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both types are DateOnly
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsDateOnly(type1) && TypeHelper.IsDateOnly(type2);
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

            var date1 = (DateOnly)parms.Object1;
            var date2 = (DateOnly)parms.Object2;

            if (!date1.Equals(date2)) {
                AddDifference(parms);
            }
        }
    }
}

#endif