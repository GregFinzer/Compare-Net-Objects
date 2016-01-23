using System;
using System.Collections;
using System.Globalization;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare two byte arrays
    /// </summary>
    public class ByteArrayComparer : BaseTypeComparer
    {
        /// <summary>
        /// Protected constructor that references the root comparer
        /// </summary>
        /// <param name="rootComparer">The root comparer.</param>
        public ByteArrayComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }

        /// <summary>
        /// If true the type comparer will handle the comparison for the type
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns><c>true</c> if it is a byte array; otherwise, <c>false</c>.</returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsByteArray(type1)
                   && TypeHelper.IsByteArray(type2);
        }

        /// <summary>
        /// Compare two byte array objects
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            //This should never happen, null check happens one level up
            if (parms == null || parms.Object1 == null || parms.Object2 == null)
                return;

            if (ListsHaveDifferentCounts(parms)) 
                return;
                
            CompareItems(parms);                
        }

        private bool ListsHaveDifferentCounts(CompareParms parms)
        {
            IList ilist1 = parms.Object1 as IList;
            IList ilist2 = parms.Object2 as IList;

            if (ilist1 == null)
                throw new ArgumentException("parms.Object1");

            if (ilist2 == null)
                throw new ArgumentException("parms.Object2");

            //Objects must be the same length
            if (ilist1.Count != ilist2.Count)
            {
                Difference difference = new Difference
                {
                    ParentObject1 = new WeakReference(parms.ParentObject1),
                    ParentObject2 = new WeakReference(parms.ParentObject2),
                    PropertyName = parms.BreadCrumb,
                    Object1Value = ilist1.Count.ToString(CultureInfo.InvariantCulture),
                    Object2Value = ilist2.Count.ToString(CultureInfo.InvariantCulture),
                    ChildPropertyName = "Count",
                    Object1 = new WeakReference(ilist1),
                    Object2 = new WeakReference(ilist2)
                };

                AddDifference(parms.Result, difference);

                if (parms.Result.ExceededDifferences)
                    return true;
            }
            return false;
        }

        private void CompareItems(CompareParms parms)
        {
            int count = 0;
            int differenceCount = 0;
            IEnumerator enumerator1 = ((IList) parms.Object1).GetEnumerator();
            IEnumerator enumerator2 = ((IList) parms.Object2).GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                byte? b1 = enumerator1.Current as byte?;
                byte? b2 = enumerator2.Current as byte?;

                if (b1 != b2)
                {
                    string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, string.Empty, string.Empty, count);

                    Difference difference = new Difference
                    {
                        ParentObject1 = new WeakReference(parms.Object1),
                        ParentObject2 = new WeakReference(parms.Object2),
                        PropertyName = currentBreadCrumb,
                        Object1Value = NiceString(b1),
                        Object2Value = NiceString(b2),
                        Object1 = new WeakReference(b1),
                        Object2 = new WeakReference(b2)
                    };

                    AddDifference(parms.Result, difference);
                    differenceCount++;

                    if (differenceCount >= parms.Result.Config.MaxByteArrayDifferences)
                        return;
                }

                count++;
            }
        }
    }
}
