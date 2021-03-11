// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService
{
    internal class ScopedService<T> : IScopedService<T>
    {
        public ScopedService(T t, IServiceScope serviceScope)
        {
            ScopedServiceInstance = t;

            ServiceScope = serviceScope;
        }

        public T ScopedServiceInstance { get; private init; }

        private IServiceScope ServiceScope { get; init; }

        public void Dispose()
        {
            ServiceScope.Dispose();
        }
    }
}
