using System;
using System.Data;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare all columns in a data row
    /// </summary>
    public class DataRowComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public DataRowComparer(RootComparer rootComparer)
            : base(rootComparer)
        { }

        /// <summary>
        /// Returns true if this is a DataRow
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsDataRow(type1) && TypeHelper.IsDataRow(type2);
        }

        /// <summary>
        /// Compare two data rows
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            DataRow dataRow1 = parms.Object1 as DataRow;
            DataRow dataRow2 = parms.Object2 as DataRow;

            //This should never happen, null check happens one level up
            if (dataRow1 == null || dataRow2 == null)
                return;

            for (int i = 0; i < Math.Min(dataRow2.Table.Columns.Count, dataRow1.Table.Columns.Count); i++)
            {
                //Only compare specific column names
                if (parms.Config.MembersToInclude.Count > 0 && !parms.Config.MembersToInclude.Contains(dataRow1.Table.Columns[i].ColumnName))
                    continue;

                //If we should ignore it, skip it
                if (parms.Config.MembersToInclude.Count == 0 && parms.Config.MembersToIgnore.Contains(dataRow1.Table.Columns[i].ColumnName))
                    continue;

                //If we should ignore read only, skip it
                if (!parms.Config.CompareReadOnly && dataRow1.Table.Columns[i].ReadOnly)
                    continue;

                //Both are null
                if (dataRow1.IsNull(i) && dataRow2.IsNull(i))
                    continue;

                string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, string.Empty, string.Empty, dataRow1.Table.Columns[i].ColumnName);

                //Check if one of them is null
                if (dataRow1.IsNull(i))
                {
                    Difference difference = new Difference
                    {
                        ParentObject1 = new WeakReference(parms.ParentObject1),
                        ParentObject2 = new WeakReference(parms.ParentObject2),
                        PropertyName = currentBreadCrumb,
                        Object1Value = "(null)",
                        Object2Value = NiceString(dataRow2[i]),
                        Object1 = new WeakReference(parms.Object1),
                        Object2 = new WeakReference(parms.Object2)
                    };

                    AddDifference(parms.Result, difference);
                    return;
                }

                if (dataRow2.IsNull(i))
                {
                    Difference difference = new Difference
                    {
                        ParentObject1 = new WeakReference(parms.ParentObject1),
                        ParentObject2 = new WeakReference(parms.ParentObject2),
                        PropertyName = currentBreadCrumb,
                        Object1Value = NiceString(dataRow1[i]),
                        Object2Value = "(null)",
                        Object1 = new WeakReference(parms.Object1),
                        Object2 = new WeakReference(parms.Object2)
                    };

                    AddDifference(parms.Result, difference);
                    return;
                }

                //Check if one of them is deleted
                if (dataRow1.RowState == DataRowState.Deleted ^ dataRow2.RowState == DataRowState.Deleted)
                {
                    Difference difference = new Difference
                    {
                        ParentObject1 = new WeakReference(parms.ParentObject1),
                        ParentObject2 = new WeakReference(parms.ParentObject2),
                        PropertyName = currentBreadCrumb,
                        Object1Value = dataRow1.RowState.ToString(),
                        Object2Value = dataRow2.RowState.ToString(),
                        Object1 = new WeakReference(parms.Object1),
                        Object2 = new WeakReference(parms.Object2)
                    };

                    AddDifference(parms.Result, difference);
                    return;
                }

                CompareParms childParms = new CompareParms();
                childParms.Result = parms.Result;
                childParms.Config = parms.Config;
                childParms.ParentObject1 = parms.Object1;
                childParms.ParentObject2 = parms.Object2;
                childParms.Object1 = dataRow1[i];
                childParms.Object2 = dataRow2[i];
                childParms.BreadCrumb = currentBreadCrumb;

                RootComparer.Compare(childParms);

                if (parms.Result.ExceededDifferences)
                    return;
            }
        }
    }
}
