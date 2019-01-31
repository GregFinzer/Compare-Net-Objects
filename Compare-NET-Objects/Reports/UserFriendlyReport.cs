using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace KellermanSoftware.CompareNetObjects.Reports
{
    /// <summary>
    /// Report for showing differences to an end user
    /// </summary>
    public class UserFriendlyReport : ISingleFileReport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserFriendlyReport"/> class.
        /// </summary>
        public UserFriendlyReport()
        {
            ChangedToText = "CHANGED TO ->";
        }

        /// <summary>
        /// The text in between the values.  Defaults to: CHANGED TO ->
        /// </summary>
        public string ChangedToText { get; set; }

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

        private string FormatProperty(Difference difference)
        {
            string shortItem = difference.GetShortItem();
            StringBuilder sb = new StringBuilder(shortItem.Length);
            string[] words = shortItem.Split(new [] {'.'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                sb.Append(StringHelper.InsertSpaces(word));
                sb.Append(" ");
            }

            return sb.ToString().Trim();
        }

        private void WriteItOut(List<Difference> differences, TextWriter writer)
        {
            foreach (var difference in differences)
            {
                writer.Write("{0}: ", FormatProperty(difference));
                writer.Write("{0} {1} ", difference.Object1Value, ChangedToText);
                writer.WriteLine("{0}", difference.Object2Value);
            }
        }

        /// <summary>
        /// Output the differences to a stream
        /// </summary>
        /// <param name="differences">A list of differences</param>
        /// <param name="stream">Where to write to</param>
        public void OutputStream(List<Difference> differences, Stream stream)
        {
            TextWriter writer = new StreamWriter(stream);
            WriteItOut(differences, writer);
            writer.Flush();

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
            StringBuilder sb = new StringBuilder(differences.Count * 40);
            TextWriter writer = new StringWriter(sb);
            WriteItOut(differences,writer);

            return sb.ToString();
        }

        #if !NETSTANDARD
        /// <summary>
        /// Launch the application for showing the file
        /// </summary>
        /// <param name="filePath">The file path</param>
        public void LaunchApplication(string filePath)
        {
            ProcessHelper.Shell(filePath, string.Empty, ProcessWindowStyle.Normal, false);
        }
        #endif

    }
}
