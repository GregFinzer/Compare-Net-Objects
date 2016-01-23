namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class LabTestCode : ILabTestCode
    {
        int ILabTestCode.LabTestCodeDescriptionTypeId
        {
            get;
            set;
        }
        System.Guid ILabTestCode.LabTestId
        {
            get;
            set;
        }
        string ILabTestCode.LabTestCodeValue
        {
            get;
            set;
        }
    }
}
