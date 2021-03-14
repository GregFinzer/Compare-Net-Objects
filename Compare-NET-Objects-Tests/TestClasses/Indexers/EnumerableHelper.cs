using System.Collections.Generic;
using System.Linq;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.Indexers
{
    internal static class EnumerableHelper
    {
        internal static IEnumerable<T> ToTrueEnumerable<T>(this IEnumerable<T> source)
        {
            return source.Select(x => x); // Ensure that type will be enumerable.
        }
    }
}
