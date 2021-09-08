// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    public class TestResultOverview
    {
        public int Id { get; set; }

        public TestResultState Status { get; set; }

        public int ConfigurationId { get; set; }

        public int? Total { get; set; }

        public int? NotRun { get; set; }

        public int? Running { get; set; }

        public int? Passed { get; set; }

        public int? Failed { get; set; }

        public int? Inconclusive { get; set; }
    }
}
