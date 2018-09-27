#if !NETSTANDARD

using System;
using System.Diagnostics;
using System.IO;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Helper methods for processes
    /// </summary>
    public static class ProcessHelper
    {
        /// <summary>
        /// Execute an external program.
        /// </summary>
        /// <param name="executablePath">Path and filename of the executable.</param>
        /// <param name="arguments">Arguments to pass to the executable.</param>
        /// <param name="windowStyle">Window style for the process (hidden, minimized, maximized, etc).</param>
        /// <param name="waitUntilFinished">Wait for the process to finish.</param>
        /// <returns>Exit Code</returns>
        public static int Shell(string executablePath, string arguments, ProcessWindowStyle windowStyle, bool waitUntilFinished)
        {
            string fileName = "";

            try
            {
                Process process = new Process();
                string assemblyPath = Path.Combine(FileHelper.GetCurrentDirectory(), Path.GetFileName(executablePath) ?? string.Empty);

                //Look for the file in the executing assembly directory
                if (File.Exists(assemblyPath))
                {
                    fileName = assemblyPath;
                    process.StartInfo.FileName = assemblyPath;
                }
                else // if there is no path to the file, an error will be thrown
                {
                    fileName = executablePath;
                    process.StartInfo.FileName = executablePath;
                }

                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.WindowStyle = windowStyle;

                //Start the Process
                process.Start();

                if (waitUntilFinished)
                {
                    process.WaitForExit();
                }

                if (waitUntilFinished)
                    return process.ExitCode;

                return 0;
            }
            catch
            {
                string message = string.Format("Shell Fail: {0} {1}", fileName, arguments);
                throw new ApplicationException(message);
            }
        }
    }
}

#endif
