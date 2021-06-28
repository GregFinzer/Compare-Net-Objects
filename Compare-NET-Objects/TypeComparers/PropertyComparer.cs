using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare two properties (Note: inherits from BaseComparer instead of TypeComparer).
    /// </summary>
    public class PropertyComparer : BaseComparer
    {
        private readonly RootComparer _rootComparer;
        private readonly IndexerComparer _indexerComparer;
        private static readonly string[] _baseList = { "Count", "Capacity", "Item" };

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
        public void PerformCompareProperties(CompareParms parms, bool ignoreBaseList= false)
        {
            List<PropertyEntity> object1Properties = GetCurrentProperties(parms, parms.Object1, parms.Object1Type);
            List<PropertyEntity> object2Properties = GetCurrentProperties(parms, parms.Object2, parms.Object2Type);

            foreach (PropertyEntity propertyEntity in object1Properties)
            {
                if (ignoreBaseList && _baseList.Contains(propertyEntity.Name))
                    continue;
                    
                CompareProperty(parms, propertyEntity, object2Properties);

                if (parms.Result.ExceededDifferences)
                    return;
            }
        }

        /// <summary>
        /// Compare a single property of a class
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="info"></param>
        /// <param name="object2Properties"></param>
        private void CompareProperty(CompareParms parms, PropertyEntity info, List<PropertyEntity> object2Properties)
        {
            //If we can't read it, skip it
            if (info.CanRead == false)
                return;

            //Skip if this is a shallow compare
            if (!parms.Config.CompareChildren && TypeHelper.CanHaveChildren(info.PropertyType))
                return;

            //Skip if it should be excluded based on the configuration
            if (info.PropertyInfo != null && ExcludeLogic.ShouldExcludeMember(parms.Config, info.PropertyInfo, info.DeclaringType))
                return;

            //This is a dynamic property to be excluded on an expando object
            if (info.IsDynamic && ExcludeLogic.ShouldExcludeDynamicMember(parms.Config, info.Name, info.DeclaringType))
                return;

            //If we should ignore read only, skip it
            if (!parms.Config.CompareReadOnly && info.CanWrite == false)
                return;

            //If we ignore types then we must get correct PropertyInfo object
            PropertyEntity secondObjectInfo = GetSecondObjectInfo(info, object2Properties);

            //If the property does not exist, and we are ignoring the object types, skip it
            if ((parms.Config.IgnoreObjectTypes || parms.Config.IgnoreConcreteTypes) && secondObjectInfo == null)
                return;

            //Check if we have custom function to validate property
            BaseTypeComparer customComparer = null;
            if (info.PropertyInfo != null)
                customComparer = CustomValidationLogic.CustomValidatorForMember(parms.Config, info.PropertyInfo, info.DeclaringType)
                                 ?? CustomValidationLogic.CustomValidatorForDynamicMember(parms.Config, info.Name, info.DeclaringType);

            object objectValue1;
            object objectValue2;
            if (!IsValidIndexer(parms.Config, info, parms.BreadCrumb))
            {
                objectValue1 = info.Value;
                objectValue2 = secondObjectInfo != null ? secondObjectInfo.Value : null;
            }
            else
            {
                _indexerComparer.CompareIndexer(parms, info, secondObjectInfo);
                return;
            }

            bool object1IsParent = objectValue1 != null && (objectValue1 == parms.Object1 || parms.Result.IsParent(objectValue1));
            bool object2IsParent = objectValue2 != null && (objectValue2 == parms.Object2 || parms.Result.IsParent(objectValue2));

            //Skip properties where both point to the corresponding parent
            if ((TypeHelper.IsClass(info.PropertyType) || TypeHelper.IsInterface(info.PropertyType) || TypeHelper.IsStruct(info.PropertyType))
                && (object1IsParent && object2IsParent))
            {
                return;
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
                BreadCrumb = currentBreadCrumb,
                CustomPropertyComparer = customComparer,
                Object1DeclaredType = info?.PropertyType,
                Object2DeclaredType = secondObjectInfo?.PropertyType
            };

            _rootComparer.Compare(childParms);
        }

        private static PropertyEntity GetSecondObjectInfo(PropertyEntity info, List<PropertyEntity> object2Properties)
        {
            foreach (var object2Property in object2Properties)
            {
                if (info.Name == object2Property.Name)
                    return object2Property;
            }

            return null;
        }

        private static List<PropertyEntity> GetCurrentProperties(CompareParms parms, object objectValue, Type objectType)
        {
            return HandleDynamicObject(objectValue, objectType)
                   ?? HandleInterfaceMembers(parms, objectValue, objectType)
                   ?? HandleNormalProperties(parms, objectValue, objectType);
        }

        private static List<PropertyEntity> HandleNormalProperties(CompareParms parms, object objectValue, Type objectType)
        {
            IEnumerable<PropertyInfo> properties = Cache.GetPropertyInfo(parms.Result.Config, objectType);
            return AddPropertyInfos(parms, objectValue, objectType, properties);
        }

        private static List<PropertyEntity> AddPropertyInfos(CompareParms parms,
            object objectValue, 
            Type objectType, 
            IEnumerable<PropertyInfo> properties)
        {
            List<PropertyEntity> currentProperties = new List<PropertyEntity>();
            foreach (var property in properties)
            {
                if (ExcludeLogic.ShouldExcludeMember(parms.Config, property, objectType))
                    continue;

                PropertyEntity propertyEntity = new PropertyEntity();
                propertyEntity.IsDynamic = false;
                propertyEntity.Name = property.Name;
                propertyEntity.CanRead = property.CanRead;
                propertyEntity.CanWrite = property.CanWrite;
                propertyEntity.PropertyType = property.PropertyType;
#if !NETSTANDARD
                propertyEntity.ReflectedType = property.ReflectedType;
#endif
                propertyEntity.Indexers.AddRange(property.GetIndexParameters());
                propertyEntity.DeclaringType = objectType;

                if (propertyEntity.CanRead && (propertyEntity.Indexers.Count == 0))
                {
                    try
                    {
                        propertyEntity.Value = property.GetValue(objectValue, null);
                    }
                    catch (System.Reflection.TargetInvocationException)
                    {
                    }
                    catch (System.NotSupportedException)
                    {
                    }
                }

                propertyEntity.PropertyInfo = property;

                currentProperties.Add(propertyEntity);
            }

            return currentProperties;
        }

        private static List<PropertyEntity> HandleInterfaceMembers(CompareParms parms, object objectValue, Type objectType)
        {
            List<PropertyEntity> currentProperties = new List<PropertyEntity>();

            if (parms.Config.InterfaceMembers.Count > 0)
            {
                Type[] interfaces = objectType.GetInterfaces();

                foreach (var type in parms.Config.InterfaceMembers)
                {
                    if (interfaces.Contains(type))
                    {
                        var properties = Cache.GetPropertyInfo(parms.Result.Config, type);

                        foreach (var property in properties)
                        {
                            PropertyEntity propertyEntity = new PropertyEntity();
                            propertyEntity.IsDynamic = false;
                            propertyEntity.Name = property.Name;
                            propertyEntity.CanRead = property.CanRead;
                            propertyEntity.CanWrite = property.CanWrite;
                            propertyEntity.PropertyType = property.PropertyType;
                            propertyEntity.Indexers.AddRange(property.GetIndexParameters());
                            propertyEntity.DeclaringType = objectType;

                            if (propertyEntity.Indexers.Count == 0)
                            {
                                propertyEntity.Value = property.GetValue(objectValue, null);
                            }

                            propertyEntity.PropertyInfo = property;
                            currentProperties.Add(propertyEntity);
                        }
                    }
                }
            }

            if (currentProperties.Count == 0)
                return null;

            return currentProperties;
        }

        private static List<PropertyEntity> HandleDynamicObject(object objectValue, Type objectType)
        {
            if (TypeHelper.IsExpandoObject(objectValue))
            {
                return AddExpandoPropertyValues(objectValue, objectType);
            }

            return null;
        }

        private static List<PropertyEntity> AddExpandoPropertyValues(Object objectValue, Type objectType)
        {
            IDictionary<string, object> expandoPropertyValues = objectValue as IDictionary<string, object>;

            if (expandoPropertyValues == null)
                return new List<PropertyEntity>();

            List<PropertyEntity> currentProperties  = new List<PropertyEntity>();
            foreach (var propertyValue in expandoPropertyValues)
            {
                PropertyEntity propertyEntity = new PropertyEntity();
                propertyEntity.IsDynamic = true;
                propertyEntity.Name = propertyValue.Key;
                propertyEntity.Value = propertyValue.Value;
                propertyEntity.CanRead = true;
                propertyEntity.CanWrite = true;
                propertyEntity.DeclaringType = objectType;

                if (propertyValue.Value == null)
                {
                    propertyEntity.PropertyType = null;
                    propertyEntity.ReflectedType = null;
                }
                else
                {
                    propertyEntity.PropertyType = propertyValue.GetType();
                    propertyEntity.ReflectedType = propertyEntity.PropertyType;
                }

                currentProperties.Add(propertyEntity);
            }

            return currentProperties;
        }

        private bool IsValidIndexer(ComparisonConfig config, PropertyEntity info, string breadCrumb)
        {
            if (info.Indexers.Count == 0)
            {
                return false;
            }

            if (info.Indexers.Count > 1)
            {
                if (config.SkipInvalidIndexers)
                    return false;

                throw new Exception("Cannot compare objects with more than one indexer for object " + breadCrumb);
            }

            if (info.Indexers[0].ParameterType != typeof(Int32))
            {
                if (config.SkipInvalidIndexers)
                    return false;

                throw new Exception("Cannot compare objects with a non integer indexer for object " + breadCrumb);
            }

#if !NETSTANDARD
            var type = info.ReflectedType;
#else
            var type = info.DeclaringType;
#endif
            if (type == null)
            {
                if (config.SkipInvalidIndexers)
                    return false;

                throw new Exception("Cannot compare objects with a null indexer for object " + breadCrumb);
            }

            if (type.GetProperty("Count") == null
                || type.GetProperty("Count").PropertyType != typeof(Int32))
            {
                if (config.SkipInvalidIndexers)
                    return false;

                throw new Exception("Indexer must have a corresponding Count property that is an integer for object " + breadCrumb);
            }

            return true;
        }
    }
}
