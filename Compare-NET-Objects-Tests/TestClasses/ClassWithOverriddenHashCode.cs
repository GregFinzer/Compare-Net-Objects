namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class ClassWithOverriddenHashCode
    {
        public string Name { get; set; }
        public object MyCircularReference { get; set; }

        public override int GetHashCode()
        {
            return 1;
        }
    }
}
