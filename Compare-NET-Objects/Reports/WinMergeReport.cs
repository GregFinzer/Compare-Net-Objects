using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
#if !NETSTANDARD
using Microsoft.Win32;
#endif

namespace KellermanSoftware.CompareNetObjects.Reports
{
    /// <summary>
    /// Output files and launch WinMerge
    /// </summary>
    public class WinMergeReport : BaseDualFileReport
    {
        private const string APPLICATION_NAME = "WinMergeU.exe";

#if NETSTANDARD1
        /// <summary>
        /// Throw a NotSupported exception if we are running under .NET Standard 1.0
        /// </summary>
        /// <param name="expectedFilePath">The path to write the expected results</param>
        /// <param name="actualFilePath">The path to write the actual results</param>
        public override void LaunchApplication(string expectedFilePath, string actualFilePath)
        {
            throw new NotSupportedException();
        }	
#else

        /// <summary>
        /// Launch the WinMerge
        /// </summary>
        /// <param name="expectedFilePath">The path to write the expected results</param>
        /// <param name="actualFilePath">The path to write the actual results</param>
        public override void LaunchApplication(string expectedFilePath, string actualFilePath)
        {
            if (!EnvironmentHelper.IsWindows())
                throw new NotSupportedException();

            if (String.IsNullOrEmpty(Path.GetDirectoryName(expectedFilePath)))
                expectedFilePath = Path.Combine(FileHelper.GetCurrentDirectory(), expectedFilePath);

            if (String.IsNullOrEmpty(Path.GetDirectoryName(actualFilePath)))
                actualFilePath = Path.Combine(FileHelper.GetCurrentDirectory(), actualFilePath);

            string args = string.Format("\"{0}\" \"{1}\"", expectedFilePath, actualFilePath);

            string winMergePath = FindWinMerge();

            if (String.IsNullOrEmpty(winMergePath))
                throw new FileNotFoundException(APPLICATION_NAME);

            ProcessHelper.Shell(winMergePath, args, ProcessWindowStyle.Normal, false);
        }
#endif

#if !NETSTANDARD
        /// <summary>
        /// Find the path of the WinMerge executable
        /// </summary>
        /// <returns>The path or null if not found</returns>
        public string FindWinMerge()
        {
            RegistryKey registryKey =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinMergeU.exe");

            if (registryKey == null)
                return null;

            object value = registryKey.GetValue("");

            if (value == null)
                return null;

            return value.ToString();
        }
#else
        /// <summary>
        /// Find the path of the WinMerge executable
        /// </summary>
        /// <returns>The path or null if not found</returns>
        public string FindWinMerge()
        {
            //It should be in the Program Files (x86) directory
            string programFilesPath = Environment.GetEnvironmentVariable("ProgramFiles(x86)");

            if (!String.IsNullOrEmpty(programFilesPath))
            {
                string[] directories = Directory.GetDirectories(programFilesPath, "WinMerge");

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
                string[] directories = Directory.GetDirectories(programFilesPath, "WinMerge");

                foreach (var directory in directories.OrderByDescending(o => o))
                {
                    string[] files = Directory.GetFiles(directory, APPLICATION_NAME);

                    if (files.Any())
                        return files.First();
                }
            }

            return null;   
        }
#endif
    }
}

