// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos;

namespace Microsoft.Protocols.TestSuites.FileSharing.SQOS.TestSuite
{
    public class SqosTestBase : CommonTestBase
    {
        #region Variables
        protected SqosClient client;
        protected uint treeId;
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

            client = new SqosClient(TestConfig.Timeout);

            // Copy the data used in test cases to the share of the SUT, e.g. the shared virtual disk files.
            sutProtocolController.CopyFile(TestConfig.FullPathShareContainingSharedVHD, @"data\*.*");
        }

        protected override void TestCleanup()
        {
            if (client != null)
            {
                try
                {
                    client.Close();
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
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "Client creates an Open to a VHDX file by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE");
            client.ConnectToVHD(
                TestConfig.FileServerNameContainingSharedVHD, 
                TestConfig.FileServerIPContainingSharedVHD,
                TestConfig.DomainName,
                TestConfig.UserName,
                TestConfig.UserPassword,
                TestConfig.DefaultSecurityPackage,
                TestConfig.UseServerGssToken,
                TestConfig.ShareContainingSharedVHD,
                TestConfig.NameOfSharedVHD);
        }
    }
}
