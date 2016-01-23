namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    public class ThreadFoo
    {
        public string Line1;
        public int Line2;
        public double Line3;

        public ThreadFoo() { }

        public ThreadFoo(ThreadFoo other)
        {
            Line1 = other.Line1;
            Line2 = other.Line2;
            Line3 = other.Line3;
        }

        public static ThreadFoo Create(int i)
        {
            return new ThreadFoo
            {
                Line1 = "#" + i.ToString(),
                Line2 = i,
                Line3 = double.MaxValue
            };
        }
    }
}
