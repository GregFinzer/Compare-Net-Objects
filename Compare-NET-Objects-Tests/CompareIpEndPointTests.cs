using System;
using System.Net;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareIpEndPointTests
    {
        #region Class Variables
        private CompareLogic _compare;
        #endregion

        #region Setup/Teardown



        /// <summary>
        /// Code that is run before each test
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            _compare = new CompareLogic();
        }

        /// <summary>
        /// Code that is run after each test
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            _compare = null;
        }
        #endregion

        #region Tests
        [Test]
        public void IPEndPointPostiveTest()
        {
            IPEndPoint ipEndPoint1 = new IPEndPoint(44, 22);
            IPEndPoint ipEndPoint2 = new IPEndPoint(44, 22);

            ComparisonResult result = _compare.Compare(ipEndPoint1, ipEndPoint2);

            if (!result.AreEqual)
                throw new Exception(result.DifferencesString);
        }

        [Test]
        public void IPEndPointNegativePortTest()
        {
            IPEndPoint ipEndPoint1 = new IPEndPoint(44, 22);
            IPEndPoint ipEndPoint2 = new IPEndPoint(44, 21);
            Assert.IsFalse(_compare.Compare(ipEndPoint1, ipEndPoint2).AreEqual);
        }

        [Test]
        public void IPEndPointNegativeAddressTest()
        {
            IPEndPoint ipEndPoint1 = new IPEndPoint(44, 22);
            IPEndPoint ipEndPoint2 = new IPEndPoint(45, 22);
            Assert.IsFalse(_compare.Compare(ipEndPoint1, ipEndPoint2).AreEqual);
        }
        #endregion
    }
}
