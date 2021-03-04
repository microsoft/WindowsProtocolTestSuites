// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Database;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService : IPTMKernelService
    {
        private IStoragePool StoragePool { get; init; }

        private IScopedServiceFactory<IRepositoryPool> ScopedServiceFactory { get; init; }

        private IDictionary<int, ITestSuite> TestSuitePool { get; init; }

        public PTMKernelService(IStoragePool storageManager, IScopedServiceFactory<IRepositoryPool> scopedServiceFactory)
        {
            StoragePool = storageManager;

            ScopedServiceFactory = scopedServiceFactory;

            TestSuitePool = new Dictionary<int, ITestSuite>();
        }
    }
}
