using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class ThreadSafeTests
    {
        private static CompareLogic CreateComparator()
        {
            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.CompareStaticFields = false;
            compareLogic.Config.CompareStaticProperties = false;
            compareLogic.Config.MaxDifferences = int.MaxValue;
            compareLogic.Config.ShowBreadcrumb = true;
            compareLogic.Config.TreatStringEmptyAndNullTheSame = false;
            return compareLogic;
        }

        [Test]
        public void ThreadPoolTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                var o1 = ThreadFoo.Create(i);
                var o2 = new ThreadFoo(o1);
                ThreadPool.QueueUserWorkItem(state =>
                {
                    var arr = state as ThreadFoo[];
                    CreateComparator().Compare(arr[0], arr[1]);
                }, new ThreadFoo[] { o1, o2 });
            }
        }
    }
}
