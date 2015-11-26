// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    public class AuthenticationTestBase : CommonTestBase
    {
        public AuthTestConfig TestConfig
        {
            get
            {
                return testConfig as AuthTestConfig;
            }
        }
        protected override void TestInitialize()
        {
            base.TestInitialize();
            testConfig = new AuthTestConfig(BaseTestSite);
        }
    }
}
