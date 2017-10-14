using System;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.ObjectHierarchy
{
    public class Bond : Holding
    {
        public string IssueSnPRating { get; set; }

        public int IssuenceCount { get; set; }

        public DateTime Maturity { get; set; }
    }
}
