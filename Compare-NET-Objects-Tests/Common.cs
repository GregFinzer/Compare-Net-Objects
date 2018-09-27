using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#if !NETSTANDARD
using System.Windows.Forms;
#endif

namespace KellermanSoftware.CompareNetObjectsTests
{
    public static class Common
    {
        const int bigDummyFileKilobyte = 1024;  //Create a 1MB File

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

        /// <summary>
        /// Get the last word
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static string GetLastWord(string sentence, string seperator)
        {
            string[] words = sentence.Split(seperator.ToCharArray());
            return words[words.Length - 1];
        }


        public static bool BytesEqual(byte[] bytes1, byte[] bytes2)
        {
            if (bytes1 == null && bytes2 != null)
                return false;
            else if (bytes2 == null && bytes1 != null)
                return false;
            else if (bytes1 == null && bytes2 == null)
                return true;
            else if (bytes1.Length != bytes2.Length)
                return false;
            else
            {
                for (int i = 0; i < bytes1.Length; i++)
                {
                    if (bytes1[i] != bytes2[i])
                        return false;
                }
            }

            return true;
        }

        public static string CurrentDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        #if !NETSTANDARD

        /// <summary>
        /// Takes a screen shot saves it to the specified directory and returns the full file path
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public static string ScreenShot(string path, string fileName)
        {
            path = System.IO.Path.Combine(path, fileName);

            try
            {
                SendKeys.SendWait("{PRTSC 2}");

                IDataObject data = Clipboard.GetDataObject();
                if (data.GetDataPresent(typeof(System.Drawing.Bitmap)))
                {
                    Image img = (System.Drawing.Bitmap)data.GetData(typeof(System.Drawing.Bitmap));

                    img.Save(path);
                }
            }
            catch
            { }

            return path;
        }

        #endif

        /// <summary>
        /// Create a large file of 100 lines to upload
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CreateBigDummyFile(string path, string fileName, int lines)
        {
            StringBuilder sb = new StringBuilder(lines * bigDummyFileKilobyte);
            path = System.IO.Path.Combine(path, fileName);

            for (long i = 0; i < bigDummyFileKilobyte * 10; i++)
            {
                sb.AppendFormat("This is text line {0} ====================================================\r\n", i);
            }

            System.IO.File.WriteAllText(path, sb.ToString());
            return path;
        }
    }
}
