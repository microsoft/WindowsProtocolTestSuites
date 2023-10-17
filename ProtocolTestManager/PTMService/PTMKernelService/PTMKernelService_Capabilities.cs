// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        public CapabilitiesConfig[] QueryCapabilitiesConfigAndCleanUp()
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<CapabilitiesConfig>();

            var result = repo.Get(q => q).ToArray();

            //Try clean up nodes that might have been left orphaned from a previous delete
            //operation.
            if (Directory.Exists(KnownStorageNodeNames.CapabilitiesSpec))
            {
                var capabilitiesConfigRootNode =
                    StoragePool.GetKnownNode(KnownStorageNodeNames.CapabilitiesSpec);
                var currentNodes = new DirectoryInfo(capabilitiesConfigRootNode.AbsolutePath)
                                            .GetDirectories()
                                            .Select(d => (d.Name, d.FullName))
                                            .ToArray();

                var expectedNodeNames =
                    result.Select(c => $@"{c.Id.ToString()}")
                          .ToDictionary(n => n);

                try
                {
                    foreach (var node in currentNodes)
                    {
                        if (!expectedNodeNames.ContainsKey(node.Name))
                        {
                            Directory.Delete(node.FullName, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.AddLog(LogLevel.Error, $"Deleting capabilities files in {capabilitiesConfigRootNode.AbsolutePath} failed with {ex}");
                }
            }

            return result;
        }

        public int CreateCapabilitiesFile(string name, string description, int testSuiteId)
        {
            var (json, testSuite) = CapabilitiesConfigWriter.Create(this, testSuiteId);

            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<CapabilitiesConfig>();

            var capabilitiesConfig = new CapabilitiesConfig
            {
                Name = name,
                Description = description,
                TestSuiteName = testSuite.TestSuiteName,
                TestSuiteVersion = testSuite.Version
            };

            try
            {
                repo.Insert(capabilitiesConfig);

                pool.Save().Wait();

                var id = capabilitiesConfig.Id;
                var capabilitiesConfigNode = StoragePool.GetKnownNode(KnownStorageNodeNames.CapabilitiesSpec).CreateNode(id.ToString());

                using var stream = new MemoryStream();
                using var sw = new StreamWriter(stream);

                var content = json.ToJsonString();

                sw.Write(content);

                sw.Flush();

                stream.Seek(0, SeekOrigin.Begin);

                capabilitiesConfigNode.CreateFile(CapabilitiesConsts.SpecsFile, stream);

                capabilitiesConfig.Path = capabilitiesConfigNode.AbsolutePath;

                repo.Update(capabilitiesConfig);
                pool.Save().Wait();

                return id;
            }
            catch
            {
                repo.Remove(capabilitiesConfig);
                pool.Save().Wait();

                throw;
            }
        }

        public void UpdateCapabilitiesFileMetadata(int id, string name, string description)
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<CapabilitiesConfig>();

            var capabilitiesConfig = repo.Get(q => q.Where(item => item.Id == id)).FirstOrDefault();
            if (capabilitiesConfig != null)
            {
                capabilitiesConfig.Name = name;
                capabilitiesConfig.Description = description;

                repo.Update(capabilitiesConfig);

                pool.Save().Wait();
            }
            else
            {
                Logger.AddLog(LogLevel.Error, $"Capabilities Config with id, {id} not found for update.");
            }
        }

        public void RemoveCapabilitiesFile(int id)
        {
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<CapabilitiesConfig>();
            var capabilitiesConfig =
                repo.Get(c => c.Where(item => item.Id == id)).FirstOrDefault();
            if (capabilitiesConfig != null)
            {
                repo.Remove(capabilitiesConfig);

                pool.Save().Wait();

                //Try remove the storage node.
                var capabilitiesConfigNode =
                    StoragePool.GetKnownNode(KnownStorageNodeNames.CapabilitiesSpec).GetNode(id.ToString());
                try
                {
                    capabilitiesConfigNode.DeleteNode();
                }
                catch (Exception ex)
                {
                    Logger.AddLog(LogLevel.Error, $"Deleting {capabilitiesConfigNode.AbsolutePath} failed with {ex}");
                }
            }
            else
            {
                Logger.AddLog(LogLevel.Information, $"Capabilities Config with id, {id} not found for removal.");
            }
        }

        public (string Name, string Path) GetCapabilitiesConfigInfo(int id)
        {
            var path = string.Empty;
            var name = string.Empty;
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<CapabilitiesConfig>();

            var capabilities =
                repo.Get(q => q.Where(item => item.Id == id)).FirstOrDefault();
            if (capabilities != null)
            {
                name = capabilities.Name;
                var capabilitiesConfigNode =
                    StoragePool.GetKnownNode(KnownStorageNodeNames.CapabilitiesSpec).GetNode(id.ToString());

                path = capabilitiesConfigNode.GetFiles()
                                               .Where(f => Path.GetFileName(f) == CapabilitiesConsts.SpecsFile)
                                               .FirstOrDefault();
            }
            else
            {
                Logger.AddLog(LogLevel.Error, $"Capabilities Config with id, {id} not found for retrieval.");
            }

            return (name, path);
        }

        /// <summary>
        /// Saves a capabilities file.
        /// </summary>
        /// <param name="id">The id of the capabilities file to save to.</param>
        /// <param name="json">Json content of the capabilities file to save.</param>
        public void SaveCapabilitiesFile(int id, JsonNode json)
        {
            using var instance = ScopedServiceFactory.GetInstance();
            var pool = instance.ScopedServiceInstance;
            var repo = pool.Get<CapabilitiesConfig>();

            var capabilitiesConfig = repo.Get(q => q.Where(item => item.Id == id)).FirstOrDefault();
            if (capabilitiesConfig != null)
            {
                var capabilitiesConfigNode = StoragePool.GetKnownNode(KnownStorageNodeNames.CapabilitiesSpec).GetNode(id.ToString());

                using var stream = new MemoryStream();
                using var sw = new StreamWriter(stream);

                var content = json.ToJsonString();

                sw.Write(content);

                sw.Flush();

                stream.Seek(0, SeekOrigin.Begin);

                capabilitiesConfigNode.WriteFile(CapabilitiesConsts.SpecsFile, stream);
            }
            else
            {
                Logger.AddLog(LogLevel.Error, $"Capabilities Config with id, {id} not found for update.");
            }
        }

        /// <summary>
        /// Filters test cases by name.
        /// </summary>
        /// <param name="testCases">The test cases to apply the filter to.</param>
        /// <param name="filter">The name filter to apply.</param>
        /// <returns>An array of filtered test cases.</returns>
        public string[] FilterCapabilitiesTestCasesByName(string[] testCases, string filter) 
        {
            filter = filter.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(filter))
            {
                return testCases;
            }

            return testCases
                .Where(t => t.Trim().ToLowerInvariant().Contains(filter))
                .ToArray();
        }

        private string[] FilterCapabilitiesTestCases<T>(string[] testCases, string filter,
            ITestSuite testSuite, Func<TestCaseInfo, T> dictSelector,
            Func<T, bool> resultsFilter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return testCases;
            }

            var testCasesInTestSuite =
                testSuite.GetTestCases(filter: null)
                .ToDictionary(t => t.FullName.Trim().ToLowerInvariant(), t => dictSelector(t));

            return testCases.Where(t =>
            {
                var fullNameLowerCase = t?.Trim()?.ToLowerInvariant();

                return fullNameLowerCase != null
                    && testCasesInTestSuite.ContainsKey(fullNameLowerCase)
                    && resultsFilter(testCasesInTestSuite[fullNameLowerCase]);
            }).ToArray();
        }

        public string[] FilterCapabilitiesTestCasesByCategory(string[] testCases, string filter,
            ITestSuite testSuite)
        {
            filter = filter.Trim().ToLowerInvariant();
            return FilterCapabilitiesTestCases(testCases, filter, testSuite,
                t => t.Category,
                categories => categories.Any(c => c.Trim().ToLowerInvariant().Contains(filter))
            );
        }

        public string[] FilterCapabilitiesTestCasesByClass(string[] testCases, string filter,
            ITestSuite testSuite)
        {
            filter = filter.Trim().ToLowerInvariant();
            return FilterCapabilitiesTestCases(testCases, filter, testSuite,
                t => t.GetClassName().Trim().ToLowerInvariant(),
                className => className.Contains(filter)
            );
        }
    }
}