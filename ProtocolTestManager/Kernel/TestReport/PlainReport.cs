// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Plain text test report
    /// </summary>
    public class PlainReport : TestReport
    {
        /// <summary>
        /// Constructor of PlainReport
        /// </summary>
        /// <param name="testCases">Executed test cases</param>
        public PlainReport(List<TestCase> testCases) : base(testCases)
        {
        }

        /// <summary>
        /// The file extension name for the report
        /// </summary>
        public override string FileExtension
        {
            get
            {
                return "txt";
            }
        }

        /// <summary>
        /// The filter string to use in SaveFileDialog
        /// </summary>
        public override string FileDialogFilter
        {
            get
            {
                return "Plain text report (*.txt)|*.txt";
            }
        }

        /// <summary>
        /// Export report to a file
        /// </summary>
        /// <param name="filename">File name of the exported report</param>
        public override bool ExportReport(string filename)
        {
            if (this.testCases.Count() == 0)
            {
                return false;
            }

            using (StreamWriter sw = new StreamWriter(filename))
            {
                string report = GetPlainReport();
                sw.Write(report);
            }
            return true;
        }

        /// <summary>
        /// Get the report string for plain test report
        /// </summary>
        public string GetPlainReport()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TestCase testcase in testCases)
            {
                string outcome = testcase.Status == TestCaseStatus.Other ? "Inconclusive" : testcase.Status.ToString();
                sb.AppendLine($"{outcome} {testcase.Name}");
            }
            return sb.ToString();
        }
    }
}
