// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

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
                TestSuiteName = testSuite.Name,
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

        public string GetCapabilitiesConfigJsonPath(int id)
        {
            var result = string.Empty;
            using var instance = ScopedServiceFactory.GetInstance();

            var pool = instance.ScopedServiceInstance;

            var repo = pool.Get<CapabilitiesConfig>();

            var capabilities =
                repo.Get(q => q.Where(item => item.Id == id)).FirstOrDefault();
            if (capabilities != null)
            {
                var capabilitiesConfigNode =
                    StoragePool.GetKnownNode(KnownStorageNodeNames.CapabilitiesSpec).GetNode(id.ToString());

                result = capabilitiesConfigNode.GetFiles()
                                               .Where(f => Path.GetFileName(f) == CapabilitiesConsts.SpecsFile)
                                               .FirstOrDefault();
            }
            else
            {
                Logger.AddLog(LogLevel.Error, $"Capabilities Config with id, {id} not found for retrieval.");
            }

            return result;
        }

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
    }
}