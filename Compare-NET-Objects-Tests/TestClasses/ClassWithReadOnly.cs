using System;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class ClassWithReadOnly
    {
        public string Name { get; set; }
        public string MyReadOnlyProperty
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
