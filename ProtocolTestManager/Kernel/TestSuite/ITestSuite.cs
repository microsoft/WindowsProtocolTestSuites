// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Interface of test suite.
    /// </summary>
    public interface ITestSuite
    {
        /// <summary>
        /// Test case list.
        /// </summary>
        IEnumerable<TestCase> TestCaseList { get; }

        /// <summary>
        /// Load test case list from given dll path.
        /// </summary>
        /// <param name="dllPath">The dll path.</param>
        void LoadFrom(IEnumerable<string> dllPath);

        /// <summary>
        /// Append category by config file.
        /// </summary>
        /// <param name="testCategory">Test category.</param>
        void AppendCategoryByConfigFile(AppConfigTestCategory testCategory);
    }
}
