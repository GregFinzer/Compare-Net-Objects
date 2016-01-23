namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public interface ILabTest
    {
        string TestName
        {
            get;
            set;
        }
        string TestDescription
        {
            get;
            set;
        }
        bool IsActive
        {
            get;
            set;
        }
        string SpecimenType
        {
            get;
            set;
        }
        int SpecimenTypeId
        {
            get;
            set;
        }
        int SpecimenRequiredTime
        {
            get;
            set;
        }
        string SpecimenRequiredIncrementType
        {
            get;
            set;
        }
        int SpecimenRequiredIncrementTypeId
        {
            get;
            set;
        }
        System.Collections.Generic.IList<LabTestSpecimenStability> LabTestSpecimenStabilityValues
        {
            get;
            set;
        }
        System.Collections.Generic.IList<LabTestPanel> LabTestPanelList
        {
            get;
            set;
        }
        System.Collections.Generic.IList<LabTestImage> LabTestImageList
        {
            get;
            set;
        }
        byte[] ContainerImage
        {
            get;
            set;
        }
        byte[] AlternateContainerImage
        {
            get;
            set;
        }
        string ContainerDescription
        {
            get;
            set;
        }
        string AlternateContainerDescription
        {
            get;
            set;
        }
        int CompletionTimeValue
        {
            get;
            set;
        }
        string CompletionTimeIncrementType
        {
            get;
            set;
        }
        int CompletionTimeIncrementTypeId
        {
            get;
            set;
        }
        int OptimumSampleAmount
        {
            get;
            set;
        }
        string OptimumSampleIncrementType
        {
            get;
            set;
        }
        int OptimumSampleIncrementTypeId
        {
            get;
            set;
        }
        int MinimumSampleAmount
        {
            get;
            set;
        }
        string MinimumSampleIncrementType
        {
            get;
            set;
        }
        int MinimumSampleIncrementTypeId
        {
            get;
            set;
        }
        string PhlebotomyInstructions
        {
            get;
            set;
        }
        string PatientInstructions
        {
            get;
            set;
        }
        System.Collections.Generic.IList<ILabTestInstructionLink> InstructionLinkList
        {
            get;
            set;
        }
        System.Collections.Generic.IList<ILabTestMethod> LabTestMethodList
        {
            get;
            set;
        }
        string LabTestPerformedBy
        {
            get;
            set;
        }
        string LabTestNotes
        {
            get;
            set;
        }
        int SpecimenRetentionTimeValue
        {
            get;
            set;
        }
        string SpecimenRetentionIncrementDescription
        {
            get;
            set;
        }
        int SpecimenRetentionTypeId
        {
            get;
            set;
        }
        int LabTestAnalyticTimeValue
        {
            get;
            set;
        }
        string LabTestAnalyticTimeDescription
        {
            get;
            set;
        }
        int LabTestAnalyticTimeTypeId
        {
            get;
            set;
        }
        string PerformedByHyperLink
        {
            get;
            set;
        }
        System.Collections.Generic.IList<LabTestCode> LabTestCodeList
        {
            get;
            set;
        }
        string LabTestGuid
        {
            get;
            set;
        }
        System.Collections.Generic.IList<LabTestAlias> LabTestAliasesList
        {
            get;
            set;
        }
        System.Collections.Generic.IList<LabTestInstructions> LabTestInstructions
        {
            get;
            set;
        }
        System.Collections.Generic.IList<LabTestPerformingLab> LabTestPerformingLabList
        {
            get;
            set;
        }
    }
}

