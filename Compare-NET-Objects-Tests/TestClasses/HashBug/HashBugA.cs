using System;
using KellermanSoftware.CompareNetObjects;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.HashBug
{
    public abstract class HashBugA : IEquatable<HashBugA>
    {
        public string Text { get; set; }

        private readonly int id;
        protected HashBugA()
        {
            this.id = new Random().Next();
        }

        public static bool operator ==(HashBugA entity1, HashBugA entity2)
        {
            return InternalEquals(entity1, entity2);
        }

        public static bool operator !=(HashBugA entity1, HashBugA entity2)
        {
            return !(entity1 == entity2);
        }

        public bool Equals(HashBugA other)
        {
            return InternalEquals(this, other);
        }

        public override bool Equals(object obj)
        {
            return InternalEquals(this, obj as HashBugA);
        }

        public override int GetHashCode()
        {
            return this.id;
        }

        private static bool InternalEquals(HashBugA entity1, HashBugA entity2)
        {
            var comparer = new CompareLogic();
            comparer.Config.UseHashCodeIdentifier = true;
            var result = comparer.Compare(entity1, entity2);

            return result.AreEqual;
        }
    }
}
