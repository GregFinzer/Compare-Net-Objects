using KellermanSoftware.CompareNetObjects.IgnoreOrderTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Logic to compare two ReadOnlyCollections.
    /// </summary>
    public class ReadOnlyCollectionComparer : BaseTypeComparer
    {
        /// <summary>
        /// The main constructor.
        /// </summary>
        public ReadOnlyCollectionComparer(RootComparer rootComparer) : base(rootComparer)
        {

        }

        /// <summary>
        /// Returns true if both objects are ReadOnlyCollections.
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsReadOnlyCollection(type1) && TypeHelper.IsReadOnlyCollection(type2);
        }

        /// <summary>
        /// Compare two ReadOnlyCollections.
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            if (!parms.Config.CompareReadOnly)
                return;

            try
            {
                parms.Result.AddParent(parms.Object1);
                parms.Result.AddParent(parms.Object2);

                Type t1 = parms.Object1.GetType();
                Type t2 = parms.Object2.GetType();

                //Check if the class type should be excluded based on the configuration
                if (ExcludeLogic.ShouldExcludeClass(parms.Config, t1, t2))
                    return;

                parms.Object1Type = t1;
                parms.Object2Type = t2;

                bool countsDifferent = CollectionsDifferentCount(parms);

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
                parms.Result.RemoveParent(parms.Object1);
                parms.Result.RemoveParent(parms.Object2);
            }
        }

        private void CompareItems(CompareParms parms)
        {
            int count = 0;

            IEnumerator enumerator1 = ((ICollection)parms.Object1).GetEnumerator();
            IEnumerator enumerator2 = ((ICollection)parms.Object2).GetEnumerator();

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

        private bool CollectionsDifferentCount(CompareParms parms)
        {
            //Get count by reflection since we can't cast it to HashSet<>
            int count1 = ((ICollection)parms.Object1).Count;
            int count2 = ((ICollection)parms.Object2).Count;

            //Objects must be the same length
            if (count1 != count2)
            {
                Difference difference = new Difference
                {
                    ParentObject1 = parms.ParentObject1,
                    ParentObject2 = parms.ParentObject2,
                    PropertyName = parms.BreadCrumb,
                    Object1Value = count1.ToString(CultureInfo.InvariantCulture),
                    Object2Value = count2.ToString(CultureInfo.InvariantCulture),
                    ChildPropertyName = "Count",
                    Object1 = parms.Object1,
                    Object2 = parms.Object2
                };

                AddDifference(parms.Result, difference);

                return true;
            }

            return false;
        }
    }
}
