using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare DateTimeOffsets with the ability to ignore millisecond differences
    /// </summary>
    public class DateTimeOffSetComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public DateTimeOffSetComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both types are DateTimeOffset
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsDateTimeOffset(type1) && TypeHelper.IsDateTimeOffset(type2);
        }

        /// <summary>
        /// Compare two DateTimeOffset
        /// </summary>
        /// <param name="parms"></param>
        public override void CompareType(CompareParms parms)
        {
            //This should never happen, null check happens one level up
            if (parms.Object1 == null || parms.Object2 == null)
                return;

            DateTimeOffset date1 = (DateTimeOffset)parms.Object1;
            DateTimeOffset date2 = (DateTimeOffset)parms.Object2;

            if (Math.Abs(date1.DateTime.Subtract(date2.DateTime).TotalMilliseconds) > parms.Config.MaxMillisecondsDateDifference)
                AddDifference(parms);

        }
    }
}
