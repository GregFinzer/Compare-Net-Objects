using System;
using System.Globalization;


namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Logic to compare two timespans
    /// </summary>
    public class TimespanComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public TimespanComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both objects are timespans
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsTimespan(type1) && TypeHelper.IsTimespan(type2);
        }

        /// <summary>
        /// Compare two timespans
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            if (((TimeSpan)parms.Object1).Ticks != ((TimeSpan)parms.Object2).Ticks)
            {
                Difference difference = new Difference
                {
                    ParentObject1 =  new WeakReference(parms.ParentObject1),
                    ParentObject2 =  new WeakReference(parms.ParentObject2),
                    PropertyName = parms.BreadCrumb,
                    Object1Value = ((TimeSpan)parms.Object1).Ticks.ToString(CultureInfo.InvariantCulture),
                    Object2Value = ((TimeSpan)parms.Object2).Ticks.ToString(CultureInfo.InvariantCulture),
                    ChildPropertyName = "Ticks",
                    Object1 = new WeakReference(parms.Object1),
                    Object2 = new WeakReference(parms.Object2)
                };

                AddDifference(parms.Result, difference);
            }
        }
    }
}
