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
        {}

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

            // Find Test suite Infomation from Registry
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey TestSuitesRegPath = Environment.Is64BitProcess ?
                HKLM.OpenSubKey(family.RegistryPath64) : HKLM.OpenSubKey(family.RegistryPath);

            if (TestSuitesRegPath == null)
            {
                return family;
            }

            //Check if the test suite is installed.
            foreach (TestSuiteFamily f in family)
            {
                foreach (TestSuiteInfo testsuite in f)
                {
                    // Find the registry key of the test suite.
                    string registryKeyName = TestSuitesRegPath.GetSubKeyNames()
                                                 .Where((s) => s.Contains(testsuite.TestSuiteName))
                                                 .FirstOrDefault();
                    if (string.IsNullOrEmpty(registryKeyName)) continue;

                    Match versionMatch = Regex.Match(registryKeyName, StringResource.VersionRegex);
                    testsuite.TestSuiteVersion = versionMatch.Value;

                    Match endpointMatch = Regex.Match(registryKeyName, StringResource.EndpointRegex);
                    testsuite.TestSuiteEndPoint = endpointMatch.Success ? endpointMatch.Value : "";
                    testsuite.IsInstalled = true;

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
    }
}
