using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

#if !PORTABLE
using System.Data;
using System.Drawing;
#endif

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Methods for detecting 
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Returns true if it is a byte array
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsByteArray(Type type)
        {
            if (!IsIList(type))
                return false;

            if (type.UnderlyingSystemType.FullName.Contains("System.Byte"))
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if the type can have children
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CanHaveChildren(Type type)
        {
            if (type == null)
                return false;

            return !IsSimpleType(type)
                && (IsClass(type)
                    || IsInterface(type)
                    || IsArray(type)
                    || IsIDictionary(type)
                    || IsIList(type)
                    || IsStruct(type)
                    || IsHashSet(type)
                    );
        }

        /// <summary>
        /// True if the type is an array
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsArray(Type type)
        {
            if (type == null)
                return false;

            return type.IsArray;
        }



        /// <summary>
        /// Returns true if it is a struct
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStruct(Type type)
        {
            if (type == null)
                return false;

            return type.IsValueType && !IsSimpleType(type);
        }

        /// <summary>
        /// Returns true if the type is a timespan
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTimespan(Type type)
        {
            if (type == null)
                return false;

            return type == typeof(TimeSpan);
        }

        /// <summary>
        /// Return true if the type is a class
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsClass(Type type)
        {
            if (type == null)
                return false;

            return type.IsClass;
        }

        /// <summary>
        /// Return true if the type is an interface
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInterface(Type type)
        {
            if (type == null)
                return false;

            return type.IsInterface;
        }

        /// <summary>
        /// Return true if the type is a URI
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsUri(Type type)
        {
            if (type == null)
                return false;

            return (typeof(Uri).IsAssignableFrom(type));
        }

        /// <summary>
        /// Return true if the type is a pointer
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPointer(Type type)
        {
            if (type == null)
                return false;

            return type == typeof(IntPtr) || type == typeof(UIntPtr);
        }

        /// <summary>
        /// Return true if the type is an enum
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnum(Type type)
        {
            if (type == null)
                return false;

            return type.IsEnum;
        }

        /// <summary>
        /// Return true if the type is a dictionary
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsIDictionary(Type type)
        {
            if (type == null)
                return false;

            return (typeof(IDictionary).IsAssignableFrom(type));
        }

        /// <summary>
        /// Return true if the type is a hashset
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsHashSet(Type type)
        {
            if (type == null)
                return false;

            return type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(HashSet<>);
        }

        /// <summary>
        /// Return true if the type is a List
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsIList(Type type)
        {
            if (type == null)
                return false;

            return (typeof(IList).IsAssignableFrom(type));
        }

        /// <summary>
        /// Return true if the type is an Enumerable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(Type type)
        {
            if (type == null)
                return false;

            return type.ReflectedType != null
              && type.ReflectedType == typeof(Enumerable);
        }

        /// <summary>
        /// Return true if the type is a Double
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDouble(Type type)
        {
            if (type == null)
                return false;

            return type == typeof(Double);
        }

        /// <summary>
        /// Return true if the type is a DateTime
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDateTime(Type type)
        {
            if (type == null)
                return false;

            return type == typeof (DateTime);
        }

        /// <summary>
        /// Return true if the type is a DateTimeOffset
        /// </summary>
        /// <param name="type"></param>
        public static bool IsDateTimeOffset(Type type)
        {
            if (type == null)
                return false;

            return type == typeof(DateTimeOffset);
        }

        /// <summary>
        /// Return true if the type is a string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsString(Type type)
        {
            if (type == null)
                return false;

            return type == typeof(string);
        }

        /// <summary>
        /// Return true if the type is a primitive type, date, decimal, string, or GUID
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSimpleType(Type type)
        {
            if (type == null)
                return false;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(DateTime)
                   || type == typeof(decimal)
                   || type == typeof(string)
                   || type == typeof(Guid);

        }

        /// <summary>
        /// Returns true if the Type is a Runtime type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsRuntimeType(Type type)
        {
            if (type == null)
                return false;

            return (typeof(Type).IsAssignableFrom(type));
        }

#if !PORTABLE
        /// <summary>
        /// Returns true if the type is an IPEndPoint
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsIpEndPoint(Type type)
        {
            if (type == null)
                return false;

            return type == typeof(IPEndPoint);
        }

        /// <summary>
        /// Returns true if the type is a dataset
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDataset(Type type)
        {
            if (type == null)
                return false;

            return type == typeof(DataSet);
        }

        /// <summary>
        /// Returns true if the type is a data table
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDataTable(Type type)
        {
            if (type == null)
                return false;

            return type == typeof(DataTable);
        }

        /// <summary>
        /// Returns true if the type is a data row
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDataRow(Type type)
        {
            if (type == null)
                return false;

            return type == typeof(DataRow);
        }

        /// <summary>
        /// Returns true if the type is a font
        /// </summary>
        /// <param name="type">The type1.</param>
        public static bool IsFont(Type type)
        {
            if (type == null)
                return false;

            return type == typeof(Font);
        }
#endif



    }
}
