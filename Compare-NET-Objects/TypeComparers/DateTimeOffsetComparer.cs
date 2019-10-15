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

            if (parms.Config.IgnoreDateTimeOffsetTimezones)
            {
                date1 = date1.ToUniversalTime();
                date2 = date2.ToUniversalTime();
            }

            if (parms.Config.CompareDateTimeOffsetWithOffsets 
                && Math.Abs((date1 - date2).TotalMilliseconds) > parms.Config.MaxMillisecondsDateDifference)
            {
                AddDifference(parms);
            }
            else if (!parms.Config.CompareDateTimeOffsetWithOffsets)
            {                
                DateTime date1NoOffset = new DateTime(date1.Year, date1.Month, date1.Day, date1.Hour, date1.Minute, date1.Second);
                date1NoOffset = date1NoOffset.AddMilliseconds(date1.Millisecond);

                DateTime date2NoOffset = new DateTime(date2.Year, date2.Month, date2.Day, date2.Hour, date2.Minute, date2.Second);
                date2NoOffset = date2NoOffset.AddMilliseconds(date2.Millisecond);

                if (Math.Abs(date1NoOffset.Subtract(date2NoOffset).TotalMilliseconds) > parms.Config.MaxMillisecondsDateDifference)
                {
                    AddDifference(parms);
                }
            }
        }
    }
}
