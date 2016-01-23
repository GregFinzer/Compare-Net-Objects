using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare two strings
    /// </summary>
    public class StringComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public StringComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both objects are a string or if one is a string and one is a a null
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return (TypeHelper.IsString(type1) && TypeHelper.IsString(type2))
                   || (TypeHelper.IsString(type1) && type2 == null)
                   || (TypeHelper.IsString(type2) && type1 == null);
        }

        /// <summary>
        /// Compare two strings
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            if (parms.Config.TreatStringEmptyAndNullTheSame 
                && ((parms.Object1 == null && parms.Object2 != null && parms.Object2.ToString() == string.Empty)
                    || (parms.Object2 == null && parms.Object1 != null && parms.Object1.ToString() == string.Empty)))
            {
                return;
            }

            if (OneOfTheStringsIsNull(parms)) return;

            string string1 = parms.Object1 as string;
            string string2 = parms.Object2 as string;

            if (string1 != string2)
            {
                AddDifference(parms);
            }
        }

        private bool OneOfTheStringsIsNull(CompareParms parms)
        {
            if (parms.Object1 == null || parms.Object2 == null)
            {
                AddDifference(parms);
                return true;
            }
            return false;
        }
    }
}
