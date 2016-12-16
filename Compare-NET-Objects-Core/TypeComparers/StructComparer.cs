using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare two structs
    /// </summary>
    public class StructComparer : BaseTypeComparer
    {
        private readonly PropertyComparer _propertyComparer;
        private readonly FieldComparer _fieldComparer;

        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public StructComparer(RootComparer rootComparer) : base(rootComparer)
        {
            _propertyComparer = new PropertyComparer(rootComparer);
            _fieldComparer = new FieldComparer(rootComparer);
        }

        /// <summary>
        /// Returns true if both objects are of type struct
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsStruct(type1) && TypeHelper.IsStruct(type2);
        }

        /// <summary>
        /// Compare two structs
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            if (parms.Result.CurrentStructDepth >= parms.Config.MaxStructDepth)
                return;

            try
            {
                parms.Result.CurrentStructDepth++;
                parms.Object1Type  = parms.Object1.GetType();
                _fieldComparer.PerformCompareFields(parms);
                _propertyComparer.PerformCompareProperties(parms);
            }
            finally
            {
                parms.Result.CurrentStructDepth--;
            }
        }
    }
}
