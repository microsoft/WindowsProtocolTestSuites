// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System.IO;
using System.Linq;

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
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestSuiteInstallation>();

            var testSuiteInstallation = new TestSuiteInstallation
            {
                Name = name,
                InstallMethod = TestSuiteInstallMethod.UploadPackage,
                Description = description,
            };

            repo.Insert(testSuiteInstallation);

            pool.Save().Wait();

            int id = testSuiteInstallation.Id;

            var testSuite = TestSuite.Create(Options.TestEnginePath, testSuiteInstallation, packageName, package, StoragePool);

            repo.Update(testSuiteInstallation);

            pool.Save().Wait();

            TestSuitePool.AddOrUpdate(id, _ => testSuite, (_, _) => testSuite);

            return id;
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
    }
}
