// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel
{
    /// <summary>
    /// Interface of test suite.
    /// </summary>
    public interface ITestSuite
    {
        /// <summary>
        /// The Id of test suite.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// The name of test suite installation.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The name of test suite.
        /// </summary>
        string TestSuiteName { get; }

        /// <summary>
        /// The version of test suite.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// The description of test suite.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Whether the test suite is removed.
        /// </summary>
        bool Removed { get; set; }

        /// <summary>
        /// The install method of test suite.
        /// </summary>
        TestSuiteInstallMethod InstallMethod { get; }

        /// <summary>
        /// The storage root of test suite.
        /// </summary>
        IStorageNode StorageRoot { get; }

        /// <summary>
        /// Get configuration files.
        /// </summary>
        /// <returns>The path of all configuration files.</returns>
        IEnumerable<string> GetConfigurationFiles();

        /// <summary>
        /// Get test assemblies.
        /// </summary>
        /// <returns>The path of test assemblies.</returns>
        IEnumerable<string> GetTestAssemblies();

        /// <summary>
        /// Get plugin adpaters.
        /// </summary>
        /// <returns>The plugin adpaters.</returns>
        IEnumerable<Adapter> GetPluginAdapters();

        /// <summary>
        /// Get test cases.
        /// </summary>
        /// <param name="filter">The optional filter expression.</param>
        /// <returns>The test cases filtered out.</returns>
        IEnumerable<TestCaseInfo> GetTestCases(string filter);

        /// <summary>
        /// Load Test Case Filter
        /// </summary>
        /// <returns></returns>
        RuleGroup[] LoadTestCaseFilter();

        /// <summary>
        /// Get test case filter.
        /// </summary>
        /// <returns>The testcase filter.</returns>
        TestManager.Kernel.TestCaseFilter GetTestCaseFilter();

        /// <summary>
        /// Get auto-detection dll file name
        /// </summary>
        /// <returns>Auto-detection file name</returns>
        string GetDetectorAssembly();

        /// <summary>
        /// Load feature mapping config from given xml node
        /// </summary>
        /// <param name="targetFilterIndex"></param>
        /// <param name="mappingFilterIndex"></param>
        void LoadFeatureMappingFromXml(out int targetFilterIndex, out int mappingFilterIndex);
    }
}
