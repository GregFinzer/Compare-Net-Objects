using System;
using System.Reflection;
using NUnitLite;

namespace Compare_NET_Objects_Core.Tests
{
    public class Program
    {
        public static int Main(string[] args)
        {
#if DNX451
            return new AutoRun().Execute(args);
#else
            return new AutoRun().Execute(typeof(Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
#endif
        }
    }
}
