using System;
using System.Collections.Generic;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    [Serializable]
    public class Person : IName
    {
        public int ID { get; set; }

        public DateTime DateCreated;

        public string Name
        {
            get;
            set;
        }
    }
}
