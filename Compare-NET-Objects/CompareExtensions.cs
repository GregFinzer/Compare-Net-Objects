using System;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// A set of BDD style comparison extensions for use with Testing Frameworks
    /// </summary>
    public static class CompareExtensions
    {
        private static readonly CompareLogic _compareLogic;

        static CompareExtensions()
        {
            Config = new ComparisonConfig();
            _compareLogic = new CompareLogic();
        }

        /// <summary>
        /// Alter the configuration for the comparison
        /// </summary>
        public static ComparisonConfig Config { get; set; }

        /// <summary>
        /// Throws a CompareException if the classes are not equal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="message"></param>
        public static void ShouldCompare<T>(this T actual, T expected, string message = null)
        {
            _compareLogic.Config = Config;
            ComparisonResult result = _compareLogic.Compare(expected, actual);

            if (!result.AreEqual)
            {
                throw new CompareException(result, BuildExpectedEqualMessage(message,result));
            }
        }

        /// <summary>
        /// Throws a CompareException if the classes are equal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="message"></param>
        public static void ShouldNotCompare<T>(this T actual, T expected, string message = null)
        {
            _compareLogic.Config = Config;
            ComparisonResult result = _compareLogic.Compare(expected, actual);

            if (result.AreEqual)
            {
                throw new CompareException(result, BuildExpectedNotEqualMessage(message, result));
            }
        }

        private static string BuildExpectedEqualMessage(string message, ComparisonResult result)
        {
            message = message ?? "Objects expected to be equal";
            return message + Environment.NewLine + result.DifferencesString + Environment.NewLine;
        }

        private static string BuildExpectedNotEqualMessage(string message, ComparisonResult result)
        {
            message = message ?? "Objects expected NOT to be equal";
            return message + Environment.NewLine + result.DifferencesString + Environment.NewLine;
        }
    }
}
