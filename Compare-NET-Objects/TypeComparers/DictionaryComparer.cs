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
            return ((TypeHelper.IsIDictionary(type1) || type1 == null) &&
                    (TypeHelper.IsIDictionary(type2) || type2 == null) &&
                    !(type1 == null && type2 == null));
        }

        /// <summary>
        /// Compare two dictionaries
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            try
            {
                parms.Result.AddParent(parms.Object1);
                parms.Result.AddParent(parms.Object2);

                //Objects must be the same length
                bool countsDifferent = DictionaryCountsDifferent(parms);

                if (parms.Result.ExceededDifferences)
                    return;

                CompareEachItem(parms);
            }
            finally
            {
                parms.Result.RemoveParent(parms.Object1);
                parms.Result.RemoveParent(parms.Object2);
            }
        }

        private void CompareEachItem(CompareParms parms)
        {
            var dict1 = ((IDictionary)parms.Object1);
            var dict2 = ((IDictionary)parms.Object2);

            if (dict1 != null)
            {
                string currentBreadCrumb = "";
                CompareParms childParms = null;

                foreach (var key in dict1.Keys)
                {
                    currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, "[" +key.ToString()+ "].Value");

                    childParms = new CompareParms
                    {
                        Result = parms.Result,
                        Config = parms.Config,
                        ParentObject1 = parms.Object1,
                        ParentObject2 = parms.Object2,
                        Object1 = dict1[key],
                        Object2 = (dict2 != null) ? dict2[key] : null,
                        BreadCrumb = currentBreadCrumb
                    };

                    RootComparer.Compare(childParms);

                    if (parms.Result.ExceededDifferences)
                        return;
                }
            }

            if (dict2 != null)
            {
                string currentBreadCrumb = "";
                CompareParms childParms = null;

                foreach (var key in dict2.Keys)
                {
                    currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, "[" + key.ToString() + "].Value");

                    childParms = new CompareParms
                    {
                        Result = parms.Result,
                        Config = parms.Config,
                        ParentObject1 = parms.Object1,
                        ParentObject2 = parms.Object2,
                        Object1 = (dict1 != null && dict1.Contains(key)) ? dict1[key] : null,
                        Object2 = dict2[key],
                        BreadCrumb = currentBreadCrumb
                    };

                    RootComparer.Compare(childParms);

                    if (parms.Result.ExceededDifferences)
                        return;
                }
            }
        }

        private bool DictionaryCountsDifferent(CompareParms parms)
        {
            IDictionary iDict1 = parms.Object1 as IDictionary;
            IDictionary iDict2 = parms.Object2 as IDictionary;

            int iDict1Count = (iDict1 == null) ? 0 : iDict1.Count;
            int iDict2Count = (iDict2 == null) ? 0 : iDict2.Count;

            if (iDict1Count == iDict2Count)
                return false;

            Difference difference = new Difference
            {
                ParentObject1 = parms.ParentObject1,
                ParentObject2 = parms.ParentObject2,
                PropertyName = parms.BreadCrumb,
                Object1Value = iDict1Count.ToString(CultureInfo.InvariantCulture),
                Object2Value = iDict2Count.ToString(CultureInfo.InvariantCulture),
                ChildPropertyName = "Count",
                Object1 = iDict1,
                Object2 = iDict2
            };

            AddDifference(parms.Result, difference);

            return true;
        }
    }
}
