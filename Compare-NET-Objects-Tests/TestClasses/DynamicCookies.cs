using System.Collections.Generic;
using System.Dynamic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    /// <summary>
    /// From Stack Overflow:  https://stackoverflow.com/questions/3565481/differences-between-expandoobject-dynamicobject-and-dynamic
    /// </summary>
    public class DynamicCookies : DynamicObject
    {
        readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (properties.ContainsKey(binder.Name))
            {
                result = properties[binder.Name];
                return true;
            }
            else
            {
                //<-- THIS IS OUR CUSTOM "NO COOKIES IN THE JAR" RESPONSE FROM OUR DYNAMIC TYPE WHEN AN UNKNOWN FIELD IS ACCESSED
                result = "I'm sorry, there are no cookies in this jar!"; 

                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            properties[binder.Name] = value;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            dynamic method = properties[binder.Name];
            result = method(args[0].ToString(), args[1].ToString());
            return true;
        }
    }
}
