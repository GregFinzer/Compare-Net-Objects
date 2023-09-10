using System;
using System.Collections.Generic;
using System.Reflection;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.Attributes;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class GetTypeInfoTests
    {
        [Test]
        public void EnsureCallingGetTypeInfoCompiles()
        {
            _ = typeof(AlPeTests).GetTypeInfo();
        }
    }
}