// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Database;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService : IPTMKernelService
    {
        private PTMKernelServiceOptions Options { get; init; }

        private IStoragePool StoragePool { get; init; }

        private IScopedServiceFactory<IRepositoryPool> ScopedServiceFactory { get; init; }

        private ConcurrentDictionary<int, ITestSuite> TestSuitePool { get; init; }

        private ConcurrentDictionary<int, IConfiguration> ConfigurationPool { get; init; }

        private ConcurrentDictionary<int, ITestRun> TestRunPool { get; init; }

        private ConcurrentDictionary<int, Dictionary<string, string>> DescriptionDictCache { get; init; }

        private ConcurrentDictionary<int, IAutoDetection> AutoDetectionPool { get; init; }

        private object syncRoot = new object();

        public PTMKernelService(IOptions<PTMKernelServiceOptions> options, IStoragePool storageManager, IScopedServiceFactory<IRepositoryPool> scopedServiceFactory)
        {
            Options = options.Value;

            StoragePool = storageManager;

            ScopedServiceFactory = scopedServiceFactory;

            TestSuitePool = new ConcurrentDictionary<int, ITestSuite>();

            ConfigurationPool = new ConcurrentDictionary<int, IConfiguration>();

            TestRunPool = new ConcurrentDictionary<int, ITestRun>();

            DescriptionDictCache = new ConcurrentDictionary<int, Dictionary<string, string>>();

            AutoDetectionPool = new ConcurrentDictionary<int, IAutoDetection>();
        }
    }
}
