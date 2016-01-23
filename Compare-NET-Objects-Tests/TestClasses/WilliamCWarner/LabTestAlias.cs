namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class LabTestAlias : ILabTestAlias
    {
        string ILabTestAlias.LabTestAlias
        {
            get;
            set;
        }
        System.Guid ILabTestAlias.LabTestAliasId
        {
            get;
            set;
        }
        System.Guid ILabTestAlias.LabTestId
        {
            get;
            set;
        }
    }
}
