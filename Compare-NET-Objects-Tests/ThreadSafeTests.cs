using System.Collections.Generic;
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
            var events = new List<ManualResetEvent>();

            for (int i = 0; i < 64; i++)
            {
                var resetEvent = new ManualResetEvent(false);
                var o1 = ThreadFoo.Create(i);
                var o2 = new ThreadFoo(o1);
                ThreadPool.QueueUserWorkItem(state =>
                {
                    var arr = state as ThreadFoo[];
                    CreateComparator().Compare(arr[0], arr[1]);
                    resetEvent.Set();
                }, new ThreadFoo[] { o1, o2 });
                events.Add(resetEvent);
            }

            WaitHandle.WaitAll(events.ToArray());
        }
    }
}
