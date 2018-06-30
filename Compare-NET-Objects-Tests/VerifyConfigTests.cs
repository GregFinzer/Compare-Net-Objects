using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses.Bal;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class VerifyConfigTests
    {
        [Test]
        public void VerifySpecShouldNotAllowAList()
        {
            //Arrange
            bool exceptionThrown = false;
            ComparisonConfig config = new ComparisonConfig();
            config.CollectionMatchingSpec = new Dictionary<Type, IEnumerable<string>>();
            config.CollectionMatchingSpec.Add(typeof(List<Offer>),new List<string>{"Id"});

            VerifyConfig verifyConfig = new VerifyConfig();

            //Act
            try
            {
                verifyConfig.VerifySpec(config);
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Assert.IsTrue(ex.Message.Contains("should be a class, not a List"));
            }
            
            Assert.IsTrue(exceptionThrown, "Expected Exception to be thrown");
        }
    }
}
