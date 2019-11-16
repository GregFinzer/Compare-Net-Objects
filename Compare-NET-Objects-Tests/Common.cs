using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace KellermanSoftware.CompareNetObjectsTests
{
    public static class Common
    {
        public static T CloneWithSerialization<T>(T original) where T : class
        {
            T result;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, original);
                stream.Seek(0, SeekOrigin.Begin);
                result = (T)binaryFormatter.Deserialize(stream);
            }

            return result;
        }
    }
}
