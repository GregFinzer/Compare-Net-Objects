
using System;
using System.Linq;
using System.Reflection;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Exclude types depending upon the configuration
    /// </summary>
    public static class ExcludeLogic 
    {
        /// <summary>
        /// Returns true if the property or field should be excluded
        /// </summary>
        /// <param name="config"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool ShouldExcludeMember(ComparisonConfig config, MemberInfo info)
        {
            //Only compare specific member names
            if (config.MembersToInclude.Count > 0 && !config.MembersToInclude.Contains(info.Name))
                return true;
            
            if (config.MembersToIgnore.Count > 0)
            {
                //Ignore by type.membername
                if (info.DeclaringType != null
                    && config.MembersToIgnore.Contains(info.DeclaringType.Name + "." + info.Name))
                    return true;

                //Ignore exactly by the name of the member
                if (config.MembersToIgnore.Count > 0 && config.MembersToIgnore.Contains(info.Name))
                    return true;

                //Wildcard member
                if (ExcludedByWildcard(config, info))
                    return true;
            }


            if (IgnoredByAttribute(config, info))
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if the property or field should be exluded by wilcard
        /// </summary>
        /// <param name="config"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool ExcludedByWildcard(ComparisonConfig config, MemberInfo info)
        {
            foreach (var memberWildcard in config.MembersToIgnore)
            {
                string small;

                if (memberWildcard.StartsWith("*") && memberWildcard.EndsWith("*") && memberWildcard.Length > 2)
                {
                    small = memberWildcard.Substring(1, memberWildcard.Length - 2);
                    if (info.Name.Contains(small))
                    {
                        return true;
                    }
                }
                else if (memberWildcard.StartsWith("*")
                         && memberWildcard.Length >= 2
                         && info.Name.EndsWith(memberWildcard.Substring(1)))
                {
                    return true;
                }
                else if (memberWildcard.EndsWith("*")
                         && memberWildcard.Length >= 2
                         && info.Name.StartsWith(memberWildcard.Substring(0, memberWildcard.Length - 2)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the class should be exluded by Attribute
        /// </summary>
        /// <param name="config"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool ShouldExcludeClass(ComparisonConfig config, Type t1, Type t2)
        {
            //Only include specific class types
            if (config.ClassTypesToInclude.Count > 0
                && (!config.ClassTypesToInclude.Contains(t1)
                    || !config.ClassTypesToInclude.Contains(t2)))
            {
                return true;
            }

            //Ignore specific class types
            if (config.ClassTypesToIgnore.Count > 0
                && (config.ClassTypesToIgnore.Contains(t1)
                    || config.ClassTypesToIgnore.Contains(t2)))
            {
                return true;
            }

            //The class is ignored by an attribute
            if (IgnoredByAttribute(config, t1.GetTypeInfo()) || IgnoredByAttribute(config, t2.GetTypeInfo()))
                return true;

            return false;
        }

        /// <summary>
        /// Check if the class type should be excluded based on the configuration
        /// </summary>
        /// <param name="config"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool ShouldExcludeType(ComparisonConfig config, Type t1, Type t2)
        {
            //Only include specific types
            if (config.TypesToInclude.Count > 0
                && (!config.TypesToInclude.Contains(t1)
                    || !config.TypesToInclude.Contains(t2)))
            {
                return true;
            }

            //Ignore specific types
            if (config.TypesToIgnore.Count > 0
                && (config.TypesToIgnore.Contains(t1)
                    || config.TypesToIgnore.Contains(t2)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if any type has attributes that should be bypassed
        /// </summary>
        /// <returns></returns>
        public static bool IgnoredByAttribute(ComparisonConfig config, MemberInfo info)
        {
            var attributes = info.GetCustomAttributes(true);

            return attributes.Any(a => config.AttributesToIgnore.Contains(a.GetType()));
        }

    }
}
