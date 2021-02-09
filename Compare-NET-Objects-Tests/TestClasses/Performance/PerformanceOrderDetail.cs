using System;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.Performance
{
    [Serializable]
    public class PerformanceOrderDetail
    {
        public long OrderDetailId { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
