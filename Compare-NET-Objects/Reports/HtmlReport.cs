using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace KellermanSoftware.CompareNetObjects.Reports
{
    /// <summary>
    /// Create an HTML file of the differences and launch the default HTML handler
    /// </summary>
    public class HtmlReport : ISingleFileReport
    {
        /// <summary>
        /// Default constructor, sets up Config object
        /// </summary>
        public HtmlReport()
        {
            Config = new HtmlConfig();
        }

        /// <summary>
        /// HtmlReport Configuration
        /// </summary>
        public HtmlConfig Config { get; set; }

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
            if (Config.GenerateFullHtml)
            {
                writer.Write(Config.HtmlHeader);
            }

            writer.WriteLine("<table class=\"diff-table\">");
            writer.WriteLine("<thead><th>{0}</th><th>{1}</th><th>{2}</th></thead>",
                Config.BreadCrumbColumName,
                Config.ExpectedColumnName,
                Config.ActualColumnName);

            foreach (var difference in differences)
            {
                writer.WriteLine(
                    "<tr><td class=\"diff-crumb\">{0}</td><td class=\"diff-expected\">{1}</td><td class=\"diff-actual\">{2}</td></tr>",
                    EscapeString(difference.GetShortItem()),
                    EscapeString(difference.Object1Value),
                    EscapeString(difference.Object2Value));
            }

            writer.WriteLine("</table>");

            if (Config.GenerateFullHtml)
            {
                writer.Write(Config.HtmlFooter);
            }
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
            StringBuilder sb = new StringBuilder(differences.Count * 40);
            TextWriter writer = new StringWriter(sb);
            WriteItOut(differences, writer);

            return sb.ToString();
        }

        /// <summary>
        /// Escape special characters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string EscapeString(object value)
        {
            if (value == null)
                return string.Empty;

            return WebHelper.HtmlEncode(value.ToString());
        }
    }
}