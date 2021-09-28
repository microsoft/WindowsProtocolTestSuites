// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        public IConfiguration[] QueryConfigurations(int? testSuiteId)
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestSuiteConfiguration>();

            var result = repo.Get(q =>
            {
                if (testSuiteId == null)
                {
                    return q;
                }

                else
                {
                    return q.Where(i => i.TestSuiteId == testSuiteId.Value);
                }
            }).Select(item => GetConfigurationInternal(item.Id, item));

            return result.ToArray();
        }

        public int CreateConfiguration(string name, int testSuiteId, string description)
        {
            var tesSuite = GetTestSuite(testSuiteId);

            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<TestSuiteConfiguration>();

            var testSuiteConfiguration = new TestSuiteConfiguration
            {
                Name = name,
                TestSuiteId = testSuiteId,
                Description = description,
            };

            repo.Insert(testSuiteConfiguration);

            pool.Save().Wait();

            int id = testSuiteConfiguration.Id;
            try
            {
                var configuration = Configuration.Create(testSuiteConfiguration, tesSuite, StoragePool);
                repo.Update(testSuiteConfiguration);
                ConfigurationPool.AddOrUpdate(id, _ => configuration, (_, _) => configuration);
            }
            catch
            {
                repo.Remove(testSuiteConfiguration);
                throw;
            }
            finally
            {
                pool.Save().Wait();
            }
            return id;
        }

        public IConfiguration GetConfiguration(int id)
        {
            return GetConfigurationInternal(id, null);
        }

        private IConfiguration GetConfigurationInternal(int id, TestSuiteConfiguration testSuiteConfiguration)
        {
            if (!ConfigurationPool.ContainsKey(id))
            {
                if (testSuiteConfiguration == null)
                {
                    using var instance = ScopedServiceFactory.GetInstance();

                    var pool = instance.ScopedServiceInstance;

                    var repo = pool.Get<TestSuiteConfiguration>();

                    testSuiteConfiguration = repo.Get(q => q.Where(i => i.Id == id)).First();
                }

                var configuration = Configuration.Open(testSuiteConfiguration, GetTestSuite(testSuiteConfiguration.TestSuiteId), StoragePool);
                ConfigurationPool.AddOrUpdate(id, _ => configuration, (_, _) => configuration);
            }

            return ConfigurationPool[id];
        }
    }
}
