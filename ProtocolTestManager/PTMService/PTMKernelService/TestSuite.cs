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

            // check if config.xml can be found in TestSuite, if not then copy from etc folder.
            string configXmlPath = Path.Combine(StorageRoot.GetNode(TestSuiteConsts.Bin).AbsolutePath, TestSuiteConsts.ConfigXml);
            if (!File.Exists(configXmlPath))
            {
                // TODO: Plugin files will add into TestSuite
                File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"etc//FileServer//{TestSuiteConsts.ConfigXml}"), configXmlPath);
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

        public string Description { get; set; }

        public bool Removed { get; set; }

        public IStorageNode StorageRoot { get; private init; }

        public IEnumerable<string> GetConfigurationFiles()
        {
            var binNode = StorageRoot.GetNode(TestSuiteConsts.Bin);

            var result = binNode.GetFiles().Where(name => Path.GetExtension(name) == ".ptfconfig");

            return result;
        }

        public static ITestSuite Create(string testEnginePath, TestSuiteInstallation testSuiteInstallation, string packageName, Stream package, IStoragePool storagePool)
        {
            int id = testSuiteInstallation.Id;

            var node = storagePool.GetKnownNode(KnownStorageNodeNames.TestSuite).CreateNode(id.ToString());

            testSuiteInstallation.Path = node.AbsolutePath;

            string packageExtension = "";

            string nameWithoutExtension = packageName;

            while (true)
            {
                string extension = Path.GetExtension(nameWithoutExtension);

                nameWithoutExtension = Path.GetFileNameWithoutExtension(nameWithoutExtension);

                if (String.IsNullOrEmpty(extension))
                {
                    break;
                }

                packageExtension = extension + packageExtension;
            }

            Utility.ExtractArchive(packageExtension, package, testSuiteInstallation.Path);

            var binNode = node.GetNode(TestSuiteConsts.Bin);

            using var versionFile = binNode.ReadFile(TestSuiteConsts.VersionFile);

            using var rs = new StreamReader(versionFile);

            testSuiteInstallation.Version = rs.ReadLine();

            var result = new TestSuite(testEnginePath, testSuiteInstallation, node);

            return result;
        }

        public static TestSuite Open(string testEnginePath, TestSuiteInstallation testSuiteInstallation, IStoragePool storagePool)
        {
            var node = storagePool.OpenNode(testSuiteInstallation.Path);

            var result = new TestSuite(testEnginePath, testSuiteInstallation, node);

            return result;
        }

        public static ITestSuite Update(string testEnginePath, TestSuiteInstallation testSuiteInstallation, string packageName, Stream package, IStoragePool storagePool)
        {
            int id = testSuiteInstallation.Id;

            var updatingNode = storagePool.GetKnownNode(KnownStorageNodeNames.TestSuite).CreateNode($"{id}_Updating_{Guid.NewGuid()}");

            testSuiteInstallation.Path = updatingNode.AbsolutePath;

            string packageExtension = "";

            string nameWithoutExtension = packageName;

            while (true)
            {
                string extension = Path.GetExtension(nameWithoutExtension);

                nameWithoutExtension = Path.GetFileNameWithoutExtension(nameWithoutExtension);

                if (string.IsNullOrEmpty(extension))
                {
                    break;
                }

                packageExtension = extension + packageExtension;
            }

            Utility.ExtractArchive(packageExtension, package, testSuiteInstallation.Path);

            var binNode = updatingNode.GetNode(TestSuiteConsts.Bin);

            using var versionFile = binNode.ReadFile(TestSuiteConsts.VersionFile);

            using var rs = new StreamReader(versionFile);

            testSuiteInstallation.Version = rs.ReadLine();

            IStorageNode node = null;
            string oldNodePath = "";
            lock (syncRoot)
            {
                node = storagePool.GetKnownNode(KnownStorageNodeNames.TestSuite).GetNode($"{id}");
                oldNodePath = Path.Combine(node.Parent.AbsolutePath, $"{id}_Old_{Guid.NewGuid()}");

                Directory.Move(node.AbsolutePath, oldNodePath);
                Directory.Move(updatingNode.AbsolutePath, node.AbsolutePath);

                testSuiteInstallation.Path = node.AbsolutePath;
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

        public RuleGroup[] LoadTestCaseFilter()
        {
            TestCaseFilter filter = new TestCaseFilter();
            XmlDocument doc = new XmlDocument();
            doc.Load(this.TestSuiteConfigFilePath);
            XmlNode configCaseRule = doc.DocumentElement.SelectSingleNode(TestSuiteConsts.ConfigCaseRule);

            var groups = configCaseRule.SelectNodes("Group");
            foreach (XmlNode group in groups)
            {
                Kernel.RuleGroup gp = Kernel.RuleGroup.FromXmlNode(group);
                filter.Add(gp);
            }

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

        public string GetDetectorAssembly()
        {
            string detectorAssembly = null;
            string configXmlPath = Path.Combine(StorageRoot.GetNode(TestSuiteConsts.Bin).AbsolutePath, TestSuiteConsts.ConfigXml);
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
