// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;

namespace Microsoft.Protocols.TestManager.PTMService.Database
{
    public class PTMServiceDbContext : DbContext
    {
        public PTMServiceDbContext()
            : base()
        {

        }

        public PTMServiceDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<TestSuiteInstallation> TestSuiteInstallations { get; set; }

        public DbSet<TestSuiteConfiguration> TestSuiteConfigurations { get; set; }

        public DbSet<TestResult> TestResults { get; set; }
    }
}
