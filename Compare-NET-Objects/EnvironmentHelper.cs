using System;
using System.IO;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Environment helper class
    /// </summary>
    public static class EnvironmentHelper
    {
        /// <summary>
        /// Returns true if we are running in Windows
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows()
        {
            string windir = Environment.GetEnvironmentVariable("windir");
            return !string.IsNullOrEmpty(windir) && windir.Contains(@"\") && Directory.Exists(windir);
        }
    }
}
