namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal class WriteOnlyPropertyClass
    {
        private int _value;

        public string Name { get; set; }

        public int WriteOnly
        {
            set { _value = value; }
        }
    }
}
