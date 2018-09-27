#if !NETSTANDARD

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace KellermanSoftware.CompareNetObjects.Reports
{
    /// <summary>
    /// Output files and launch Beyond Compare
    /// </summary>
    public class BeyondCompareReport : BaseDualFileReport
    {
        private const string APPLICATION_NAME = "BCompare.exe";

        /// <summary>
        /// Launch the WinMerge
        /// </summary>
        /// <param name="expectedFilePath">The path to write the expected results</param>
        /// <param name="actualFilePath">The path to write the actual results</param>
        public override void LaunchApplication(string expectedFilePath, string actualFilePath)
        {
            if (String.IsNullOrEmpty(Path.GetDirectoryName(expectedFilePath)))
                expectedFilePath = Path.Combine(FileHelper.GetCurrentDirectory(), expectedFilePath);

            if (String.IsNullOrEmpty(Path.GetDirectoryName(actualFilePath)))
                actualFilePath = Path.Combine(FileHelper.GetCurrentDirectory(), actualFilePath);

            string args = string.Format("\"{0}\" \"{1}\"", expectedFilePath, actualFilePath);

            string beyondComparePath = FindBeyondCompare();

            if (String.IsNullOrEmpty(beyondComparePath))
                throw new FileNotFoundException(APPLICATION_NAME);

            ProcessHelper.Shell(beyondComparePath, args, ProcessWindowStyle.Normal, false);
        }

        /// <summary>
        /// Find the path of the Beyond Compare executable
        /// </summary>
        /// <returns>The path or null if not found</returns>
        public string FindBeyondCompare()
        {
            //It should be in the Program Files (x86) directory
            string programFilesPath = Environment.GetEnvironmentVariable("ProgramFiles(x86)");

            if (!String.IsNullOrEmpty(programFilesPath))
            {
                string[] directories = Directory.GetDirectories(programFilesPath, "Beyond Compare*");

                foreach (var directory in directories.OrderByDescending(o => o))
                {
                    string[] files = Directory.GetFiles(directory, APPLICATION_NAME);

                    if (files.Any())
                        return files.First();
                }
            }

            programFilesPath = Environment.GetEnvironmentVariable("ProgramFiles");

            if (!String.IsNullOrEmpty(programFilesPath))
            {
                string[] directories = Directory.GetDirectories(programFilesPath, "Beyond Compare*");

                foreach (var directory in directories.OrderByDescending(o => o))
                {
                    string[] files = Directory.GetFiles(directory, APPLICATION_NAME);

                    if (files.Any())
                        return files.First();
                }
            }

            return null;
        }
    }
}

#endif
