using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class IndexerObject

    {
        private readonly IReadOnlyList<int> _values;

        public int Count => _values.Count;
        public int this[int index] => _values[index];

        public IndexerObject(IReadOnlyList<int> values)
        {
            _values = values;
        }
    }
}
