using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare Decimal values with the ability to specify the precision
    /// </summary>
    public class DecimalComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public DecimalComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both types are double
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsDecimal(type1) && TypeHelper.IsDecimal(type2);
        }

        /// <summary>
        /// Compare two decimals
        /// </summary>
        /// <param name="parms"></param>
        public override void CompareType(CompareParms parms)
        {
            //This should never happen, null check happens one level up
            if (parms.Object1 == null || parms.Object2 == null)
                return;

           decimal decimal1 = (decimal)parms.Object1;
           decimal decimal2 = (decimal)parms.Object2;

           decimal difference = Math.Abs(decimal1 * parms.Config.DecimalPrecision);

           if (Math.Abs(decimal1 - decimal2) > difference)
                AddDifference(parms);
        }
    }
}
