using System;
using System.Collections.Generic;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
#if !DNCORE
    [Serializable]
#endif

    public class Person : IName
    {
        public DateTime DateCreated;

        public string Name
        {
            get;
            set;
        }
    }
}
