using System;
using System.Collections;
using System.Globalization;
using KellermanSoftware.CompareNetObjects.IgnoreOrderTypes;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Logic to compare two dictionaries
    /// </summary>
    public class DictionaryComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public DictionaryComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both types are dictionaries
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsIDictionary(type1) && TypeHelper.IsIDictionary(type2);
        }

        /// <summary>
        /// Compare two dictionaries
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            //This should never happen, null check happens one level up
            if (parms.Object1 == null || parms.Object2 == null)
                return;

            try
            {
                parms.Result.AddParent(parms.Object1.GetHashCode());
                parms.Result.AddParent(parms.Object2.GetHashCode());

                //Objects must be the same length
                bool countsDifferent = DictionaryCountsDifferent(parms);

                if (parms.Result.ExceededDifferences)
                    return;

                if (parms.Config.IgnoreCollectionOrder)
                {
                    IgnoreOrderLogic logic = new IgnoreOrderLogic(RootComparer);
                    logic.CompareEnumeratorIgnoreOrder(parms, countsDifferent);
                }
                else
                {
                    CompareEachItem(parms);
                }

            }
            finally
            {
                parms.Result.RemoveParent(parms.Object1.GetHashCode());
                parms.Result.RemoveParent(parms.Object2.GetHashCode());
            }
        }

        private void CompareEachItem(CompareParms parms)
        {
            var enumerator1 = ((IDictionary) parms.Object1).GetEnumerator();
            var enumerator2 = ((IDictionary) parms.Object2).GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, "Key");

                CompareParms childParms = new CompareParms
                {
                    Result = parms.Result,
                    Config = parms.Config,
                    ParentObject1 = parms.Object1,
                    ParentObject2 = parms.Object2,
                    Object1 = enumerator1.Key,
                    Object2 = enumerator2.Key,
                    BreadCrumb = currentBreadCrumb
                };

                RootComparer.Compare(childParms);

                if (parms.Result.ExceededDifferences)
                    return;

                currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, "Value");

                childParms = new CompareParms
                {
                    Result = parms.Result,
                    Config = parms.Config,
                    ParentObject1 = parms.Object1,
                    ParentObject2 = parms.Object2,
                    Object1 = enumerator1.Value,
                    Object2 = enumerator2.Value,
                    BreadCrumb = currentBreadCrumb
                };

                RootComparer.Compare(childParms);

                if (parms.Result.ExceededDifferences)
                    return;
            }
        }

        private bool DictionaryCountsDifferent(CompareParms parms)
        {
            IDictionary iDict1 = parms.Object1 as IDictionary;
            IDictionary iDict2 = parms.Object2 as IDictionary;

            if (iDict1 == null)
                throw new ArgumentException("parms.Object1");

            if (iDict2 == null)
                throw new ArgumentException("parms.Object2");

            if (iDict1.Count != iDict2.Count)
            {
                Difference difference = new Difference
                                            {
                                                ParentObject1 = new WeakReference(parms.ParentObject1),
                                                ParentObject2 = new WeakReference(parms.ParentObject2),
                                                PropertyName = parms.BreadCrumb,
                                                Object1Value = iDict1.Count.ToString(CultureInfo.InvariantCulture),
                                                Object2Value = iDict2.Count.ToString(CultureInfo.InvariantCulture),
                                                ChildPropertyName = "Count",
                                                Object1 = new WeakReference(iDict1),
                                                Object2 = new WeakReference(iDict2)
                                            };

                AddDifference(parms.Result, difference);

                return true;
            }
            return false;
        }
    }
}
