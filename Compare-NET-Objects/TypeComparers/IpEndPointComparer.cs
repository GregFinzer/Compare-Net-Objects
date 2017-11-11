using System;
using System.Globalization;
using System.Net;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Logic to compare two IP End Points
    /// </summary>
    public class IpEndPointComparer : BaseTypeComparer 
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public IpEndPointComparer(RootComparer rootComparer)
            : base(rootComparer)
        {}

        /// <summary>
        /// Returns true if both objects are an IP End Point
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsIpEndPoint(type1) && TypeHelper.IsIpEndPoint(type2);
        }

        /// <summary>
        /// Compare two IP End Points
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            IPEndPoint ipEndPoint1 = parms.Object1 as IPEndPoint;
            IPEndPoint ipEndPoint2 = parms.Object2 as IPEndPoint;

            //Null check happens above
            if (ipEndPoint1 == null || ipEndPoint2 == null)
                return;

            ComparePort(parms, ipEndPoint1, ipEndPoint2);

            if (parms.Result.ExceededDifferences)
                return;

            CompareAddress(parms, ipEndPoint1, ipEndPoint2);
        }



        private void ComparePort(CompareParms parms, IPEndPoint ipEndPoint1, IPEndPoint ipEndPoint2)
        {
            if (ipEndPoint1.Port != ipEndPoint2.Port)
            {
                Difference difference = new Difference
                                            {
                                                ParentObject1 = parms.ParentObject1,
                                                ParentObject2 = parms.ParentObject2,
                                                PropertyName = parms.BreadCrumb,
                                                Object1Value = ipEndPoint1.Port.ToString(CultureInfo.InvariantCulture),
                                                Object2Value = ipEndPoint2.Port.ToString(CultureInfo.InvariantCulture),
                                                ChildPropertyName = "Port",
                                                Object1 = ipEndPoint1,
                                                Object2 = ipEndPoint2
                                            };

                AddDifference(parms.Result, difference);
            }
        }

        private void CompareAddress(CompareParms parms, IPEndPoint ipEndPoint1, IPEndPoint ipEndPoint2)
        {
            if (ipEndPoint1.Address.ToString() != ipEndPoint2.Address.ToString())
            {
                Difference difference = new Difference
                {
                    ParentObject1 = parms.ParentObject1,
                    ParentObject2 = parms.ParentObject2,
                    PropertyName = parms.BreadCrumb,
                    Object1Value = ipEndPoint1.Address.ToString(),
                    Object2Value = ipEndPoint2.Address.ToString(),
                    ChildPropertyName = "Address",
                    Object1 = ipEndPoint1,
                    Object2 = ipEndPoint2
                };

                AddDifference(parms.Result, difference);
            }
        }
    }
}
