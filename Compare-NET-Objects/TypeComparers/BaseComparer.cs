﻿using System;
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
                Debug.WriteLine(sb.ToString());

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
                ParentObject1 = new WeakReference(parameters.ParentObject1),
                ParentObject2 = new WeakReference(parameters.ParentObject2),
                PropertyName = parameters.BreadCrumb,
                Object1Value = NiceString(parameters.Object1),
                Object2Value = NiceString(parameters.Object2),
                Object1 = new WeakReference(parameters.Object1),
                Object2 = new WeakReference(parameters.Object2),               
            };
            parameters.Result.NeedsApproval = parameters.Result.NeedsApproval ? true : parameters.NeedsApproval;
            difference.ApprovalDifference = parameters.NeedsApproval;
            difference.DisplayName = parameters.DisplayName;
            AddDifference(parameters,difference);
        }

        /// <summary>
        /// Add a difference to the result
        /// </summary>
        /// <param name="difference">The difference to add to the result</param>
        /// <param name="parms">The comparison parameters</param>
        protected void AddDifference(CompareParms parms, Difference difference)
        {
            if (parms.Result == null)
                throw new ArgumentNullException("result");

            if (difference == null)
                throw new ArgumentNullException("difference");
            difference.ApprovalDifference = parms.NeedsApproval;
            difference.DisplayName = parms.DisplayName;
            parms.Result.NeedsApproval = parms.Result.NeedsApproval ? true : parms.NeedsApproval;
            difference.ActualName = parms.Result.Config.ActualName;
            difference.ExpectedName = parms.Result.Config.ExpectedName;

            difference.Object1TypeName = difference.Object1 != null && difference.Object1.Target != null 
                ? difference.Object1.Target.GetType().Name : "null";

            difference.Object2TypeName = difference.Object2 != null && difference.Object2.Target != null 
                ? difference.Object2.Target.GetType().Name : "null";

            parms.Result.Differences.Add(difference);
            parms.Result.Config.DifferenceCallback(difference);
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

                #if !PORTABLE
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
