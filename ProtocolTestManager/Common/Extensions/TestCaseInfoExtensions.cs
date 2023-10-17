// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.Common
{
    /// <summary>
    /// Extension methods for the <see cref="TestCaseInfo" /> class.
    /// </summary>
    public static class TestCaseInfoExtensions
    {
        /// <summary>
        /// Gets the class name for a test case.
        /// </summary>
        /// <param name="testCaseInfo">The <see cref="TestCaseInfo"/> containing the test case to get the class name for.</param>
        /// <returns>The class name for the specified test case.</returns>
        public static string GetClassName(this TestCaseInfo testCaseInfo)
        {
            var className = testCaseInfo?.FullName;
            if(className == null)
            {
                return string.Empty;
            }

            var startIndex = className.LastIndexOf('.');
            if (startIndex > 0)
            {
                className = className.Substring(0, startIndex);
            }

            return className;
        }
    }
}