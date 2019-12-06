using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KellermanSoftware.CompareNetObjects.TypeComparers;

namespace KellermanSoftware.CompareNetObjects.IgnoreOrderTypes
{
    /// <summary>
    /// Logic for comparing lists that are out of order based on a key
    /// </summary>
    public class IgnoreOrderLogic : BaseComparer
    {
        private readonly RootComparer _rootComparer;
        private readonly Dictionary<string, bool> _alreadyCompared = new Dictionary<string, bool>();

        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreOrderLogic"/> class.
        /// </summary>
        /// <param name="rootComparer">The root comparer.</param>
        public IgnoreOrderLogic(RootComparer rootComparer)
        {
            _rootComparer = rootComparer;
        }

        /// <summary>
        /// Compares the enumerators and ignores the order
        /// </summary>
        public void CompareEnumeratorIgnoreOrder(CompareParms parms, bool countsDifferent)
        {
            CompareOutOfOrder(parms, false);
        }

        private class InstanceCounter
        {
            public InstanceCounter(object value, int counter)
            {
                Counter = counter;
                ObjectValue = value;
            }

            public int Counter;
            public readonly object ObjectValue;
        }

        private void CompareOutOfOrder(CompareParms parms, bool reverseCompare)
        {
            IEnumerator enumerator1;
            IEnumerator enumerator2;
            List<string> matchingSpec1 = null;
            List<string> matchingSpec2 = null;

            var list1 = new Dictionary<string, InstanceCounter>();
            var list2 = new Dictionary<string, InstanceCounter>();
            Type dataType1 = null;
            Type dataType2 = null;

            if (!reverseCompare)
            {
                enumerator1 = ((IEnumerable)parms.Object1).GetEnumerator();
                enumerator2 = ((IEnumerable)parms.Object2).GetEnumerator();
            }
            else
            {
                enumerator1 = ((IEnumerable)parms.Object2).GetEnumerator();
                enumerator2 = ((IEnumerable)parms.Object1).GetEnumerator();
            }

            while (enumerator1.MoveNext())
            {
                var data = enumerator1.Current;
                if (data != null
                    && parms.Config.ClassTypesToIgnore.Contains(data.GetType()))
                {
                    continue;
                }

                dataType1 = dataType1 ?? data?.GetType();
                matchingSpec1 = matchingSpec1 ?? GetMatchingSpec(parms.Result, dataType1);
                var matchingIndex = GetMatchIndex(parms.Result, matchingSpec1, data);
                if (!list1.ContainsKey(matchingIndex))
                    list1.Add(matchingIndex, new InstanceCounter(data, 1));
                else
                    list1[matchingIndex].Counter++;
            }

            while (enumerator2.MoveNext())
            {
                var data = enumerator2.Current;
                if (data != null
                    && parms.Config.ClassTypesToIgnore.Contains(data.GetType()))
                {
                    continue;
                }

                dataType2 = dataType2 ?? data?.GetType();
                matchingSpec2 = matchingSpec2 ?? GetMatchingSpec(parms.Result, dataType2);
                var matchingIndex = GetMatchIndex(parms.Result, matchingSpec2, data);
                if (!list2.ContainsKey(matchingIndex))
                    list2.Add(matchingIndex, new InstanceCounter(data, 1));
                else
                    list2[matchingIndex].Counter++;
            }

            while (list1.Count > 0)
            {
                KeyValuePair<string, InstanceCounter> item1 = list1.First();

                string currentBreadCrumb = $"{parms.BreadCrumb}[{item1.Key}]";

                bool bothObjectValuesNull = item1.Value.ObjectValue == null
                                            && list2.ContainsKey(item1.Key)
                                            && list2[item1.Key].ObjectValue == null;

                object item2Value = list2.ContainsKey(item1.Key) ? list2[item1.Key].ObjectValue : null;

                if (bothObjectValuesNull)
                {
                    if (--list2[item1.Key].Counter == 0)
                        list2.Remove(item1.Key); // Matched, so remove from dictionary so we don't double-dip on it
                }
                else if (item2Value != null)
                {
                    CompareParms childParams = new CompareParms
                    {
                        Result = parms.Result,
                        Config = parms.Config,
                        ParentObject1 = parms.Object1,
                        ParentObject2 = parms.Object2,
                        Object1 = item1.Value.ObjectValue,
                        Object2 = item2Value,
                        BreadCrumb = currentBreadCrumb
                    };

                    _rootComparer.Compare(childParams);
                    if (--list2[item1.Key].Counter == 0)
                        list2.Remove(item1.Key); // Matched, so remove from dictionary so we don't double-dip on it
                }
                else
                {
                    Difference difference = new Difference
                    {
                        ParentObject1 = parms.ParentObject1,
                        ParentObject2 = parms.ParentObject2,
                        PropertyName = currentBreadCrumb,
                        Object1Value = reverseCompare ? "(null)" : NiceString(item1.Value.ObjectValue),
                        Object2Value = reverseCompare ? NiceString(item1.Value.ObjectValue) : "(null)",
                        ChildPropertyName = "Item",
                        Object1 = reverseCompare ? null : item1.Value.ObjectValue,
                        Object2 = reverseCompare ? item1.Value.ObjectValue : null
                    };

                    AddDifference(parms.Result, difference);
                }

                //_alreadyCompared.Add(item1.Key, true);
                if (parms.Result.ExceededDifferences)
                    return;

                if (--list1[item1.Key].Counter == 0)
                    list1.Remove(item1.Key);
            }

            while (list2.Count > 0)
            {
                var item2 = list2.First();
                Difference difference = new Difference
                {
                    ParentObject1 = parms.ParentObject1,
                    ParentObject2 = parms.ParentObject2,
                    PropertyName = $"{parms.BreadCrumb}[{item2.Key}]",
                    Object1Value = reverseCompare ? NiceString(item2.Value.ObjectValue) : "(null)",
                    Object2Value = reverseCompare ? "(null)" : NiceString(item2.Value.ObjectValue),
                    ChildPropertyName = "Item",
                    Object1 = reverseCompare ? item2.Value.ObjectValue : null,
                    Object2 = reverseCompare ? null : item2.Value.ObjectValue
                };
                AddDifference(parms.Result, difference);
                list2.Remove(item2.Key);
                if (parms.Result.ExceededDifferences)
                    return;
            }
        }

        private string GetMatchIndex(ComparisonResult result, List<string> spec, object currentObject)
        {
            if (currentObject == null)
                return "(null)";

            List<PropertyInfo> properties = Cache.GetPropertyInfo(result.Config, currentObject.GetType()).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var item in spec)
            {
                var info = properties.FirstOrDefault(o => o.Name == item);

                if (info == null)
                {
                    throw new Exception(
                        $"Invalid CollectionMatchingSpec.  No such property {item} for type {currentObject.GetType().Name} ");
                }

                var propertyValue = info.GetValue(currentObject, null);

                if (result.Config.TreatStringEmptyAndNullTheSame && info.PropertyType == typeof(string) && propertyValue == null)
                {
                    propertyValue = string.Empty;
                }
                else if (propertyValue != null && result.Config.CaseSensitive == false && info.PropertyType == typeof(string))
                {
                    propertyValue = ((string)propertyValue).ToLowerInvariant();
                }

                if (propertyValue == null)
                {
                    sb.AppendFormat("{0}:(null),", item);
                }
                else
                {
                    var decimals = BitConverter.GetBytes(decimal.GetBits(result.Config.DecimalPrecision)[3])[2];
                    var formatString = $"{{0}}:{{1{(TypeHelper.IsDecimal(propertyValue) ? $":N{decimals}" : string.Empty)}}},";

                    sb.Append(string.Format(formatString, item, propertyValue));
                }
            }

            if (sb.Length == 0)
                sb.Append(RespectNumberToString(currentObject));

            return sb.ToString().TrimEnd(',');
        }

        private static string RespectNumberToString(object o)
        {

#if NETSTANDARD
            string typeString = o.GetType().Name;

            switch (typeString)
            {
                case "Decimal":
                    return ((decimal)o).ToString("G29");
                case "Double":
                    return ((double)o).ToString("G");
                case "Single":
                    return ((float)o).ToString("G");
                default:
                    return o.ToString();
            }
#else
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Decimal:
                    return ((decimal)o).ToString("G29");
                case TypeCode.Double:
                    return ((double)o).ToString("G");
                case TypeCode.Single:
                    return ((float)o).ToString("G");
                default:
                    return o.ToString();
            }
#endif
        }

        private List<string> GetMatchingSpec(ComparisonResult result, Type type)
        {
            if (type == null)
                return new List<string> { "(null)" };

            //The user defined a key for the order
            var matchingBasePresent = result.Config.CollectionMatchingSpec.Keys.FirstOrDefault(k => k.IsAssignableFrom(type));
            if (matchingBasePresent != null)
            {
                return result.Config.CollectionMatchingSpec.First(p => p.Key == matchingBasePresent).Value.ToList();
            }

            //Make a key out of primitive types, date, decimal, string, guid, and enum of the class
            List<string> list = Cache.GetPropertyInfo(result.Config, type)
                .Where(o => o.CanWrite && (TypeHelper.IsSimpleType(o.PropertyType) || TypeHelper.IsEnum(o.PropertyType)))
                .Select(o => o.Name).ToList();

            //Remove members to ignore in the key
            foreach (var member in result.Config.MembersToIgnore)
            {
                list.Remove(member);
            }

            return list;
        }
    }
}