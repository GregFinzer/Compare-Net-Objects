//Provided for backward compatibility from 1.7.4

#region Includes
using System;
using System.Collections.Generic;
#endregion

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Obsolete Use CompareLogic instead
    /// </summary>
    [Obsolete("Use CompareLogic instead", true)]
    public class CompareObjects
    {
        #region Class Variables

        private readonly CompareLogic _logic;
        private ComparisonResult _result;
        #endregion

        #region Constructor

        /// <summary>
        /// Obsolete Use CompareLogic instead
        /// </summary>
        [Obsolete("Use CompareLogic instead", true)]
        public CompareObjects()
        {
            _logic = new CompareLogic();
            _result = new ComparisonResult(_logic.Config);
        }

#if !PORTABLE && !NEWPCL
        /// <summary>
        /// Obsolete Use CompareLogic instead
        /// </summary>
        [Obsolete("Use CompareLogic instead", true)]
        public CompareObjects(bool useAppConfigSettings)
        {
            _logic = new CompareLogic(useAppConfigSettings);
            _result = new ComparisonResult(_logic.Config);
        }
#endif

        #endregion

        #region Properties


#if !PORTABLE && !NEWPCL
        /// <summary>
        /// Obsolete Use the ComparisonResult.ElapsedMilliseconds returned from CompareLogic.Compare
        /// </summary>
        [Obsolete("Use the ComparisonResult.ElapsedMilliseconds returned from CompareLogic.Compare", true)]
        public long ElapsedMilliseconds
        {
            get { return _result.ElapsedMilliseconds; }
        }
#endif

        /// <summary>
        /// Obsolete Use CompareLogic.Config.ShowBreadcrumb instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.ShowBreadcrumb instead", true)]
        public bool ShowBreadcrumb
        {
            get { return _logic.Config.ShowBreadcrumb; }
            set { _logic.Config.ShowBreadcrumb = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.MembersToIgnore for members or CompareLogic.Config.ClassTypesToIgnore instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.MembersToIgnore for members or CompareLogic.Config.ClassTypesToIgnore instead", true)]
        public List<string> ElementsToIgnore
        {
            get { return _logic.Config.MembersToIgnore; }
            set { _logic.Config.MembersToIgnore = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.MembersToInclude or CompareLogic.Config.ClassTypesToInclude instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.MembersToInclude or CompareLogic.Config.ClassTypesToInclude instead", true)]
        public List<string> ElementsToInclude
        {
            get { return _logic.Config.MembersToInclude; }
            set { _logic.Config.MembersToInclude = value; }
        }

        //Security restriction in Silverlight prevents getting private properties and fields
#if !PORTABLE && !NEWPCL

        /// <summary>
        /// Obsolete Use CompareLogic.Config.ComparePrivateProperties instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.ComparePrivateProperties instead", true)]
        public bool ComparePrivateProperties
        {
            get { return _logic.Config.ComparePrivateProperties; }
            set { _logic.Config.ComparePrivateProperties = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.ComparePrivateFields instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.ComparePrivateFields instead", true)]
        public bool ComparePrivateFields
        {
            get { return _logic.Config.ComparePrivateFields; }
            set { _logic.Config.ComparePrivateFields = value; }
        }
#endif

        /// <summary>
        /// Obsolete Use CompareLogic.Config.CompareStaticProperties instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.CompareStaticProperties instead", true)]
        public bool CompareStaticProperties
        {
            get { return _logic.Config.CompareStaticProperties; }
            set { _logic.Config.CompareStaticProperties = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.CompareStaticFields instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.CompareStaticFields instead", true)]
        public bool CompareStaticFields
        {
            get { return _logic.Config.CompareStaticFields; }
            set { _logic.Config.CompareStaticFields = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.CompareChildren instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.CompareChildren instead", true)]
        public bool CompareChildren
        {
            get { return _logic.Config.CompareChildren; }
            set { _logic.Config.CompareChildren = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.CompareReadOnly instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.CompareReadOnly instead", true)]
        public bool CompareReadOnly
        {
            get { return _logic.Config.CompareReadOnly; }
            set { _logic.Config.CompareReadOnly = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.CompareFields instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.CompareFields instead", true)]
        public bool CompareFields
        {
            get { return _logic.Config.CompareFields; }
            set { _logic.Config.CompareFields = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.IgnoreCollectionOrder instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.IgnoreCollectionOrder instead", true)]
        public bool IgnoreCollectionOrder
        {
            get { return _logic.Config.IgnoreCollectionOrder; }
            set { _logic.Config.IgnoreCollectionOrder = value; }
        }


        /// <summary>
        /// Obsolete Use CompareLogic.Config.CompareProperties instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.CompareProperties instead", true)]
        public bool CompareProperties
        {
            get { return _logic.Config.CompareProperties; }
            set { _logic.Config.CompareProperties = value; }
        }


        /// <summary>
        /// Obsolete Use CompareLogic.Config.MaxDifferences instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.MaxDifferences instead", true)]
        public int MaxDifferences
        {
            get { return _logic.Config.MaxDifferences; }
            set { _logic.Config.MaxDifferences = value; }
        }

        /// <summary>
        /// Obsolete Use the ComparisonResult.Differences returned from CompareLogic.Compare
        /// </summary>
        [Obsolete("Use the ComparisonResult.Differences returned from CompareLogic.Compare", true)]
        public List<Difference> Differences
        {
            get { return _result.Differences; }
            set { _result.Differences = value; }
        }

        /// <summary>
        /// Obsolete Use the ComparisonResult.DifferencesString returned from CompareLogic.Compare
        /// </summary>
        [Obsolete("Use the ComparisonResult.DifferencesString returned from CompareLogic.Compare", true)]
        public string DifferencesString
        {
            get { return _result.DifferencesString; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.AutoClearCache instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.AutoClearCache instead", true)]
        public bool AutoClearCache
        {
            get { return _logic.Config.AutoClearCache; }
            set { _logic.Config.AutoClearCache = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.Caching instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.Caching instead", true)]
        public bool Caching
        {
            get { return _logic.Config.Caching; }
            set { _logic.Config.Caching = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.AttributesToIgnore instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.AttributesToIgnore instead", true)]
        public List<Type> AttributesToIgnore
        {
            get { return _logic.Config.AttributesToIgnore; }
            set { _logic.Config.AttributesToIgnore = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.IgnoreObjectTypes instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.IgnoreObjectTypes instead", true)]
        public bool IgnoreObjectTypes
        {
            get { return _logic.Config.IgnoreObjectTypes; }
            set { _logic.Config.IgnoreObjectTypes = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.CustomComparers instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.CustomComparers", true)]
        public Func<Type, bool> IsUseCustomTypeComparer { get; set; }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.CustomComparers instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.CustomComparers", true)]
        public Action<CompareObjects, object, object, string> CustomComparer { get; set; }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.ExpectedName instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.ExpectedName instead", true)]
        public string ExpectedName
        {
            get { return _logic.Config.ExpectedName; }
            set { _logic.Config.ExpectedName = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.ActualName instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.ActualName instead", true)]
        public string ActualName
        {
            get { return _logic.Config.ActualName; }
            set { _logic.Config.ActualName = value; }
        }

        /// <summary>
        /// Obsolete Use CompareLogic.Config.DifferenceCallback instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.DifferenceCallback instead", true)]
        public Action<Difference> DifferenceCallback
        {
            get { return _logic.Config.DifferenceCallback; }
            set { _logic.Config.DifferenceCallback = value; }
        }


        /// <summary>
        /// Obsolete Use CompareLogic.Config.CollectionMatchingSpec instead
        /// </summary>
        [Obsolete("Use CompareLogic.Config.CollectionMatchingSpec instead", true)]
        public Dictionary<Type, IEnumerable<string>> CollectionMatchingSpec
        {
            get { return _logic.Config.CollectionMatchingSpec; }
            set { _logic.Config.CollectionMatchingSpec = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Obsolete Use CompareLogic.Compare instead
        /// </summary>
        [Obsolete("Use CompareLogic.Compare instead", true)]
        public bool Compare(object object1, object object2)
        {
            _result = _logic.Compare(object1, object2);

            return _result.AreEqual;
        }

        /// <summary>
        /// Obsolete Use CompareLogic.ClearCache instead
        /// </summary>
        [Obsolete("Use CompareLogic.ClearCache instead", true)]
        public void ClearCache()
        {
            _logic.ClearCache();
        }

        #endregion
    }
}
