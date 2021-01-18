using System;
using System.Dynamic;
using System.Xml.Linq;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class DynamicXMLNode : DynamicObject
    {
        XElement node;
        public DynamicXMLNode(XElement node)
        {
            this.node = node;
        }
        public DynamicXMLNode()
        {
        }
        public DynamicXMLNode(String name)
        {
            node = new XElement(name);
        }
        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            XElement setNode = node.Element(binder.Name);
            if (setNode != null)
                setNode.SetValue(value);
            else
            {
                if (value.GetType() == typeof(DynamicXMLNode))
                    node.Add(new XElement(binder.Name));
                else
                    node.Add(new XElement(binder.Name, value));
            }
            return true;
        }
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            XElement getNode = node.Element(binder.Name);
            if (getNode != null)
            {
                result = new DynamicXMLNode(getNode);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }
}
