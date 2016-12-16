using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Logic to compare to enum values
    /// </summary>
    public class EnumComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor with a default root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public EnumComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both objects are of type enum
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsEnum(type1) && TypeHelper.IsEnum(type2);
        }

        /// <summary>
        /// Compare two enums
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            if (parms.Object1.ToString() != parms.Object2.ToString())
            {
                AddDifference(parms);
            }
        }
    }
}
