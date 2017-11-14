using System;
using System.Collections.Generic;
using System.Reflection;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Generic class for holding a Property Info, or Dynamic Info
    /// </summary>
    public class PropertyEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyEntity()
        {
            Indexers = new List<ParameterInfo>();
        }

        /// <summary>
        /// If true, this is a dynamic property
        /// </summary>
        public bool IsDynamic { get; set; }

        /// <summary>
        /// Name of the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the property
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Let me reflect on this day
        /// </summary>
        public Type ReflectedType { get; set; }

        /// <summary>
        /// The type of the parent
        /// </summary>
        public Type DeclaringType { get; set; }

        /// <summary>
        /// The type of the property
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// If the property can be read from
        /// </summary>
        public bool CanRead { get; set; }

        /// <summary>
        /// If the property can be written to
        /// </summary>
        public bool CanWrite { get; set; }

        /// <summary>
        /// Indexers for the property
        /// </summary>
        public List<ParameterInfo> Indexers { get; set; }

        /// <summary>
        /// Reference to the property info
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }
    }
}
