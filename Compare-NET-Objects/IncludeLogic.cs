
using System;
using System.Linq;
using System.Reflection;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Include types depending upon the configuration
    /// </summary>
    internal static class IncludeLogic 
    {
        /// <summary>
        /// Returns true if the property or field should be included
        /// </summary>
        /// <param name="config"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool ShouldIncludeMember(ComparisonConfig config, MemberInfo info)
        {
            if (IncludedByAttribute(config, info))
                return true;

            return false;
        }

        /// <summary>
        /// Check if the class type should be included based on the configuration
        /// </summary>
        /// <param name="config"></param>
        /// <param name="t1"></param>       
        /// <returns></returns>
        public static bool ShouldIncludeClassType(ComparisonConfig config, Type t1)
        {
           
            //The class is ignored by an attribute
            if (IncludedByAttribute(config, t1))
                return true;

            return false;
        }

        /// <summary>
        /// Check if any type has attributes that should be included
        /// </summary>
        /// <returns></returns>
        public static bool IncludedByAttribute(ComparisonConfig config, MemberInfo info)
        {
            var attributes = info.GetCustomAttributes(true);
            return attributes.Any(a => config.AttributesToInclude.Contains(a.GetType()));
        }

    }
}
