using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class HashSetWrapper
    {

        public HashSetWrapper()
        {
            HashSetCollection = new HashSet<HashSetClass>();

        }

        public int StatusId { get; set; }

        public string Name { get; set; }

        public ICollection<HashSetClass> HashSetCollection { get; set; }

    }
}
