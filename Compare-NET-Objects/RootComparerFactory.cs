﻿using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects.TypeComparers;


namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Factory to create a root comparer
    /// </summary>
    public static class RootComparerFactory
    {
        #region Class Variables
        private static readonly object _locker = new object();
        private static RootComparer _rootComparer;
        #endregion

        #region Methods

        /// <summary>
        /// Get the current root comparer
        /// </summary>
        /// <returns></returns>
        public static RootComparer GetRootComparer()
        {
            lock(_locker)
                if (_rootComparer == null)
                    _rootComparer= BuildRootComparer();

            return _rootComparer;
        }

        private static RootComparer BuildRootComparer()
        {
            _rootComparer = new RootComparer();

            _rootComparer.TypeComparers = new List<BaseTypeComparer>();

            _rootComparer.TypeComparers.Add(new StringComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new DateTimeComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new DecimalComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new DoubleComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new PointerComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new SimpleTypeComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new ReadOnlyCollectionComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new ListComparer(_rootComparer));

#if NETFULL || NETSTANDARD2_0 || NETSTANDARD2_1
            _rootComparer.TypeComparers.Add(new IpEndPointComparer(_rootComparer));
#endif

            _rootComparer.TypeComparers.Add(new RuntimeTypeComparer(_rootComparer));

#if !NETSTANDARD
            _rootComparer.TypeComparers.Add(new FontComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new DatasetComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new DataTableComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new DataRowComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new DataColumnComparer(_rootComparer));
#endif
            _rootComparer.TypeComparers.Add(new EnumerableComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new ByteArrayComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new DictionaryComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new HashSetComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new CollectionComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new UriComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new StringBuilderComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new ClassComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new DateTimeOffSetComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new TimespanComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new StructComparer(_rootComparer));
            _rootComparer.TypeComparers.Add(new ImmutableArrayComparer(_rootComparer));

            return _rootComparer;
        }
        #endregion
    }
}
