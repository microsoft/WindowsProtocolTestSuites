// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.Common
{
    /// <summary>
    /// Represents detailed test case information
    /// </summary>
    public class TestCaseDetail
    {
        /// <summary>
        /// The name of the test case
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The fully qualified name of the test case
        /// </summary>
        public string FullyQualifiedName { get; set; }

        /// <summary>
        /// The start time of the test case
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// The end time of the test case
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// The result of the test case
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// The source assembly of the test case
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The test class of the test case
        /// </summary>
        public string ClassType { get; set; }

        /// <summary>
        /// The Categories of the test case
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// The ErrorStackTrace log of the test case
        /// </summary>
        public List<string> ErrorStackTrace { get; set; }

        /// <summary>
        /// The ErrorMessage log of the test case
        /// </summary>
        public List<string> ErrorMessage { get; set; }

        /// <summary>
        /// The StandardOut log of the test case
        /// </summary>
        public List<StandardOutDetail> StandardOut { get; set; }

        /// <summary>
        /// The Types in StandardOut log 
        /// </summary>
        public List<string> StandardOutTypes { get; set; }

        /// <summary>
        /// The path of the capture file if any
        /// </summary>
        public string CapturePath { get; set; }
    }
}
