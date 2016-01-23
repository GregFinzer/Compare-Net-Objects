using System;
using System.ComponentModel;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class Person2 : IName, IDataErrorInfo
    {
        public DateTime DateCreated;

        public string Name
        {
            get;
            set;
        }

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get { return string.Empty; }
        }
    }
}
