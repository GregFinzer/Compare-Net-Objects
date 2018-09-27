using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace KellermanSoftware.CompareNetObjects.Reports
{
    /// <summary>
    /// Create a CSV file of the differences and launch the default CSV handler (usually Excel)
    /// </summary>
    public class CsvReport : ISingleFileReport
    {
        #if !NETSTANDARD
        /// <summary>
        /// Output the differences to a file
        /// </summary>
        /// <param name="differences">A list of differences</param>
        /// <param name="filePath">The file path</param>
        public void OutputFile(List<Difference> differences, string filePath)
        {
            if (String.IsNullOrEmpty(Path.GetDirectoryName(filePath)))
                filePath = Path.Combine(FileHelper.GetCurrentDirectory(), filePath);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (TextWriter writer = new StreamWriter(fileStream))
                {
                    WriteItOut(differences, writer);
                }
            }            
        }

        #endif

        private void WriteItOut(List<Difference> differences, TextWriter writer)
        {
            writer.WriteLine("Bread Crumb,Expected,Actual");

            foreach (var difference in differences)
            {
                writer.Write("{0},", EscapeString(difference.GetShortItem()));
                writer.Write("{0},", EscapeString(difference.Object1Value));
                writer.WriteLine("{0}", EscapeString(difference.Object2Value));
            }
        }

        /// <summary>
        /// Output the differences to a stream
        /// </summary>
        /// <param name="differences">A list of differences</param>
        /// <param name="stream">An output stream</param>
        public void OutputStream(List<Difference> differences, Stream stream)
        {
            TextWriter writer = new StreamWriter(stream);
            WriteItOut(differences, writer);

            if (stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);
        }


        /// <summary>
        /// Output the differences to a string
        /// </summary>
        /// <param name="differences">A list of differences</param>
        /// <returns>A string</returns>
        public string OutputString(List<Difference> differences)
        {
            StringBuilder sb = new StringBuilder(differences.Count*40);
            TextWriter writer = new StringWriter(sb);
            WriteItOut(differences, writer);

            return sb.ToString();
        }

        #if !NETSTANDARD
        /// <summary>
        /// Launch the WinMerge
        /// </summary>
        /// <param name="filePath">The differences file</param>
        public void LaunchApplication(string filePath)
        {
            ProcessHelper.Shell(filePath, string.Empty, ProcessWindowStyle.Normal, false);
        }
        #endif

        /// <summary>
        /// Escape special characters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string EscapeString(object value)
        {
            if (value == null)
                return string.Empty;

            string data = value.ToString();

            // CSV rules: http://en.wikipedia.org/wiki/Comma-separated_values#Basic_rules
            // From the rules:
            // 1. if the data has quote, escape the quote in the data
            // 2. if the data contains the delimiter (in our case ','), double-quote it
            // 3. if the data contains the new-line, double-quote it.

            if (data.Contains("\""))
            {
                data = data.Replace("\"", "\"\"");
            }

            if (data.Contains(",") || data.Contains(Environment.NewLine))
            {
                data = String.Format("{0}{1}{2}", "\"", data, "\"");
            }

            return data;
        }
    }
}
