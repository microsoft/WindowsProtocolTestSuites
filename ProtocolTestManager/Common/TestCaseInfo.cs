// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.Common
{
    /// <summary>
    /// Test case info.
    /// </summary>
    public class TestCaseInfo
    {
        /// <summary>
        /// The categories of the test case
        /// </summary>
        public string[] Category { get; set; }

        /// <summary>
        /// The description of the test case
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The tool tip of the test case on the UI
        /// </summary>
        public string ToolTipOnUI { get; set; }

        /// <summary>
        /// The name of the test case.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The fully qualified name of the test case
        /// </summary>
        public string FullName { get; set; }
    }
}
