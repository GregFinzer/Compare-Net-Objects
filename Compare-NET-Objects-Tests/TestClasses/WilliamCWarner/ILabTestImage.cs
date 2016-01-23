namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public interface ILabTestImage
    {
        byte[] LabImage
        {
            get;
            set;
        }
        string LabImageDescription
        {
            get;
            set;
        }
        string LabImageName
        {
            get;
            set;
        }
        int LabImageTypeId
        {
            get;
            set;
        }
        bool IsMainImage
        {
            get;
            set;
        }
    }
}
