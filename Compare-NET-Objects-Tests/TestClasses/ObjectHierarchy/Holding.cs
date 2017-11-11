using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.ObjectHierarchy
{
    public class Holding
    {
        public int Id { get; set; }

        public List<Identifier> Identifiers { get; set; }

        public List<Attribute> Attributes { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
