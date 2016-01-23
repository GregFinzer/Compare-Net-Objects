using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class ObjectTypeClass
    {
        public object FieldObject;
        public object PropertyObject { get; set; }
        public static object StaticObject { get; set; }
    }
}
