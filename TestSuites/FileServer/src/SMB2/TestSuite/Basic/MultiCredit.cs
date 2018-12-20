// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class MultiCredit : SMB2TestBase
    {
        #region Variables
        private string uncSharePath;
        private string contentWrite;
        private string contentRead;
        #endregion

        #region Test Initialize and Cleanup
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Case Initialize and Clean up
        protected override void TestInitialize()
        {
            base.TestInitialize();

            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.Credit)]
        [Description("This test case is designed to test whether server can handle one request which consumes more than one credit.")]
        public void BVT_MultiCredit_OneRequestWithMultiCredit()
        {
            TestConfig.CheckDialect(DialectRevision.Smb21);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LARGE_MTU);            

            Guid clientGuid = Guid.NewGuid();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client by sending the following requests: 1. NEGOTIATE; 2. SESSION_SETUP; 3. TREE_CONNECT");
            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServerOverTCP(TestConfig.SutIPAddress);
            Capabilities_Values clientCapabilities = Capabilities_Values.GLOBAL_CAP_LARGE_MTU;
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: clientCapabilities);
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            uint treeId;
            client.TreeConnect(uncSharePath, out treeId);

            int bufferSize = (int)client.MaxBufferSize;
            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            ushort grantedCredit = 0;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends CREATE request with {0} credits", client.Credits);
            client.Create(
                treeId,
                GetTestFileName(uncSharePath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));

                    BaseTestSite.Log.Add(LogEntryKind.Debug,
                        "The MaxBufferSize of the server is {0}.",
                        client.MaxBufferSize);
                    BaseTestSite.Log.Add(LogEntryKind.Debug,
                        "Server has granted {0} credits to the client.",
                        client.Credits);

                    // Make sure client hold enough credits for test
                    ushort maxBufferSizeInCredit = (ushort)((client.MaxBufferSize -1)/ 65536 + 1);
                    if (client.Credits < maxBufferSizeInCredit)
                    {
                        if (client.Credits < 2)
                        {
                            BaseTestSite.Assert.Inconclusive(
                                "This test case is not applicable when the server only grants {0} credits", 
                                client.Credits);
                        }
                        // Test max buffer according to granted credits.
                        bufferSize = (client.Credits - 1) * 65536;
                    }

                    grantedCredit = header.CreditRequestResponse;
                });

            contentWrite = Smb2Utility.CreateRandomStringInByte(bufferSize);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client attempts to write {0} bytes content to file when server grants {1} credits",
                client.MaxBufferSize, client.Credits);

            client.Write(
                treeId,
                fileId,
                contentWrite,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));

                    grantedCredit = header.CreditRequestResponse;
                });

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client attempts to read {0} bytes content from file when server grants {1} credit",
                bufferSize, client.Credits);
            client.Read(treeId, fileId, 0, (uint)contentWrite.Length, out contentRead);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: 1. CLOSE; 2. TREE_DISCONNECT; 3. LOG_OFF; 4. DISCONNECT");
            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
            client.Disconnect();
        }
    }
}
