// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Protocols.TestManager.PTMService.Database;

namespace Microsoft.Protocols.TestManager.PTMService.DatabaseMigration
{
    public class PTMServiceDbContextForMigration : PTMServiceDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = ptmservice.db");
        }
    }
}
