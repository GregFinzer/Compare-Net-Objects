namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public interface ILabTestCode
    {
        int LabTestCodeDescriptionTypeId
        {
            get;
            set;
        }
        System.Guid LabTestId
        {
            get;
            set;
        }
        string LabTestCodeValue
        {
            get;
            set;
        }
    }
}
