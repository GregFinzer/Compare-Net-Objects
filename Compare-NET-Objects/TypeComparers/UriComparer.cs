using System;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare two URIs
    /// </summary>
    public class UriComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public UriComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both types are a URI
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsUri(type1) && TypeHelper.IsUri(type2);
        }

        /// <summary>
        /// Compare two URIs
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            Uri uri1 = parms.Object1 as Uri;
            Uri uri2 = parms.Object2 as Uri;

            //This should never happen, null check happens one level up
            if (uri1 == null || uri2 == null)
                return;

            if (uri1.OriginalString != uri2.OriginalString)
            {
                Difference difference = new Difference
                {
                    ParentObject1 = new WeakReference(parms.ParentObject1),
                    ParentObject2 = new WeakReference(parms.ParentObject2),
                    PropertyName = parms.BreadCrumb,
                    Object1Value = NiceString(uri1.OriginalString),
                    Object2Value = NiceString(uri2.OriginalString),
                    ChildPropertyName = "OriginalString",
                    Object1 = new WeakReference(parms.Object1),
                    Object2 = new WeakReference(parms.Object2)
                };

                AddDifference(parms.Result, difference);
            }
        }
    }
}
