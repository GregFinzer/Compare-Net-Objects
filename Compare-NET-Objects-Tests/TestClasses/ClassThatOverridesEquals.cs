using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class ClassThatOverridesEquals
    {
        public ClassThatOverridesEquals()
        {
            Name = string.Empty;
        }

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
            compareLogic.Config = new ComparisonConfig();
            compareLogic.Config.AutoClearCache = true;
            compareLogic.Config.Caching = false;
            compareLogic.Config.UseHashCodeIdentifier = true;
            compareLogic.Config.MembersToIgnore = new List<string>();

            compareLogic.Config.CompareProperties = true;
            ComparisonResult result = null;
            try
            {
                if (compareLogic != null)
                    result = compareLogic.Compare(object1, object2);
            }
            catch
            {
                return false;
            }
            return result?.AreEqual ?? false;
        }
    }
}
