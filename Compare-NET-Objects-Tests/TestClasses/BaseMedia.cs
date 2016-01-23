using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class BaseMedia
    {
        private string _privateString;

        public void SetPrivateString(string value)
        {
            _privateString = value;
        }
    }
}
