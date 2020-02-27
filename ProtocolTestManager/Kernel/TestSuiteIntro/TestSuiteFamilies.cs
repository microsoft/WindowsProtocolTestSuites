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

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines all the available test suites in PTM.
    /// </summary>
    public class TestSuiteFamilies : List<TestSuiteFamily>
    {

        public string TestSuiteSelectionConfigXml;
        public string RegistryPath;
        public string RegistryPath64;

        public TestSuiteFamilies()
        { }

        /// <summary>
        /// Load test suites
        /// </summary>
        /// <param name="filename">The name of an XML file, which defines all the basic info of all available test suites</param>
        /// <returns></returns>
        public static TestSuiteFamilies Load(string filename)
        {
            TestSuiteFamilies family = new TestSuiteFamilies();
            // The XML file is automatically created by IDE under such a path
            // It can be revised both in IDE and notepad.
            family.TestSuiteSelectionConfigXml = filename;
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.DtdProcessing = DtdProcessing.Prohibit;
            XmlReader xmlReader = XmlReader.Create(family.TestSuiteSelectionConfigXml, settings);
            doc.Load(xmlReader);

            // Get Registry info 
            XmlNode registryPathNode = doc.DocumentElement.SelectSingleNode("DefaultRegistryPath");
            if (registryPathNode != null)
            {
                family.RegistryPath = registryPathNode.InnerText.Trim();
            }
            else
            {
                throw new InvalidOperationException(StringResource.RegistryPathNotSpecified);
            }

            XmlNode registryPathNode64 = doc.DocumentElement.SelectSingleNode("DefaultRegistryPath64");
            if (registryPathNode64 != null)
            {
                family.RegistryPath64 = registryPathNode64.InnerText.Trim();
            }
            else
            {
                throw new InvalidOperationException(StringResource.RegistryPathNotSpecified);
            }

            // Add group and test suites
            if (doc.DocumentElement.SelectNodes("Group") != null)
            {
                foreach (XmlNode groupXN in doc.DocumentElement.SelectNodes("Group"))
                {
                    TestSuiteFamily newTestSuitesGroup = new TestSuiteFamily();
                    newTestSuitesGroup.Name = groupXN.Attributes["name"].Value;
                    foreach (XmlNode testSuiteXN in groupXN.SelectNodes("TestSuite"))
                    {
                        TestSuiteInfo testSuite = new TestSuiteInfo();
                        testSuite.TestSuiteName = testSuiteXN.Attributes["name"].Value;
                        testSuite.TestSuiteFolderFormat = (testSuiteXN.Attributes["folder"] != null) ? testSuiteXN.Attributes["folder"].Value : null;
                        testSuite.ShortDescription = testSuiteXN.SelectSingleNode("Description").InnerText.Trim();
                        XmlNode testSuiteDetail = testSuiteXN.SelectSingleNode("TestSuiteDetail");
                        if (testSuiteDetail != null)
                        {
                            testSuite.DetailDescription = testSuiteDetail.InnerText;
                        }
                        else
                        {
                            testSuite.DetailDescription = string.Format(StringResource.DefaultDescription, testSuite.TestSuiteName);
                        }
                        XmlNode testSuitePackage = testSuiteXN.SelectSingleNode("Installer");
                        if (testSuitePackage != null)
                        {
                            testSuite.Installer = testSuitePackage.InnerText.Trim();
                        }
                        newTestSuitesGroup.Add(testSuite);
                    }
                    family.Add(newTestSuitesGroup);
                }
            }

            // find test suites that are available
            foreach (var f in family)
            {
                foreach (var testSuite in f)
                {
                    testSuite.IsInstalled = false;
                }
            }

            // Find Test suite Information from Registry
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey[] testSuitesRegPathList;
            if (Environment.Is64BitProcess)
            {
                // 32-bit and 64-bit
                testSuitesRegPathList = new RegistryKey[]
                {
                    HKLM.OpenSubKey(family.RegistryPath),
                    HKLM.OpenSubKey(family.RegistryPath64)
                };
            }
            else
            {
                // 32-bit only
                testSuitesRegPathList = new RegistryKey[]
                {
                    HKLM.OpenSubKey(family.RegistryPath)
                };
            }

            // Check if all registry paths do not exist
            testSuitesRegPathList = testSuitesRegPathList.Where(Entry => Entry != null).ToArray();
            if (testSuitesRegPathList.Length == 0)
            {
                return family;
            }

            //Check if the test suite is installed.
            foreach (TestSuiteFamily f in family)
            {
                foreach (TestSuiteInfo testsuite in f)
                {
                    // Find the registry key of the test suite.

                    testsuite.IsInstalled = FindTestSuiteInformationInRegistry(testSuitesRegPathList, testsuite);

                    if (!testsuite.IsInstalled)
                    {
                        continue;
                    }

                    testsuite.TestSuiteFolder = (testsuite.TestSuiteFolderFormat != null) ?
                            testsuite.TestSuiteFolderFormat
                                .Replace("$(TestSuiteName)", testsuite.TestSuiteName)
                                .Replace("$(TestSuiteVersion)", testsuite.TestSuiteVersion)
                                .Replace("$(TestSuiteEndpoint)", testsuite.TestSuiteEndPoint)
                            : string.Format(StringResource.TestSuiteFolder,
                                testsuite.TestSuiteName,
                                testsuite.TestSuiteEndPoint,
                                testsuite.TestSuiteVersion);
                    string lastProfileFile = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "Protocol Test Manager",
                        testsuite.TestSuiteName,
                        testsuite.TestSuiteVersion,
                        "lastprofile.ptm");
                    testsuite.LastProfile = lastProfileFile;
                    testsuite.IsConfiged = File.Exists(lastProfileFile);


                }
            }
            return family;
        }

        /// <summary>
        /// Find test suite related information in registry.
        /// </summary>
        /// <param name="registryList">registry root path for test suite.</param>
        /// <param name="testSuite">Test suite name.</param>
        /// <returns>True if found, otherwise false.</returns>
        private static bool FindTestSuiteInformationInRegistry(RegistryKey[] registryList, TestSuiteInfo testSuite)
        {
            string latestTestSuiteVersionString = null;
            Version latestTestSuiteVersion = null;
            string latestTestSuiteEndPoint = null;

            foreach (var registryPath in registryList)
            {
                var registryKeyNames = registryPath.GetSubKeyNames()
                                                     .Where(s => s.Contains(testSuite.TestSuiteName));

                if (!registryKeyNames.Any())
                {
                    // no match entry
                    continue;
                }

                foreach (var registryKeyName in registryKeyNames)
                {
                    // match version and endpoint
                    Match versionMatch = Regex.Match(registryKeyName, StringResource.VersionRegex);
                    Match endpointMatch = Regex.Match(registryKeyName, StringResource.EndpointRegex);

                    // update version and endpoint
                    if (Version.TryParse(versionMatch.Value, out var currentVersion))
                    {
                        if (latestTestSuiteVersion == null || currentVersion > latestTestSuiteVersion)
                        {
                            latestTestSuiteVersionString = versionMatch.Value;
                            latestTestSuiteVersion = currentVersion;
                            latestTestSuiteEndPoint = endpointMatch.Success ? endpointMatch.Value : "";
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(latestTestSuiteVersionString) && string.IsNullOrEmpty(latestTestSuiteEndPoint))
            {
                // not found
                return false;
            }

            testSuite.TestSuiteVersion = latestTestSuiteVersionString;
            testSuite.TestSuiteEndPoint = latestTestSuiteEndPoint;

            // found
            return true;
        }
    }
}
