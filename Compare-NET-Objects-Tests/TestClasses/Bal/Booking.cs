using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.Bal
{
    public class Booking
    {
        public Booking()
        {
            Offers = new List<Offer>();
        }

        public List<Offer> Offers { get; set; }
    }
}
