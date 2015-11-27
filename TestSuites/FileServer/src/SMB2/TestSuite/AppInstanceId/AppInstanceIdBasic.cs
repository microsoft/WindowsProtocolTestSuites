// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class AppInstanceIdBasic : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient clientForInitialOpen;
        private Smb2FunctionalClient clientForReOpen;
        private string fileName;
        private string sharePath;
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

            fileName = string.Format("{0}_{1}.txt", CurrentTestCaseName, Guid.NewGuid());
            sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
        }

        protected override void TestCleanup()
        {
            if (clientForInitialOpen != null)
            {
                try
                {
                    clientForInitialOpen.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect clientBeforeFailover: {0}", ex.ToString());
                }
            }

            if (clientForReOpen != null)
            {
                try
                {
                    clientForReOpen.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect clientAfterFailover: {0}", ex.ToString());
                }
            }

            sutProtocolController.DeleteFile(sharePath, fileName);
            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.AppInstanceId)]
        [Description("The case is designed to test when client fails over to a new client, the previous open can be closed by the new client with the same AppInstanceId.")]
        public void BVT_AppInstanceId()
        {
            // Client1 opens a file with DurableHandleRequestV2 and AppInstanceId.
            // Client2 opens that file with DurableHandleRequestV2 and with the same AppInstanceId successfully.
            // Client1 writes to that file but fails since the first open is closed.
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);
            #endregion
            
            AppInstanceIdTest(sameAppInstanceId: true, containCreateDurableContext: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.AppInstanceId)]
        [TestCategory(TestCategories.InvalidIdentifier)]
        [Description("The case is designed to test when client fails over to a new client, the previous opened file cannot be closed by the new client with different AppInstanceId.")]
        public void AppInstanceId_Negative_DifferentAppInstanceIdInReopen()
        {
            // Client1 opens a file with DurableHandleRequestV2 and AppInstanceId.
            // Client2 opens that file with DurableHandleRequestV2 and with the different AppInstanceId, but fails.
            // Client1 writes to that file and succeeds since the first open is not closed.
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);
            #endregion

            AppInstanceIdTest(sameAppInstanceId: false, containCreateDurableContext: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.AppInstanceId)]
        [TestCategory(TestCategories.Positive)]
        [Description("The case is designed to test if the server implements dialect 3.11, " + 
            "and when client fails over to a new client, the previous open can be closed by the new client with the same AppInstanceId. " + 
            "AppInstanceId should work without DH2Q create context.")]
        public void AppInstanceId_Smb311()
        {
            // Client1 opens a file with create context AppInstanceId, no create context DurableHandleRequestV2
            // Client1 writes to that file.
            // Client2 opens that file with the same AppInstanceId successfully.
            // Client1 writes to that file but fails since the first open is closed.
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb311);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID);
            #endregion

            AppInstanceIdTest(sameAppInstanceId: true, containCreateDurableContext: false);
        }

        private void AppInstanceIdTest(bool sameAppInstanceId, bool containCreateDurableContext)
        {
            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            #region Client 1 Connect to Server
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start the first client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT");
            clientForInitialOpen = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            clientForInitialOpen.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress, TestConfig.ClientNic1IPAddress);
            clientForInitialOpen.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            clientForInitialOpen.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            uint treeIdForInitialOpen;
            clientForInitialOpen.TreeConnect(sharePath, out treeIdForInitialOpen);

            Guid appInstanceId = Guid.NewGuid();
            FILEID fileIdForInitialOpen;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "The first client sends CREATE request for exclusive open with SMB2_CREATE_APP_INSTANCE_ID create context.");
            Smb2CreateContextResponse[] serverCreateContexts;
            Smb2CreateContextRequest[] createContextsRequestForInitialOpen = null;
            if (containCreateDurableContext)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context is also included in the CREATE request.");
                createContextsRequestForInitialOpen = new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = Guid.NewGuid()
                    },
                    new Smb2CreateAppInstanceId
                    {
                         AppInstanceId = appInstanceId
                    }
                };
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context is not included in the CREATE request.");
                createContextsRequestForInitialOpen = new Smb2CreateContextRequest[] { 
                    new Smb2CreateAppInstanceId
                    {
                         AppInstanceId = appInstanceId
                    }
                };
            }

            clientForInitialOpen.Create(
                treeIdForInitialOpen,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdForInitialOpen,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                createContextsRequestForInitialOpen,
                shareAccess: ShareAccess_Values.NONE);
            #endregion

            #region Client 2 Connect to Server

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start the second client by sending the following requests: NEGOTIATE; SESSION-SETUP; TREE_CONNECT");
            clientForReOpen = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            clientForReOpen.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress, TestConfig.ClientNic2IPAddress);
            clientForReOpen.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            clientForReOpen.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            uint treeIdForReOpen;
            clientForReOpen.TreeConnect(sharePath, out treeIdForReOpen);

            FILEID fileIdForReOpen;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "The second client sends CREATE request for exclusive open with the {0} SMB2_CREATE_APP_INSTANCE_ID of the first client.", sameAppInstanceId ? "same" : "different");
            Smb2CreateContextRequest[] createContextsRequestForReOpen = null;
            if (containCreateDurableContext)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context is also included in the CREATE request.");
                createContextsRequestForReOpen = new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = Guid.NewGuid()
                    },
                    new Smb2CreateAppInstanceId
                    {
                         AppInstanceId = sameAppInstanceId ? appInstanceId : Guid.NewGuid()
                    }
                };
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context is not included in the CREATE request.");
                createContextsRequestForReOpen = new Smb2CreateContextRequest[] { 
                    new Smb2CreateAppInstanceId
                    {
                         AppInstanceId = sameAppInstanceId ? appInstanceId : Guid.NewGuid()
                    }
                };
            }

            clientForReOpen.Create(
                treeIdForReOpen,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdForReOpen,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                createContextsRequestForReOpen,
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    if (sameAppInstanceId)
                    {
                        BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status, "The second client should create the open successfully.");
                    }
                    else
                    {
                        BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, header.Status, "The second client should not create the open successfully.");
                    }
                });

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends another WRITE request.");

            if (sameAppInstanceId)
            {
                clientForInitialOpen.Write(treeIdForInitialOpen, fileIdForInitialOpen, content,
                    checker: (header, response) =>
                    {
                        BaseTestSite.Assert.AreNotEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "The initial open is closed. Write should not succeed. Actually server returns with {0}.", Smb2Status.GetStatusCode(header.Status));
                        BaseTestSite.CaptureRequirementIfAreEqual(
                            Smb2Status.STATUS_FILE_CLOSED,
                            header.Status,
                            RequirementCategory.STATUS_FILE_CLOSED.Id,
                            RequirementCategory.STATUS_FILE_CLOSED.Description);
                    });

                // The first open is closed, no need to do clean up job.
                // Clean up the second client.
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF; DISCONNECT");
                clientForReOpen.Close(treeIdForReOpen, fileIdForReOpen);
                clientForReOpen.TreeDisconnect(treeIdForReOpen);
                clientForReOpen.LogOff();
                clientForReOpen.Disconnect();
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "The initial open is not closed. Write should succeed.");
                clientForInitialOpen.Write(treeIdForInitialOpen, fileIdForInitialOpen, content);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the first client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
                clientForInitialOpen.Close(treeIdForInitialOpen, fileIdForInitialOpen);
                clientForInitialOpen.TreeDisconnect(treeIdForInitialOpen);
                clientForInitialOpen.LogOff();

                // The second client doesn't create the open succesfully, so no need to close the open.
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending the following requests: TREE_DISCONNECT; LOG_OFF; DISCONNECT");
                clientForReOpen.TreeDisconnect(treeIdForReOpen);
                clientForReOpen.LogOff();
                clientForReOpen.Disconnect();
            }

            #endregion
        }
        #endregion
    }
}
