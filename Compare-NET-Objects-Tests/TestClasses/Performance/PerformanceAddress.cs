using System;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.Performance
{
    [Serializable]
    public class PerformanceAddress
    {
        public long AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
