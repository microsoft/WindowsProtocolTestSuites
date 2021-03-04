// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.IO;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        public ITestSuite[] QueryTestSuites()
        {
            throw new NotImplementedException();
        }

        public ITestSuite GetTestSuite(int id)
        {
            throw new NotImplementedException();
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

            var testSuite = TestSuite.Create(testSuiteInstallation, packageName, package, StoragePool);

            repo.Update(testSuiteInstallation);

            pool.Save().Wait();

            TestSuitePool.Add(id, testSuite);

            return id;
        }
    }
}
