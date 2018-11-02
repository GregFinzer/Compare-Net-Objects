using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KellermanSoftware.CompareNetObjects
{
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
        public string HtmlHeader
        {
            get { return _htmlHeader.Replace("%TITLE%", HtmlTitle).Replace("%CSS%", Style); }
            set { _htmlHeader = value; }
        }

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
