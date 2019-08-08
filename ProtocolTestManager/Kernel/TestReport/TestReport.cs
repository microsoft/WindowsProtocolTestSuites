// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// An base class for test report
    /// </summary>
    public abstract class TestReport
    {
        /// <summary>
        /// List of executed test cases
        /// </summary>
        protected List<TestCase> testCases;

        /// <summary>
        /// Constructor of TestReport
        /// </summary>
        /// <param name="testCases">Executed test cases</param>
        public TestReport(List<TestCase> testCases)
        {
            this.testCases = testCases;
        }

        /// <summary>
        /// The file extension name for the report
        /// </summary>
        public abstract string FileExtension { get; }

        /// <summary>
        /// The filter string to use in SaveFileDialog
        /// </summary>
        public abstract string FileDialogFilter { get; }

        /// <summary>
        /// Export report to a file
        /// </summary>
        /// <param name="filename">File name of the exported report</param>
        public abstract bool ExportReport(string filename);
    }
}
