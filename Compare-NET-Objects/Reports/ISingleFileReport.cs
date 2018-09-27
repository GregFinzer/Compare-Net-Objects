using System.Collections.Generic;
using System.IO;


namespace KellermanSoftware.CompareNetObjects.Reports
{
    /// <summary>
    /// Defines a Single File Report
    /// </summary>
    public interface ISingleFileReport
    {
        #if !NETSTANDARD
        /// <summary>
        /// Output the differences to a file
        /// </summary>
        /// <param name="differences">A list of differences</param>
        /// <param name="filePath">The file path</param>
        void OutputFile(List<Difference> differences, string filePath);
        #endif

        /// <summary>
        /// Output the differences to a stream
        /// </summary>
        /// <param name="differences">A list of differences</param>
        /// <param name="stream">Where to write to</param>
        void OutputStream(List<Difference> differences, Stream stream);

        /// <summary>
        /// Output the differences to a string
        /// </summary>
        /// <param name="differences">A list of differences</param>
        /// <returns>A string</returns>
        string OutputString(List<Difference> differences);

        #if !NETSTANDARD
        /// <summary>
        /// Launch the application for showing the file
        /// </summary>
        /// <param name="filePath">The file path</param>
        void LaunchApplication(string filePath);
        #endif

    }
}
