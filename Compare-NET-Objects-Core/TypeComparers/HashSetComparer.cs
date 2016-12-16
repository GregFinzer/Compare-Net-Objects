using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using KellermanSoftware.CompareNetObjects.IgnoreOrderTypes;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Logic to compare two hash sets
    /// </summary>
    public class HashSetComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public HashSetComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both objects are hash sets
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsHashSet(type1) && TypeHelper.IsHashSet(type2);
        }

        /// <summary>
        /// Compare two hash sets
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            try
            {
                parms.Result.AddParent(parms.Object1.GetHashCode());
                parms.Result.AddParent(parms.Object2.GetHashCode());

                Type t1 = parms.Object1.GetType();
                parms.Object1Type = t1;

                Type t2 = parms.Object2.GetType();
                parms.Object2Type = t2;

                bool countsDifferent = HashSetsDifferentCount(parms);

                if (parms.Result.ExceededDifferences)
                    return;

                if (parms.Config.IgnoreCollectionOrder)
                {
                    IgnoreOrderLogic logic = new IgnoreOrderLogic(RootComparer);
                    logic.CompareEnumeratorIgnoreOrder(parms, countsDifferent);
                }
                else
                {
                    CompareItems(parms);
                }
            }
            finally
            {
                parms.Result.RemoveParent(parms.Object1.GetHashCode());
                parms.Result.RemoveParent(parms.Object2.GetHashCode());
            }
        }

        private void CompareItems(CompareParms parms)
        {
            int count = 0;

            //Get enumerators by reflection
            MethodInfo method1Info = Cache.GetMethod(parms.Object1Type, "GetEnumerator");
            IEnumerator enumerator1 = (IEnumerator)method1Info.Invoke(parms.Object1, null);

            MethodInfo method2Info = Cache.GetMethod(parms.Object2Type, "GetEnumerator");
            IEnumerator enumerator2 = (IEnumerator) method2Info.Invoke(parms.Object2, null);

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, string.Empty, string.Empty, count);

                CompareParms childParms = new CompareParms
                {
                    Result = parms.Result,
                    Config = parms.Config,
                    ParentObject1 = parms.Object1,
                    ParentObject2 = parms.Object2,
                    Object1 = enumerator1.Current,
                    Object2 = enumerator2.Current,
                    BreadCrumb = currentBreadCrumb
                };

                RootComparer.Compare(childParms);

                if (parms.Result.ExceededDifferences)
                    return;

                count++;
            }
        }

        private bool HashSetsDifferentCount(CompareParms parms)
        {
            //Get count by reflection since we can't cast it to HashSet<>
            int hashSet1Count = (int) Cache.GetPropertyValue(parms.Result, parms.Object1Type, parms.Object1, "Count");
            int hashSet2Count = (int)Cache.GetPropertyValue(parms.Result, parms.Object2Type, parms.Object2, "Count");

            //Objects must be the same length
            if (hashSet1Count != hashSet2Count)
            {
                Difference difference = new Difference
                                            {
                                                ParentObject1 = new WeakReference(parms.ParentObject1),
                                                ParentObject2 = new WeakReference(parms.ParentObject2),
                                                PropertyName = parms.BreadCrumb,
                                                Object1Value = hashSet1Count.ToString(CultureInfo.InvariantCulture),
                                                Object2Value = hashSet2Count.ToString(CultureInfo.InvariantCulture),
                                                ChildPropertyName = "Count",
                                                Object1 = new WeakReference(parms.Object1),
                                                Object2 = new WeakReference(parms.Object2)
                                            };

                AddDifference(parms.Result, difference);

                return true;
            }

            return false;
        }
    }
}
