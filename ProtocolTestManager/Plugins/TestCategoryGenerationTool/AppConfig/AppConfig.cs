// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace Microsoft.Protocols.TestManager.TestCategoryGenerationTool
{
    /// <summary>
    /// This class represents test suite specific configurations for the PTM.
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Target type of test case
        /// </summary>
        public TestCaseType TargetType { get; set; }

        /// <summary>
        /// Name of test suite
        /// </summary>
        private string TestSuiteName { get; set; }

        /// <summary>
        /// Path template for test suite folder.
        /// </summary>
        private string TestSuiteFolderFormat { get; set; }

        /// <summary>
        /// Path of config.xml
        /// </summary>
        private string ConfigXmlPath = "config.xml";

        /// <summary>
        /// Path of TestSuiteIntro.xml
        /// </summary>
        private const string TestSuiteIntroXmlPath = @"../../../UI/TestSuiteIntro.xml";

        /// <summary>
        /// Test suite assemblies.
        /// </summary>
        public List<string> TestSuiteAssembly;

        /// <summary>
        /// Enum of test case types
        /// </summary>
        public enum TestCaseType : ushort
        {
            /// <summary>
            /// File Server
            /// </summary>
            FILE_SERVER = 0x01,

            /// <summary>
            /// Kerberos
            /// </summary>
            KERBEROS = 0x02,

            /// <summary>
            /// MS-RAA
            /// </summary>
            RAA = 0x03,

            /// <summary>
            /// MS-AZOD
            /// </summary>
            AZOD = 0x04,

            /// <summary>
            /// ADFamily
            /// </summary>
            AD_FAMILY = 0x05,

            /// <summary>
            /// MS-ADOD
            /// </summary>
            ADOD = 0x06,

            /// <summary>
            /// RDP-Client
            /// </summary>
            RDP_CLIENT = 0x07,

            /// <summary>
            /// RDP-Server
            /// </summary>
            RDP_SERVER = 0x08,

            /// <summary>
            /// MS-ADFSPIP
            /// </summary>
            ADFSPIP = 0x09,

            /// <summary>
            /// BranchCache
            /// </summary>
            BRANCH_CACHE = 0x0a,
        }

        /// <summary>
        /// Get test case type
        /// </summary>
        /// <param name="testCaseType"></param>
        /// <returns>TestCaseType</returns>
        private TestCaseType GetTestCaseType(string testCaseType)
        {
            TestCaseType type;
            if (testCaseType.Equals("FileServer"))
            {
                type = TestCaseType.FILE_SERVER;
            }
            else if (testCaseType.Equals("Kerberos"))
            {
                type = TestCaseType.KERBEROS;
            }
            else if (testCaseType.Equals("MS-RAA"))
            {
                type = TestCaseType.RAA;
            }
            else if (testCaseType.Equals("MS-AZOD"))
            {
                type = TestCaseType.AZOD;
            }
            else if (testCaseType.Equals("ADFamily"))
            {
                type = TestCaseType.AD_FAMILY;
            }
            else if (testCaseType.Equals("MS-ADOD"))
            {
                type = TestCaseType.ADOD;
            }
            else if (testCaseType.Equals("RDP-Client"))
            {
                type = TestCaseType.RDP_CLIENT;
            }
            else if (testCaseType.Equals("RDP-Server"))
            {
                type = TestCaseType.RDP_SERVER;
            }
            else if (testCaseType.Equals("MS-ADFSPIP"))
            {
                type = TestCaseType.ADFSPIP;
            }
            else if (testCaseType.Equals("BranchCache"))
            {
                type = TestCaseType.BRANCH_CACHE;
            }
            else
            {
                return (TestCaseType)0xff;
            }
            TestSuiteName = testCaseType;
            return type;
        }

        /// <summary>
        /// Get test suite folder
        /// </summary>
        /// <param name="type"></param>
        /// <param name="endpoint"></param>
        /// <param name="version"></param>
        /// <returns>Test suite folder</returns>
        private string GetTestSuiteFolder(TestCaseType type, string endpoint, string version)
        {
            switch (type)
            {
                case TestCaseType.AD_FAMILY:
                case TestCaseType.FILE_SERVER:
                case TestCaseType.KERBEROS:
                case TestCaseType.RAA:
                    break;
                case TestCaseType.ADOD:
                case TestCaseType.AZOD:
                case TestCaseType.BRANCH_CACHE:
                    return TestSuiteFolderFormat.Replace("$(TestSuiteVersion)", version);
                case TestCaseType.RDP_CLIENT:
                case TestCaseType.RDP_SERVER:
                    return TestSuiteFolderFormat
                        .Replace("$(TestSuiteVersion)",  version)
                        .Replace("$(TestSuiteEndpoint)", endpoint);
                default:
                    return null;
            }
            return string.Format(TestSuiteFolderFormat, TestSuiteName, endpoint, version);
        }

        /// <summary>
        /// Load configuration file and return TestSuiteDir
        /// </summary>
        /// <returns>TestSuiteDir</returns>
        public string CreateTestSuiteDir(TestCaseType type)
        {
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.DtdProcessing = DtdProcessing.Prohibit;
            XmlReader xmlReader = XmlReader.Create(TestSuiteIntroXmlPath, settings);
            doc.Load(xmlReader);

            // Get Registry info 
            string registryPath = null;
            XmlNode registryPathNode = doc.DocumentElement.SelectSingleNode("DefaultRegistryPath");
            if (registryPathNode != null)
            {
                registryPath = registryPathNode.InnerText.Trim();
            }
            else
            {
                throw new InvalidOperationException("DefaultRegistryPath is not found");
            }

            string registryPath64 = null;
            XmlNode registryPathNode64 = doc.DocumentElement.SelectSingleNode("DefaultRegistryPath64");
            if (registryPathNode64 != null)
            {
                registryPath64 = registryPathNode64.InnerText.Trim();
            }
            else
            {
                throw new InvalidOperationException("DefaultRegistryPath64 is not found");
            }

            bool isFound = false;
            foreach (XmlNode groupXN in doc.DocumentElement.SelectNodes("Group"))
            {
                foreach (XmlNode testSuiteXN in groupXN.SelectNodes("TestSuite"))
                {
                    if (TestSuiteName.Equals(testSuiteXN.Attributes["name"].Value))
                    {
                        TestSuiteFolderFormat = (testSuiteXN.Attributes["folder"] != null) ? testSuiteXN.Attributes["folder"].Value : null;
                        isFound = true;
                        break;
                    }
                }
                if (isFound)
                {
                    break;
                }
            }

            // If TestSuiteFolderFormat is not found in xml file, set it to default TestSuiteFolderFormat
            if (TestSuiteFolderFormat == null)
            {
                TestSuiteFolderFormat = "C:\\MicrosoftProtocolTests\\{0}\\{1}-Endpoint\\{2}";
            }

            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey TestSuitesRegPath = Environment.Is64BitProcess ?
                HKLM.OpenSubKey(registryPath64) : HKLM.OpenSubKey(registryPath);
            string registryKeyName = TestSuitesRegPath.GetSubKeyNames()
                             .Where((s) => s.Contains(TestSuiteName))
                             .FirstOrDefault();
            Match endpointMatch = Regex.Match(registryKeyName, "(Server)|(Client)");
            string testSuiteEndPoint = endpointMatch.Value;
            Match versionMatch = Regex.Match(registryKeyName, "\\d\\.\\d\\.\\d{1,4}\\.\\d");
            string testSuiteVersion = versionMatch.Value;
            return GetTestSuiteFolder(type, testSuiteEndPoint, testSuiteVersion);
        }

        /// <summary>
        /// Load configuration file
        /// </summary>
        /// <returns>An instance of AppConfig class.</returns>
        public static AppConfig LoadConfig(string testSuiteName)
        {
            AppConfig config = new AppConfig();
            config.TargetType = config.GetTestCaseType(testSuiteName);
            if (config.TargetType == (TestCaseType)(0xff))
            {
                Program.usage();
                System.Environment.Exit(-1);
            }
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.DtdProcessing = DtdProcessing.Prohibit;

            XmlReader xmlReader = XmlReader.Create(config.ConfigXmlPath, settings);
            doc.Load(xmlReader);

            // Load DllFileNames
            string testSuiteDir = config.CreateTestSuiteDir(config.TargetType);
            config.TestSuiteAssembly = new List<string>();
            XmlNode DllFileNamesNode = doc.DocumentElement.SelectSingleNode("DllFileNames");
            foreach (XmlNode xn in DllFileNamesNode.SelectNodes("DllFileName"))
            {
                string name = xn.InnerText.Trim();
                config.TestSuiteAssembly.Add(System.IO.Path.Combine(testSuiteDir, name));
            }
            return config;
        }
    }
}
