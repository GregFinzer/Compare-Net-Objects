using System;
using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public enum Level 
    {
        Company,
        Division,
        Department,
        Employee
    }

    [Serializable]
    public class Entity : IEntity
    {
        private List<IEntity> _children = new List<IEntity>();

        public string Description
        {
            get;
            set;
        }

        public Level EntityLevel
        {
            get;
            set;
        }

        public IEntity Parent
        {
            get;
            set;
        }

        public List<IEntity> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        //can be circular refs, so bye bye for now
        //public override string ToString()
        //{
        //    return string.Format("{0} {{Parent:{1}}} {{Children:{2}}}", Description, null == Parent? "[null]": Parent.ToString(), _children.Count);
        //}
    }
}
