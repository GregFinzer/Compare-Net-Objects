using System;
using System.Collections;


namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare two objects of type class
    /// </summary>
    public class ClassComparer : BaseTypeComparer
    {
        private readonly PropertyComparer _propertyComparer;
        private readonly FieldComparer _fieldComparer;

        /// <summary>
        /// Constructor for the class comparer
        /// </summary>
        /// <param name="rootComparer">The root comparer instantiated by the RootComparerFactory</param>
        public ClassComparer(RootComparer rootComparer) : base(rootComparer)
        {
            _propertyComparer = new PropertyComparer(rootComparer);
            _fieldComparer = new FieldComparer(rootComparer);
        }

        /// <summary>
        /// Returns true if the both objects are a class
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return (TypeHelper.IsClass(type1) && TypeHelper.IsClass(type2))
                || (TypeHelper.IsInterface(type1) && TypeHelper.IsInterface(type2));
        }

        /// <summary>
        /// Compare two classes
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            try
            {
                parms.Result.AddParent(parms.Object1.GetHashCode());
                parms.Result.AddParent(parms.Object2.GetHashCode());

                //Custom classes that implement IEnumerable may have the same hash code
                //Ignore objects with the same hash code
                if (!(parms.Object1 is IEnumerable)
                    && ReferenceEquals(parms.Object1, parms.Object2))
                {
                    return;
                }

                Type t1 = parms.Object1.GetType();
                Type t2 = parms.Object2.GetType();

                //Check if the class type should be excluded based on the configuration
                if (ExcludeLogic.ShouldExcludeClassType(parms.Config, t1, t2))
                    return;

                parms.Object1Type = t1;
                parms.Object2Type = t2;

                //Compare the properties
                if (parms.Config.CompareProperties)
                    _propertyComparer.PerformCompareProperties(parms);

                //Compare the fields
                if (parms.Config.CompareFields)
                    _fieldComparer.PerformCompareFields(parms);
            }
            finally
            {
                parms.Result.RemoveParent(parms.Object1.GetHashCode());
                parms.Result.RemoveParent(parms.Object2.GetHashCode());
            }
        }
    }
}
