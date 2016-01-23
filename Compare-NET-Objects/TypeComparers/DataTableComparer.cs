using System;
using System.Data;
using System.Globalization;


namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare all rows in a data table
    /// </summary>
    public class DataTableComparer : BaseTypeComparer
    {      
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public DataTableComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both objects are of type DataTable
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsDataTable(type1) && TypeHelper.IsDataTable(type2);
        }

        /// <summary>
        /// Compare two datatables
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            DataTable dataTable1 = parms.Object1 as DataTable;
            DataTable dataTable2 = parms.Object2 as DataTable;

            //This should never happen, null check happens one level up
            if (dataTable1 == null || dataTable2 == null)
                return;

            //Only compare specific table names
            if (parms.Config.MembersToInclude.Count > 0 && !parms.Config.MembersToInclude.Contains(dataTable1.TableName))
                return;

            //If we should ignore it, skip it
            if (parms.Config.MembersToInclude.Count == 0 && parms.Config.MembersToIgnore.Contains(dataTable1.TableName))
                return;

            //There must be the same amount of rows in the datatable
            if (dataTable1.Rows.Count != dataTable2.Rows.Count)
            {
                Difference difference = new Difference
                {
                    ParentObject1 = new WeakReference(parms.ParentObject1),
                    ParentObject2 = new WeakReference(parms.ParentObject2),
                    PropertyName = parms.BreadCrumb,
                    Object1Value = dataTable1.Rows.Count.ToString(CultureInfo.InvariantCulture),
                    Object2Value = dataTable2.Rows.Count.ToString(CultureInfo.InvariantCulture),
                    ChildPropertyName = "Rows.Count",
                    Object1 = new WeakReference(parms.Object1),
                    Object2 = new WeakReference(parms.Object2)
                };

                AddDifference(parms.Result, difference);

                if (parms.Result.ExceededDifferences)
                    return;
            }

            if (ColumnCountsDifferent(parms)) return;

            CompareEachRow(parms);
        }

        private bool ColumnCountsDifferent(CompareParms parms)
        {
            DataTable dataTable1 = parms.Object1 as DataTable;
            DataTable dataTable2 = parms.Object2 as DataTable;

            if (dataTable1 == null)
                throw new ArgumentException("parms.Object1");

            if (dataTable2 == null)
                throw new ArgumentException("parms.Object2");

            if (dataTable1.Columns.Count != dataTable2.Columns.Count)
            {
                Difference difference = new Difference
                {
                    ParentObject1 = new WeakReference(parms.ParentObject1),
                    ParentObject2 = new WeakReference(parms.ParentObject2),
                    PropertyName = parms.BreadCrumb,
                    Object1Value = dataTable1.Columns.Count.ToString(CultureInfo.InvariantCulture),
                    Object2Value = dataTable2.Columns.Count.ToString(CultureInfo.InvariantCulture),
                    ChildPropertyName = "Columns.Count",
                    Object1 = new WeakReference(parms.Object1),
                    Object2 = new WeakReference(parms.Object2)
                };

                AddDifference(parms.Result, difference);

                if (parms.Result.ExceededDifferences)
                    return true;
            }
            return false;
        }

        private void CompareEachRow(CompareParms parms)
        {
            DataTable dataTable1 = parms.Object1 as DataTable;
            DataTable dataTable2 = parms.Object2 as DataTable;

            if (dataTable1 == null)
                throw new ArgumentException("parms.Object1");

            if (dataTable2 == null)
                throw new ArgumentException("parms.Object2");

            for (int i = 0; i < Math.Min(dataTable1.Rows.Count, dataTable2.Rows.Count); i++)
            {
                string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, "Rows", string.Empty, i);

                CompareParms childParms = new CompareParms
                {
                    Result = parms.Result,
                    Config = parms.Config,
                    ParentObject1 = parms.Object1,
                    ParentObject2 = parms.Object2,
                    Object1 = dataTable1.Rows[i],
                    Object2 = dataTable2.Rows[i],
                    BreadCrumb = currentBreadCrumb
                };

                RootComparer.Compare(childParms);

                if (parms.Result.ExceededDifferences)
                    return;
            }
        }


    }
}
