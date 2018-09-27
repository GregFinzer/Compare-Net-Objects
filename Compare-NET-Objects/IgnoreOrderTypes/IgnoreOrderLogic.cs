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
        private readonly List<string> _alreadyCompared = new List<string>();


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
            if (countsDifferent)
            {
                CompareOutOfOrder(parms, false);

                if (!parms.Result.ExceededDifferences)
                {
                    CompareOutOfOrder(parms, true);
                }
            }
            else
            {
                bool comparedInOrder = CompareInOrder(parms);

                if (!comparedInOrder && !parms.Result.ExceededDifferences)
                {
                    CompareOutOfOrder(parms, false);

                    if (!parms.Result.ExceededDifferences)
                    {
                        CompareOutOfOrder(parms, true);
                    }
                }
            }
        }


        private bool CompareInOrder(CompareParms parms)
        {
            IEnumerator enumerator1 = ((IEnumerable) parms.Object1).GetEnumerator();
            IEnumerator enumerator2 = ((IEnumerable) parms.Object2).GetEnumerator();
            List<string> matchingSpec;

            while (enumerator1.MoveNext())
            {
                if (enumerator1.Current == null)
                {
                    return false;
                }

                Type enumerator1Type = enumerator1.Current.GetType();

                if (parms.Config.ClassTypesToIgnore.Contains(enumerator1Type))
                {
                    return false;
                }

                matchingSpec = GetMatchingSpec(parms.Result, enumerator1Type);

                string matchIndex1 = GetMatchIndex(parms.Result, matchingSpec, enumerator1.Current);

                if (enumerator2.MoveNext())
                {
                    if (_alreadyCompared.Contains(matchIndex1))
                    {
                        continue;
                    }

                    if (enumerator2.Current == null)
                    {
                        return false;
                    }

                    Type enumerator2Type = enumerator2.Current.GetType();

                    if (parms.Config.ClassTypesToIgnore.Contains(enumerator2Type))
                    {
                        return false;
                    }

                    matchingSpec = GetMatchingSpec(parms.Result, enumerator2Type);
                    string matchIndex2 = GetMatchIndex(parms.Result, matchingSpec, enumerator2.Current);

                    if (matchIndex1 == matchIndex2)
                    {
                        string currentBreadCrumb = string.Format("{0}[{1}]", parms.BreadCrumb, matchIndex1);

                        CompareParms childParms = new CompareParms
                        {
                            Result = parms.Result,
                            Config = parms.Config,
                            ParentObject1 = enumerator1.Current,
                            ParentObject2 = enumerator2.Current,
                            Object1 = enumerator1.Current,
                            Object2 = enumerator2.Current,
                            BreadCrumb = currentBreadCrumb
                        };

                        _rootComparer.Compare(childParms);
                        _alreadyCompared.Add(matchIndex1);
                    }
                    else
                    {
                        return false;
                    }
                }

                if (parms.Result.ExceededDifferences)
                    break;
            }

            return true;
        }

        private void CompareOutOfOrder(CompareParms parms,
            bool reverseCompare)
        {
            IEnumerator enumerator1;
            List<string> matchingSpec;

            if (!reverseCompare)
            {
                enumerator1 = ((IEnumerable) parms.Object1).GetEnumerator();
            }
            else
            {
                enumerator1 = ((IEnumerable)parms.Object2).GetEnumerator();
            }

            while (enumerator1.MoveNext())
            {
                if (enumerator1.Current == null)
                {
                    continue;
                }

                Type enumerator1Type = enumerator1.Current.GetType();

                if (parms.Config.ClassTypesToIgnore.Contains(enumerator1Type))
                {
                    continue;
                }

                matchingSpec = GetMatchingSpec(parms.Result, enumerator1Type);

                string matchIndex1 = GetMatchIndex(parms.Result, matchingSpec, enumerator1.Current);

                if (_alreadyCompared.Contains(matchIndex1))
                    continue;

                string currentBreadCrumb = string.Format("{0}[{1}]", parms.BreadCrumb, matchIndex1);
                IEnumerator enumerator2;

                if (!reverseCompare)
                {
                    enumerator2 = ((IEnumerable) parms.Object2).GetEnumerator();
                }
                else
                {
                    enumerator2 = ((IEnumerable)parms.Object1).GetEnumerator();
                }

                bool found = false;

                while (enumerator2.MoveNext())
                {
                    if (enumerator2.Current == null)
                    {
                        continue;
                    }

                    Type enumerator2Type = enumerator2.Current.GetType();

                    if (parms.Config.ClassTypesToIgnore.Contains(enumerator2Type))
                    {
                        continue;
                    }

                    matchingSpec = GetMatchingSpec(parms.Result, enumerator2Type);
                    string matchIndex2 = GetMatchIndex(parms.Result, matchingSpec, enumerator2.Current);

                    if (matchIndex1 == matchIndex2)
                    {
                        CompareParms childParms = new CompareParms
                        {
                            Result = parms.Result,
                            Config = parms.Config,
                            ParentObject1 = parms.Object1,
                            ParentObject2 = parms.Object2,
                            Object1 = enumerator1.Current,
                            Object2 = enumerator2.Current,
                            BreadCrumb = currentBreadCrumb
                        };

                        _rootComparer.Compare(childParms);
                        _alreadyCompared.Add(matchIndex1);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Difference difference = new Difference
                    {
                        ParentObject1 = parms.ParentObject1,
                        ParentObject2 = parms.ParentObject2,
                        PropertyName = currentBreadCrumb,
                        Object1Value = reverseCompare ? "(null)" : NiceString(enumerator1.Current),
                        Object2Value = reverseCompare ? NiceString(enumerator1.Current) : "(null)",
                        ChildPropertyName = "Item",
						Object1 = reverseCompare ? null : enumerator1.Current,
						Object2 = reverseCompare ? enumerator1.Current : null
                    };

                    AddDifference(parms.Result, difference);                    
                }
                if (parms.Result.ExceededDifferences)
                    return;

            }

         
        }



        private string GetMatchIndex(ComparisonResult result, List<string> spec, object currentObject)
        {
            List<PropertyInfo> properties = Cache.GetPropertyInfo(result.Config, currentObject.GetType()).ToList();
            StringBuilder sb = new StringBuilder();

            foreach (var item in spec)
            {
                var info = properties.FirstOrDefault(o => o.Name == item);

                if (info == null)
                {
                    throw new Exception(string.Format("Invalid CollectionMatchingSpec.  No such property {0} for type {1} ",
                        item,
                        currentObject.GetType().Name));
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
                    sb.AppendFormat("{0}:(null),",item);
                }
                else
                {
                    string formatString = string.Format("{{0}}:{{1{0}}},", info.PropertyType.Name == "Decimal" ? ":N" : string.Empty);
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
                case "Single":
                    return ((double)o).ToString("G");
                default:
                    return o.ToString();
            }
#else
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Decimal:
                    return ((decimal)o).ToString("G29");
                case TypeCode.Double:
                case TypeCode.Single:
                    return ((double)o).ToString("G");
                default:
                    return o.ToString();
            }
#endif
        }

        private List<string> GetMatchingSpec(ComparisonResult result,Type type)
        {
            //The user defined a key for the order
            var matchingBasePresent = result.Config.CollectionMatchingSpec.Keys.FirstOrDefault(k => k.IsAssignableFrom(type));
            if (matchingBasePresent != null)
            {
                return result.Config.CollectionMatchingSpec.First(p => p.Key == matchingBasePresent).Value.ToList();
            }

            //Make a key out of primative types, date, decimal, string, guid, and enum of the class
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
