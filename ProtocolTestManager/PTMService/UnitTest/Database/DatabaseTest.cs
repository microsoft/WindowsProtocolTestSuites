// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Database;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest
{
    [TestClass]
    public class DatabaseTest
    {
        private string[] existingTestSuiteNames = new string[]
        {
            "FileServer",
            "RDPClient",
            "RDPServer",
        };

        private string existingConfigurationName = "Cluster";

        private string existingConfigurationTestSuiteName = "FileServer";

        private IServiceScope serviceScope;

        private IRepositoryPool pool1;

        private IRepositoryPool pool2;

        [TestInitialize]
        public void TestInitialize()
        {
            long timeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var dbName = $"ptmservice.test.running{timeStamp}.db";
            File.Copy("ptmservice.test.db", dbName, true);
            Thread.Sleep(1000);
            var services = new ServiceCollection();

            services.AddPTMServiceDbContext($"Data Source = {dbName}");

            services.AddRepositoryPool();

            var serviceProvider = services.BuildServiceProvider();

            serviceScope = serviceProvider.CreateScope();

            pool1 = serviceScope.ServiceProvider.GetService<IRepositoryPool>();

            pool2 = serviceScope.ServiceProvider.GetService<IRepositoryPool>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            serviceScope.Dispose();
        }

        [TestMethod]
        public async Task AddOne()
        {
            // Add by pool1
            var repo1 = pool1.Get<TestSuiteInstallation>();

            var item = new TestSuiteInstallation
            {
                Name = "Test",
            };

            repo1.Insert(item);

            await pool1.Save();

            // Get by pool2
            var repo2 = pool2.Get<TestSuiteInstallation>();

            var result = repo2.Get(q => q.Where(t => t.Id == item.Id));

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(item.Id, result.First().Id);

            Assert.AreEqual(item.Name, result.First().Name);
        }

        [TestMethod]
        public async Task AddMany()
        {
            // Add by pool1
            var repo1 = pool1.Get<TestSuiteInstallation>();

            int oldCount = repo1.Get(q => q).Count();

            var names = new string[]
            {
                "Test 1",
                "Test 2",
                "Test 3",
            };

            var items = names.Select(s => new TestSuiteInstallation
            {
                Name = s,
            });

            foreach (var item in items)
            {
                repo1.Insert(item);
            }

            await pool1.Save();

            // Get by pool2
            var repo2 = pool2.Get<TestSuiteInstallation>();

            var result = repo2.Get(q => q);

            Assert.AreEqual(oldCount + names.Count(), result.Count());

            foreach (string name in names)
            {
                Assert.IsNotNull(result.FirstOrDefault(t => t.Name == name));
            }
        }

        [TestMethod]
        public void GetOne()
        {
            // Get by pool1
            var repo1 = pool1.Get<TestSuiteInstallation>();

            var result = repo1.Get(q => q.Where(t => t.Name == existingTestSuiteNames.First()));

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(existingTestSuiteNames.First(), result.First().Name);
        }

        [TestMethod]
        public void GetMany()
        {
            // Get by pool1
            var repo1 = pool1.Get<TestSuiteInstallation>();

            var result = repo1.Get(q => q).Select(q => q.Name).OrderBy(s => s);

            Assert.IsTrue(Enumerable.SequenceEqual(existingTestSuiteNames, result));
        }

        [TestMethod]
        public async Task Update()
        {
            // Update by pool1
            var repo1 = pool1.Get<TestSuiteInstallation>();

            var result = repo1.Get(q => q.Where(t => t.Name == existingTestSuiteNames.First())).First();

            result.Name = "Test";

            repo1.Update(result);

            await pool1.Save();

            // Get by pool2
            var repo2 = pool2.Get<TestSuiteInstallation>();

            var updatedItem = repo2.Get(q => q.Where(t => t.Id == result.Id)).First();

            Assert.AreEqual(result.Name, updatedItem.Name);
        }

        [TestMethod]
        public async Task Remove()
        {
            // Remove by pool1
            var repo1 = pool1.Get<TestSuiteInstallation>();

            var result = repo1.Get(q => q).First();

            repo1.Remove(result);

            await pool2.Save();

            // Get by pool2
            var repo2 = pool2.Get<TestSuiteInstallation>();

            int count = repo2.Get(q => q.Where(t => t.Id == result.Id)).Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ForeignGet()
        {
            // Get by pool1
            var repo1 = pool1.Get<TestSuiteConfiguration>();

            var result = repo1.Get(q => q.Where(c => c.Name == existingConfigurationName).Include(c => c.TestSuiteInstallation)).First();

            Assert.IsNotNull(result.TestSuiteInstallation);

            Assert.AreEqual(existingConfigurationTestSuiteName, result.TestSuiteInstallation.Name);
        }

        [TestMethod]
        public async Task ForeignAdd()
        {
            // Add by pool1
            var ts = pool1.Get<TestSuiteInstallation>().Get(q => q.Where(t => t.Name == existingTestSuiteNames.First())).First();

            var repo1 = pool1.Get<TestSuiteConfiguration>();

            var repoConf = pool1.Get<TestSuiteConfiguration>();

            var conf = new TestSuiteConfiguration
            {
                Name = "Test Configuration",
                TestSuiteInstallation = ts,
            };

            repoConf.Insert(conf);

            await pool1.Save();

            // Get by pool2
            var repo2 = pool2.Get<TestSuiteConfiguration>();

            var result = repo2.Get(q => q.Where(c => c.Id == conf.Id).Include(c => c.TestSuiteInstallation)).First();

            Assert.AreEqual(conf.Id, result.Id);

            Assert.AreEqual(ts.Id, result.TestSuiteId);

            Assert.IsNotNull(result.TestSuiteInstallation);

            Assert.AreEqual(ts.Id, result.TestSuiteInstallation.Id);
        }
    }
}
