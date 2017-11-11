using System;
using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.ObjectHierarchy
{
    public class HoldingsReport
    {
        public int Id { get; set; }

        public DateTime GeneratedAt { get; set; }

        public List<Holding> Holdings { get; set; }
    }
}
