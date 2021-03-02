// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using System;

namespace Microsoft.Protocols.TestManager.PTMService.Storage
{
    public static class StoragePoolExtensions
    {
        public static IServiceCollection AddStoragePool(this IServiceCollection services)
        {
            services.AddSingleton<IStoragePool, StoragePool>();

            return services;
        }
    }

    public static class StoragePoolOptionsExtensions
    {
        public static IServiceCollection ConfigureStoragePoolOptions(this IServiceCollection services, Action<StoragePoolOptions> action)
        {
            var options = new StoragePoolOptions();

            action(options);

            services.AddSingleton(options);

            return services;
        }
    }
}
