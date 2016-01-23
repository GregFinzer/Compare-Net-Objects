using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class ClassWithGuid
    {
        public ClassWithGuid()
        {
            MyGuid = Guid.NewGuid();
        }

        public Guid MyGuid { get; set; }
        public int SomeInteger { get; set; }
    }
}
