using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KellermanSoftware.CompareNetObjects.IgnoreOrderTypes;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare objects that implement IList
    /// </summary>
    public class ListComparer : BaseTypeComparer
    {
        private readonly PropertyComparer _propertyComparer;
        private readonly FieldComparer _fieldComparer;

        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public ListComparer(RootComparer rootComparer) : base(rootComparer)
        {
            _propertyComparer = new PropertyComparer(rootComparer);
            _fieldComparer = new FieldComparer(rootComparer);
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
                parms.Result.AddParent(parms.Object1);
                parms.Result.AddParent(parms.Object2);

                Type t1 = parms.Object1.GetType();
                Type t2 = parms.Object2.GetType();

                //Check if the class type should be excluded based on the configuration
                if (ExcludeLogic.ShouldExcludeClass(parms.Config, t1, t2))
                    return;

                parms.Object1Type = t1;
                parms.Object2Type = t2;

                if (parms.Result.ExceededDifferences)
                    return;

                bool countsDifferent = ListsHaveDifferentCounts(parms);

                // If items is collections, need to use default compare logic, not ignore order logic.
                // We cannot ignore order for nested collections because we will get an reflection exception.
                // May be need to display some warning or write about this behavior in documentation.
                if (parms.Config.IgnoreCollectionOrder && !ChildShouldBeComparedWithoutOrder(parms))
                {
                    // TODO: allow IndexerComparer to works with types (now it works only with properties).
                    IgnoreOrderLogic ignoreOrderLogic = new IgnoreOrderLogic(RootComparer);
                    ignoreOrderLogic.CompareEnumeratorIgnoreOrder(parms, countsDifferent);
                }
                else
                {
                    CompareItems(parms);
                }

                //Properties on the root of a collection
                CompareProperties(parms);
                CompareFields(parms);
            }
            finally
            {
                parms.Result.RemoveParent(parms.Object1);
                parms.Result.RemoveParent(parms.Object2);
            }

        }

        private void CompareFields(CompareParms parms)
        {
            if (parms.Config.CompareFields)
            {
                _fieldComparer.PerformCompareFields(parms);
            }
        }

        private void CompareProperties(CompareParms parms)
        {
            if (parms.Config.CompareProperties)
            {
                _propertyComparer.PerformCompareProperties(parms, true);
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
                        ParentObject1 = parms.ParentObject1,
                        ParentObject2 = parms.ParentObject2,
                        PropertyName = parms.BreadCrumb,
                        Object1Value = ilist1.Count.ToString(CultureInfo.InvariantCulture),
                        Object2Value = ilist2.Count.ToString(CultureInfo.InvariantCulture),
                        ChildPropertyName = "Count",
                        Object1 = ilist1,
                        Object2 = ilist2
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

        private bool ChildShouldBeComparedWithoutOrder(CompareParms parms)
        {
            IEnumerator enumerator1 = ((IEnumerable)parms.Object1).GetEnumerator();

            // We should ensure that all items is enumerable, list or dictionary.
            bool hasItems = false;
            var results = new List<bool>();
            while (enumerator1.MoveNext())
            {
                hasItems = true;

                if (enumerator1.Current is null)
                    continue;

                Type type = enumerator1.Current.GetType();
                bool shouldCompareAndIgnoreOrder =
                    TypeHelper.IsEnumerable(type) ||
                    TypeHelper.IsIList(type) ||
                    TypeHelper.IsIDictionary(type);

                results.Add(shouldCompareAndIgnoreOrder);
            }

            // Take into account that items can be objects with mixed types.
            // Throw an exception that this case is unsupported.
            if (hasItems && results.Count > 0)
            {
                bool firstResult = results[0];
                if (results.Any(x => x != firstResult))
                {
                    throw new NotSupportedException(
                        "Collection has nested collections and some other types. " +
                        "IgnoreCollectionOrder should be false for such cases."
                    );
                }

                return firstResult;
            }

            // If all items is null, we can compare as usual.
            // Order does not change anything in this case.
            return hasItems;
        }

        private void CompareItems(CompareParms parms)
        {
            int count = 0;
            IEnumerator enumerator1 = ((IList) parms.Object1).GetEnumerator();
            IEnumerator enumerator2 = ((IList) parms.Object2).GetEnumerator();

            bool enumerator1HasNext;
            bool enumerator2HasNext;
            do
            {
                enumerator1HasNext = enumerator1.MoveNext();
                enumerator2HasNext = enumerator2.MoveNext();

                string currentBreadCrumb = AddBreadCrumb(
                    parms.Config,
                    parms.BreadCrumb,
                    string.Empty,
                    string.Empty,
                    count);

                if (enumerator1HasNext && enumerator2HasNext)
                {
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
                }
                else if (enumerator1HasNext)
                {
                    AddDifference(
                        parms.Result,
                        new Difference()
                        {
                            ParentObject1 = parms.ParentObject1,
                            ParentObject2 = parms.ParentObject2,
                            PropertyName = currentBreadCrumb,
                            Object1Value = NiceString(enumerator1.Current),
                            Object1 = enumerator1.Current,
                            ActualName = "Actual" + currentBreadCrumb,
                            ExpectedName = "Expected" + currentBreadCrumb,
                        });
                }
                else if (enumerator2HasNext)
                {
                    AddDifference(
                        parms.Result,
                        new Difference()
                        {
                            ParentObject1 = parms.ParentObject1,
                            ParentObject2 = parms.ParentObject2,
                            PropertyName = currentBreadCrumb,
                            Object2Value = NiceString(enumerator2.Current),
                            Object2 = enumerator2.Current,
                            ActualName = "Actual" + currentBreadCrumb,
                            ExpectedName = "Expected" + currentBreadCrumb,
                        });
                }

                if (parms.Result.ExceededDifferences)
                    return;

                count++;
                    
            } while (enumerator1HasNext || enumerator2HasNext);
        }
    }
}
