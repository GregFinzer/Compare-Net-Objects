using System;

namespace KellermanSoftware.CompareNetObjectsTests.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CompareIgnoreAttribute : Attribute
    {
    }
}
