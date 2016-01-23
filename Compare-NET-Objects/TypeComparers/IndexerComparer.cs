using System;
using System.Globalization;
using System.Reflection;
using KellermanSoftware.CompareNetObjects.IgnoreOrderTypes;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Logic to compare an integer indexer (Note, inherits from BaseComparer, not TypeComparer)
    /// </summary>
    public class IndexerComparer : BaseComparer
    {
        private readonly RootComparer _rootComparer;

        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public IndexerComparer(RootComparer rootComparer)
        {
            _rootComparer = rootComparer;
        }

        /// <summary>
        /// Compare an integer indexer
        /// </summary>
        public void CompareIndexer(CompareParms parms, PropertyInfo info)
        {
            if (info == null || info.ReflectedType == null)
                throw new ArgumentNullException("info");

            int indexerCount1 = (int)info.ReflectedType.GetProperty("Count").GetGetMethod().Invoke(parms.Object1, new object[] { });
            int indexerCount2 = (int)info.ReflectedType.GetProperty("Count").GetGetMethod().Invoke(parms.Object2, new object[] { });

            bool differentCounts = IndexersHaveDifferentLength(parms, info);

            if (parms.Result.ExceededDifferences)
                return;

            if (parms.Config.IgnoreCollectionOrder)
            {
                var enumerable1 = new IndexerCollectionLooper(parms.Object1, info, indexerCount1);
                var enumerable2 = new IndexerCollectionLooper(parms.Object2, info, indexerCount2);

                CompareParms childParms = new CompareParms
                {
                    Result = parms.Result,
                    Config = parms.Config,
                    ParentObject1 = parms.Object1,
                    ParentObject2 = parms.Object2,
                    Object1 = enumerable1,
                    Object2 = enumerable2,
                    BreadCrumb = parms.BreadCrumb
                };

                IgnoreOrderLogic logic = new IgnoreOrderLogic(_rootComparer);
                logic.CompareEnumeratorIgnoreOrder(childParms, differentCounts);
            }
            else
            {
                string currentCrumb;

                // Run on indexer
                for (int i = 0; i < indexerCount1; i++)
                {
                    currentCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, info.Name, string.Empty, i);
                    object objectValue1 = info.GetValue(parms.Object1, new object[] { i });
                    object objectValue2 = null;

                    if (i < indexerCount2)
                        objectValue2 = info.GetValue(parms.Object2, new object[] { i });

                    CompareParms childParms = new CompareParms
                    {
                        Result = parms.Result,
                        Config = parms.Config,
                        ParentObject1 = parms.Object1,
                        ParentObject2 = parms.Object2,
                        Object1 = objectValue1,
                        Object2 = objectValue2,
                        BreadCrumb = currentCrumb
                    };

                    _rootComparer.Compare(childParms);

                    if (parms.Result.ExceededDifferences)
                        return;
                }

                if (indexerCount1 < indexerCount2)
                {
                    for (int j = indexerCount1; j < indexerCount2; j++)
                    {
                        currentCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, info.Name, string.Empty, j);
                        object objectValue2 = info.GetValue(parms.Object2, new object[] { j });

                        CompareParms childParms = new CompareParms
                        {
                            Result = parms.Result,
                            Config = parms.Config,
                            ParentObject1 = parms.Object1,
                            ParentObject2 = parms.Object2,
                            Object1 = null,
                            Object2 = objectValue2,
                            BreadCrumb = currentCrumb
                        };

                        _rootComparer.Compare(childParms);

                        if (parms.Result.ExceededDifferences)
                            return;
                    }
                }
            }
        }

        private bool IndexersHaveDifferentLength(CompareParms parms, PropertyInfo info)
        {
            if (info == null || info.ReflectedType == null)
                throw new ArgumentNullException("info");

            int indexerCount1 = (int)info.ReflectedType.GetProperty("Count").GetGetMethod().Invoke(parms.Object1, new object[] { });
            int indexerCount2 = (int)info.ReflectedType.GetProperty("Count").GetGetMethod().Invoke(parms.Object2, new object[] { });

            if (indexerCount1 != indexerCount2)
            {
                string currentCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, info.Name);
                Difference difference = new Difference
                                            {
                                                ParentObject1 = new WeakReference(parms.ParentObject1),
                                                ParentObject2 = new WeakReference(parms.ParentObject2),
                                                PropertyName = currentCrumb,
                                                Object1Value = indexerCount1.ToString(CultureInfo.InvariantCulture),
                                                Object2Value = indexerCount2.ToString(CultureInfo.InvariantCulture),
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
