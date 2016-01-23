using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare dates with the option to ignore based on milliseconds
    /// </summary>
    public class DateComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public DateComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both types are DateTime
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsDateTime(type1) && TypeHelper.IsDateTime(type2);
        }

        /// <summary>
        /// Compare two DateTime variables
        /// </summary>
        /// <param name="parms"></param>
        public override void CompareType(CompareParms parms)
        {
            //This should never happen, null check happens one level up
            if (parms.Object1 == null || parms.Object2 == null)
                return;

            DateTime date1 = (DateTime) parms.Object1;
            DateTime date2 = (DateTime) parms.Object2;

            if (Math.Abs(date1.Subtract(date2).TotalMilliseconds) > parms.Config.MaxMillisecondsDateDifference)
                AddDifference(parms);

        }
    }
}
