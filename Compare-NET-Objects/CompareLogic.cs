// This software is provided free of charge from Kellerman Software.
// It may be used in any project, including commercial for sale projects.
//
// Check out our other great software at:
// http://www.kellermansoftware.com
// *  Free Quick Reference Pack for Developers
// *  Free Sharp Zip Wrapper
// *  NUnit Test Generator
// * .NET Caching Library
// * .NET Email Validation Library
// * .NET FTP Library
// * .NET Encryption Library
// * .NET Logging Library
// * Themed Winform Wizard
// * Unused Stored Procedures
// * AccessDiff
// * .NET SFTP Library
// * Ninja Database Pro (Object database for .NET, Silverlight, Windows Phone 7)
// * Ninja WinRT Database (Object database for Windows 8 Runtime, Windows Phone 8)
// * Knight Data Access Layer (ORM, LINQ Provider, Generator)
// * CSV Reports (CSV Reader, Writer)
// * What's Changed? (Compare words, strings, streams, and text files)
// * .NET Excel Reports (Create Excel reports without excel being installed)
// * .NET Word Reports (Create reports based on Microsoft Word files without having Microsoft Word installed)
// * Config Helper Pro (Read and write to the registry, config files, and INI files with 100% managed code)
// * Connection String Creator
// * USPS Street Standardization Library
// * .NET Link Tracker
// * .NET PGP Library

#region Includes

using System;
using System.Collections.Generic;
using System.IO;

#if !NETSTANDARD
using System.Runtime.Serialization.Json;
#endif

#if !NETSTANDARD
using KellermanSoftware.CompareNetObjects.Properties;
#endif

#endregion

#region License
//Microsoft Public License (Ms-PL)

//This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.

//1. Definitions

//The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.

//A "contribution" is the original software, or any additions or changes to the software.

//A "contributor" is any person that distributes its contribution under this license.

//"Licensed patents" are a contributor's patent claims that read directly on its contribution.

//2. Grant of Rights

//(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.

//(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

//3. Conditions and Limitations

//(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.

//(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.

//(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.

//(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.

//(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
#endregion

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Class that allows comparison of two objects of the same type to each other.  Supports classes, lists, arrays, dictionaries, child comparison and more.
    /// </summary>
    /// <example>
    /// CompareLogic compareLogic = new CompareLogic();
    /// 
    /// Person person1 = new Person();
    /// person1.DateCreated = DateTime.Now;
    /// person1.Name = "Greg";
    ///
    /// Person person2 = new Person();
    /// person2.Name = "John";
    /// person2.DateCreated = person1.DateCreated;
    ///
    /// ComparisonResult result = compareLogic.Compare(person1, person2);
    /// 
    /// if (!result.AreEqual)
    ///    Console.WriteLine(result.DifferencesString);
    /// 
    /// </example>
    public class CompareLogic : ICompareLogic
    {
        #region Class Variables
        private ComparisonConfig _config;
        #endregion

        #region Properties

        /// <summary>
        /// The default configuration
        /// </summary>
        public ComparisonConfig Config
        {
            get { return _config;}
            set
            {
                _config = value; 
                VerifyConfig verifyConfig = new VerifyConfig();
                verifyConfig.Verify(value);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Set up defaults for the comparison
        /// </summary>
        public CompareLogic()
        {
            Config = new ComparisonConfig();
        }

        /// <summary>
        /// Pass in the configuration
        /// </summary>
        /// <param name="config"></param>
        public CompareLogic(ComparisonConfig config)
        {
            Config = config;
        }

#if !NETSTANDARD

        /// <summary>
        /// Set up defaults for the comparison
        /// </summary>
        /// <param name="useAppConfigSettings">If true, use settings from the app.config</param>
        public CompareLogic(bool useAppConfigSettings)
        {
            Config = new ComparisonConfig();

            if (useAppConfigSettings)
                SetupWithAppConfigSettings();
        }

        private void SetupWithAppConfigSettings()
        {
            Config.MembersToIgnore = Settings.Default.MembersToIgnore == null
                                ? new List<string>()
                                : new List<string>((IEnumerable<string>)Settings.Default.MembersToIgnore);

            if (Settings.Default.MembersToIgnore != null)
            {
                foreach (var member in Settings.Default.MembersToIgnore)
                {
                    Config.MembersToIgnore.Add(member);
                }
            }

            Config.CompareStaticFields = Settings.Default.CompareStaticFields;
            Config.CompareStaticProperties = Settings.Default.CompareStaticProperties;

            Config.ComparePrivateProperties = Settings.Default.ComparePrivateProperties;
            Config.ComparePrivateFields = Settings.Default.ComparePrivateFields;

            Config.CompareChildren = Settings.Default.CompareChildren;
            Config.CompareReadOnly = Settings.Default.CompareReadOnly;
            Config.CompareFields = Settings.Default.CompareFields;
            Config.IgnoreCollectionOrder = Settings.Default.IgnoreCollectionOrder;
            Config.CompareProperties = Settings.Default.CompareProperties;
            Config.Caching = Settings.Default.Caching;
            Config.AutoClearCache = Settings.Default.AutoClearCache;
            Config.MaxDifferences = Settings.Default.MaxDifferences;
            Config.IgnoreUnknownObjectTypes = Settings.Default.IgnoreUnknownObjectTypes;
            Config.IgnoreObjectDisposedException = Settings.Default.IgnoreObjectDisposedException;
        }
#endif

        #endregion

        #region Public Methods
        /// <summary>
        /// Compare two objects of the same type to each other.
        /// </summary>
        /// <remarks>
        /// Check the Differences or DifferencesString Properties for the differences.
        /// Default MaxDifferences is 1 for performance
        /// </remarks>
        /// <param name="expectedObject">The expected object value to compare</param>
        /// <param name="actualObject">The actual object value to compare</param>
        /// <returns>True if they are equal</returns>
        public ComparisonResult Compare(object expectedObject, object actualObject)
        {
            ComparisonResult result = new ComparisonResult(Config);

#if !NETSTANDARD
                result.Watch.Start();
#endif

            RootComparer rootComparer = RootComparerFactory.GetRootComparer();

            CompareParms parms = new CompareParms
            {
                Config = Config,
                Result = result,
                Object1 = expectedObject,
                Object2 = actualObject,
                BreadCrumb = string.Empty
            };

            rootComparer.Compare(parms);

            if (Config.AutoClearCache)
                ClearCache();

#if !NETSTANDARD
                result.Watch.Stop();
#endif

            return result;
        }

        /// <summary>
        /// Reflection properties and fields are cached. By default this cache is cleared automatically after each compare.
        /// </summary>
        public void ClearCache()
        {
            Cache.ClearCache();
        }

#if !NETSTANDARD
        /// <summary>
        /// Save the current configuration to the passed stream
        /// </summary>
        /// <param name="stream"></param>
        public void SaveConfiguration(Stream stream)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ComparisonConfig));
            ser.WriteObject(stream, Config);

            if (stream.CanSeek && stream.Position > 0)
                stream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Load the current configuration from a json stream
        /// </summary>
        /// <param name="stream"></param>
        public void LoadConfiguration(Stream stream)
        {
            if (stream.CanSeek && stream.Position > 0)
                stream.Seek(0, SeekOrigin.Begin);

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ComparisonConfig));
            Config = (ComparisonConfig)ser.ReadObject(stream);
        }
#endif

#if !NETSTANDARD
        /// <summary>
        /// Load the current configuration from a json stream
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadConfiguration(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ComparisonConfig));
                Config = (ComparisonConfig)ser.ReadObject(stream);
            }
        }

        /// <summary>
        /// Save the current configuration to a json file
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveConfiguration(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ComparisonConfig));
                ser.WriteObject(stream, Config);
            }
        }
#endif
#endregion

    }
}
