using System;
using System.Collections.Generic;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    [Serializable]
    public class GenericEntity<T> where T : IEntity
    {
        public List<T> MyList { get; set; }
    }
}
