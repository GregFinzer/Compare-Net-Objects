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
        #region Class Variables
        private string _differencesString;
        /// <summary>
        /// Keep track of parent objects in the object hierarchy
        /// </summary>
        private readonly Dictionary<object, int> _parents = new Dictionary<object, int>();
        #endregion

        #region Constructors
        /// <summary>
        /// Set the configuration for the comparison
        /// </summary>
        /// <param name="config"></param>
        public ComparisonResult(ComparisonConfig config)
        {
            Config = config;
            Differences = new List<Difference>();
            Watch = new Stopwatch();
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

        /// <summary>
        /// Used to time how long the comparison took
        /// </summary>
        internal Stopwatch Watch { get; set; }

        /// <summary>
        /// The amount of time in milliseconds it took for the comparison
        /// </summary>
        public long ElapsedMilliseconds
        {
            get { return Watch.ElapsedMilliseconds; }
        }
        
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
        /// <param name="objectReference"></param>
        protected internal void AddParent(object objectReference)
        {
            if (objectReference == null)
                return;

            if (!_parents.ContainsKey(objectReference))
            {
                _parents.Add(objectReference, 0);
            }
            else
            {
                _parents[objectReference]++;
            }
        }



        /// <summary>
        /// Remove parent, handle references count
        /// </summary>
        /// <param name="objectReference"></param>
        protected internal void RemoveParent(object objectReference)
        {
            if (objectReference == null)
                return;

            if (_parents.ContainsKey(objectReference))
            {
                if (_parents[objectReference] <= 1)
                    _parents.Remove(objectReference);
                else _parents[objectReference]--;
            }
        }

        /// <summary>
        /// Returns true if we have encountered this parent before
        /// </summary>
        /// <param name="objectReference"></param>
        /// <returns></returns>
        protected internal bool IsParent(object objectReference)
        {
            if (objectReference == null)
                return false;

            return _parents.ContainsKey(objectReference);
        }
        #endregion


    }
}
