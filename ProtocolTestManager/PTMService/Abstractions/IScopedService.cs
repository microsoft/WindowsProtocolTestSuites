// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions
{
    /// <summary>
    /// Interface of scoped service.
    /// </summary>
    /// <typeparam name="T">The type of scoped service.</typeparam>
    public interface IScopedService<T> : IDisposable
    {
        /// <summary>
        /// The instance of scoped service.
        /// </summary>
        public T ScopedServiceInstance { get; }
    }
}
