// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions.Database
{
    /// <summary>
    /// Interface of repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Get entities by the given query function.
        /// </summary>
        /// <param name="queryFunc">The query function.</param>
        /// <returns></returns>
        IEnumerable<T> Get(Func<IQueryable<T>, IQueryable<T>> queryFunc);

        /// <summary>
        /// Insert an entity.
        /// </summary>
        /// <param name="e">The entity to insert.</param>
        void Insert(T e);

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="e">The entity to update.</param>
        void Update(T e);

        /// <summary>
        /// Remove an entity.
        /// </summary>
        /// <param name="e">The entity to remove.</param>
        void Remove(T e);
    }
}
