namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public interface ILabTestInstructionLink
    {
        string DisplayText
        {
            get;
            set;
        }
        string Hyperlink
        {
            get;
            set;
        }
        int InstructionTypeId
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
