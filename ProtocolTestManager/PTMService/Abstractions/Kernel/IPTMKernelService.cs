// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel
{
    /// <summary>
    /// Interface of PTM kernel service.
    /// </summary>
    public interface IPTMKernelService
    {
        #region Test suite related members.

        /// <summary>
        /// Query test suites.
        /// </summary>
        /// <returns>The test suites queried out.</returns>
        ITestSuite[] QueryTestSuites();

        /// <summary>
        /// Get test suite.
        /// </summary>
        /// <param name="id">The Id of test suite.</param>
        /// <returns>The test suite.</returns>
        ITestSuite GetTestSuite(int id);

        /// <summary>
        /// Install test suite by package.
        /// </summary>
        /// <param name="name">The name of test suite.</param>
        /// <param name="packageName">The package name.</param>
        /// <param name="package">The package.</param>
        /// <param name="description">The description to test suite.</param>
        /// <returns>The Id of test suite.</returns>
        int InstallTestSuite(string name, string packageName, Stream package, string description);

        #endregion

        #region Configuration related members.

        /// <summary>
        /// Query configurations by filter.
        /// </summary>
        /// <param name="testSuiteId">The optional test suite Id of configuration.</param>
        /// <returns>The configurations queried out.</returns>
        IConfiguration[] QueryConfigurations(int? testSuiteId);

        /// <summary>
        /// Create a configuration.
        /// </summary>
        /// <param name="name">The name of configuration.</param>
        /// <param name="testSuiteId">The test suite Id.</param>
        /// <param name="description">The description of configuration.</param>
        /// <returns>The configuration Id.</returns>
        int CreateConfiguration(string name, int testSuiteId, string description);

        /// <summary>
        /// Get a configuration.
        /// </summary>
        /// <param name="id">The configuration Id.</param>
        /// <returns>The configuration.</returns>
        IConfiguration GetConfiguration(int id);

        #endregion

        #region Test run related members.

        /// <summary>
        /// Get a test run.
        /// </summary>
        /// <param name="id">The test run Id.</param>
        /// <returns>The test run.</returns>
        ITestRun GetTestRun(int id);

        /// <summary>
        /// Create a test run.
        /// </summary>
        /// <param name="configurationId">The configuration Id.</param>
        /// <param name="selectedTestCases">The optional selected test cases.</param>
        /// <returns>The test run Id.</returns>
        int CreateTestRun(int configurationId, string[] selectedTestCases);

        #endregion
    }
}
