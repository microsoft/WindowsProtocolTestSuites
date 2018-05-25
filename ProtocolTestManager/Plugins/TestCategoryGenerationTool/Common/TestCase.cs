// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.TestCategoryGenerationTool
{
    /// <summary>
    /// Represents a test case.
    /// </summary>
    public class TestCase
    {
        /// <summary>
        /// The categories of the test case
        /// </summary>
        public List<string> Category { get; set; }

        /// <summary>
        /// The name of the test case.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor of TestCase
        /// </summary>
        public TestCase()
        {
            Category = new List<string>();
        }
    }
}
