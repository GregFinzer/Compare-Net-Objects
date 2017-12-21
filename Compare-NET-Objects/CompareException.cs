using System;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// CompareException with a Result Property
    /// </summary>
    public class CompareException : Exception
    {
        /// <summary>
        /// CompareException Constructor
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
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
