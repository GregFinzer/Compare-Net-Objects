using System.Drawing;


namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class FontClass
    {
        public Font FieldObject;
        public Font PropertyObject { get; set; }
        public static Font StaticObject { get; set; }
    }
}
