namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.ObjectHierarchy
{
    public class FundShare : Holding
    {
        public string FundLegalName { get; set; }

        public int ShareCount { get; set; }

        public decimal NetAssetValue { get; set; }
    }
}
