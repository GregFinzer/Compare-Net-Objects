namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public interface ILabTestPerformingLab
    {
        string PerformingLab
        {
            get;
            set;
        }
        string PerformingLabHyperLink
        {
            get;
            set;
        }
        System.Guid LabTestId
        {
            get;
            set;
        }
    }
}
