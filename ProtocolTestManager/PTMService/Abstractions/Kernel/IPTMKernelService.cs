// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.PTMService.Common.Profile;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel
{
    public delegate void DetectionCallback(DetectionOutcome outcome);

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

        /// <summary>
        /// Update test suite by package.
        /// </summary>
        /// <param name="id">The ID of the test suite.</param>
        /// <param name="name">The name of test suite.</param>
        /// <param name="packageName">The package name.</param>
        /// <param name="package">The package.</param>
        /// <param name="description">The description to test suite.</param>
        void UpdateTestSuite(int id, string name, string packageName, Stream package, string description);

        /// <summary>
        /// Remove test suite.
        /// </summary>
        /// <param name="id">The ID of the test suite.</param>
        void RemoveTestSuite(int id);

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
        /// Query test runs.
        /// </summary>
        /// <param name="pageSize">The maximum item number per page.</param>
        /// <param name="pageIndex">The page number.</param>
        /// <param name="queryFunc">The function to filter test results by a query.</param>
        /// <param name="totalPage">Total page number.</param>
        /// <returns>The test runs.</returns>
        ITestRun[] QueryTestRuns(int pageSize, int pageIndex, Func<TestResult, bool> queryFunc, out int totalPage);

        /// <summary>
        /// return if any test suite is running
        /// </summary>
        /// <returns>true if any test suite is running</returns>
        bool IsTestSuiteRunning();

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

        /// Remove a test run.
        /// </summary>
        /// <param name="id">The test run Id.</param>
        void RemoveTestRun(int id);

        /// <summary>
        /// Get the path to a test run report.
        /// </summary>
        /// <param name="testResultId">The tes restult Id.</param>
        /// <param name="format">The report format.</param>
        /// <param name="testCases">The test case list to be exported.</param>
        /// <returns>The path to the test run report.</returns>
        string GetTestRunReport(int testResultId, ReportFormat format, string[] testCases);

        #endregion

        #region Save and Load profile

        /// <summary>
        /// Saves profile
        /// </summary>
        /// <param name="request">Profile request.</param>
        /// <returns>Returns location where file was saved.</returns>
        string SaveProfileSettingsByTestResult(int testResultId);

        /// <summary>
        /// Saves profile
        /// </summary>
        /// <param name="request">Profile request.</param>
        /// <returns>Returns location where file was saved.</returns>
        string SaveProfileSettings(ProfileExportRequest request);

        /// <summary>
        /// Loads a ptm profile.
        /// </summary>
        /// <param name="request">Profile request.</param>
        void LoadProfileSettings(ProfileRequest request);

        /// <summary>
        /// Ensures a profile name matches test manager format.
        /// </summary>
        /// <param name="profileName">Profile name.</param>
        /// <returns>Returns compliant profile name.</returns>
        string EnsureProfileName(string profileName);

        #endregion

        #region Test Suite Auto-Detection

        /// <summary>
        /// Creates a new auto detector instance.
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        /// <returns>Auto detector configuration id.</returns>
        int CreateAutoDetector(int configurationId);

        /// <summary>
        /// Gets or create a new auto detector instance.
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        /// <returns>Auto detector instance.</returns>
        IAutoDetection GetAutoDetection(int configurationId);
        
        /// <summary>
        /// Gets the properties required for auto-detection.
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        /// <returns>Prerequisites object.</returns>
        PrerequisiteView GetPrerequisites(int configurationId);

        /// <summary>
        /// Sets the property values required for auto-detection.
        /// </summary>
        /// <param name="Property">List of Property.</param>
        /// <param name="configurationId">Test suite configuration id.</param>
        /// <returns>Returns true if succeeded, otherwise false.</returns>
        bool SetPrerequisites(List<Property> prerequisiteProperties, int configurationId);

        /// <summary>
        /// Gets a list of the detection steps.
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        /// <returns>A list of the detection steps.</returns>
        List<DetectingItem> GetDetectedSteps(int configurationId);

        /// <summary>
        /// Get detection status
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        /// <returns>Detection Status</returns>
        DetectionOutcome GetDetectionOutcome(int configurationId);

        /// <summary>
        /// Reset AutoDetection
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        void Reset(int configurationId);

        /// <summary>
        /// Begins the auto-detection.
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        /// <param name="callback">Callback function when the detection finished.</param>
        void StartDetection(int configurationId, DetectionCallback callback);

        /// <summary>
        /// Stop the auto-detection
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        /// <param name="callback">Action to perform after stopping auto detection.</param>
        void StopDetection(int configurationId, Action callBack);


        /// <summary>
        /// Gets an object represents the detection summary.
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        /// <returns>An object</returns>
        List<ResultItemMap> GetDetectionSummary(int configurationId);

        /// <summary>
        /// Apply Auto Detection Result
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        void ApplyDetectionResult(int configurationId);
        
        /// <summary>
        /// Get AutoDetection Log Content
        /// </summary>
        /// <param name="configurationId">Test suite configuration id.</param>
        /// <returns>AutoDetection Log Content</returns>
        string GetDetectionLog(int configurationId);

        IEnumerable<PropertyGroup> GetConfigurationProperties(int configurationId);

        void SetConfigurationProperties(int configurationId, IEnumerable<PropertyGroup> propertyGroupes);

        #endregion
    }
}
