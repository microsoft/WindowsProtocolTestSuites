// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
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
        /// Create an instance of given report format.
        /// </summary>
        /// <param name="format">Report format</param>
        /// <param name="testCases">A list of executed test cases</param>
        /// <returns>An instance of TestReport. Or null if format is incorrect</returns>
        public static TestReport GetInstance(string format, List<TestCase> testCases)
        {
            string ns = typeof(TestReport).Namespace;
            string className = $"{ns}.{format}Report";
            Type t = Type.GetType(className);
            if (t == null)
            {
                return null;
            }
            else
            {
                TestReport report = (TestReport)Activator.CreateInstance(t, testCases);
                return report;
            }
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
