// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.Database
{
    internal class RepositoryPool<T> : IRepositoryPool where T : DbContext
    {
        private T DbContext { get; init; }

        private Dictionary<Type, object> RepositoryStore { get; init; }

        public RepositoryPool(T t)
        {
            DbContext = t;

            RepositoryStore = new Dictionary<Type, object>();
        }

        public IRepository<E> Get<E>() where E : class
        {
            if (!RepositoryStore.ContainsKey(typeof(E)))
            {
                RepositoryStore[typeof(E)] = new Repository<E>(DbContext);
            }

            return RepositoryStore[typeof(E)] as IRepository<E>;
        }

        public async Task Save()
        {
            await DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
