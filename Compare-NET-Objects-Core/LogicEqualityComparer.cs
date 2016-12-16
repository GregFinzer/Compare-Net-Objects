// LogicEqualityComparer: Initial contribution by David Rieman

using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>Implements methods to support the comparison of objects for equality, in a customizable fashion.</summary>
    public class LogicEqualityComparer : LogicEqualityComparer<object> { }

    /// <summary>Implements methods to support the comparison of objects for equality, in a customizable fashion.</summary>
    /// <typeparam name="T">The comparison object type.</typeparam>
    public class LogicEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly CompareLogic _comparer = new CompareLogic();

        /// <summary>Defines the configuration and logic by which Equals comparisons will be performed.</summary>
        public CompareLogic CompareLogic
        {
            get { return _comparer; }
        }

        /// <summary>Gets or sets a value indicating whether the base object hashes should be used.</summary>
        /// <remarks>
        /// False by default to allow CompareLogic to evaluate equivalence of otherwise instance-sensitive hashing objects.
        /// NOTE: Any object which doesn't override GetHashCode will behave this way, so this property should generally be left false.
        /// </remarks>
        public bool UseObjectHashes { get; set; }

        /// <summary>Compare two objects of the same type to each other.</summary>
        /// <returns>True if the objects are considered equivalent, according to the current CompareLogic.</returns>
        public bool Equals(T x, T y)
        {
            return CompareLogic.Compare(x, y).AreEqual;
        }

        /// <summary>Retrieves the hash of the specified object.</summary>
        /// <param name="obj">The object to retrieve a hash for.</param>
        public int GetHashCode(T obj)
        {
            return UseObjectHashes ? obj.GetHashCode() : 0;
        }
    }
}