using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Details about the comparison
    /// </summary>
    public class ComparisonResult
    {
        private string _differencesString;

        #region Constructors
        /// <summary>
        /// Set the configuration for the comparison
        /// </summary>
        /// <param name="config"></param>
        public ComparisonResult(ComparisonConfig config)
        {
            Config = config;
            Differences = new List<Difference>();

            #if !NETSTANDARD
                Watch = new Stopwatch();
            #endif
        }
        #endregion

        #region Properties
        /// <summary>
        /// Keep track of the depth of structs being compared.  Used with ComparisonConfig.MaxStructDepth
        /// </summary>
        internal int CurrentStructDepth { get; set; }

        /// <summary>
        /// Configuration
        /// </summary>
        public ComparisonConfig Config { get; private set; }

        #if !NETSTANDARD
            internal Stopwatch Watch { get; set; }

            /// <summary>
            /// The amount of time in milliseconds it took for the comparison
            /// </summary>
            public long ElapsedMilliseconds
            {
                get { return Watch.ElapsedMilliseconds; }
            }
        #endif

        /// <summary>
        /// Keep track of parent objects in the object hiearchy
        /// </summary>
        internal readonly Dictionary<int, int> Parents = new Dictionary<int, int>();

        /// <summary>
        /// The differences found during the compare
        /// </summary>
        public List<Difference> Differences { get; set; }

        /// <summary>
        /// The differences found in a string suitable for a textbox
        /// </summary>
        public string DifferencesString
        {
            get
            {
                if (String.IsNullOrEmpty(_differencesString))
                {
                    StringBuilder sb = new StringBuilder(4096);

                    sb.AppendLine();
                    sb.AppendFormat("Begin Differences ({0} differences):{1}", Differences.Count, Environment.NewLine);

                    foreach (Difference item in Differences)
                    {
                        sb.AppendLine(item.ToString());
                    }

                    sb.AppendFormat("End Differences (Maximum of {0} differences shown).", Config.MaxDifferences);

                    _differencesString = sb.ToString();
                }

                return _differencesString;
            }
            set { _differencesString = value; }
        }

        /// <summary>
        /// Returns true if the objects are equal
        /// </summary>
        public bool AreEqual
        {
            get { return Differences.Count == 0; }
        }

        /// <summary>
        /// Returns true if the number of differences has reached the maximum
        /// </summary>
        public bool ExceededDifferences
        {
            get { return Differences.Count >= Config.MaxDifferences; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add parent, handle references count
        /// </summary>
        /// <param name="hash"></param>
        protected internal void AddParent(int hash)
        {
            if (hash == 0)
            {
                return;
            }

            if (!Parents.ContainsKey(hash))
            {
                Parents.Add(hash, 1);
            }
            else
            {
                Parents[hash]++;
            }
        }



        /// <summary>
        /// Remove parent, handle references count
        /// </summary>
        /// <param name="hash"></param>
        protected internal void RemoveParent(int hash)
        {
            if (Parents.ContainsKey(hash))
            {
                if (Parents[hash] <= 1)
                    Parents.Remove(hash);
                else Parents[hash]--;
            }
        }
        #endregion


    }
}
