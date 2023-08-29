using KellermanSoftware.CompareNetObjects;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class ClassThatOverridesEquals
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;
            ClassThatOverridesEquals demo = (ClassThatOverridesEquals)obj;
            return Compare(demo, this);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private bool Compare(object object1, object object2)
        {
            CompareLogic compareLogic = new CompareLogic();

            //THIS IS WHAT PREVENTS THE STACK OVERFLOW
            compareLogic.Config.UseHashCodeIdentifier = true;
            
            ComparisonResult result = compareLogic.Compare(object1, object2);
            return result.AreEqual;
        }
    }
}
