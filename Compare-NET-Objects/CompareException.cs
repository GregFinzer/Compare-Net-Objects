using System;

namespace KellermanSoftware.CompareNetObjects
{
    public class CompareException : Exception
    {
        public CompareException(ComparisonResult result, string message) : base(message)
        {
            Result = result;
        }

        /// <summary>
        /// The comparison Result
        /// </summary>
        public ComparisonResult Result { get; private set; }
    }
}
