using System;
using System.Collections;
using System.Globalization;
using KellermanSoftware.CompareNetObjects.IgnoreOrderTypes;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare objects that implement IList
    /// </summary>
    public class ListComparer : BaseTypeComparer
    {

        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public ListComparer(RootComparer rootComparer) : base(rootComparer)
        {
            
        }

        /// <summary>
        /// Returns true if both objects implement IList
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsIList(type1) && TypeHelper.IsIList(type2);
        }


        /// <summary>
        /// Compare two objects that implement IList
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

                bool countsDifferent = ListsHaveDifferentCounts(parms);

                if (parms.Result.ExceededDifferences)
                    return;

                if (parms.Config.IgnoreCollectionOrder)
                {
                    IgnoreOrderLogic ignoreOrderLogic = new IgnoreOrderLogic(RootComparer);
                    ignoreOrderLogic.CompareEnumeratorIgnoreOrder(parms, countsDifferent);
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

        private bool ListsHaveDifferentCounts(CompareParms parms)
        {
            IList ilist1 = parms.Object1 as IList;
            IList ilist2 = parms.Object2 as IList;

            if (ilist1 == null)
                throw new ArgumentException("parms.Object1");

            if (ilist2 == null)
                throw new ArgumentException("parms.Object2");

            try
            {
                //Objects must be the same length
                if (ilist1.Count != ilist2.Count)
                {
                    Difference difference = new Difference
                    {
                        ParentObject1 = new WeakReference(parms.ParentObject1),
                        ParentObject2 = new WeakReference(parms.ParentObject2),
                        PropertyName = parms.BreadCrumb,
                        Object1Value = ilist1.Count.ToString(CultureInfo.InvariantCulture),
                        Object2Value = ilist2.Count.ToString(CultureInfo.InvariantCulture),
                        ChildPropertyName = "Count",
                        Object1 = new WeakReference(ilist1),
                        Object2 = new WeakReference(ilist2)
                    };

                    AddDifference(parms.Result, difference);

                    return true;
                }
            }
            catch (ObjectDisposedException)
            {
                if (!parms.Config.IgnoreObjectDisposedException)
                    throw;

                return true;
            }

            return false;
        }

        private void CompareItems(CompareParms parms)
        {
            int count = 0;
            IEnumerator enumerator1 = ((IList) parms.Object1).GetEnumerator();
            IEnumerator enumerator2 = ((IList) parms.Object2).GetEnumerator();

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


    }
}
