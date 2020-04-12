using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.HashBug
{
    public class HashBugB : HashBugA
    {
        public ICollection<HashBugC> CollectionOfC { get; set; }
    }
}
