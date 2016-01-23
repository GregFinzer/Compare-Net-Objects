using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class LabTest : ILabTest
    {
        public string TestName { get; set; }
        public string TestDescription { get; set; }
        public bool IsActive { get; set; }
        public string SpecimenType { get; set; }
        public int SpecimenTypeId { get; set; }
        public int SpecimenRequiredTime { get; set; }
        public string SpecimenRequiredIncrementType { get; set; }
        public int SpecimenRequiredIncrementTypeId { get; set; }
        public IList<LabTestSpecimenStability> LabTestSpecimenStabilityValues { get; set; }
        public IList<LabTestPanel> LabTestPanelList { get; set; }
        public IList<LabTestImage> LabTestImageList { get; set; }
        public byte[] ContainerImage { get; set; }
        public byte[] AlternateContainerImage { get; set; }
        public string ContainerDescription { get; set; }
        public string AlternateContainerDescription { get; set; }
        public int CompletionTimeValue { get; set; }
        public string CompletionTimeIncrementType { get; set; }
        public int CompletionTimeIncrementTypeId { get; set; }
        public int OptimumSampleAmount { get; set; }
        public string OptimumSampleIncrementType { get; set; }
        public int OptimumSampleIncrementTypeId { get; set; }
        public int MinimumSampleAmount { get; set; }
        public string MinimumSampleIncrementType { get; set; }
        public int MinimumSampleIncrementTypeId { get; set; }
        public string PhlebotomyInstructions { get; set; }
        public string PatientInstructions { get; set; }
        public IList<ILabTestInstructionLink> InstructionLinkList { get; set; }
        public IList<ILabTestMethod> LabTestMethodList { get; set; }
        public string LabTestPerformedBy { get; set; }
        public string LabTestNotes { get; set; }
        public int SpecimenRetentionTimeValue { get; set; }
        public string SpecimenRetentionIncrementDescription { get; set; }
        public int SpecimenRetentionTypeId { get; set; }
        public int LabTestAnalyticTimeValue { get; set; }
        public string LabTestAnalyticTimeDescription { get; set; }
        public int LabTestAnalyticTimeTypeId { get; set; }
        public string PerformedByHyperLink { get; set; }
        public IList<LabTestCode> LabTestCodeList { get; set; }
        public string LabTestGuid { get; set; }
        public IList<LabTestAlias> LabTestAliasesList { get; set; }
        public IList<LabTestInstructions> LabTestInstructions { get; set; }
        public IList<LabTestPerformingLab> LabTestPerformingLabList { get; set; }
    }
}
