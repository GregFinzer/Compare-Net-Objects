namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class LabTestPanel : ILabTestPanel
    {
        string ILabTestPanel.LabTestPanelName
        {
            get;
            set;
        }
        System.Guid ILabTestPanel.ParentId
        {
            get;
            set;
        }
        System.Guid ILabTestPanel.LabTestId
        {
            get;
            set;
        }
    }
}
