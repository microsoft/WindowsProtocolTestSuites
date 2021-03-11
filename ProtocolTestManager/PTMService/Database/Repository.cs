// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.Database
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private DbContext DbContext { get; init; }

        private DbSet<T> DbSet { get; init; }

        public Repository(DbContext context)
        {
            DbContext = context;

            DbSet = context.Set<T>();
        }

        IEnumerable<T> IRepository<T>.Get(Func<IQueryable<T>, IQueryable<T>> queryFunc)
        {
            var result = queryFunc(DbSet);

            return result.AsEnumerable();
        }

        void IRepository<T>.Insert(T e)
        {
            DbSet.AddAsync(e);
        }

        void IRepository<T>.Remove(T e)
        {
            if (DbContext.Entry(e).State == EntityState.Detached)
            {
                DbSet.Attach(e);
            }

            DbSet.Remove(e);
        }

        void IRepository<T>.Update(T e)
        {
            DbSet.Attach(e);

            DbContext.Entry(e).State = EntityState.Modified;
        }
    }
}
