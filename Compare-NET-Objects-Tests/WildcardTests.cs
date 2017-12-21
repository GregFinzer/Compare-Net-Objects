using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class WildcardTests
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

        [Test]
        public void IgnoreWildcardContainsPostive()
        {
            Invoice invoice1 = new Invoice();
            invoice1.InvoiceId = 1;
            invoice1.BillingFirstName = "Arnold";
            invoice1.BillingLastName = "Swartzenegger";

            Invoice invoice2 = new Invoice();
            invoice2.InvoiceId = 1;
            invoice2.BillingFirstName = "Bruce";
            invoice2.BillingLastName = "Willis";

            _compare.Config.MembersToIgnore.Add("*ing*");
            var result = _compare.Compare(invoice1, invoice2);
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void IgnoreWildcardContainsNegative()
        {
            Invoice invoice1 = new Invoice();
            invoice1.InvoiceId = 1;
            invoice1.BillingFirstName = "Arnold";
            invoice1.BillingLastName = "Swartzenegger";

            Invoice invoice2 = new Invoice();
            invoice2.InvoiceId = 1;
            invoice2.BillingFirstName = "Bruce";
            invoice2.BillingLastName = "Willis";

            _compare.Config.MembersToIgnore.Add("*Mommy*");
            var result = _compare.Compare(invoice1, invoice2);
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void IgnoreWildcardStartsWithPostive()
        {
            Invoice invoice1 = new Invoice();
            invoice1.InvoiceId = 1;
            invoice1.BillingFirstName = "Arnold";
            invoice1.BillingLastName = "Swartzenegger";

            Invoice invoice2 = new Invoice();
            invoice2.InvoiceId = 1;
            invoice2.BillingFirstName = "Bruce";
            invoice2.BillingLastName = "Willis";

            _compare.Config.MembersToIgnore.Add("Billing*");
            var result = _compare.Compare(invoice1, invoice2);
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void IgnoreWildcardStartsWithNegative()
        {
            Invoice invoice1 = new Invoice();
            invoice1.InvoiceId = 1;
            invoice1.BillingFirstName = "Arnold";
            invoice1.BillingLastName = "Swartzenegger";

            Invoice invoice2 = new Invoice();
            invoice2.InvoiceId = 1;
            invoice2.BillingFirstName = "Bruce";
            invoice2.BillingLastName = "Willis";

            _compare.Config.MembersToIgnore.Add("Filling*");
            var result = _compare.Compare(invoice1, invoice2);
            Assert.IsFalse(result.AreEqual);
        }

        [Test]
        public void IgnoreWildcardEndsWithPostive()
        {
            Invoice invoice1 = new Invoice();
            invoice1.InvoiceId = 1;
            invoice1.BillingFirstName = "Arnold";
            invoice1.BillingLastName = "Swartzenegger";

            Invoice invoice2 = new Invoice();
            invoice2.InvoiceId = 2;
            invoice2.BillingFirstName = "Arnold";
            invoice2.BillingLastName = "Swartzenegger";

            _compare.Config.MembersToIgnore.Add("*Id");
            var result = _compare.Compare(invoice1, invoice2);
            Assert.IsTrue(result.AreEqual);
        }

        [Test]
        public void IgnoreWildcardEndsWithNegative()
        {
            Invoice invoice1 = new Invoice();
            invoice1.InvoiceId = 1;
            invoice1.BillingFirstName = "Arnold";
            invoice1.BillingLastName = "Swartzenegger";

            Invoice invoice2 = new Invoice();
            invoice2.InvoiceId = 2;
            invoice2.BillingFirstName = "Arnold";
            invoice2.BillingLastName = "Swartzenegger";

            _compare.Config.MembersToIgnore.Add("*Hero");
            var result = _compare.Compare(invoice1, invoice2);
            Assert.IsFalse(result.AreEqual);
        }
    }
}
