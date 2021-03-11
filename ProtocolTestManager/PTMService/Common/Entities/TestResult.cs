// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Protocols.TestManager.PTMService.Common.Entities
{
    public enum TestResultState
    {
        Created,
        Running,
        Failed,
        Finished,
    }

    public class TestResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(TestSuiteConfiguration))]
        public int TestSuiteConfigurationId { get; set; }

        public TestResultState State { get; set; }

        public int? Total { get; set; }

        public int? NotRun { get; set; }

        public int? Passed { get; set; }

        public int? Failed { get; set; }

        public int? Inconclusive { get; set; }

        public string Description { get; set; }

        public TestSuiteConfiguration TestSuiteConfiguration { get; set; }
    }
}
