using System;

namespace KellermanSoftware.CompareNetObjects
{
#if !NEWPCL
    /// <summary>
    /// Extensions for Type to provide backward compatibility between latest and older .net Framework APIs.
    /// </summary>
    public static class TypeBackwardsCompatibilityExtensions
    {
        /// <summary>
        /// Function to provide compilation compatibility between older code and newer style.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The type.</returns>
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }
    }
#endif
}
