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
using System.Text;
using System.Xml;
using Rule = Microsoft.Protocols.TestManager.PTMService.Common.Types.Rule;
using RuleGroup = Microsoft.Protocols.TestManager.PTMService.Common.Types.RuleGroup;
using RuleSelectStatus = Microsoft.Protocols.TestManager.PTMService.Common.Types.RuleSelectStatus;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal class Configuration : IConfiguration
    {
        private Configuration(TestSuiteConfiguration testSuiteConfiguration, ITestSuite testSuite, IStorageNode storageRoot)
        {
            Id = testSuiteConfiguration.Id;

            TestSuite = testSuite;

            Name = testSuiteConfiguration.Name;

            Description = testSuiteConfiguration.Description;

            StorageRoot = storageRoot;

            LoadFeatureMappingFromXml();
        }

        private IEnumerable<RuleGroup> selectedRules;

        private int targetFilterIndex = -1;
        private int mappingFilterIndex = -1;
        private Dictionary<string, List<Rule>> featureMappingTable = new Dictionary<string, List<Rule>>();
        private Dictionary<string, List<Rule>> reverseMappingTable = new Dictionary<string, List<Rule>>();

        public int Id { get; private init; }

        public IEnumerable<RuleGroup> Rules
        {
            get
            {
                var profileStorage = StorageRoot.GetNode(ConfigurationConsts.ProfileConfig);
                RuleGroup[] ruleGroups = TestSuite.LoadTestCaseFilter();
                if (Directory.Exists(profileStorage.AbsolutePath))
                {
                    var query = profileStorage.GetFiles().ToList().Where(f => f.EndsWith(Path.Combine(profileStorage.AbsolutePath, ConfigurationConsts.Profile)));
                    if (query.Any())
                    {
                        LoadSelectedRules(ruleGroups, query.First());
                    }
                }

                // create mapping table
                CreateMappingTable(ruleGroups);

                return ruleGroups;
            }
            set
            {
                var profileStorage = StorageRoot.GetNode(ConfigurationConsts.ProfileConfig);
                var profileXmlPath = Path.Combine(profileStorage.AbsolutePath, ConfigurationConsts.Profile);

                XmlDocument xmlDoc = new XmlDocument();
                var rootElement = xmlDoc.CreateElement("RuleGroups");
                foreach (var group in value)
                {
                    var ruleGroupEle = xmlDoc.CreateElement("RuleGroup");
                    ruleGroupEle.SetAttribute("name", group.Name);

                    if (group.Rules != null && group.Rules.Count > 0)
                    {
                        Stack<KeyValuePair<Common.Types.Rule,string>> ruleStack = new Stack<KeyValuePair<Common.Types.Rule, string>>();
                        foreach (var rule in group.Rules)
                        {
                            ruleStack.Push(new KeyValuePair<Common.Types.Rule, string>(rule,group.Name));
                        }
                        while (ruleStack.Count > 0)
                        {
                            var ruleStackItem = ruleStack.Pop();
                            Common.Types.Rule myRule = ruleStackItem.Key;
                            string parentPath = ruleStackItem.Value;
                            if (myRule.SelectStatus != RuleSelectStatus.UnSelected && myRule.Count == 0)
                            {
                                var ruleEle = xmlDoc.CreateElement("Rule");
                                // When parent name is already in rule name, we needn't to add it again.
                                if (myRule.Name.Contains(parentPath))
                                {
                                    ruleEle.SetAttribute("name", myRule.Name);
                                }
                                else
                                {
                                    ruleEle.SetAttribute("name", parentPath + '.' + myRule.Name);
                                }
                                ruleGroupEle.AppendChild(ruleEle);
                            }
                            foreach (var childRule in myRule)
                            {
                                ruleStack.Push(new KeyValuePair<Common.Types.Rule, string>(childRule, parentPath + '.' + myRule.Name));
                            }
                        }
                    }

                    rootElement.AppendChild(ruleGroupEle);
                }

                xmlDoc.AppendChild(rootElement);

                if (!Directory.Exists(profileStorage.AbsolutePath))
                {
                    Directory.CreateDirectory(profileStorage.AbsolutePath);
                }
                using var xmlTextWriter = new XmlTextWriter(profileXmlPath, Encoding.UTF8);
                xmlDoc.Save(xmlTextWriter);
            }
        }

        public IEnumerable<RuleGroup> SelectedRules
        {
            get { return this.selectedRules; }
        }

        public int TargetFilterIndex 
        { 
            get { return this.targetFilterIndex; } 
        }

        public int MappingFilterIndex
        {
            get { return this.mappingFilterIndex; }
        }

        public Dictionary<string, List<Rule>> FeatureMappingTable
        {
            get { return this.featureMappingTable; }
        }

        public Dictionary<string, List<Rule>> ReverseMappingTable
        {
            get { return this.reverseMappingTable; }
        }

        public bool IsConfigured
        {
            get
            {
                return File.Exists(Path.Combine(this.StorageRoot.AbsolutePath, @"config\profile.xml"));
            }
        }

        private void LoadFeatureMappingFromXml()
        {
            TestSuite.LoadFeatureMappingFromXml(out targetFilterIndex, out mappingFilterIndex);
        }

        private void CreateMappingTable(RuleGroup[] ruleGroups)
        {
            featureMappingTable = new Dictionary<string, List<Rule>>();
            reverseMappingTable = new Dictionary<string, List<Rule>>();
            RuleGroup targetFilterGroup = ruleGroups[targetFilterIndex];
            RuleGroup mappingFilterGroup = ruleGroups[mappingFilterIndex];
            Dictionary<string, Rule> mappingRuleTable = CreateRuleTableFromRuleGroup(mappingFilterGroup);
            Dictionary<string, Rule> targetRuleTable = CreateRuleTableFromRuleGroup(targetFilterGroup);

            var testCaseList = TestSuite.GetTestCases(null);

            foreach (TestCaseInfo testCase in testCaseList)
            {
                List<string> categories = testCase.Category.ToList();
                foreach (string target in targetRuleTable.Keys)
                {
                    if (categories.Contains(target))
                    {
                        Rule currentRule;
                        foreach (string category in categories)
                        {
                            if (!category.Equals(target))
                            {
                                mappingRuleTable.TryGetValue(category, out currentRule);
                                if (currentRule == null)
                                {
                                    continue;
                                }
                                updateMappingTable(featureMappingTable, target, currentRule);
                                // Add item to reverse mapping table
                                updateMappingTable(reverseMappingTable, category, targetRuleTable[target]);                                
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void updateMappingTable(Dictionary<string, List<Rule>> mappingTable, string target, Rule currentRule)
        {
            if (mappingTable.ContainsKey(target))
            {
                if (!mappingTable[target].Contains(currentRule))
                {
                    mappingTable[target].Add(currentRule);
                }
            }
            else
            {
                mappingTable[target] = new List<Rule> { currentRule };
            }
        }

        /// <summary>
        /// Create a dictionary (key: rule name, value: rule) to store rules from a given ruleGroup to speedup rule lookup performance
        /// </summary>
        /// <param name="ruleGroup">A rule group</param>
        /// <returns>A rule table</returns>
        private Dictionary<string, Rule> CreateRuleTableFromRuleGroup(RuleGroup ruleGroup)
        {
            Dictionary<string, Rule> ruleTable = new Dictionary<string, Rule>();
            Stack<Rule> ruleStack = new Stack<Rule>();
            foreach (Rule r in ruleGroup.Rules) ruleStack.Push(r);
            while (ruleStack.Count > 0)
            {
                Rule r = ruleStack.Pop();
                if (r.Categories.Length > 0 &&
                    !ruleTable.ContainsKey(r.Categories[0]))
                {
                    ruleTable.Add(r.Categories[0], r);
                }
                foreach (Rule childRule in r) ruleStack.Push(childRule);
            }
            return ruleTable;
        }
        public IEnumerable<PropertyGroup> Properties
        {
            get
            {
                var ptfConfigStorage = StorageRoot.GetNode(ConfigurationConsts.PtfConfig);

                var ptfConfig = new PtfConfig(ptfConfigStorage.GetFiles().ToList());

                var result = ptfConfig.FileProperties.Values.SelectMany(property => property).Select(property =>
                {
                    var configItem = ptfConfig.GetPropertyNodeByName(property);

                    return new Property
                    {
                        Key = property,
                        Name = property.Split('.').Last(),
                        Choices = configItem.ChoiceItems,
                        Description = configItem.Description,
                        Value = configItem.Value,
                    };
                }).GroupBy(property =>
                {
                    var parts = property.Key.Split('.');

                    if (parts.Length == 1)
                    {
                        return ConfigurationConsts.DefaultGroup;
                    }
                    else
                    {
                        return parts.SkipLast(1).Last();
                    }
                }).Select(group =>
                {
                    return new PropertyGroup
                    {
                        Name = group.Key,
                        Items = group,
                    };
                });

                return result;
            }

            set
            {
                var ptfConfigStorage = StorageRoot.GetNode(ConfigurationConsts.PtfConfig);

                var ptfConfig = new PtfConfig(ptfConfigStorage.GetFiles().ToList());

                var properties = value.SelectMany(i => i.Items);

                foreach (var property in properties)
                {
                    ptfConfig.SetPropertyValue(property.Key, property.Value);
                }

                ptfConfig.Save();
            }
        }

        public IEnumerable<Adapter> Adapters
        {
            get
            {
                var ptfConfigStorage = StorageRoot.GetNode(ConfigurationConsts.PtfConfig);

                var ptfConfig = new PtfConfig(ptfConfigStorage.GetFiles().ToList());

                var result = ptfConfig.adapterTable.Select(kvp =>
                {
                    string name = kvp.Key;

                    var node = kvp.Value;

                    string type = node.Attributes[ConfigurationConsts.AdapterKindAttributeName].Value;

                    var adapter = new Adapter
                    {
                        Name = name,
                        DisplayName = name,
                        SupportedKinds = Array.Empty<AdapterKind>()
                    };

                    switch (type)
                    {
                        case ConfigurationConsts.AdapterKindManaged:
                            {
                                adapter.Kind = AdapterKind.Managed;

                                adapter.AdapterType = node.Attributes[ConfigurationConsts.AdapterTypeAttributeName].Value;

                                adapter.ScriptDirectory = null;

                                adapter.ShellScriptDirectory = null;
                            }
                            break;

                        case ConfigurationConsts.AdapterKindPowerShell:
                            {
                                adapter.Kind = AdapterKind.PowerShell;

                                adapter.AdapterType = null;

                                adapter.ScriptDirectory = node.Attributes[ConfigurationConsts.AdapterScriptDirectoryAttributeName].Value;

                                adapter.ShellScriptDirectory = null;
                            }
                            break;

                        case ConfigurationConsts.AdapterKindShell:
                            {
                                adapter.Kind = AdapterKind.Shell;

                                adapter.AdapterType = null;

                                adapter.ScriptDirectory = null;

                                adapter.ShellScriptDirectory = node.Attributes[ConfigurationConsts.AdapterScriptDirectoryAttributeName].Value;
                            }
                            break;

                        case ConfigurationConsts.AdapterKindInteractive:
                            {
                                adapter.Kind = AdapterKind.Interactive;

                                adapter.AdapterType = null;

                                adapter.ScriptDirectory = null;

                                adapter.ShellScriptDirectory = null;
                            }
                            break;

                        default:
                            throw new InvalidOperationException("The adapter type is invalid or not supported.");
                    }

                    return adapter;
                });

                return result;
            }

            set
            {
                var ptfConfigStorage = StorageRoot.GetNode(ConfigurationConsts.PtfConfig);

                var ptfConfig = new PtfConfig(ptfConfigStorage.GetFiles().ToList());

                foreach (var adapter in value)
                {
                    IAdapterConfig config;

                    switch (adapter.Kind)
                    {
                        case AdapterKind.Managed:
                            {
                                config = new ManagedAdapterNode(adapter.Name, adapter.DisplayName, adapter.AdapterType);
                            }
                            break;

                        case AdapterKind.PowerShell:
                            {
                                config = new PowerShellAdapterNode(adapter.Name, adapter.DisplayName, adapter.ScriptDirectory);
                            }
                            break;

                        case AdapterKind.Shell:
                            {
                                config = new ShellAdapterNode(adapter.Name, adapter.DisplayName, adapter.ShellScriptDirectory);
                            }
                            break;

                        case AdapterKind.Interactive:
                            {
                                config = new InteractiveAdapterNode(adapter.Name, adapter.DisplayName);
                            }
                            break;

                        default:
                            throw new InvalidOperationException("The adapter type is invalid or not supported.");
                    }

                    ptfConfig.ApplyAdapterConfig(config);

                    ptfConfig.Save();
                }
            }
        }

        public string Description { get; set; }

        public string Name { get; set; }

        public IStorageNode StorageRoot { get; private init; }

        public ITestSuite TestSuite { get; private init; }

        public static Configuration Create(TestSuiteConfiguration testSuiteConfiguration, ITestSuite testSuite, IStoragePool storagePool)
        {
            var configurationNode = storagePool.GetKnownNode(KnownStorageNodeNames.Configuration).CreateNode(testSuiteConfiguration.Id.ToString());

            testSuiteConfiguration.Path = configurationNode.AbsolutePath;

            var files = testSuite.GetConfigurationFiles();

            var ptfConfigStorage = configurationNode.CreateNode(ConfigurationConsts.PtfConfig);

            foreach (string file in files)
            {
                string name = Path.GetFileName(file);

                File.Copy(file, Path.Combine(ptfConfigStorage.AbsolutePath, name));
            }

            var result = new Configuration(testSuiteConfiguration, testSuite, configurationNode);

            return result;
        }

        public static Configuration Open(TestSuiteConfiguration testSuiteConfiguration, ITestSuite testSuite, IStoragePool storagePool)
        {
            var storageNode = storagePool.OpenNode(testSuiteConfiguration.Path);

            var result = new Configuration(testSuiteConfiguration, testSuite, storageNode);

            return result;
        }

        public IEnumerable<string> GetApplicableTestCases()
        {
            // In order to get the actual applicable test cases, support of plugin is needed.
            // So far, we return all test cases.
            var result = TestSuite.GetTestCases(null).Select(testCaseInfo => testCaseInfo.FullName);

            return result;
        }

        private bool LoadSelectedRules(RuleGroup[] ruleGroups, string profilePath)
        {
            XmlDocument ruleGroupFile = new XmlDocument();
            var selectedRuleGroups = new List<RuleGroup>();
            try
            {
                ruleGroupFile.Load(profilePath);

                foreach (XmlNode ruleGroupNode in ruleGroupFile.DocumentElement.SelectNodes("RuleGroup"))
                {
                    var ruleGroup = new RuleGroup()
                    {
                        Name = ruleGroupNode.Attributes["name"].Value,
                        Rules = new List<Common.Types.Rule>(),
                    };

                    foreach (XmlNode ruleNode in ruleGroupNode.SelectNodes("Rule"))
                    {
                        string ruleName = ruleNode.Attributes["name"].Value;
                        string searchRuleName = ruleName.Contains('%') ?
                            ruleName.Split('%')[0] :
                            ruleName;

                        Common.Types.Rule rule = FindRuleByName(ruleGroups, searchRuleName);
                        if (rule != null)
                        {
                            rule.SelectStatus = RuleSelectStatus.Selected;
                            ruleGroup.Rules.Add(new Common.Types.Rule() { Name = ruleName });
                        }
                    }

                    selectedRuleGroups.Add(ruleGroup);
                }
                this.selectedRules = selectedRuleGroups;
            }

            catch (XmlException e)
            {
                throw new XmlException(string.Format("Load profile failed", e.Message));
            }
            //Apply rules
            return true;
        }

        private Common.Types.Rule FindRuleByName(RuleGroup[] ruleGroups, string RuleName)
        {
            string[] ruleParts = RuleName.Split('.');
            int level = 0;
            foreach (RuleGroup ruleGroup in ruleGroups)
            {
                if (ruleGroup.Name == ruleParts[0])
                {
                    level = 1;
                    Stack<Common.Types.Rule> ruleStack = new Stack<Common.Types.Rule>();
                    foreach (Common.Types.Rule childRule in ruleGroup.Rules)
                    {
                        ruleStack.Push(childRule);
                    }
                    while (ruleStack.Count != 0)
                    {
                        Common.Types.Rule topRule = ruleStack.Pop();
                        if (topRule.Name == ruleParts[level])
                        {
                            level++;
                            if (level == ruleParts.Length)
                            {
                                return topRule;
                            }
                            foreach (Common.Types.Rule childRule in topRule)
                            {
                                ruleStack.Push(childRule);
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
