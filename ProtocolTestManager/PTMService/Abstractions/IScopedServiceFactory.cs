// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions
{
    /// <summary>
    /// Interface of scoped service factory.
    /// </summary>
    /// <typeparam name="T">The type of scoped service.</typeparam>
    public interface IScopedServiceFactory<T>
    {
        /// <summary>
        /// Get an instance of scoped service.
        /// </summary>
        /// <returns>The scoped service.</returns>
        IScopedService<T> GetInstance();
    }
}
