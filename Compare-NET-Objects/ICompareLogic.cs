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
        /// <param name="expectedObject">The expected object value to compare</param>
        /// <param name="actualObject">The actual object value to compare</param>
        /// <returns>True if they are equal</returns>
        ComparisonResult Compare(object expectedObject, object actualObject);

        /// <summary>
        /// Reflection properties and fields are cached. By default this cache is cleared automatically after each compare.
        /// </summary>
        void ClearCache();
    }
}