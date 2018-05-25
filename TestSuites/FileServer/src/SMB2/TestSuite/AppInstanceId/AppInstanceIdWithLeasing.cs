// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public partial class AppInstanceIdExtendedTest : SMB2TestBase
    {
        /// <summary>
        /// Boolean flag indicating whether LeaseBreak is received
        /// </summary>
        private bool isLeaseBreakReceived = false;

        #region Test Case
        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate file with lease before client failover and expect no lease state is maintained after client failover.")]
        public void AppInstanceId_FileLeasing_NoLeaseInReOpen()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING
                | NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            #endregion

            AppInstanceIdTestWithLeasing(
                false,
                LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING,
                AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE,
                AccessMask.GENERIC_WRITE);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate file with lease before client failover and expect no lease state is maintained after client failover.")]
        public void AppInstanceId_DirectoryLeasing_NoLeaseInReOpen()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING
                | NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);
            #endregion

            AppInstanceIdTestWithLeasing(
                true,
                LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING,
                AccessMask.GENERIC_READ,
                AccessMask.DELETE);
        }
        #endregion

        #region Common Method
        /// <summary>
        /// Test AppInstanceId with leasing
        /// </summary>
        /// <param name="isDirectory">Set true if test lease on directory, otherwise set false</param>
        /// <param name="requestedLeaseState">Lease state that client will request</param>
        /// <param name="accessMask">Access mask that client is used to access file/directory</param>
        /// <param name="accessMaskToTriggerBreak">Access mask that a separate client is used to access file/directory to trigger LeaseBreak</param>
        private void AppInstanceIdTestWithLeasing(bool isDirectory, LeaseStateValues requestedLeaseState, AccessMask accessMask, AccessMask accessMaskToTriggerBreak)
        {
            testDirectory = CreateTestDirectory(uncSharePath);

            #region InitialOpen: Connect to server via Nic1 and create an open with AppInstanceId and leasing
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "InitialOpen: Connect to share via Nic1.");
            FILEID fileIdForInitialOpen;
            uint treeIdForInitialOpen;
            ConnectShare(TestConfig.SutIPAddress, TestConfig.ClientNic1IPAddress, clientForInitialOpen, out treeIdForInitialOpen);

            #region Create an open with AppInstanceId and leasing
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "InitialOpen: Create an open with AppInstanceId and leasing.");
            appInstanceId = Guid.NewGuid();
            Smb2CreateContextResponse[] serverCreateContexts;
            status = clientForInitialOpen.Create(
                treeIdForInitialOpen,
                isDirectory ? testDirectory : fileName,
                isDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdForInitialOpen,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = Guid.NewGuid(),
                    },
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = Guid.NewGuid(),
                        LeaseState = requestedLeaseState,
                    },
                    new Smb2CreateAppInstanceId
                    {
                         AppInstanceId = appInstanceId
                    }
                },
                accessMask: accessMask,
                shareAccess: ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE);

            #endregion

            if (!isDirectory)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "InitialOpen: Write contents to file.");
                status = clientForInitialOpen.Write(treeIdForInitialOpen, fileIdForInitialOpen, contentWrite);
            }
            #endregion

            #region ReOpen: Connect to server via Nic2 and create an open with same AppInstanceId but without leasing
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "ReOpen: Connect to same share via Nic2.");
            FILEID fileIdForReOpen;
            uint treeIdForReOpen;
            clientForReOpen.Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(this.OnLeaseBreakNotificationReceived);
            ConnectShare(TestConfig.SutIPAddress, TestConfig.ClientNic2IPAddress, clientForReOpen, out treeIdForReOpen);

            #region Create an open with AppInstanceId but without leasing
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "ReOpen: Create an open with AppInstanceId but without leasing.");
            status = clientForReOpen.Create(
                treeIdForReOpen,
                isDirectory ? testDirectory : fileName,
                isDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdForReOpen,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = Guid.NewGuid(),
                    },
                    new Smb2CreateAppInstanceId
                    {
                        // Use the same application instance id to force the server close all files
                        // And will clear previous lease
                        AppInstanceId = appInstanceId
                    }
                },
                accessMask: AccessMask.GENERIC_READ);
            #endregion

            if (!isDirectory)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "ReOpen: Read the contents written by InitialOpen.");
                status = clientForReOpen.Read(treeIdForReOpen, fileIdForReOpen, 0, (uint)contentWrite.Length, out contentRead);

                BaseTestSite.Assert.IsTrue(
                    contentRead.Equals(contentWrite),
                    "The written content should equal to read content.");
            }
            #endregion

            #region ReOpen: Access same file/directory from a separate client and expect SUCCESS
            if (!isDirectory)
            {
                Smb2FunctionalClient separateClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client3: Connect to same server.");
                separateClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress, TestConfig.ClientNic2IPAddress);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client3: Trigger a Lease Break.");
                TriggerBreakFromClient(separateClient,
                    TestConfig.RequestDialects,
                    isDirectory,
                    LeaseStateValues.SMB2_LEASE_NONE,
                    accessMaskToTriggerBreak);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client3: Disconnect from the server.");
                separateClient.Disconnect();
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client3: Attempt to create an empty file to trigger lease break.");
                sutProtocolController.CreateFile(Path.Combine(Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare), testDirectory), CurrentTestCaseName, string.Empty);
            }

            Thread.Sleep(500);
            BaseTestSite.Assert.AreNotEqual(
                true,
                isLeaseBreakReceived,
                "No LeaseBreak is expected to be received");
            #endregion

            #region Client tear down
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "ReOpen: Close file.");
            status = clientForReOpen.Close(treeIdForReOpen, fileIdForReOpen);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "ReOpen: Disconnect from the share.");
            status = clientForReOpen.TreeDisconnect(treeIdForReOpen);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "ReOpen: Log Off.");
            status = clientForReOpen.LogOff();

            #endregion
        }

        /// <summary>
        /// Trigger LeaseBreak from a separate client
        /// </summary>
        /// <param name="client">Smb2FunctionalClient object that attempt to access file/directory to trigger lease break</param>
        /// <param name="requestDialect">Dialect in request</param>
        /// <param name="isDirectory">Set true if access a directory, otherwise set false</param>
        /// <param name="requestedLeaseState">Lease state that client will request</param>
        /// <param name="accessMask">Access mask that client is used to access file/directory</param>
        private void TriggerBreakFromClient(
            Smb2FunctionalClient client,
            DialectRevision[] requestDialect,
            bool isDirectory,
            LeaseStateValues requestedLeaseState,
            AccessMask accessMask)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Below steps will trigger a lease break by accessing same file/directory from a separate client with LeaseState {0} and AccessMask {1}", requestedLeaseState, accessMask);
            #region Negotiate
            status = client.Negotiate(
                requestDialect,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
            #endregion

            #region SESSION_SETUP
            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            #endregion

            #region TREE_CONNECT to share
            uint treeId;
            status = client.TreeConnect(uncSharePath, out treeId);
            #endregion

            #region CREATE
            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            status = client.Create(
                treeId,
                isDirectory ? testDirectory : fileName,
                isDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] 
                { 
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = Guid.NewGuid(),
                        LeaseState = requestedLeaseState
                    }
                },
                accessMask: accessMask);
            #endregion

            status = client.Close(treeId, fileId);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Finish triggering lease break.");

        }

        protected override void OnLeaseBreakNotificationReceived(Packet_Header respHeader, LEASE_BREAK_Notification_Packet leaseBreakNotify)
        {
            BaseTestSite.Log.Add(LogEntryKind.Debug, "LeaseBreakNotification was received from server");
            isLeaseBreakReceived = true;
        }
        #endregion
    }
}
