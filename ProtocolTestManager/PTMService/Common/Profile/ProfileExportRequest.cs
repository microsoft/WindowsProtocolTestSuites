// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.Common.Profile
{
    public class ProfileExportRequest
    {
        /// <summary>
        /// Test suite id.
        /// </summary>
        public int TestSuiteId { get; set; }

        /// <summary>
        /// Configuration id.
        /// </summary>
        public int ConfigurationId { get; set; }

        /// <summary>
        /// Selected test cases.
        /// </summary>
        public string[] SelectedTestCases { get; set; }

        /// <summary>
        /// Test result id.
        /// </summary>
        public int TestResultId { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; set; }
    }
}