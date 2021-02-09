using System;
using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.Performance
{
    [Serializable]
    public class PerformanceOrder
    {
        public long OrderId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public PerformanceAddress BillingAddress { get; set; }
        public PerformanceAddress ShippingAddress { get; set; }
        public List<PerformanceOrderDetail> OrderDetails { get; set; }
    }
}
