﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class represents test suite specific configurations for the PTM.
    /// </summary>
    public class AppConfig
    {
        #region Html result related
        /// <summary>
        /// Html result root folder
        /// </summary>
        public const string HtmlResultFolderName = "HtmlTestResults";

        /// <summary>
        /// The folder saving the html log files.
        /// </summary>
        public const string HtmlLogFileFolder = "Html";

        /// <summary>
        /// The "Passed" status used in txt file names.
        /// </summary>
        public const string HtmlLogStatusPassed = "Passed";

        /// <summary>
        /// The "Failed" status used in txt file names.
        /// </summary>
        public const string HtmlLogStatusFailed = "Failed";

        /// <summary>
        /// The "Inconclusive" status used in txt file names.
        /// </summary>
        public const string HtmlLogStatusInconclusive = "Inconclusive";

        /// <summary>
        /// Keyword used to search the test status in the html log file
        /// </summary>
        public const string ResultKeyword = "\"Result\":\"Result: ";

        /// <summary>
        /// File name of the index html
        /// </summary>
        public const string IndexHtmlFileName = "index.html";
        #endregion

        /// <summary>
        /// The name of the test suite
        /// </summary>
        public string TestSuiteName { get; set; }

        /// <summary>
        /// The version of the test suite
        /// </summary>
        public string TestSuiteVersion { get; set; }

        /// <summary>
        /// The installation directory of the test suite
        /// </summary>
        public string TestSuiteDirectory { get; set; }

        /// <summary>
        /// The installation directory of PTM
        /// </summary>
        public string PtmInstallDirectory { get; set; }

        /// <summary>
        /// The location of the doc directory
        /// </summary>
        public string DocDirectory { get; set; }

        /// <summary>
        /// The location of the etc directory
        /// </summary>
        public string EtcDirectory { get; set; }

        /// <summary>
        /// Path like C:\Users\administrator.CONTOSO\AppData\Roaming\Protocol Test Manager\MS-XXXX\1.0.1234.0
        /// </summary>
        public string AppDataDirectory { get; set; }
        /// <summary>
        /// Test suite assemblies.
        /// </summary>
        public List<string> TestSuiteAssembly;

        /// <summary>
        /// PTMConfig files in bin folder
        /// </summary>
        public List<string> PtfConfigFiles{get; private set;}

        /// <summary>
        /// The PTMConfig files used as the default values
        /// </summary>
        public List<string> DefaultPtfConfigFiles { get; private set; }

        /// <summary>
        /// The PTMConfig files in source code
        /// </summary>
        public List<string> PtfConfigFilesInSource { get; private set; }

        /// <summary>
        /// The order of the properties on UI
        /// </summary>
        public Dictionary<string, int> PropertyGroupOrder { get; private set; }

        /// <summary>
        /// Test suite user guide
        /// </summary>
        public string UserGuide { get; private set; }

        /// <summary>
        /// The detector engine
        /// </summary>
        public string DetectorAssemblyPath { get; private set; }

        /// <summary>
        /// The config file to define the additional categories
        /// </summary>
        public AppConfigTestCategory TestCategory { get; private set; }

        //Execution engine
        /// <summary>
        /// Name of test list.
        /// </summary>
        public string TestListName;

        /// <summary>
        /// The path of VSTest.
        /// </summary>
        public string VSTestPath;

        /// <summary>
        /// The arguments of VSTest.
        /// </summary>
        public string VSTestArguments;

        /// <summary>
        /// The setting of test.
        /// </summary>
        public string TestSetting;

        /// <summary>
        /// WelcomePage
        /// </summary>
        public List<string> TestSuiteIntroduction { get; set; }


        /// <summary>
        /// Test case selection rules
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "By Design")]
        public XmlNode RuleDefinitions;

        /// <summary>
        /// Adapter definitions node.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "By Design")]
        public XmlNode AdapterDefinitions;

        /// <summary>
        /// A list of predefined adapters.
        /// </summary>
        public List<PtfAdapterView> PredefinedAdapters;

        /// <summary>
        /// Loads the configuration file
        /// </summary>
        /// <param name="testSuiteName">The name of the test suite.</param>
        /// <param name="testSuiteVersion">The version of test suite</param>
        /// <param name="testSuiteDir">The working directory for the test suite.</param>
        /// <param name="installDir">The install directory of PTM.</param>
        /// <returns>An instance of AppConfig class.</returns>
        public static AppConfig LoadConfig(string testSuiteName, string testSuiteVersion, string testSuiteDir, string installDir)
        {
            AppConfig config = new AppConfig();
            config.TestSuiteName = testSuiteName;
            config.TestSuiteVersion = testSuiteVersion;
            config.InitFolders(testSuiteDir, installDir);

            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.DtdProcessing = DtdProcessing.Prohibit;
            XmlReader xmlReader = XmlReader.Create(System.IO.Path.Combine(config.EtcDirectory, StringResource.ConfigFilename), settings);
            doc.Load(xmlReader);
            //Config File
            XmlNode ptfFileNamesNode = doc.DocumentElement.SelectSingleNode("PtfFileNames");
            config.PtfConfigFiles = new List<string>();
            List<string> PtfConfigFileNames = new List<string>();
            foreach (XmlNode xn in ptfFileNamesNode.SelectNodes("PtfFileName"))
            {
                PtfConfigFileNames.Add(xn.InnerText.Trim());
            }
            //Config file in source code
            XmlNode ConfigFilePathNode = doc.DocumentElement.SelectSingleNode("ConfigFilePath");
            config.PtfConfigFilesInSource = new List<string>();
            if (ConfigFilePathNode != null)
            {
                foreach (XmlNode xn in ConfigFilePathNode.SelectNodes("PtfFileName"))
                {
                    config.PtfConfigFilesInSource.Add(
                        Path.Combine(testSuiteDir, xn.InnerText.Trim()));
                }
            }
            // Group Order
            config.PropertyGroupOrder = null;
            XmlNode groupOrder = doc.DocumentElement.SelectSingleNode("PropertyGroupOrder");
            if (groupOrder != null)
            {
                config.PropertyGroupOrder = new Dictionary<string, int>();
                foreach (XmlNode xn in groupOrder.SelectNodes("PropertyGroup"))
                {                    
                    string name = xn.Attributes["name"].Value;
                    int order = int.Parse(xn.Attributes["order"].Value);
                    config.PropertyGroupOrder.Add(name, order);
                }
            }

            // Detector
            string detectorAssembly = doc.DocumentElement.SelectSingleNode("AutoDetectionDllName").InnerText.Trim();

            config.DetectorAssemblyPath = Path.Combine(installDir, "lib", detectorAssembly);
            //Config Case
            config.TestSuiteAssembly = new List<string>();
            XmlNode DllFileNamesNode = doc.DocumentElement.SelectSingleNode("DllFileNames");
            foreach (XmlNode xn in DllFileNamesNode.SelectNodes("DllFileName"))
            {
                string name = xn.InnerText.Trim();
                config.TestSuiteAssembly.Add(System.IO.Path.Combine(testSuiteDir, name));
            }

            //TestSetting
            config.TestSetting = doc.DocumentElement.SelectSingleNode("TestSetting").InnerText.Trim();

            //Config Test Engine
            string vspath;
            if (Environment.GetEnvironmentVariable("VS110COMNTOOLS") != null &&
                File.Exists(vspath = Path.Combine(Environment.GetEnvironmentVariable("VS110COMNTOOLS"), StringResource.VSTestLocation)))
            {
                config.VSTestPath = vspath;
            }
            else if (Environment.GetEnvironmentVariable("VS120COMNTOOLS") != null &&
                File.Exists(vspath = Path.Combine(Environment.GetEnvironmentVariable("VS120COMNTOOLS"), StringResource.VSTestLocation)))
            {
                config.VSTestPath = vspath;
            }
            else if (Environment.GetEnvironmentVariable("VS140COMNTOOLS") != null &&
                File.Exists(vspath = Path.Combine(Environment.GetEnvironmentVariable("VS140COMNTOOLS"), StringResource.VSTestLocation)))
            {
                config.VSTestPath = vspath;
            }
            else
            {
                throw new Exception(StringResource.VSTestNotInstalled);
            }
            config.VSTestArguments = "";
            foreach (string singleDllpath in config.TestSuiteAssembly)
            {
                config.VSTestArguments += " " + singleDllpath;
            }
            config.VSTestArguments += " /Settings:\"" + config.TestSetting + "\"";

            // TestCategories
            var testCategoryNode = doc.DocumentElement.SelectSingleNode("TestCategories");
            if (testCategoryNode != null)
            {
                string categoryConfigFile = System.IO.Path.Combine(config.EtcDirectory, testCategoryNode.InnerText.Trim());
                config.TestCategory = GetTestCategoryFromConfig(categoryConfigFile);
            }
            else
            {
                config.TestCategory = null;
            }

            //Welcome Page
            config.TestSuiteIntroduction = new List<string>();

            foreach (XmlNode xn in doc.DocumentElement.SelectNodes("WelcomePage"))
            {
                foreach (XmlNode paragraphNode in xn.SelectNodes("Paragraph"))
                {
                    config.TestSuiteIntroduction.Add(paragraphNode.InnerText.Trim());
                }
            }

            //User Guide
            config.UserGuide = System.IO.Path.Combine(config.DocDirectory, "index.html");

            for (int i = 0; i < PtfConfigFileNames.Count; i++)
            {
                config.PtfConfigFiles.Add(System.IO.Path.Combine(testSuiteDir, "Bin", PtfConfigFileNames[i]));
            }

            config.RuleDefinitions = doc.DocumentElement.SelectSingleNode("ConfigCaseRule");

            // Adapters
            config.AdapterDefinitions = doc.DocumentElement.SelectSingleNode("Adapters");
            config.PredefinedAdapters = new List<PtfAdapterView>();
            if (config.AdapterDefinitions != null)
            {
                foreach (XmlNode node in config.AdapterDefinitions)
                {
                    string displayName = node.Attributes["displayname"].Value;
                    string name = node.Attributes["name"].Value;
                    PtfAdapterView adapterView = new PtfAdapterView()
                    {
                        Name = name,
                        FriendlyName = displayName
                    };

                    foreach (XmlNode subnode in node.SelectNodes("Adapter"))
                    {
                        string type = subnode.Attributes["type"].Value;
                        switch (type)
                        {
                            case "powershell":
                                {
                                    string scriptDir = subnode.Attributes["scriptdir"].Value;
                                    adapterView.PowerShellAdapter = new PowerShellAdapterNode(name, displayName, scriptDir);
                                }
                                break;
                            case "interactive":
                                adapterView.InteractiveAdapter = new InteractiveAdapterNode(name, displayName);
                                break;
                            case "managed":
                                {
                                    string adapterType = subnode.Attributes["adaptertype"].Value;
                                    adapterView.ManagedAdapter = new ManagedAdapterNode(name, displayName, adapterType);
                                }
                                break;
                            case "script":
                                {
                                    string scriptDir = subnode.Attributes["scriptdir"].Value;
                                    adapterView.ScriptAdapter = new ScriptAdapterNode(name, displayName, scriptDir);
                                }
                                break;
                        }
                    }
                    config.PredefinedAdapters.Add(adapterView);
                }
            }
            return config;
        }

        private void InitFolders(string testSuiteDir, string installDir)
        {
            if (!Directory.Exists(testSuiteDir))
            {
                throw new Exception(string.Format(StringResource.DirectoryNotFound, testSuiteDir));
            }
            TestSuiteDirectory = testSuiteDir;
            PtmInstallDirectory = installDir;
            DocDirectory = Path.Combine(installDir, "doc", TestSuiteName);
            EtcDirectory = Path.Combine(installDir, "etc", TestSuiteName);
            AppDataDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Protocol Test Manager",
                TestSuiteName,
                TestSuiteVersion);
        }

        /// <summary>
        /// Copy the ptfconfig files to the AppData directory as the default value.
        /// </summary>
        public void InitDefaultConfigurations()
        {
            if (!Directory.Exists(AppDataDirectory))
                Directory.CreateDirectory(AppDataDirectory);
            string defaultFileDir = System.IO.Path.Combine(AppDataDirectory, "default");
            if (!Directory.Exists(defaultFileDir)) Directory.CreateDirectory(defaultFileDir);
            DefaultPtfConfigFiles = new List<string>();
            foreach (string file in PtfConfigFiles)
            {
                string ptfConfigFileName = System.IO.Path.GetFileName(file);
                string defaultFile = System.IO.Path.Combine(defaultFileDir, ptfConfigFileName);
                if (!File.Exists(defaultFile)) File.Copy(file, defaultFile);
                DefaultPtfConfigFiles.Add(defaultFile);
            }
        }

        /// <summary>
        /// Loads the help messages of the methods in the adapters from the test suite assemblies.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        public void GetAdapterMethods()
        {
            if (PredefinedAdapters.Count > 0)
            {
                List<Type> adapters = new List<Type>();
                //Load adapters from test suite assemblies.
                foreach (string DllFileName in TestSuiteAssembly)
                {
                    Assembly assembly = Assembly.LoadFrom(DllFileName);
                    Type[] types = assembly.GetTypes();

                    foreach (Type type in types)
                    {
                        if (type.IsInterface)
                        {
                            var interfaces = type.GetInterfaces();
                            //If the interface is not inherited from IAdapter, ignore it.
                            if (interfaces.FirstOrDefault(o => o.FullName == "Microsoft.Protocols.TestTools.IAdapter") == null) continue;
                            adapters.Add(type);
                        }
                    }
                }
                foreach (PtfAdapterView adapterView in PredefinedAdapters)
                {
                    var adaptertype = adapters.FirstOrDefault(o => o.Name == adapterView.Name);
                    if (adaptertype != null)
                    {
                        foreach (MethodInfo method in adaptertype.GetMethods())
                        {
                            AdapterMethod m = new AdapterMethod(method);
                            adapterView.Methods.Add(m);
                        }
                    }
                }
            }
        }

        private static AppConfigTestCategory GetTestCategoryFromConfig(string categoryConfigFile)
        {
            if (string.IsNullOrEmpty(categoryConfigFile) || !File.Exists(categoryConfigFile))
            {
                return null;
            }

            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.DtdProcessing = DtdProcessing.Prohibit;
            XmlReader xmlReader = XmlReader.Create(categoryConfigFile, settings);
            doc.Load(xmlReader);

            AppConfigTestCategory testCategory = new AppConfigTestCategory();
            var testCaseNodes = doc.DocumentElement.SelectNodes("TestCase");            

            foreach (XmlNode testCaseNode in testCaseNodes)
            {
                AppConfigTestCase testCase = new AppConfigTestCase();
                // Get test case name.
                testCase.Name = testCaseNode.Attributes["name"].Value;

                // Get test case categories.
                List<string> testCategories = new List<string>();
                foreach (XmlNode categoryNode in testCaseNode.SelectNodes("Category"))
                {
                    string categoryName = categoryNode.Attributes["name"].Value;
                    if (!testCategories.Contains(categoryName))
                    {
                        testCategories.Add(categoryName);
                    }
                }
                testCase.Categories = testCategories;

                testCategory.TestCases.Add(testCase);
            }

            return testCategory;
        }

        private AppConfig()
        {
        }
    }

    /// <summary>
    /// The class for TestCategories node of config.xml
    /// </summary>
    public class AppConfigTestCategory
    {
        /// <summary>
        /// A list of test case.
        /// </summary>
        public List<AppConfigTestCase> TestCases { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AppConfigTestCategory()
        {
            TestCases = new List<AppConfigTestCase>();
        }
    }

    /// <summary>
    /// The class for TestCase node under TestCategories node of config.xml
    /// </summary>
    public class AppConfigTestCase
    {
        /// <summary>
        /// Test case name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A list of test category.
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AppConfigTestCase()
        {
            Categories = new List<string>();
        }
    }
}
