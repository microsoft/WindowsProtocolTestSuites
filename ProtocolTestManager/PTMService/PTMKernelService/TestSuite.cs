// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using Rule = Microsoft.Protocols.TestManager.Kernel.Rule;
using RuleGroup = Microsoft.Protocols.TestManager.PTMService.Common.Types.RuleGroup;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal class TestSuite : ITestSuite
    {
        private TestSuite(string testEnginePath, TestSuiteInstallation testSuiteInstallation, IStorageNode storageRoot)
        {
            TestEnginePath = testEnginePath;

            Id = testSuiteInstallation.Id;

            Name = testSuiteInstallation.Name;

            Description = testSuiteInstallation.Description;

            Removed = testSuiteInstallation.Removed;

            InstallMethod = testSuiteInstallation.InstallMethod;

            Version = testSuiteInstallation.Version;

            StorageRoot = storageRoot;

            // check if Plugin/config.xml can be found in TestSuite, if not then throw a new exception.
            string configXmlPath = Path.Combine(StorageRoot.GetNode(TestSuiteConsts.PluginFolderName).AbsolutePath, TestSuiteConsts.PluginConfigXml);
            if (!File.Exists(configXmlPath))
            {
                throw new Exception($"TestSuite requires plugin xml:{configXmlPath}!");
            }
            this.TestSuiteConfigFilePath = configXmlPath;
        }

        private static object syncRoot = new object();

        private string TestEnginePath { get; init; }

        private string TestSuiteConfigFilePath { get; init; }

        public int Id { get; private init; }

        public string Version { get; set; }

        public TestSuiteInstallMethod InstallMethod { get; private init; }

        public string Name { get; set; }

        private string testSuiteName;

        public string TestSuiteName
        {
            get
            {
                if (string.IsNullOrEmpty(testSuiteName))
                {
                    var doc = new XmlDocument();
                    doc.Load(this.TestSuiteConfigFilePath);
                    var testSuiteNameNode = doc.DocumentElement.SelectSingleNode(TestSuiteConsts.TestSuiteName);

                    // Fall back to test suite installation name when test suite name is not provided.
                    if (testSuiteNameNode == null)
                    {
                        testSuiteName = Name;
                    }
                    else
                    {
                        testSuiteName = testSuiteNameNode.InnerText;
                    }

                }

                return testSuiteName;
            }
        }

        public string Description { get; set; }

        public bool Removed { get; set; }

        public IStorageNode StorageRoot { get; private init; }

        public IEnumerable<string> GetConfigurationFiles()
        {
            var binNode = StorageRoot.GetNode(TestSuiteConsts.Bin);

            var result = binNode.GetFiles().Where(name => Path.GetExtension(name) == ".ptfconfig");

            return result;
        }

        public static ITestSuite Create(string testEnginePath, TestSuiteInstallation testSuiteInstallation, IStorageNode testSuiteNode)
        {
            return new TestSuite(testEnginePath, testSuiteInstallation, testSuiteNode);
        }

        public static TestSuite Open(string testEnginePath, TestSuiteInstallation testSuiteInstallation, IStoragePool storagePool)
        {
            var node = storagePool.OpenNode(testSuiteInstallation.Path);

            var result = new TestSuite(testEnginePath, testSuiteInstallation, node);

            return result;
        }

        public static ITestSuite Update(string testEnginePath, TestSuiteInstallation testSuiteInstallation, IStorageNode extractNode, IStoragePool storagePool, string version)
        {
            int id = testSuiteInstallation.Id;

            IStorageNode node = null;
            string oldNodePath = "";
            lock (syncRoot)
            {
                node = storagePool.GetKnownNode(KnownStorageNodeNames.TestSuite).GetNode($"{id}");
                oldNodePath = Path.Combine(node.Parent.AbsolutePath, $"{id}_Old_{Guid.NewGuid()}");

                Directory.Move(node.AbsolutePath, oldNodePath);
                node.CopyFromNode(extractNode, true);

                testSuiteInstallation.Path = node.AbsolutePath;
                testSuiteInstallation.Version = version;
            }

            var result = new TestSuite(testEnginePath, testSuiteInstallation, node);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (Directory.Exists(oldNodePath))
                {
                    var flagFilePath = Path.Combine(oldNodePath, "updated");
                    File.WriteAllText(flagFilePath, $"Updated at {DateTime.Now}");
                }
            });

            return result;
        }

        public IEnumerable<TestCaseInfo> GetTestCases(string filter)
        {
            var testEngine = new TestEngine(TestEnginePath)
            {
                TestAssemblies = GetTestAssemblies().ToList(),
                WorkingDirectory = $"{StorageRoot.AbsolutePath}{Path.DirectorySeparatorChar}",
            };

            var result = testEngine.LoadTestCases(filter).Select(testCase => new TestCaseInfo
            {
                Name = testCase.Name,
                FullName = testCase.FullName,
                Category = testCase.Category.ToArray(),
                Description = testCase.Description,
                ToolTipOnUI = testCase.ToolTipOnUI,
            });

            return result;
        }

        public IEnumerable<string> GetTestAssemblies()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.TestSuiteConfigFilePath);
            XmlNode DllFileNamesNode = doc.DocumentElement.SelectSingleNode(TestSuiteConsts.DllFileNames);

            var assemblyList = new List<string>();
            foreach (XmlNode xn in DllFileNamesNode.SelectNodes("DllFileName"))
            {
                string dllFileName = xn.InnerText.Trim();
                assemblyList.Add(dllFileName);
            }

            var result = assemblyList.Select(name => Path.Combine(StorageRoot.GetNode(TestSuiteConsts.Bin).AbsolutePath, name));
            return result;
        }

        public IEnumerable<Adapter> GetPluginAdapters()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.TestSuiteConfigFilePath);
            XmlNode AdapterNode = doc.DocumentElement.SelectSingleNode(TestSuiteConsts.Adapters);
            var adapterList = new List<Adapter>();
            if (AdapterNode != null && AdapterNode.ChildNodes.Count > 0)
            {
                foreach (XmlNode xn in AdapterNode.SelectNodes("Adapter"))
                {
                    var nameAttribute = xn.Attributes["name"];
                    if (nameAttribute != null)
                    {
                        string name = nameAttribute.Value;
                        string scriptdir = string.Empty;
                        string shellscriptdir = string.Empty;
                        string adaptertype = string.Empty;
                        AdapterKind kind = AdapterKind.Managed;
                        List<AdapterKind> supportedKinds = new List<AdapterKind>();
                        foreach (XmlNode ixn in xn.SelectNodes("Adapter"))
                        {
                            var typeAttribute = ixn.Attributes["type"];
                            if (typeAttribute != null)
                            {
                                string type = typeAttribute.Value;
                                kind = (AdapterKind)Enum.Parse(kind.GetType(), type, true);
                                supportedKinds.Add(kind);

                                if (kind == AdapterKind.PowerShell)
                                {
                                    var scriptdirAttribute = ixn.Attributes["scriptdir"];
                                    if (scriptdirAttribute != null)
                                    {
                                        scriptdir = scriptdirAttribute.Value;
                                    }
                                }
                                else if (kind == AdapterKind.Shell)
                                {
                                    var shellscriptdirAttribute = ixn.Attributes["scriptdir"];
                                    if (shellscriptdirAttribute != null)
                                    {
                                        shellscriptdir = shellscriptdirAttribute.Value;
                                    }
                                }
                                else if (kind == AdapterKind.Managed)
                                {
                                    var adaptertypeAttribute = ixn.Attributes["adaptertype"];
                                    if (adaptertypeAttribute != null)
                                    {
                                        adaptertype = adaptertypeAttribute.Value;
                                    }
                                }
                            }
                        }
                        var displaynameAttribute = xn.Attributes["displayname"];
                        string displayname = string.Empty;
                        if (displaynameAttribute != null)
                        {
                            displayname = displaynameAttribute.Value;
                        }

                        adapterList.Add(new Adapter()
                        {
                            Name = name,
                            DisplayName = string.IsNullOrEmpty(displayname) ? name : displayname,
                            AdapterType = adaptertype,
                            Kind = supportedKinds.Count > 0 ? supportedKinds[0] : kind,
                            ScriptDirectory = scriptdir,
                            SupportedKinds = supportedKinds.ToArray(),
                            ShellScriptDirectory = shellscriptdir
                        });
                    }
                }
            }

            return adapterList;
        }

        /// <summary>
        /// Gets the TestCaseFilter object for current test suite.
        /// </summary>
        /// <returns>A TestCaseFilter object</returns>
        public TestCaseFilter GetTestCaseFilter()
        {
            var filter = new TestCaseFilter();
            XmlDocument doc = new XmlDocument();
            doc.Load(this.TestSuiteConfigFilePath);
            XmlNode configCaseRule = doc.DocumentElement.SelectSingleNode(TestSuiteConsts.ConfigCaseRule);

            var groups = configCaseRule.SelectNodes("Group");
            foreach (XmlNode group in groups)
            {
                Kernel.RuleGroup gp = Kernel.RuleGroup.FromXmlNode(group);
                filter.Add(gp);
            }
            return filter;
        }

        public RuleGroup[] LoadTestCaseFilter()
        {
            var filter = GetTestCaseFilter();

            var ruleGroups = new List<RuleGroup>();

            foreach (var group in filter)
            {
                //ruleGroups.Add()
                RuleGroup ruleGroup = new RuleGroup()
                {
                    Name = group.Name,
                    DisplayName = group.Name,
                    Rules = new List<Common.Types.Rule>()
                };
                AddItems(ruleGroup.Rules, group);
                ruleGroups.Add(ruleGroup);
            }

            return ruleGroups.ToArray();
        }


        public void LoadFeatureMappingFromXml(out int targetFilterIndex, out int mappingFilterIndex)
        {
            targetFilterIndex = -1;
            mappingFilterIndex = -1;

            XmlDocument doc = new XmlDocument();
            doc.Load(this.TestSuiteConfigFilePath);
            XmlNode configCaseRule = doc.DocumentElement.SelectSingleNode(TestSuiteConsts.ConfigCaseRule);
            var groups = configCaseRule.SelectNodes("Group");

            XmlNode featureMappingNode = doc.DocumentElement.SelectSingleNode(TestSuiteConsts.FeatureMapping);
            if (featureMappingNode == null)
            {
                return;
            }

            // Parse Config section
            var featureMappingConfig = featureMappingNode.SelectSingleNode("Config");
            Dictionary<string, int> configTable = GetFeatureMappingConfigFromXmlNode(featureMappingConfig);
            if (configTable.TryGetValue("targetFilterIndex", out int _targetFilterIndex) && configTable.TryGetValue("mappingFilterIndex", out int _mappingFilterIndex))
            {
                if ((_targetFilterIndex == _mappingFilterIndex) ||
                (_targetFilterIndex >= groups.Count || _mappingFilterIndex >= groups.Count))
                {
                    return;
                }
                targetFilterIndex = _targetFilterIndex;
                mappingFilterIndex = _mappingFilterIndex;
            }
        }

        /// <summary>
        /// Create a config table from a given xml node
        /// </summary>
        /// <param name="featureMappingConfig"></param>
        /// <returns>A feature mapping config table</returns>
        private Dictionary<string, int> GetFeatureMappingConfigFromXmlNode(XmlNode featureMappingConfig)
        {
            Dictionary<string, int> featureMappingConfigTable = new Dictionary<string, int>();
            var configs = featureMappingConfig.SelectNodes("Config");
            foreach (XmlNode config in configs)
            {
                featureMappingConfigTable.Add(config.Attributes[0].Value, Convert.ToInt32(config.Attributes[1].Value));
            }
            return featureMappingConfigTable;
        }

        public string GetDetectorAssembly()
        {
            string detectorAssembly = null;
            string configXmlPath = Path.Combine(StorageRoot.GetNode(TestSuiteConsts.PluginFolderName).AbsolutePath, TestSuiteConsts.PluginConfigXml);
            XmlDocument doc = new XmlDocument();
            doc.Load(configXmlPath);
            XmlNode autoDetectionDllNode = doc.DocumentElement.SelectSingleNode(TestSuiteConsts.AutoDetectionDllName);

            detectorAssembly = autoDetectionDllNode.InnerText.Trim();
            var detectorAssemblyPath = Path.Combine(StorageRoot.GetNode(TestSuiteConsts.Bin).AbsolutePath, detectorAssembly);
            return detectorAssemblyPath;
        }

        private void AddItems(IList<Common.Types.Rule> displayRules, List<Rule> rules)
        {
            foreach (var rule in rules)
            {
                Common.Types.Rule displayRule = new Common.Types.Rule()
                {
                    DisplayName = rule.Name,
                    Name = rule.Name,
                    Categories = rule.CategoryList.ToArray(),
                    SelectStatus = Common.Types.RuleSelectStatus.UnSelected,
                };

                if (rule.Count > 0)
                {
                    AddItems(displayRule, rule);
                }
                displayRules.Add(displayRule);
            }
        }

    }
}
