using System;
using System.IO;

namespace KellermanSoftware.CompareNetObjects
{
    public static class EnvironmentHelper
    {
        public static bool IsWindows()
        {
            string windir = Environment.GetEnvironmentVariable("windir");
            return !string.IsNullOrEmpty(windir) && windir.Contains(@"\") && Directory.Exists(windir);
        }
    }
}
