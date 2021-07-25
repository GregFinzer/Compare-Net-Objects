using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare all the fields of a class or struct (Note: inherits from BaseComparer instead of TypeComparer).
    /// </summary>
    public class FieldComparer : BaseComparer
    {
        private readonly RootComparer _rootComparer;

        /// <summary>
        /// Constructor with a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public FieldComparer(RootComparer rootComparer)
        {
            _rootComparer = rootComparer;
        }

        /// <summary>
        /// Compare the fields of a class
        /// </summary>
        public void PerformCompareFields(CompareParms parms)
        {
            var currentFields = GetCurrentFields(parms);

            foreach (FieldInfo item in currentFields)
            {
                CompareField(parms, item);

                if (parms.Result.ExceededDifferences)
                    return;
            }
        }

        private void CompareField(CompareParms parms, FieldInfo item)
        {
            //Skip if this is a shallow compare
            if (!parms.Config.CompareChildren && TypeHelper.CanHaveChildren(item.FieldType))
                return;

            //Skip if it should be excluded based on the configuration
            if (ExcludeLogic.ShouldExcludeMember(parms.Config, item, parms.Object1Type))
                return;

            //If we ignore types then we must get correct FieldInfo object
            FieldInfo secondFieldInfo = GetSecondFieldInfo(parms, item);

            //If the field does not exist, and we are ignoring the object types, skip it
            if ((parms.Config.IgnoreObjectTypes || parms.Config.IgnoreConcreteTypes) && secondFieldInfo == null)
                return;

            object objectValue1 = item.GetValue(parms.Object1);
            object objectValue2 = secondFieldInfo != null ? secondFieldInfo.GetValue(parms.Object2) : null;

            bool object1IsParent = objectValue1 != null && (objectValue1 == parms.Object1 || parms.Result.IsParent(objectValue1));
            bool object2IsParent = objectValue2 != null && (objectValue2 == parms.Object2 || parms.Result.IsParent(objectValue2));

            //Skip fields that point to the parent
            if ((TypeHelper.IsClass(item.FieldType) || TypeHelper.IsInterface(item.FieldType))
                && (object1IsParent || object2IsParent))
            {
                return;
            }

            string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, item.Name);

            CompareParms childParms = new CompareParms
            {
                Result = parms.Result,
                Config = parms.Config,
                ParentObject1 = parms.Object1,
                ParentObject2 = parms.Object2,
                Object1 = objectValue1,
                Object2 = objectValue2,
                BreadCrumb = currentBreadCrumb
            };

            _rootComparer.Compare(childParms);
        }

        private static FieldInfo GetSecondFieldInfo(CompareParms parms, FieldInfo item)
        {
            FieldInfo secondFieldInfo = null;
            if (parms.Config.IgnoreObjectTypes || parms.Config.IgnoreConcreteTypes)
            {
                IEnumerable<FieldInfo> secondObjectFieldInfos = Cache.GetFieldInfo(parms.Config, parms.Object2Type);

                foreach (var fieldInfo in secondObjectFieldInfos)
                {
                    if (fieldInfo.Name != item.Name) continue;

                    secondFieldInfo = fieldInfo;
                    break;
                }
            }
            else
                secondFieldInfo = item;

            return secondFieldInfo;
        }

        private static IEnumerable<FieldInfo> GetCurrentFields(CompareParms parms)
        {
            IEnumerable<FieldInfo> currentFields = null;

            //Interface Member Logic
            if (parms.Config.InterfaceMembers.Count > 0)
            {
                Type[] interfaces = parms.Object1Type.GetInterfaces();

                foreach (var type in parms.Config.InterfaceMembers)
                {
                    if (interfaces.Contains(type))
                    {
                        currentFields = Cache.GetFieldInfo(parms.Config, type);
                        break;
                    }
                }
            }

            if (currentFields == null)
                currentFields = Cache.GetFieldInfo(parms.Config, parms.Object1Type);
            return currentFields;
        }
    }
}
