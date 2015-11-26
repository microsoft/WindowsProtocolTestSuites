// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines a test suite with all it's releated information.
    /// </summary>
    public class TestSuiteFamily : List<TestSuiteInfo>
    {
        /// <summary>
        /// The name of this test suite.
        /// </summary>
        public string Name { get; set; }
    }
}
