using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;


namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class CompareStringBuilder
    {
        #region Class Variables
        private CompareLogic _compare;
        #endregion

        #region Setup and Teardown
        /// <summary>
        /// Code that is run before each test
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            _compare = new CompareLogic();
        }
        #endregion

        #region Tests

        [Test]
        public void CompareStringBuilderDifferent()
        {
            //Arrange
            StringBuilder sb1 = new StringBuilder("This is test");
            StringBuilder sb2 = new StringBuilder("This is changed");

            //Act
            ComparisonResult result = _compare.Compare(sb1, sb2);

            //Assert
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void CompareStringBuilderSame()
        {
            //Arrange
            StringBuilder sb1 = new StringBuilder("This is test");
            StringBuilder sb2 = new StringBuilder("This is test");

            //Act
            ComparisonResult result = _compare.Compare(sb1, sb2);

            //Assert
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void CompareStringBuilderNullEmptyEqual()
        {
            //Arrange
            StringBuilder sb1 = null;
            StringBuilder sb2 = new StringBuilder(string.Empty);
            _compare.Config.TreatStringEmptyAndNullTheSame = true;

            //Act
            ComparisonResult result = _compare.Compare(sb1, sb2);

            //Assert
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [Test]
        public void CompareStringBuilderCaseInsensitive()
        {
            //Arrange
            StringBuilder sb1 = new StringBuilder("This is TEST");
            StringBuilder sb2 = new StringBuilder("This is test");
            _compare.Config.CaseSensitive = false;

            //Act
            ComparisonResult result = _compare.Compare(sb1, sb2);

            //Assert
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }


        #endregion
    }
}
