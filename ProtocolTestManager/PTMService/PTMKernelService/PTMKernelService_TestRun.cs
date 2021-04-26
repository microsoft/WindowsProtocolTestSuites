// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        public ITestRun[] QueryTestRuns(int pageSize, int pageIndex, Func<TestResult, bool> queryFunc, out int totalPage)
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestResult>();

            var all = repo.Get(q => q.Where(queryFunc).OrderByDescending(item => item.Id).AsQueryable()).Select(item => GetTestRunInternal(item.Id, item));

            int count = all.Count();

            if (count % pageSize == 0)
            {
                totalPage = count / pageSize;
            }
            else
            {
                totalPage = count / pageSize + 1;
            }

            var result = all.Skip(pageSize * pageIndex).Take(pageSize);

            return result.ToArray();
        }

        public ITestRun GetTestRun(int id)
        {
            return GetTestRunInternal(id, null);
        }

        private ITestRun GetTestRunInternal(int id, TestResult testResult)
        {
            if (!TestRunPool.ContainsKey(id))
            {
                if (testResult == null)
                {
                    using var instance = ScopedServiceFactory.GetInstance();

                    var pool = instance.ScopedServiceInstance;

                    var repo = pool.Get<TestResult>();

                    testResult = repo.Get(q => q.Where(item => item.Id == id)).First();
                }

                var configuration = GetConfiguration(testResult.TestSuiteConfigurationId);
                var testRun = TestRun.Open(Options.TestEnginePath, testResult, configuration, StoragePool, Update);

                TestRunPool.AddOrUpdate(id, _ => testRun, (_, _) => testRun);
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

            TestRunPool.AddOrUpdate(id, _ => testRun, (_, _) => testRun);

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
