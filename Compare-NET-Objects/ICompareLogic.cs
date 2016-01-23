namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Interface for mocking
    /// </summary>
    public interface ICompareLogic
    {
        /// <summary>
        /// The default configuration
        /// </summary>
        ComparisonConfig Config { get; set; }

        /// <summary>
        /// Compare two objects of the same type to each other.
        /// </summary>
        /// <remarks>
        /// Check the Differences or DifferencesString Properties for the differences.
        /// Default MaxDifferences is 1 for performance
        /// </remarks>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <returns>True if they are equal</returns>
        ComparisonResult Compare(object object1, object object2);

        /// <summary>
        /// Reflection properties and fields are cached. By default this cache is cleared automatically after each compare.
        /// </summary>
        void ClearCache();
    }
}