// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class DirectoryLeasing : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client;
        private uint status;
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

        protected override void TestInitialize()
        {
            base.TestInitialize();
            client = new Smb2FunctionalClient(TestConfig.LongerTimeout, TestConfig, BaseTestSite);
            client.Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);
        }

        protected override void TestCleanup()
        {
            if(client != null)
            {
                try
                {
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnect client: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }

        #endregion

        #region Test Case
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [Description("This test case is designed to test whether server can handle READ|WRITE|HANDLE leasing on a directory correctly.")]
        public void BVT_DirectoryLeasing_ReadWriteHandleCaching()
        {            
            DirecotryLeasing(LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle READ leasing on a directory correctly.")]
        public void DirectoryLeasing_ReadCaching()
        {
            DirecotryLeasing(LeaseStateValues.SMB2_LEASE_READ_CACHING);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle READ|WRITE leasing on a directory correctly.")]
        public void DirectoryLeasing_ReadWriteCaching()
        {
            DirecotryLeasing(LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_READ_CACHING);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle READ|HANDLE leasing on a directory correctly.")]
        public void DirectoryLeasing_ReadHandleCaching()
        {
            DirecotryLeasing(LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Negative test case to test SMB3 directory leasing feature using SMB2.1 dialect.")]
        public void DirectoryLeasing_Negative_SMB21()
        {
            DirecotryLeasing(
                new DialectRevision[] { DialectRevision.Smb2002, DialectRevision.Smb21 }, //Requested dialect
                DialectRevision.Smb21, //Expected dialect
                LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Negative test case to test SMB3 directory leasing feature using SMB2.002 dialect.")]
        public void DirectoryLeasing_Negative_SMB2002()
        {
            DirecotryLeasing(
                new DialectRevision[] { DialectRevision.Smb2002 }, //Requested dialect
                DialectRevision.Smb2002, //Expected dialect
                LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.InvalidIdentifier)]
        [Description("Negative test case to test SMB3 directory leasing with invalid lease key set in lease break acknowledgment message.")]
        public void DirectoryLeasing_Negative_InvalidLeaseKeyInLeaseBreakAck()
        {
            DirecotryLeasing(TestConfig.RequestDialects, 
                DialectRevision.Smb30, 
                LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING, 
                LeaseBreakAckType.InvalidLeaseKey);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Negative test case to test SMB3 directory leasing with invalid lease state set in lease break acknowledgment message.")]
        public void DirectoryLeasing_Negative_InvalidLeaseStateInLeaseBreakAck()
        {
            DirecotryLeasing(TestConfig.RequestDialects,
                DialectRevision.Smb30,
                LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING,
                LeaseBreakAckType.InvalidLeaseState);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.InvalidIdentifier)]
        [Description("Negative test case to test SMB3 directory leasing with invalid client GUID set in lease break acknowledgment message.")]
        public void DirectoryLeasing_Negative_InvalidClientGuidInLeaseBreakAck()
        {
            DirecotryLeasing(TestConfig.RequestDialects,
                DialectRevision.Smb30,
                LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING,
                LeaseBreakAckType.InvalidClientGuid);
        }
        #endregion

        #region Common Methods
        private void DirecotryLeasing(
            DialectRevision[] requestDialect, 
            DialectRevision expectedDialect, 
            LeaseStateValues leaseState,
            LeaseBreakAckType leaseBreakAckType = LeaseBreakAckType.None)
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                @"Start to create a directory on share \\{0}\{1} with lease state {2}",
                TestConfig.SutComputerName, TestConfig.BasicFileShare, leaseState);

            string testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            #region Negotiate
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start the client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT");
            Capabilities_Values capabilities = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
            status = client.Negotiate(
                requestDialect,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: capabilities,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Negotiation should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    TestConfig.CheckNegotiateDialect(expectedDialect, response);
                    if (expectedDialect >= DialectRevision.Smb30)
                        TestConfig.CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING, response);
                });
            #endregion

            #region SESSION_SETUP
            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            #endregion

            #region TREE_CONNECT to share
            string uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            uint treeId;
            status = client.TreeConnect(uncSharePath, out treeId);
            #endregion

            #region CREATE
            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep, 
                "Client sends CREATE request for the directory with the LeaseState set to {0} in SMB2_CREATE_REQUEST_LEASE_V2.", leaseState);
            status = client.Create(
                treeId,
                testDirectory,
                CreateOptions_Values.FILE_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] 
                { 
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = Guid.NewGuid(),
                        LeaseState = leaseState
                    }
                },
                checker: (Packet_Header header, CREATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "CREATE should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    if (expectedDialect == DialectRevision.Smb2002 || expectedDialect == DialectRevision.Smb21)
                    {
                        BaseTestSite.Assert.AreEqual(
                            OplockLevel_Values.OPLOCK_LEVEL_NONE,
                            response.OplockLevel,
                            "The expected oplock level is OPLOCK_LEVEL_NONE.");
                    }
                });

            #endregion

            if (expectedDialect >= DialectRevision.Smb30)
            {
                // Break the lease with creating another file in the directory
                sutProtocolController.CreateFile(Path.Combine(Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare), testDirectory), CurrentTestCaseName, string.Empty);

                // Wait until LEASE_BREAK_Notification is received
                BaseTestSite.Assert.IsTrue(
                    // Wait for notification arrival
                    notificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                    "LeaseBreakNotification should be raised.");

                if (receivedLeaseBreakNotify.Flags == LEASE_BREAK_Notification_Packet_Flags_Values.SMB2_NOTIFY_BREAK_LEASE_FLAG_ACK_REQUIRED)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Server requires an LEASE_BREAK_ACK on this LEASE_BREAK_NOTIFY");
                    
                    #region LEASE_BREAK_ACK
                    switch (leaseBreakAckType)
                    {
                        case LeaseBreakAckType.None:
                            status = client.LeaseBreakAcknowledgment(treeId, receivedLeaseBreakNotify.LeaseKey, receivedLeaseBreakNotify.NewLeaseState);
                            break;

                        case LeaseBreakAckType.InvalidLeaseState:
                            LeaseStateValues newLeaseState = LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
                            BaseTestSite.Log.Add(
                                LogEntryKind.Comment,
                                "Client attempts to send LEASE_BREAK_ACK with an invalid LeaseState {0} on this LEASE_BREAK_NOTIFY", newLeaseState);
                            status = client.LeaseBreakAcknowledgment(
                                treeId,
                                receivedLeaseBreakNotify.LeaseKey,
                                newLeaseState,
                                checker: (header, response) =>
                                {
                                    BaseTestSite.Assert.AreNotEqual(
                                        Smb2Status.STATUS_SUCCESS,
                                        header.Status,
                                        "LEASE_BREAK_ACK with invalid LeaseState is not expected to SUCCESS, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                                    BaseTestSite.CaptureRequirementIfAreEqual(
                                        Smb2Status.STATUS_REQUEST_NOT_ACCEPTED,
                                        header.Status,
                                        RequirementCategory.STATUS_REQUEST_NOT_ACCEPTED.Id,
                                        RequirementCategory.STATUS_REQUEST_NOT_ACCEPTED.Description);
                                });
                            break;
                            
                        case LeaseBreakAckType.InvalidLeaseKey:
                            Guid invalidLeaseKey = Guid.NewGuid();
                            BaseTestSite.Log.Add(
                                LogEntryKind.Debug,
                                "Client attempts to send LEASE_BREAK_ACK with an invalid LeaseKey {0} on this LEASE_BREAK_NOTIFY", invalidLeaseKey);
                            status = client.LeaseBreakAcknowledgment(
                                treeId,
                                invalidLeaseKey,
                                receivedLeaseBreakNotify.NewLeaseState,
                                checker: (header, response) =>
                                {
                                    BaseTestSite.Assert.AreNotEqual(
                                        Smb2Status.STATUS_SUCCESS,
                                        header.Status,
                                        "LEASE_BREAK_ACK with invalid LeaseKey is not expected to SUCCESS, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                                    BaseTestSite.CaptureRequirementIfAreEqual(
                                        Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND,
                                        header.Status,
                                        RequirementCategory.STATUS_OBJECT_NAME_NOT_FOUND.Id,
                                        RequirementCategory.STATUS_OBJECT_NAME_NOT_FOUND.Description);
                                });
                            break;

                        case LeaseBreakAckType.InvalidClientGuid:
                            BaseTestSite.Log.Add(LogEntryKind.Debug, "Initialize a new different client to attempts to send LEASE_BREAK_ACK");
                            Smb2FunctionalClient newClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

                            newClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
                            
                            #region Negotiate
                            status = newClient.Negotiate(
                                requestDialect,
                                TestConfig.IsSMB1NegotiateEnabled,
                                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
                            #endregion

                            #region SESSION_SETUP
                            status = newClient.SessionSetup(
                                TestConfig.DefaultSecurityPackage,
                                TestConfig.SutComputerName,
                                TestConfig.AccountCredential,
                                TestConfig.UseServerGssToken);
                            #endregion

                            #region TREE_CONNECT to share
                            uint newTreeId;
                            status = newClient.TreeConnect(uncSharePath, out newTreeId);
                            #endregion

                            status = newClient.LeaseBreakAcknowledgment(
                                newTreeId,
                                receivedLeaseBreakNotify.LeaseKey,
                                receivedLeaseBreakNotify.NewLeaseState,
                                checker: (header, response) =>
                                {
                                    BaseTestSite.Assert.AreNotEqual(
                                        Smb2Status.STATUS_SUCCESS,
                                        header.Status,
                                        "LEASE_BREAK_ACK is not expected to SUCCESS when the open is closed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                                    BaseTestSite.CaptureRequirementIfAreEqual(
                                        Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND,
                                        header.Status,
                                        RequirementCategory.STATUS_OBJECT_NAME_NOT_FOUND.Id,
                                        RequirementCategory.STATUS_OBJECT_NAME_NOT_FOUND.Description);
                                });

                            status = newClient.TreeDisconnect(newTreeId);
                                
                            status = newClient.LogOff();

                            BaseTestSite.Log.Add(
                                LogEntryKind.Comment,
                                "Initialize a new different client to attempts to send LEASE_BREAK_ACK");
                            break;

                        default:
                            throw new InvalidOperationException("Unexpected LeaseBreakAckType " + leaseBreakAckType);
                    }
                    
                    #endregion
                }
                else
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Server does not require an LEASE_BREAK_ACK on this LEASE_BREAK_NOTIFY");
                }
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: TREE_DISCONNECT; LOG_OFF");
            #region TREE_DISCONNECT
            status = client.TreeDisconnect(treeId);
            #endregion

            #region LOGOFF
            status = client.LogOff();
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                @"Complete creating a directory on share \\{0}\{1} with lease state {2}",
                TestConfig.SutComputerName, TestConfig.BasicFileShare, leaseState);
        }
        
        private void DirecotryLeasing(LeaseStateValues leaseState)
        {
            DirecotryLeasing(TestConfig.RequestDialects, TestConfig.MaxSmbVersionSupported, leaseState);
        }
        #endregion
   }
}
