namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class LabTestPerformingLab : ILabTestPerformingLab
    {
        string ILabTestPerformingLab.PerformingLab
        {
            get;
            set;
        }
        string ILabTestPerformingLab.PerformingLabHyperLink
        {
            get;
            set;
        }
        System.Guid ILabTestPerformingLab.LabTestId
        {
            get;
            set;
        }
    }
}
