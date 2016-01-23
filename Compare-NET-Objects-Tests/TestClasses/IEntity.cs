using System;
using System.Collections.Generic;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public interface IEntity
    {
        string Description
        {
            get;
            set;
        }

        Level EntityLevel
        {
            get;
            set;
        }

        IEntity Parent
        {
            get;
            set;
        }

        List<IEntity> Children
        {
            get;
            set;
        }
    }
}
