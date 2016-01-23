using System;
using System.Data;
using System.Globalization;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare all tables and all rows in all tables
    /// </summary>
    public class DatasetComparer : BaseTypeComparer
    {

        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public DatasetComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both objects are data sets
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsDataset(type1) && TypeHelper.IsDataset(type2);
        }

        /// <summary>
        /// Compare two data sets
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            DataSet dataSet1 = parms.Object1 as DataSet;
            DataSet dataSet2 = parms.Object2 as DataSet;

            //This should never happen, null check happens one level up
            if (dataSet1 == null || dataSet2 == null)
                return;

            if (TableCountsDifferent(parms, dataSet2, dataSet1)) return;

            CompareEachTable(parms, dataSet1, dataSet2);
        }

        private bool TableCountsDifferent(CompareParms parms, DataSet dataSet2, DataSet dataSet1)
        {
            if (dataSet1.Tables.Count != dataSet2.Tables.Count)
            {
                Difference difference = new Difference
                                            {
                                                ParentObject1 = new WeakReference(parms.ParentObject1),
                                                ParentObject2 = new WeakReference(parms.ParentObject2),
                                                PropertyName = parms.BreadCrumb,
                                                Object1Value = dataSet1.Tables.Count.ToString(CultureInfo.InvariantCulture),
                                                Object2Value = dataSet2.Tables.Count.ToString(CultureInfo.InvariantCulture),
                                                ChildPropertyName = "Tables.Count",
                                                Object1 = new WeakReference(parms.Object1),
                                                Object2 = new WeakReference(parms.Object2)
                                            };

                AddDifference(parms.Result, difference);

                if (parms.Result.ExceededDifferences)
                    return true;
            }
            return false;
        }

        private void CompareEachTable(CompareParms parms, DataSet dataSet1, DataSet dataSet2)
        {
            for (int i = 0; i < Math.Min(dataSet1.Tables.Count, dataSet2.Tables.Count); i++)
            {
                string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, "Tables", string.Empty,
                                                         dataSet1.Tables[i].TableName);

                CompareParms childParms = new CompareParms();
                childParms.Result = parms.Result;
                childParms.Config = parms.Config;
                childParms.BreadCrumb = currentBreadCrumb;
                childParms.ParentObject1 = dataSet1;
                childParms.ParentObject2 = dataSet2;
                childParms.Object1 = dataSet1.Tables[i];
                childParms.Object2 = dataSet2.Tables[i];

                RootComparer.Compare(childParms);

                if (parms.Result.ExceededDifferences)
                    return;
            }
        }
    }
}
