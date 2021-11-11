// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Database;
using System;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService
{
    internal class ScopedServiceFactory : IScopedServiceFactory<IRepositoryPool>
    {
        private IServiceProvider ServiceProvider { get; init; }

        public ScopedServiceFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IScopedService<IRepositoryPool> GetInstance()
        {
            var scope = ServiceProvider.CreateScope();

            var instance = scope.ServiceProvider.GetService<IRepositoryPool>();

            var result = new ScopedService<IRepositoryPool>(instance, scope);

            return result;
        }
    }
}
