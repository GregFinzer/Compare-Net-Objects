namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class LabTestImage : ILabTestImage
    {
        byte[] ILabTestImage.LabImage
        {
            get;
            set;
        }
        string ILabTestImage.LabImageDescription
        {
            get;
            set;
        }
        string ILabTestImage.LabImageName
        {
            get;
            set;
        }
        int ILabTestImage.LabImageTypeId
        {
            get;
            set;
        }
        bool ILabTestImage.IsMainImage
        {
            get;
            set;
        }
    }
}
