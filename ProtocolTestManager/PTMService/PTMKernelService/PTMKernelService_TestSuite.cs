// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        public ITestSuite[] QueryTestSuites()
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestSuiteInstallation>();

            var result = repo.Get(q => q).Select(item => GetTestSuiteInternal(item.Id, item)).ToArray();

            return result;
        }

        public ITestSuite GetTestSuite(int id)
        {
            return GetTestSuiteInternal(id, null);
        }

        public int InstallTestSuite(string name, string packageName, Stream package, string description)
        {
            var extractNode = ExtractPackage(packageName, package, StoragePool);
            var version = GetTestSuiteVersion(extractNode);

            if (!CheckTestSuiteVersion(version))
            {
                extractNode.DeleteNode();
                throw new NotSupportedException($"PTMService only supports version {TestSuiteConsts.SupportedMinVersion} or above version, you could try use PTMGUI to run previous versions");
            }

            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestSuiteInstallation>();

            var testSuiteInstallation = new TestSuiteInstallation
            {
                Name = name,
                InstallMethod = TestSuiteInstallMethod.UploadPackage,
                Description = description,
                Version = version
            };

            try
            {
                repo.Insert(testSuiteInstallation);

                pool.Save().Wait();

                int id = testSuiteInstallation.Id;
                var testSuiteNode = StoragePool.GetKnownNode(KnownStorageNodeNames.TestSuite).CreateNode(id.ToString());
                testSuiteNode.CopyFromNode(extractNode, true);
                testSuiteInstallation.Path = testSuiteNode.AbsolutePath;

                var testSuite = TestSuite.Create(Options.TestEnginePath, testSuiteInstallation, testSuiteNode);

                repo.Update(testSuiteInstallation);
                pool.Save().Wait();

                TestSuitePool.AddOrUpdate(id, _ => testSuite, (_, _) => testSuite);

                return id;
            }
            catch
            {
                repo.Remove(testSuiteInstallation);
                pool.Save().Wait();

                throw;
            }
        }

        public void UpdateTestSuite(int id, string name, string packageName, Stream package, string description)
        {
            var extractNode = ExtractPackage(packageName, package, StoragePool);
            var version = GetTestSuiteVersion(extractNode);
            if (!CheckTestSuiteVersion(version))
            {
                extractNode.DeleteNode();
                throw new NotSupportedException($"PTMService only supports version {TestSuiteConsts.SupportedMinVersion} or above version, you could try use PTMGUI to run previous versions");
            }

            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestSuiteInstallation>();

            var testSuiteInstallation = repo.Get(q => q.Where(item => item.Id == id)).First();
            testSuiteInstallation.Name = name;
            testSuiteInstallation.InstallMethod = TestSuiteInstallMethod.UploadPackage;
            testSuiteInstallation.Description = description;

            var testSuite = TestSuite.Update(Options.TestEnginePath, testSuiteInstallation, extractNode, StoragePool, version);

            repo.Update(testSuiteInstallation);

            pool.Save().Wait();

            TestSuitePool.AddOrUpdate(id, _ => testSuite, (_, _) => testSuite);
        }

        public void RemoveTestSuite(int id)
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestSuiteInstallation>();

            var testSuite = GetTestSuite(id);
            testSuite.Removed = true;

            var removedTestSuiteInstallation = new TestSuiteInstallation
            {
                Id = id,
                Name = testSuite.Name,
                Version = testSuite.Version,
                InstallMethod = testSuite.InstallMethod,
                Description = testSuite.Description,
                Path = testSuite.StorageRoot.AbsolutePath,
                Removed = testSuite.Removed,
            };

            repo.Update(removedTestSuiteInstallation);

            pool.Save().Wait();

            TestSuitePool.AddOrUpdate(id, _ => testSuite, (_, _) => testSuite);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                var flagFilePath = Path.Combine(testSuite.StorageRoot.AbsolutePath, "removed");
                File.WriteAllText(flagFilePath, $"Removed at {DateTime.Now}");
            });
        }

        private ITestSuite GetTestSuiteInternal(int id, TestSuiteInstallation testSuiteInstallation)
        {
            if (!TestSuitePool.ContainsKey(id))
            {
                if (testSuiteInstallation == null)
                {
                    using var instance = ScopedServiceFactory.GetInstance();

                    var pool = instance.ScopedServiceInstance;

                    var repo = pool.Get<TestSuiteInstallation>();

                    testSuiteInstallation = repo.Get(q => q.Where(item => item.Id == id)).First();
                }

                var testSuite = TestSuite.Open(Options.TestEnginePath, testSuiteInstallation, StoragePool);
                TestSuitePool.AddOrUpdate(id, _ => testSuite, (_, _) => testSuite);
            }

            return TestSuitePool[id];
        }

        private bool CheckTestSuiteVersion(string version)
        {
            Version currentVersion = new Version(version);
            Version minSupportedVersion = new Version(TestSuiteConsts.SupportedMinVersion);
            return (currentVersion >= minSupportedVersion);
        }

        private static IStorageNode ExtractPackage(string packageName, Stream package, IStoragePool storagePool)
        {
            string packageExtension = GetPackageExtension(packageName);
            var extractNode = storagePool.GetKnownNode(KnownStorageNodeNames.TestSuite).CreateNode(Guid.NewGuid().ToString());
            Utility.ExtractArchive(packageExtension, package, extractNode.AbsolutePath);
            return extractNode;
        }

        private static string GetPackageExtension(string packageName)
        {
            string packageExtension = (new FileInfo(packageName)).Extension;
            if (packageExtension.Equals(".gz") && packageName.EndsWith(".tar.gz"))
            {
                packageExtension = ".tar.gz";
            }

            return packageExtension;
        }

        private static string GetTestSuiteVersion(IStorageNode testSuiteInstallNode)
        {
            var binNode = testSuiteInstallNode.GetNode(TestSuiteConsts.Bin);
            using var versionFile = binNode.ReadFile(TestSuiteConsts.VersionFile);
            using var rs = new StreamReader(versionFile);
            var version = rs.ReadLine();
            return version;
        }
    }
}
