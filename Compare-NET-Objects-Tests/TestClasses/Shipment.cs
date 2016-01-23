using System;
using KellermanSoftware.CompareNetObjectsTests.Attributes;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    class Shipment
    {
        public long IdentCode { get; set; }
        public String Customer { get; set; }
        [CompareIgnore]
        public DateTime InsertDate { get; set; }
    }
}
