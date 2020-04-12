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
        /// Keep track of parent objects in the object hierarchy by using reference equals
        /// </summary>
        private readonly Dictionary<object, int> _referenceParents = new Dictionary<object, int>();
        private readonly  Dictionary<int, int> _hashParents = new Dictionary<int, int>();
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

            if (Config.UseHashCodeIdentifier && _hashParents == null)
                return;

            if (!Config.UseHashCodeIdentifier && _referenceParents == null)
                return;

            if (Config.UseHashCodeIdentifier)
            {
                int hash = objectReference.GetHashCode();

                if (hash == 0)
                    return;

                if (!_hashParents.ContainsKey(hash))
                {
                    _hashParents.Add(hash, 1);
                }
                else
                {
                    _hashParents[hash]++;
                }
            }
            else
            {
                if (!_referenceParents.ContainsKey(objectReference))
                {
                    _referenceParents.Add(objectReference, 0);
                }
                else
                {
                    _referenceParents[objectReference]++;
                }
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

            if (Config.UseHashCodeIdentifier && _hashParents == null)
                return;

            if (!Config.UseHashCodeIdentifier && _referenceParents == null)
                return;

            try
            {
                if (Config.UseHashCodeIdentifier)
                {
                    int hash = objectReference.GetHashCode();

                    if (_hashParents.ContainsKey(hash))
                    {
                        if (_hashParents[hash] <= 1)
                        {
                            _hashParents.Remove(hash);
                        }
                        else
                        {
                            _hashParents[hash]--;
                        }
                    }
                }
                else
                {
                    if (_referenceParents.ContainsKey(objectReference))
                    {
                        if (_referenceParents[objectReference] <= 1)
                        {
                            _referenceParents.Remove(objectReference);
                        }
                        else
                        {
                            _referenceParents[objectReference]--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

            if (Config.UseHashCodeIdentifier && _hashParents == null)
                return false;

            if (!Config.UseHashCodeIdentifier && _referenceParents == null)
                return false;

            if (Config.UseHashCodeIdentifier)
            {
                int hash = objectReference.GetHashCode();

                if (hash == 0)
                    return false;

                return _hashParents.ContainsKey(hash);
            }
            else
            {
                return _referenceParents.ContainsKey(objectReference);
            }
        }
        #endregion


    }
}
