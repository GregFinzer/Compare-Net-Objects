using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.Bal
{
    public class Offer
    {
        public Offer()
        {
            Products = new List<BookingProduct>();
        }

        public long Id { get; set; }
        public string Label { get; set; }
        public List<BookingProduct> Products { get; set; }
        public int Order { get; set; }
    }
}
