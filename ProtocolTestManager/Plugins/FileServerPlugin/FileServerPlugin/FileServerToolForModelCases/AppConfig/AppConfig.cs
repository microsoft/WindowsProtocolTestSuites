// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace Microsoft.Protocols.TestManager.FileServerToolForModelCases
{
    /// <summary>
    /// This class represents test suite specific configurations for the PTM.
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Name of test suite
        /// </summary>
        const string TestSuiteName = "FileServer";

        /// <summary>
        /// Path template for test suite folder.
        /// </summary>
        const string TestSuitePath = "C:\\MicrosoftProtocolTests\\{0}\\{1}-Endpoint\\{2}";

        /// <summary>
        /// Path of config.xml
        /// </summary>
        string ConfigXmlPath = "config.xml";

        /// <summary>
        /// Path of TestSuiteIntro.xml
        /// </summary>
        const string TestSuiteIntroXmlDir = @"../../../UI/TestSuiteIntro.xml";

        /// <summary>
        /// Test suite assemblies.
        /// </summary>
        public List<string> TestSuiteAssembly;

        /// <summary>
        /// Load configuration file and set TestSuiteDir
        /// </summary>
        public string CreateTestSuiteDir()
        {
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.DtdProcessing = DtdProcessing.Prohibit;
            XmlReader xmlReader = XmlReader.Create(TestSuiteIntroXmlDir, settings);
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
            return string.Format(TestSuitePath, TestSuiteName, testSuiteEndPoint, testSuiteVersion);
        }

        /// <summary>
        /// Load configuration file
        /// </summary>
        /// <returns>An instance of AppConfig class.</returns>
        public static AppConfig LoadConfig()
        {
            AppConfig config = new AppConfig();
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.DtdProcessing = DtdProcessing.Prohibit;

            string testSuiteDir = config.CreateTestSuiteDir();

            XmlReader xmlReader = XmlReader.Create(config.ConfigXmlPath, settings);
            doc.Load(xmlReader);

            // Load DllFileNames
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
