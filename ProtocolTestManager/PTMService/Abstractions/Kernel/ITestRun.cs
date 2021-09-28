// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel
{
    /// <summary>
    /// Interface of test run.
    /// </summary>
    public interface ITestRun
    {
        /// <summary>
        /// The Id of test run.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// The configuration of test run.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// The storage root of test run.
        /// </summary>
        IStorageNode StorageRoot { get; }

        /// <summary>
        /// The state of test run.
        /// </summary>
        TestResultState State { get; }

        /// <summary>
        /// Number of total test cases.
        /// </summary>
        int? Total { get; }

        /// <summary>
        /// Number of total not run cases.
        /// </summary>
        int? NotRun { get; }

        /// <summary>
        /// Number of passed test cases.
        /// </summary>
        int? Passed { get; }

        /// <summary>
        /// Number of failed test cases.
        /// </summary>
        int? Failed { get; }

        /// <summary>
        /// Number of inconclusive test cases.
        /// </summary>
        int? Inconclusive { get; }

        /// <summary>
        /// Abort the test run.
        /// </summary>
        void Abort();

        /// <summary>
        /// Get running status.
        /// </summary>
        /// <returns>The running status.</returns>
        IDictionary<string, TestCaseOverview> GetRunningStatus();

        /// <summary>
        /// Get test case result.
        /// </summary>
        /// <param name="name">The name of test case.</param>
        /// <returns>The test case result.</returns>
        TestCaseResult GetTestCaseResult(string name);

        /// <summary>
        /// Get test case detail.
        /// </summary>
        /// <param name="name">The name of test case.</param>
        /// <returns>Whether test case detail is found and the test case detail found.</returns>
        (bool found, TestCaseDetail detail) GetTestCaseDetail(string name);
    }
}
