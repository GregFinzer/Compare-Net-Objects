#if !NETSTANDARD

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KellermanSoftware.CompareNetObjects.Reports
{
    /// <summary>
    /// Abstract Base Duel File Report that has default Output
    /// </summary>
    public abstract class BaseDualFileReport : IDualFileReport
    {
        /// <summary>
        /// Create two difference files and compare in WinMerge
        /// </summary>
        /// <param name="differences">A list of the differences</param>
        /// <param name="expectedFilePath">The path to write the expected results</param>
        /// <param name="actualFilePath">The path to write the actual results</param>
        public virtual void OutputFiles(List<Difference> differences, string expectedFilePath, string actualFilePath)
        {

            if (differences == null)
                throw new ArgumentNullException("differences");

            StringBuilder sb1 = new StringBuilder(differences.Count*80);
            StringBuilder sb2 = new StringBuilder(differences.Count*80);

            bool anyLongerThan80 = differences.Any(o =>
                o.Object1Value.Replace(Environment.NewLine, "|").Length > 80
                || o.Object2Value.Replace(Environment.NewLine, "|").Length > 80);

            foreach (var difference in differences)
            {
                if (anyLongerThan80)
                {
                    sb1.Append(difference.GetShortItem());
                    sb1.Append(", ");
                    sb1.AppendLine(difference.Object1Value.Replace(Environment.NewLine, "|"));

                    sb2.Append(difference.GetShortItem());
                    sb2.Append(", ");
                    sb2.AppendLine(difference.Object2Value.Replace(Environment.NewLine, "|"));
                }
                else
                {
                    sb1.Append(difference.Object1Value.Replace(Environment.NewLine, "|"));
                    sb1.Append(", ");
                    sb1.AppendLine(difference.GetShortItem());

                    sb2.Append(difference.Object2Value.Replace(Environment.NewLine, "|"));
                    sb2.Append(", ");
                    sb2.AppendLine(difference.GetShortItem());
                }
            }

            if (String.IsNullOrEmpty(Path.GetDirectoryName(expectedFilePath)))
                expectedFilePath = Path.Combine(FileHelper.GetCurrentDirectory(), expectedFilePath);

            if (String.IsNullOrEmpty(Path.GetDirectoryName(actualFilePath)))
                actualFilePath = Path.Combine(FileHelper.GetCurrentDirectory(), actualFilePath);

            File.WriteAllText(expectedFilePath, sb1.ToString());
            File.WriteAllText(actualFilePath, sb2.ToString());
        }

        /// <summary>
        /// Launch application to compare two files
        /// </summary>
        /// <param name="expectedFilePath">The path for the expected file results</param>
        /// <param name="actualFilePath">The path for the actual file results</param>
        public abstract void LaunchApplication(string expectedFilePath, string actualFilePath);
    }
}

#endif
