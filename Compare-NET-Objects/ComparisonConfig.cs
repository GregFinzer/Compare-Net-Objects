using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using System.Linq;
#if !NETSTANDARD
using System.Runtime.Serialization;
#endif

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Configuration
    /// </summary>
#if !NETSTANDARD
    [DataContract]
#endif
    public class ComparisonConfig
    {
#region Class Variables
        private Action<Difference> _differenceCallback;
        private int _maxStructDepth;

        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ComparisonConfig()
        {
            Reset();
        }
        #endregion

        #region Properties

        /// <summary>
        /// When comparing strings or StringBuilder types, perform a case sensitive comparison.  The default is true.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool CaseSensitive { get; set; }

        /// <summary>
        /// Ignore exceptions when objects are disposed
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool IgnoreObjectDisposedException { get; set; }

        /// <summary>
        /// Ignore millisecond differences between DateTime values or DateTimeOffset values.  The default is 0 (any time difference will be shown).
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public int MaxMillisecondsDateDifference { get; set; }

        /// <summary>
        /// When comparing DateTimeOffsets, offsets will be compared as well as the UtcDateTimes. The default is false.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool CompareDateTimeOffsetWithOffsets  { get; set; }

        /// <summary>
        /// When comparing struct, the depth to compare for children.  The default is 2, the max is 5
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public int MaxStructDepth
        {
            get { return _maxStructDepth; }
            set
            {
                if (value < 1 || value > 5)
                {
                    throw new ArgumentOutOfRangeException("MaxStructDepth", "Cannot be less than 1 or greater than 5");
                }

                _maxStructDepth = value;
            }
        }

        /// <summary>
        /// If true, unknown object types will be ignored instead of throwing an exception.  The default is false.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool IgnoreUnknownObjectTypes { get; set; }

        /// <summary>
        /// If true, invalid indexers will be skipped.  The default is false.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool SkipInvalidIndexers { get; set; }

        /// <summary>
        /// If a class implements an interface then only members of the interface will be compared.  The default is all members are compared. 
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public List<Type> InterfaceMembers { get; set; }

        /// <summary>
        /// Show breadcrumb at each stage of the comparision.  The default is false.
        /// This is useful for debugging deep object graphs.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool ShowBreadcrumb { get; set; }

        /// <summary>
        /// A list of class types to be ignored in the comparison. The default is to compare all class types.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public List<Type> ClassTypesToIgnore { get; set; }

        /// <summary>
        /// Only these class types will be compared. The default is to compare all class types.
        /// </summary>
        /// <remarks>If you specify a class type here no other class types will be compared unless it is in this list.</remarks>
#if !NETSTANDARD
        [DataMember]
#endif
        public List<Type> ClassTypesToInclude { get; set; }

        /// <summary>
        /// A list of types to be ignored in the comparison. The default is to compare all types.  A typical thing to not compare are GUIDs
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public List<Type> TypesToIgnore { get; set; }

        /// <summary>
        /// Only these types will be compared. The default is to compare all types.
        /// </summary>
        /// <remarks>If you specify a type here no others will be compared unless it is in this list.  You must specify ALL Types that you want to compare.</remarks>
#if !NETSTANDARD
        [DataMember]
#endif
        public List<Type> TypesToInclude { get; set; }

        /// <summary>
        /// Ignore Data Table Names, Data Table Column Names, properties, or fields by name during the comparison. Case sensitive. The default is to compare all members.
        /// </summary>
        /// <example>MembersToIgnore.Add("CreditCardNumber");
        /// MembersToIgnore.Add("Invoice.InvoiceGuid");
        /// MembersToIgnore.Add("*Id");
        /// </example>
#if !NETSTANDARD
        [DataMember]
#endif
        public List<string> MembersToIgnore { get; set; }

        /// <summary>
        /// Only compare elements by name for Data Table Names, Data Table Column Names, properties and fields. Case sensitive. The default is to compare all members.
        /// </summary>
        /// <example>MembersToInclude.Add("FirstName")</example>
#if !NETSTANDARD
        [DataMember]
#endif
        public List<string> MembersToInclude { get; set; }

#if !NETSTANDARD1_3
        /// <summary>
        /// If true, private properties and fields will be compared. The default is false.  Silverlight and WinRT restricts access to private variables.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool ComparePrivateProperties { get; set; }
#endif

#if !NETSTANDARD1_3
        /// <summary>
        /// If true, private fields will be compared. The default is false.  Silverlight and WinRT restricts access to private variables.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool ComparePrivateFields { get; set; }
#endif

        /// <summary>
        /// If true, static properties will be compared.  The default is true.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool CompareStaticProperties { get; set; }

        /// <summary>
        /// If true, static fields will be compared.  The default is true.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool CompareStaticFields { get; set; }

        /// <summary>
        /// If true, child objects will be compared. The default is true. 
        /// If false, and a list or array is compared list items will be compared but not their children.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool CompareChildren { get; set; }

        /// <summary>
        /// If true, compare read only properties (only the getter is implemented). The default is true.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool CompareReadOnly { get; set; }

        /// <summary>
        /// If true, compare fields of a class (see also CompareProperties).  The default is true.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool CompareFields { get; set; }

        /// <summary>
        /// If true, compare each item within a collection to every item in the other.  The default is false. WARNING: setting this to true significantly impacts performance.  
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool IgnoreCollectionOrder { get; set; }

        /// <summary>
        /// If true, compare properties of a class (see also CompareFields).  The default is true.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool CompareProperties { get; set; }

        /// <summary>
        /// The maximum number of differences to detect.  The default is 1 for performance reasons.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public int MaxDifferences { get; set; }

        /// <summary>
        /// The maximum number of differences to detect when comparing byte arrays.  The default is 1.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public int MaxByteArrayDifferences { get; set; }

        /// <summary>
        /// Reflection properties and fields are cached. By default this cache is cleared after each compare.  Set to false to keep the cache for multiple compares.
        /// </summary>
        /// <seealso cref="Caching"/>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool AutoClearCache { get; set; }

        /// <summary>
        /// By default properties and fields for types are cached for each compare.  By default this cache is cleared after each compare.
        /// </summary>
        /// <seealso cref="AutoClearCache"/>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool Caching { get; set; }

        /// <summary>
        /// A list of attributes to ignore a class, property or field
        /// </summary>
        /// <example>AttributesToIgnore.Add(typeof(XmlIgnoreAttribute));</example>
#if !NETSTANDARD
        [DataMember]
#endif
        public List<Type> AttributesToIgnore { get; set; }

        /// <summary>
        /// If true, objects will be compared ignore their type diferences.  The default is false.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool IgnoreObjectTypes { get; set; }

        /// <summary>
        /// In the differences string, this is the name for expected name. The default is: Expected 
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public string ExpectedName { get; set; }

        /// <summary>
        /// In the differences string, this is the name for the actual name. The default is: Actual
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public string ActualName { get; set; }

        /// <summary>
        /// Callback invoked each time the comparer finds a difference. The default is no call back.
        /// </summary>
        public Action<Difference> DifferenceCallback
        {
            get { return _differenceCallback; }
            set
            {
                if (null != value)
                {
                    _differenceCallback = value;
                }
            }
        }

        /// <summary>
        /// Sometimes one wants to match items between collections by some key first, and then
        /// compare the matched objects.  Without this, the comparer basically says there is no 
        /// match in collection B for any given item in collection A that doesn't Compare with a result of true.  
        /// The results of this aren't particularly useful for object graphs that are mostly the same, but not quite. 
        /// Enter CollectionMatchingSpec
        /// 
        /// The enumerable strings should be property (not field, for now, to keep it simple) names of the
        /// Type when encountered that will be used for matching
        /// 
        /// You can use complex type properties, too, as part of the key to match.  To match on all props/fields on 
        /// such a matching key, Don't set this property (default comparer behavior)
        /// NOTE: types are looked up as exact.  e.g. if foo is an entry in the dictionary and bar is a 
        /// sub-class of foo, upon encountering a bar type, the comparer will not find the entry of foo
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public Dictionary<Type, IEnumerable<string>> CollectionMatchingSpec { get; set; }

        /// <summary>
        /// A list of custom comparers that take priority over the built in comparers
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public List<BaseTypeComparer> CustomComparers { get; set; }

        /// <summary>
        /// If true, string.empty and null will be treated as equal for Strings and String Builder. The default is false.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public bool TreatStringEmptyAndNullTheSame { get; set; }

        /// <summary>
        /// If true, leading and trailing whitespaces will be ignored for Strings and String Builder. The default is false.
        /// </summary>
#if !DNCORE
        [DataMember]
#endif
        public bool IgnoreStringLeadingTrailingWhitespace { get; set; }

        /// <summary>
        /// The precision to compare double values.  The default is 0.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public double DoublePrecision { get; set; }
        
        /// <summary>
        /// The precision to compare decimal values.  The default is 0.
        /// </summary>
#if !NETSTANDARD
        [DataMember]
#endif
        public decimal DecimalPrecision { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Backing member that supports <see cref="HasWildcardMembersToExclude"/>
        /// </summary>
        private bool? _hasWildcardInMembersToIgnore;

        /// <summary>
        /// Computed value of whether or not exclusion list has wildcards.
        /// </summary>
        public bool HasWildcardMembersToExclude()
        {
            if (_hasWildcardInMembersToIgnore.HasValue)
            {
                return _hasWildcardInMembersToIgnore.Value;
            }

            _hasWildcardInMembersToIgnore = MembersToIgnore.Any(x => x.IndexOf("*") > -1);
            return _hasWildcardInMembersToIgnore.Value;
        }

        /// <summary>
        /// Reset the configuration to the default values
        /// </summary>
        public void Reset()
        {
            AttributesToIgnore = new List<Type>();
            _differenceCallback = d => { };

            MembersToIgnore = new List<string>();
            _hasWildcardInMembersToIgnore = null;

            MembersToInclude = new List<string>();
            ClassTypesToIgnore = new List<Type>();
            ClassTypesToInclude = new List<Type>();
            TypesToIgnore = new List<Type>();
            TypesToInclude = new List<Type>();

            CompareStaticFields = true;
            CompareStaticProperties = true;
#if !NETSTANDARD
            ComparePrivateProperties = false;
            ComparePrivateFields = false;
#endif
            CompareChildren = true;
            CompareReadOnly = true;
            CompareFields = true;
            CompareDateTimeOffsetWithOffsets = false;
            IgnoreCollectionOrder = false;
            CompareProperties = true;
            Caching = true;
            AutoClearCache = true;
            IgnoreObjectTypes = false;
            MaxDifferences = 1;
            ExpectedName = "Expected";
            ActualName = "Actual";
            CustomComparers = new List<BaseTypeComparer>();
            TreatStringEmptyAndNullTheSame = false;
            InterfaceMembers = new List<Type>();
            SkipInvalidIndexers = false;
            MaxByteArrayDifferences = 1;
            CollectionMatchingSpec = new Dictionary<Type, IEnumerable<string>>();
            IgnoreUnknownObjectTypes = false;
            MaxStructDepth = 2;
            CaseSensitive = true;
            IgnoreStringLeadingTrailingWhitespace = false;
        }
#endregion
    }
}
