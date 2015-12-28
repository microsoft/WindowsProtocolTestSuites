// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SQOS.TestSuite
{
    public class SqosTestBase : CommonTestBase
    {
        #region Variables
        protected Smb2FunctionalClient client;
        protected uint treeId;
        protected FILEID fileId;
        #endregion

        public SqosTestConfig TestConfig
        {
            get
            {
                return testConfig as SqosTestConfig;
            }
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();

            testConfig = new SqosTestConfig(BaseTestSite);

            BaseTestSite.DefaultProtocolDocShortName = "MS-SQOS";

            BaseTestSite.Log.Add(LogEntryKind.Debug, "SecurityPackage for authentication: " + TestConfig.DefaultSecurityPackage);

            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            treeId = 0;
            fileId = FILEID.Zero;

            #region Check Applicability
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_STORAGE_QOS_CONTROL);
            #endregion
        }

        protected override void TestCleanup()
        {
            if (client != null)
            {
                try
                {
                    client.Close(treeId, fileId);
                    client.TreeDisconnect(treeId);
                    client.LogOff();
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect client: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }

        protected void ConnectToVHD()
        {
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.FileServerNameContainingSqosVHD, TestConfig.FileServerIPContainingSqosVHD);
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "Client creates an Open to a VHDX file by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE");
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.FileServerNameContainingSqosVHD,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            client.TreeConnect(Smb2Utility.GetUncPath(TestConfig.FileServerNameContainingSqosVHD, TestConfig.ShareContainingSqosVHD), out treeId);

            Smb2CreateContextResponse[] response;
            client.Create(treeId, TestConfig.NameOfSqosVHD, CreateOptions_Values.NONE, out fileId, out response, RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE);
        }
    }
}
