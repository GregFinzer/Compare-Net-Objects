using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare dates with the option to ignore based on milliseconds
    /// </summary>
    public class DateTimeComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public DateTimeComparer(RootComparer rootComparer) : base(rootComparer)
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

            if (date1.Kind != date2.Kind)
            {
                if (date1.Kind == DateTimeKind.Unspecified && parms.Config.DateTimeKindToUseWhenUnspecified != null)
                    date1 = DateTime.SpecifyKind(date1, parms.Config.DateTimeKindToUseWhenUnspecified.Value);

                if (date2.Kind == DateTimeKind.Unspecified && parms.Config.DateTimeKindToUseWhenUnspecified != null)
                    date2 = DateTime.SpecifyKind(date2, parms.Config.DateTimeKindToUseWhenUnspecified.Value);

                date1 = date1.ToUniversalTime();
                date2 = date2.ToUniversalTime();
            }

            if (Math.Abs(date1.Subtract(date2).TotalMilliseconds) > parms.Config.MaxMillisecondsDateDifference)
                AddDifference(parms);

        }
    }
}
