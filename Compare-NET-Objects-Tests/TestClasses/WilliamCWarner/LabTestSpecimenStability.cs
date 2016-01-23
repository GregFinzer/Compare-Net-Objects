namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class LabTestSpecimenStability : ILabTestSpecimenStability
    {
        string ILabTestSpecimenStability.TemperatureDescription
        {
            get;
            set;
        }
        int ILabTestSpecimenStability.TemperatureDescriptionTypeId
        {
            get;
            set;
        }
        int ILabTestSpecimenStability.DurationAtTemperature
        {
            get;
            set;
        }
        string ILabTestSpecimenStability.DurationTypeDescription
        {
            get;
            set;
        }
        int ILabTestSpecimenStability.DurationTypeId
        {
            get;
            set;
        }
        bool ILabTestSpecimenStability.IsPreferred
        {
            get;
            set;
        }
    }
}