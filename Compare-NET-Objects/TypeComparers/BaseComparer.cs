using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Common functionality for all Comparers
    /// </summary>
    public class BaseComparer
    {

        /// <summary>
        /// Add a breadcrumb to an existing breadcrumb
        /// </summary>
        /// <param name="config">Comparison configuration</param>
        /// <param name="existing">The existing breadcrumb</param>
        /// <param name="name">The field or property name</param>
        /// <returns>The new breadcrumb</returns>
        protected string AddBreadCrumb(ComparisonConfig config, string existing, string name)
        {
            return AddBreadCrumb(config, existing, name, string.Empty, null);
        }

        /// <summary>
        /// Add a breadcrumb to an existing breadcrumb
        /// </summary>
        /// <param name="config">The comparison configuration</param>
        /// <param name="existing">The existing breadcrumb</param>
        /// <param name="name">The property or field name</param>
        /// <param name="extra">Extra information to output after the name</param>
        /// <param name="index">The index for an array, list, or row</param>
        /// <returns>The new breadcrumb</returns>
        protected string AddBreadCrumb(ComparisonConfig config, string existing, string name, string extra, int index)
        {
            return AddBreadCrumb(config, existing, name, extra, index >= 0 ? index.ToString(CultureInfo.InvariantCulture) : null);
        }

        /// <summary>
        /// Add a breadcrumb to an existing breadcrumb
        /// </summary>
        /// <param name="config">Comparison configuration</param>
        /// <param name="existing">The existing breadcrumb</param>
        /// <param name="name">The field or property name</param>
        /// <param name="extra">Extra information to append after the name</param>
        /// <param name="index">The index if it is an array, list, row etc.</param>
        /// <returns>The new breadcrumb</returns>
        protected string AddBreadCrumb(ComparisonConfig config, string existing, string name, string extra, string index)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            bool useIndex = !String.IsNullOrEmpty(index);

            if (name == null)
                throw new ArgumentNullException("name");
            
            bool useName = name.Length > 0;
            StringBuilder sb = new StringBuilder();

            sb.Append(existing);

            if (useName)
            {
                //Do not put a period at the beginning
                if (sb.Length > 0)
                {
                    sb.AppendFormat(".");
                }
                
                sb.Append(name);
            }

            sb.Append(extra);

            if (useIndex)
            {
                // ReSharper disable RedundantAssignment
                int result = -1;
                // ReSharper restore RedundantAssignment
                sb.AppendFormat(Int32.TryParse(index, out result) ? "[{0}]" : "[\"{0}\"]", index);
            }

            if (config.ShowBreadcrumb)
            {
#if (DEBUG && !PORTABLE) || DNCORE
                Console.WriteLine(sb.ToString());
#endif

#if !PORTABLE && !DNCORE && !DEBUG
                Trace.WriteLine(sb.ToString());
#endif
            }

            return sb.ToString();
        }

        /// <summary>
        /// Add a difference for the current parameters
        /// </summary>
        /// <param name="parameters"></param>
        protected void AddDifference(CompareParms parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            Difference difference = new Difference
            {
                ParentObject1 = parameters.ParentObject1,
                ParentObject2 = parameters.ParentObject2,
                PropertyName = parameters.BreadCrumb,
                Object1Value = NiceString(parameters.Object1),
                Object2Value = NiceString(parameters.Object2),
                Object1 = parameters.Object1,
                Object2 = parameters.Object2
            };

            AddDifference(parameters.Result,difference);
        }

        /// <summary>
        /// Add a difference to the result
        /// </summary>
        /// <param name="difference">The difference to add to the result</param>
        /// <param name="result">The comparison result</param>
        protected void AddDifference(ComparisonResult result, Difference difference)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            if (difference == null)
                throw new ArgumentNullException("difference");

            difference.ActualName = result.Config.ActualName;
            difference.ExpectedName = result.Config.ExpectedName;

            difference.Object1TypeName = difference.Object1 != null && difference.Object1 != null 
                ? difference.Object1.GetType().Name : "null";

            difference.Object2TypeName = difference.Object2 != null && difference.Object2 != null 
                ? difference.Object2.GetType().Name : "null";    

            result.Differences.Add(difference);
            result.Config.DifferenceCallback(difference);
        }



        /// <summary>
        /// Convert an object to a nicely formatted string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string NiceString(object value)
        {
            try
            {
                if (value == null)
                    return "(null)";

                #if !PORTABLE && !DNCORE
                    if (value == DBNull.Value)
                        return "System.DBNull.Value";
                #endif

                return value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }




    }
}
