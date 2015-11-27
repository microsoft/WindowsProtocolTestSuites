// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public partial class SMB2TestBase : CommonTestBase
    {
        public SMB2TestConfig TestConfig
        {
            get
            {
                return testConfig as SMB2TestConfig;
            }
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();

            testConfig = new SMB2TestConfig(BaseTestSite);

            BaseTestSite.DefaultProtocolDocShortName = "MS-SMB2";

            BaseTestSite.Log.Add(LogEntryKind.Debug, "SecurityPackage for authentication: " + TestConfig.DefaultSecurityPackage);
        }

        protected void ReadIpAddressesFromTestConfig(out List<IPAddress> clientIps, out List<IPAddress> serverIps)
        {
            clientIps = new List<IPAddress>();
            serverIps = new List<IPAddress>();
            clientIps.Add(TestConfig.ClientNic1IPAddress);
            clientIps.Add(TestConfig.ClientNic2IPAddress);
            serverIps.Add(TestConfig.SutIPAddress);
            serverIps.Add(TestConfig.SutAlternativeIPAddress);
        }

        protected void CheckCreateContextResponses(Smb2CreateContextResponse[] createContextResponse, params BaseResponseChecker[] checkers)
        {
            CheckCreateContextResponsesExistence(createContextResponse, true, checkers);
        }

        protected void CheckCreateContextResponsesNotExist(Smb2CreateContextResponse[] createContextResponse, params BaseResponseChecker[] checkers)
        {
            CheckCreateContextResponsesExistence(createContextResponse, false, checkers);
        }

        private void CheckCreateContextResponsesExistence(Smb2CreateContextResponse[] createContextResponse, bool shouldExist, params BaseResponseChecker[] checkers)
        {
            foreach (var checker in checkers)
            {
                bool found = false;
                if (createContextResponse != null)
                {
                    foreach (var response in createContextResponse)
                    {
                        if (response.GetType() == checker.ResponseType)
                        {
                            checker.Check(response);
                            found = true;
                            break;
                        }
                    }
                }
                if (shouldExist && !found)
                {
                    BaseTestSite.Assert.Fail("The expected response {0} does not exist.", checker.ResponseType);
                }

                if (!shouldExist && found)
                {
                    BaseTestSite.Assert.Fail("The response {0} should not exist.", checker.ResponseType);
                }
            }
        }
    }
}
