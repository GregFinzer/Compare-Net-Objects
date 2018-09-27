#if !NETSTANDARD
    
using System.Data;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareDataTableTests
    {
        #region Class Variables
        private CompareLogic _compare;
        #endregion

        #region Setup/Teardown



        /// <summary>
        /// Code that is run before each test
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            _compare = new CompareLogic();
        }

        /// <summary>
        /// Code that is run after each test
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            _compare = null;
        }
        #endregion

        #region Tests
        [Test]
        public void DataTableNegativeDataTest()
        {
            DataSet ds1 = CreateMockDataset();
            DataSet ds2 = Common.CloneWithSerialization(ds1);
            ds2.Tables[0].Rows[2][0] = "Chunky Chocolate Heaven";
            Assert.IsFalse(_compare.Compare(ds1.Tables[0], ds2.Tables[0]).AreEqual);
        }
        [Test]
        public void DataTableNegativeColumnNameTest()
        {
            DataSet ds1 = CreateMockDataset();
            DataSet ds2 = Common.CloneWithSerialization(ds1);
            ds2.Tables[0].Columns[0].ColumnName = "Flavour";
            Assert.IsFalse(_compare.Compare(ds1.Tables[0], ds2.Tables[0]).AreEqual);
        }
        [Test]
        public void DataTableNegativeMultipleColumnNameTest()
        {
            DataSet ds1 = CreateMockDataset();
            DataSet ds2 = Common.CloneWithSerialization(ds1);
            ds2.Tables[0].Columns[0].ColumnName = "Flavour";
            ds2.Tables[0].Columns[1].ColumnName = "Prix";
            Assert.IsFalse(_compare.Compare(ds1.Tables[0], ds2.Tables[0]).AreEqual);
        }
        #endregion

        #region Supporting Methods
        private DataSet CreateMockDataset()
        {
            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable("IceCream");
            ds1.Tables.Add(dt);
            dt.Columns.Add("Flavor", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            DataRow dr = dt.NewRow();
            dr["Flavor"] = "Chocolate";
            dr["Price"] = 1.99M;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Flavor"] = "Vanilla";
            dr["Price"] = 1.98M;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Flavor"] = "Banana Prune Delight";
            dr["Price"] = 2.99M;
            dt.Rows.Add(dr);
            return ds1;
        }
        #endregion
    }
}

#endif
