// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Dfsc;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.DFSC.TestSuite
{
    public abstract partial class DFSCTestBase : CommonTestBase
    {
        #region Variables
        protected DfscClient client;
        protected DFSCTestUtility utility;
        #endregion

        public DFSCTestConfig TestConfig
        {
            get
            {
                return testConfig as DFSCTestConfig;
            }
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();

            testConfig = new DFSCTestConfig(BaseTestSite);

            BaseTestSite.DefaultProtocolDocShortName = "MS-DFSC";

            BaseTestSite.Log.Add(LogEntryKind.Debug, "SecurityPackage for authentication: " + TestConfig.DefaultSecurityPackage);

            client = new DfscClient();
            utility = new DFSCTestUtility(BaseTestSite, TestConfig);
        }

        protected override void TestCleanup()
        {
            try
            {
                client.Disconnect(TestConfig.Timeout);
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect client: {0}", ex.ToString());
            }
            client.Dispose();
            base.TestCleanup();
        }
    }
}
