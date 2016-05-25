using System;


namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Compare Parameters
    /// </summary>
    public class CompareParms
    {
        /// <summary>
        /// The configuration settings
        /// </summary>
        public ComparisonConfig Config { get; set; }

        /// <summary>
        /// The type of the first object
        /// </summary>
        public Type Object1Type { get; set; }

        /// <summary>
        /// The type of the second object
        /// </summary>
        public Type Object2Type { get; set; }

        /// <summary>
        /// Details about the comparison
        /// </summary>
        public ComparisonResult Result { get; set; }

        /// <summary>
        /// A reference to the parent object1
        /// </summary>
        public object ParentObject1 { get; set; }

        /// <summary>
        /// A reference to the parent object2
        /// </summary>
        public object ParentObject2 { get; set; }

        /// <summary>
        /// The first object to be compared
        /// </summary>
        public object Object1 { get; set; }
        
        /// <summary>
        /// The second object to be compared
        /// </summary>
        public object Object2 { get; set; }

        /// <summary>
        /// The breadcrumb in the tree
        /// </summary>
        public string BreadCrumb { get; set; }


        /// <summary>
        /// If a difference is found we need to know if it is an approved change or not
        /// </summary>
        public bool NeedsApproval { get; set; }
        /// <summary>
        /// Userfriendly display name for the property type or class
        /// </summary>
        public string DisplayName { get; set; }
    }
}
