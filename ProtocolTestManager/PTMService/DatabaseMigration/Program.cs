// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;

namespace Microsoft.Protocols.TestManager.PTMService.DatabaseMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new PTMServiceDbContextForMigration();

            context.Database.Migrate();
        }
    }
}
