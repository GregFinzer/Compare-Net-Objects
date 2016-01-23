namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public interface ILabTestSpecimenStability
    {
        string TemperatureDescription
        {
            get;
            set;
        }
        int TemperatureDescriptionTypeId
        {
            get;
            set;
        }
        int DurationAtTemperature
        {
            get;
            set;
        }
        string DurationTypeDescription
        {
            get;
            set;
        }
        int DurationTypeId
        {
            get;
            set;
        }
        bool IsPreferred
        {
            get;
            set;
        }
    }
}