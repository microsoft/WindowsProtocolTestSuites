// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Database;

namespace Microsoft.Protocols.TestManager.PTMService.Database
{
    public static class PTMServiceDbContextExtensions
    {
        public static IServiceCollection AddPTMServiceDbContext(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<PTMServiceDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlite(connectionString);
            });
        }
    }

    public static class RepositoryPoolExtensions
    {
        public static IServiceCollection AddRepositoryPool(this IServiceCollection services)
        {
            return services.AddScoped<IRepositoryPool, RepositoryPool<PTMServiceDbContext>>();
        }
    }
}
