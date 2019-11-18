using System;
using System.Reflection;

namespace KellermanSoftware.CompareNetObjects
{
    using KellermanSoftware.CompareNetObjects.TypeComparers;

    /// <summary>
    /// Get custom validator based on property
    /// </summary>
    public static class CustomValidationLogic
    {
        /// <summary>
        /// Get validator for a member of an expando object
        /// </summary>
        /// <param name="config"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static BaseTypeComparer CustomValidatorForDynamicMember(ComparisonConfig config, string name, Type type)
        {
            BaseTypeComparer customValidator = null;

            if (config.CustomPropertyComparers.Count == 0)
            {
                return customValidator;
            }

            //Only compare specific member names
            customValidator = GetValidatorByName(config, name);
            if (customValidator != null)
                return customValidator;

            //Get by type.membername
            customValidator = GetValidatorByName(config, type.Name + "." + name);
            if (customValidator != null)
                return customValidator;

            return customValidator;
        }

        /// <summary>
        /// Get validator for a member
        /// </summary>
        /// <param name="config"></param>
        /// <param name="info"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static BaseTypeComparer CustomValidatorForMember(ComparisonConfig config, MemberInfo info, Type objectType)
        {
            BaseTypeComparer customValidator = null;

            if (config.CustomPropertyComparers.Count == 0)
            {
                return customValidator;
            }

            //Get by objecttype.membername
            customValidator = GetValidatorByName(config, objectType.Name + "." + info.Name);
            if (customValidator != null)
                return customValidator;

            //Get by declaringType.membername
            if (info.DeclaringType != null)
            {
                customValidator = GetValidatorByName(config, info.DeclaringType.Name + "." + info.Name);
                if (customValidator != null)
                    return customValidator;
            }

            //Get exactly by the name of the member
            customValidator = GetValidatorByName(config, info.Name);
            if (customValidator != null)
                return customValidator;

            return customValidator;
        }

        private static BaseTypeComparer GetValidatorByName(ComparisonConfig config, string name)
        {
             config.CustomPropertyComparers.TryGetValue(name, out var comparer);
             return comparer;
        }
    }
}
