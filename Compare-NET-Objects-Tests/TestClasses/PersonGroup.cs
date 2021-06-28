using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    class PersonGroup
    {
        public Person Manager { get; set; }
        public IList<Person> Members { get; set; }
    }

    class GroupManager : Person
    {
        public string Title;
    }

    class GroupManagerNotInherited
    {
        public string Name { get; set; }
        public string Title;
    }

}
