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
    }
}
