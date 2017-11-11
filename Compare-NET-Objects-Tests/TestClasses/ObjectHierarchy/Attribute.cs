namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.ObjectHierarchy
{
    public class Attribute
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Source { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}; Value: {Value}; Source: {Source}";
        }
    }
}
