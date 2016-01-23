namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class LabTestInstructions : ILabTestInstructions
    {
        string ILabTestInstructions.LabTestInstruction
        {
            get;
            set;
        }
        int ILabTestInstructions.LabTestInstructionTypeId
        {
            get;
            set;
        }
        System.Guid ILabTestInstructions.LabTestId
        {
            get;
            set;
        }
    }
}
