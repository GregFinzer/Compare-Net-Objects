using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare two properties (Note inherits from BaseComparer instead of TypeComparer
    /// </summary>
    public class PropertyComparer : BaseComparer
    {
        private readonly RootComparer _rootComparer;
        private readonly IndexerComparer _indexerComparer;

        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public PropertyComparer(RootComparer rootComparer)
        {
            _rootComparer = rootComparer;
            _indexerComparer = new IndexerComparer(rootComparer);
        }

        /// <summary>
        /// Compare the properties of a class
        /// </summary>
        public void PerformCompareProperties(CompareParms parms)
        {
            IEnumerable<PropertyInfo> currentProperties = GetCurrentProperties(parms);

            foreach (PropertyInfo info in currentProperties)
            {
                //If we can't read it, skip it
                if (info.CanRead == false)
                    continue;

                //Skip if this is a shallow compare
                if (!parms.Config.CompareChildren && TypeHelper.CanHaveChildren(info.PropertyType))
                    continue;

                //Skip if it should be excluded based on the configuration
                if (ExcludeLogic.ShouldExcludeMember(parms.Config, info))
                    continue;    

                //If we should ignore read only, skip it
                if (!parms.Config.CompareReadOnly && info.CanWrite == false)
                    continue;

                //If we ignore types then we must get correct PropertyInfo object
                var secondObjectInfo = GetSecondObjectInfo(parms, info);

                object objectValue1;
                object objectValue2;
                if (!IsValidIndexer(parms.Config, info, parms.BreadCrumb))
                {
                    objectValue1 = info.GetValue(parms.Object1, null);
                    objectValue2 = secondObjectInfo != null ? secondObjectInfo.GetValue(parms.Object2, null) : null;
                }
                else
                {
                    _indexerComparer.CompareIndexer(parms, info);
                    continue;
                }

                bool object1IsParent = objectValue1 != null && (objectValue1 == parms.Object1 || parms.Result.Parents.ContainsKey(objectValue1.GetHashCode()));
                bool object2IsParent = objectValue2 != null && (objectValue2 == parms.Object2 || parms.Result.Parents.ContainsKey(objectValue2.GetHashCode()));

                //Skip properties where both point to the corresponding parent
                if ((TypeHelper.IsClass(info.PropertyType) || TypeHelper.IsInterface(info.PropertyType) || TypeHelper.IsStruct(info.PropertyType)) 
                    && (object1IsParent && object2IsParent))
                {
                    continue;
                }

                string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, info.Name);

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

                if (parms.Result.ExceededDifferences)
                    return;
            }
        }

        private static PropertyInfo GetSecondObjectInfo(CompareParms parms, PropertyInfo info)
        {
            PropertyInfo secondObjectInfo = null;
            if (parms.Config.IgnoreObjectTypes)
            {
                var secondObjectPropertyInfos = Cache.GetPropertyInfo(parms.Result, parms.Object2Type);

                foreach (var propertyInfo in secondObjectPropertyInfos)
                {
                    if (propertyInfo.Name != info.Name) continue;

                    secondObjectInfo = propertyInfo;
                    break;
                }
            }
            else
                secondObjectInfo = info;
            return secondObjectInfo;
        }

        private static IEnumerable<PropertyInfo> GetCurrentProperties(CompareParms parms)
        {
            IEnumerable<PropertyInfo> currentProperties = null;

            //Interface Member Logic
            if (parms.Config.InterfaceMembers.Count > 0)
            {
                Type[] interfaces = parms.Object1Type.GetInterfaces();

                foreach (var type in parms.Config.InterfaceMembers)
                {
                    if (interfaces.Contains(type))
                    {
                        currentProperties = Cache.GetPropertyInfo(parms.Result, type);
                        break;
                    }
                }
            }

            if (currentProperties == null)
                currentProperties = Cache.GetPropertyInfo(parms.Result, parms.Object1Type);
            return currentProperties;
        }

        private bool IsValidIndexer(ComparisonConfig config, PropertyInfo info, string breadCrumb)
        {
            ParameterInfo[] indexers = info.GetIndexParameters();

            if (indexers.Length == 0)
            {
                return false;
            }

            if (config.SkipInvalidIndexers)
                return false;

            if (indexers.Length > 1)
            {
                if (config.SkipInvalidIndexers)
                    return false;

                throw new Exception("Cannot compare objects with more than one indexer for object " + breadCrumb);
            }

            if (indexers[0].ParameterType != typeof(Int32))
            {
                if (config.SkipInvalidIndexers)
                    return false;

                throw new Exception("Cannot compare objects with a non integer indexer for object " + breadCrumb);
            }

            if (info.ReflectedType == null)
            {
                if (config.SkipInvalidIndexers)
                    return false;

                throw new Exception("Cannot compare objects with a null indexer for object " + breadCrumb);
            }

            if (info.ReflectedType.GetProperty("Count") == null
                || info.ReflectedType.GetProperty("Count").PropertyType != typeof(Int32))
            {
                if (config.SkipInvalidIndexers)
                    return false;

                throw new Exception("Indexer must have a corresponding Count property that is an integer for object " + breadCrumb);
            }

            return true;
        }
    }
}
