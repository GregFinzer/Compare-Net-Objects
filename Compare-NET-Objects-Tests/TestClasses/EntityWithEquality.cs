using System;
using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{

    [Serializable]
    public class EntityWithEquality : Entity
    {
        public override int GetHashCode()
        {
            if (null != this.Description)
                return this.Description.GetHashCode();

            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var realObj = obj as Entity;

            if (null == realObj)
                return false;

            return realObj.Description.Equals(this.Description);

        }
    }
}
