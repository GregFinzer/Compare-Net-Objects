using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsIntegrationTests
{
    [TestFixture]
    public class ConfigTests
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

        #region Save and Load Configuration Tests

#if !NETSTANDARD

        [Test]
        public void SaveConfigurationTest()
        {
            //Arrange
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            if (File.Exists(filePath))
                File.Delete(filePath);

            //Act
            _compare.SaveConfiguration(filePath);

            //Assert
            Assert.IsTrue(File.Exists(filePath));
        }

        [Test]
        public void LoadConfigurationTest()
        {
            //Arrange
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            _compare.Config.CaseSensitive = false;
            _compare.SaveConfiguration(filePath);

            //Act
            _compare.Config = new ComparisonConfig(); //Wipe out the current config
            _compare.LoadConfiguration(filePath);

            //Assert
            Assert.IsFalse(_compare.Config.CaseSensitive);
        }

#endif
#endregion
    }
}
