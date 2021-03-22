// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using System;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        public ITestRun GetTestRun(int id)
        {
            if (!TestRunPool.ContainsKey(id))
            {
                using var instance = ScopedServiceFactory.GetInstance();

                var pool = instance.ScopedServiceInstance;

                var repo = pool.Get<TestResult>();

                var testResult = repo.Get(q => q.Where(item => item.Id == id)).First();

                var configuration = GetConfiguration(testResult.TestSuiteConfigurationId);

                TestRunPool.Add(id, TestRun.Open(Options.TestEnginePath, testResult, configuration, StoragePool, Update));
            }

            return TestRunPool[id];
        }

        public int CreateTestRun(int configurationId, string[] selectedTestCases)
        {
            var configuration = GetConfiguration(configurationId);

            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestResult>();

            var testResult = new TestResult
            {
                TestSuiteConfigurationId = configurationId,
                State = TestResultState.Created,
            };

            repo.Insert(testResult);

            pool.Save().Wait();

            int id = testResult.Id;

            var testRun = TestRun.Create(Options.TestEnginePath, testResult, configuration, StoragePool, Update);

            repo.Update(testResult);

            pool.Save().Wait();

            testRun.Run(selectedTestCases);

            TestRunPool.Add(id, testRun);

            return id;
        }

        private void Update(int id, Action<TestResult> updater)
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestResult>();

            var testResult = repo.Get(q => q.Where(item => item.Id == id)).First();

            updater(testResult);

            repo.Update(testResult);

            pool.Save().Wait();
        }
    }
}
