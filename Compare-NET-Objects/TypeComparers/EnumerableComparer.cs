using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Logic to compare two LINQ enumerators
    /// </summary>
    public class EnumerableComparer :BaseTypeComparer
    {
        private readonly ListComparer _compareIList;

        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public EnumerableComparer(RootComparer rootComparer) : base(rootComparer)
        {
            _compareIList = new ListComparer(rootComparer);
        }

        /// <summary>
        /// Returns true if either object is of type LINQ Enumerator
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            if (type1 == null || type2 == null)
                return false;

            return TypeHelper.IsEnumerable(type1) || TypeHelper.IsEnumerable(type2);
        }

        /// <summary>
        /// Compare two objects that implement LINQ Enumerator
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            Type t1 = parms.Object1.GetType();
            Type t2 = parms.Object2.GetType();

            var l1 = TypeHelper.IsEnumerable(t1) ? ConvertEnumerableToList(parms.Object1) : parms.Object1;
            var l2 = TypeHelper.IsEnumerable(t2) ? ConvertEnumerableToList(parms.Object2) : parms.Object2;

            parms.Object1 = l1;
            parms.Object2 = l2;

            _compareIList.CompareType(parms);
        }

        private object ConvertEnumerableToList(object source)
        {
            var type = source.GetType();

            if (type.IsArray)
                return source;

            Type enumerableGenArg = null;
            foreach (var inter in type.GetInterfaces())
            {
#if PORTABLE
                if (inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    enumerableGenArg = inter.GetGenericArguments()[0];
                    break;
                }
#else
                if (inter.GetTypeInfo().IsGenericType && inter.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    enumerableGenArg = inter.GetGenericArguments()[0];
                    break;
                }
#endif
            }

            if (enumerableGenArg == null)
            {
#if PORTABLE || DNCORE
                throw new Exception("Cannot get IEnumerable definition");
#else
                throw new ApplicationException("Cannot get IEnumerable definition");
#endif

            }

            MethodInfo toList = typeof(Enumerable).GetMethod("ToList");
            MethodInfo constructedToList = toList.MakeGenericMethod(enumerableGenArg);
            object resultList = constructedToList.Invoke(null, new[] { source });

            return resultList;
        }
    }
}
