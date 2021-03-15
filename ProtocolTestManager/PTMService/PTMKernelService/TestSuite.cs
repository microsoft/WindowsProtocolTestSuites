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

            InstallMethod = testSuiteInstallation.InstallMethod;

            Version = testSuiteInstallation.Version;

            StorageRoot = storageRoot;
        }

        private string TestEnginePath { get; init; }

        public int Id { get; private init; }

        public string Version { get; set; }

        public TestSuiteInstallMethod InstallMethod { get; private init; }

        public string Name { get; set; }

        public string Description { get; set; }

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
            // In order to get the actual test assemblies, support of plugin is needed.
            // So far, we return hard-coded dll files.
            var fakeDlls = new List<string>
            {
                "MS-SMB2_ServerTestSuite.dll",
            };

            var result = fakeDlls.Select(name => Path.Combine(StorageRoot.GetNode(TestSuiteConsts.Bin).AbsolutePath, name));

            return result;
        }
    }
}
