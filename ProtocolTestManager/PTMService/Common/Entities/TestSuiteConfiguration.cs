// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Protocols.TestManager.PTMService.Common.Entities
{
    public class TestSuiteConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(TestSuiteInstallation))]
        public int TestSuiteId { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public TestSuiteInstallation TestSuiteInstallation { get; set; }
    }
}
