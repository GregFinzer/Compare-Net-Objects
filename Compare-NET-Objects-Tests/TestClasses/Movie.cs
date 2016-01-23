using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KellermanSoftware.CompareNetObjectsTests.Attributes;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class Movie : BaseMedia
    {
        public string Name { get; set; }

        [ExcludeFromEquality]
        public decimal PaymentForTomCruise { get; set; }
    }
}
