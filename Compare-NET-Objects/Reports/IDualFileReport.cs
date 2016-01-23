using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjects.Reports
{
    /// <summary>
    /// Define a dual file report like Beyond Compare, WinMerge etc.
    /// </summary>
    public interface IDualFileReport
    {
        /// <summary>
        /// Output the differences to two files
        /// </summary>
        /// <param name="differences">A list of differences</param>
        /// <param name="expectedFilePath">The path to write the expected results</param>
        /// <param name="actualFilePath">The path to write the actual results</param>
        void OutputFiles(List<Difference> differences, string expectedFilePath, string actualFilePath);

        /// <summary>
        /// Launch the comparison application
        /// </summary>
        /// <param name="expectedFilePath">The path to write the expected results</param>
        /// <param name="actualFilePath">The path to write the actual results</param>
        void LaunchApplication(string expectedFilePath, string actualFilePath);
    }
}
