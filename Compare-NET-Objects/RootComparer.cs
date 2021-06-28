using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using System.Reflection;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// The base comparer which contains all the type comparers
    /// </summary>
    public class RootComparer : BaseComparer
    {
        #region Properties


        /// <summary>
        /// A list of the type comparers
        /// </summary>
        internal List<BaseTypeComparer> TypeComparers { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Compare two objects
        /// </summary>
        public bool Compare(CompareParms parms)
        {
            try
            {
                if (parms.Object1 == null && parms.Object2 == null)
                    return true;

                Type t1 = parms.Object1 != null ? parms.Object1.GetType() : null;
                Type t2 = parms.Object2 != null ? parms.Object2.GetType() : null;

                if (ExcludeLogic.ShouldExcludeType(parms.Config, t1, t2))
                    return true;

                BaseTypeComparer customComparer = parms.Config.CustomComparers.FirstOrDefault(o => o.IsTypeMatch(t1, t2));

                if (customComparer != null)
                {
                    customComparer.CompareType(parms);
                }
                else if (parms.CustomPropertyComparer != null)
                {
                    parms.CustomPropertyComparer.CompareType(parms);
                }
                else
                {
                    BaseTypeComparer typeComparer = TypeComparers.FirstOrDefault(o => o.IsTypeMatch(t1, t2));

                    if (typeComparer != null)
                    {
                        if (parms.Config.IgnoreObjectTypes || !TypesDifferent(parms, t1, t2))
                        {
                            typeComparer.CompareType(parms);
                        }
                    }
                    else
                    {
                        if (EitherObjectIsNull(parms)) return false;

                        if (!parms.Config.IgnoreObjectTypes && t1 != null)
                            throw new NotSupportedException("Cannot compare object of type " + t1.Name);
                    }
                }

            }
            catch (ObjectDisposedException)
            {
                if (!parms.Config.IgnoreObjectDisposedException)
                    throw;

                return true;
            }

            return parms.Result.AreEqual;
        }

        private bool TypesDifferent(CompareParms parms, Type t1, Type t2)
        {
            //Objects must be the same type and not be null
            if (!parms.Config.IgnoreObjectTypes
                && parms.Object1 != null 
                && parms.Object2 != null 
                && t1 != t2)
            {
                //Only care if they are in the same inheritance hierarchy or decleared as the same interface.
                if (parms.Config.IgnoreConcreteTypes
                && (parms.Object1DeclaredType != null
                && parms.Object2DeclaredType != null
                && parms.Object1DeclaredType == parms.Object2DeclaredType
                || (t1.IsAssignableFrom(t2) || t2.IsAssignableFrom(t1))))
                {
                    return false;
                }

                Difference difference = new Difference
                {
                    ParentObject1 = parms.ParentObject1,
                    ParentObject2 = parms.ParentObject2,
                    PropertyName = parms.BreadCrumb,
                    Object1Value = t1.FullName,
                    Object2Value = t2.FullName,
                    ChildPropertyName = "GetType()",
                    MessagePrefix = "Different Types",
                    Object1 = parms.Object1,
                    Object2 = parms.Object2
                };

                AddDifference(parms.Result, difference);
                return true;
            }

            return false;
        }

        private bool EitherObjectIsNull(CompareParms parms)
        {
            //Check if one of them is null
            if (parms.Object1 == null || parms.Object2 == null)
            {
                AddDifference(parms);
                return true;
            }

            return false;
        }

        #endregion
    }
}
