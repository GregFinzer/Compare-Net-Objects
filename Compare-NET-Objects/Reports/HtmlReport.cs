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
            //writer.WriteLine("Bread Crumb,Expected,Actual");
            writer.WriteLine("<thead><th>{0}</th><th>{1}</th><th>{2}</th></thead>", Config.BreadCrumbColumName, Config.ExpectedColumnName, Config.ActualColumnName);

            foreach (var difference in differences)
            {
                //writer.Write("{0},", EscapeString(difference.GetShortItem()));
                //writer.Write("{0},", EscapeString(difference.Object1Value));
                //writer.WriteLine("{0}", EscapeString(difference.Object2Value));
                writer.WriteLine("<tr><td class=\"diff-crumb\">{0}</td><td class=\"diff-expected\">{1}</td><td class=\"diff-actual\">{2}</td></tr>",
                EscapeString(difference.GetShortItem()),
                EscapeString(difference.Object1Value),
                EscapeString(difference.Object2Value)
                );
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

    /// <summary>
    /// Config object for HtmlReport
    /// </summary>
    public class HtmlConfig
    {
        /// <summary>
        /// The header value of the Bread Crumb column
        /// </summary>
        public string BreadCrumbColumName { get; set; }
        /// <summary>
        /// The header value of the Expected column
        /// </summary>
        public string ExpectedColumnName { get; set; }
        /// <summary>
        /// The header value of the Actual column
        /// </summary>
        public string ActualColumnName { get; set; }
        /// <summary>
        /// If true, the output will be complete html, if false, it will just be the table
        /// </summary>
        public bool GenerateFullHtml { get; set; }
        /// <summary>
        /// Setting this will overwrite the default html header (html, head, body tags)
        /// </summary>
        public string HtmlHeader { get { return _htmlHeader.Replace("%TITLE%", HtmlTitle).Replace("%CSS%", Style); } set { _htmlHeader = value; } }
        /// <summary>
        /// Setting this will overwrite the default html footer (closing body, html tags)
        /// </summary>
        public string HtmlFooter { get; set; }
        /// <summary>
        /// The title of the page - only visible if GenerateFullHtml == true
        /// </summary>
        public string HtmlTitle { get; set; }
        /// <summary>
        /// The CSS Style of the page - only used if the GenerateFullHtml == true
        /// </summary>
        public string Style { get; set; }

        private string _htmlHeader;

        /// <summary>
        /// Default constructor, sets default values
        /// </summary>
        public HtmlConfig()
        {
            // set all the defaults
            BreadCrumbColumName = "Bread Crumb";
            ExpectedColumnName = "Expected";
            ActualColumnName = "Actual";
            GenerateFullHtml = false;
            HtmlTitle = "Document";
            _htmlHeader = _header;
            HtmlFooter = _footer;
            Style = _css;
        }

        /// <summary>
        /// Appends to the existing Style value
        /// </summary>
        /// <param name="css">Any css to append</param>
        public void IncludeCustomCSS(string css)
        {
            Style += css;
        }

        /// <summary>
        /// Replaces the existing Style value
        /// </summary>
        /// <param name="css">Any css to use</param>
        public void ReplaceCSS(string css)
        {
            Style = css;
        }

        private const string _header = @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <meta http-equiv=""X-UA-Compatible"" content=""ie=edge"">
    <title>%TITLE%</title>
    <style>
    %CSS%
    </style>
</head>
<body>
  ";

        private const string _footer = @"</body>
</html>";

        private const string _css = @"
    //.diff-crumb {background: gray;}
    .diff-table tr:nth-child(odd) { background-color:#eee; }
    .diff-table tr:nth-child(even) { background-color:#fff; }
    //.diff-expected {color: red;}
  .diff-actual {color:red;}
  ";
    }
}
