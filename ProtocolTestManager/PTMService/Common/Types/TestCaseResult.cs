// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.Common.Types
{
    public class TestCaseResult
    {
        public TestCaseOverview Overview { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Output { get; set; }
    }
}
