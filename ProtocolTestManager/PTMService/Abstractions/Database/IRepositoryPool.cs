// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions.Database
{
    /// <summary>
    /// The interface of repository pool.
    /// </summary>
    public interface IRepositoryPool : IDisposable
    {
        /// <summary>
        /// Get repository of the given type.
        /// </summary>
        /// <typeparam name="E">The entity type.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<E> Get<E>() where E : class;

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <returns>The task of save changes.</returns>
        Task Save();
    }
}
