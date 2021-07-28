// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Protocols.TestManager.PTMService.Common.Entities
{
    public class TestSuiteInstallation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public TestSuiteInstallMethod InstallMethod { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public bool Removed { get; set; }
    }
}
