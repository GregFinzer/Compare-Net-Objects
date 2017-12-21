using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare two StringBuilders
    /// </summary>
    public class StringBuilderComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public StringBuilderComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both objects are a StringBuilder
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return (TypeHelper.IsStringBuilder(type1) && TypeHelper.IsStringBuilder(type2))
                   || (TypeHelper.IsStringBuilder(type1) && type2 == null)
                   || (TypeHelper.IsStringBuilder(type2) && type1 == null);
        }

        /// <summary>
        /// Compare two string builders
        /// </summary>
        /// <param name="parms"></param>
        public override void CompareType(CompareParms parms)
        {
            if (parms.Config.TreatStringEmptyAndNullTheSame
                && ((parms.Object1 == null && parms.Object2 != null && parms.Object2.ToString() == string.Empty)
                    || (parms.Object2 == null && parms.Object1 != null && parms.Object1.ToString() == string.Empty)))
            {
                return;
            }

            string object1String = parms.Object1.ToString();
            string object2String = parms.Object2.ToString();

            if (!parms.Config.CaseSensitive)
            {
                if (!String.Equals(object1String, object2String, StringComparison.OrdinalIgnoreCase))
                {
                    AddDifference(parms);
                }
            }
            else if (object1String != object2String)
            {
                AddDifference(parms);
            }
        }
    }
}
