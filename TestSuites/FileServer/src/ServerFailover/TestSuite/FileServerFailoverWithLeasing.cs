// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite
{
    public partial class FileServerFailoverExtendedTest : ServerFailoverTestBase
    {
        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Failover)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate directory with lease and durable handle before server failover and expect lease break notification.")]
        public void FileServerFailover_DirectoryLeasing()
        {
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.ClusteredFileServerName, TestConfig.ClusteredFileShare);
            testDirectory = CreateTestDirectory(uncSharePath);

            // Add expected NewLeaseState 
            expectedNewLeaseState = LeaseStateValues.SMB2_LEASE_NONE;

            FileServerFailoverWithLeasing(
                true,
                LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING,
                AccessMask.GENERIC_READ,
                AccessMask.DELETE);
        }

        [TestMethod]
        [TestCategory(TestCategories.Failover)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate file with lease and durable handle before server failover and expect lease break notification.")]
        public void FileServerFailover_FileLeasing()
        {
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.ClusteredFileServerName, TestConfig.ClusteredFileShare);

            // Add expected NewLeaseState 
            expectedNewLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;

            FileServerFailoverWithLeasing(
                false,
                LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING,
                AccessMask.GENERIC_READ,
                AccessMask.GENERIC_WRITE);
        }
        #endregion

        #region Common Methods
        /// <summary>
        /// Test FileServer failover with leasing
        /// </summary>
        /// <param name="isDirectory">True to indicate target is directory, otherwise is file</param>
        /// <param name="requestedLeaseState">Original LeaseState to request for durable open</param>
        /// <param name="accessMask">Original AccessMask to request for durable open</param>
        /// <param name="accessMaskToTriggerBreak">AccessMask that a separate client to request to open the same file/directory to trigger LeaseBreakNotification</param>
        private void FileServerFailoverWithLeasing(bool isDirectory, LeaseStateValues requestedLeaseState, AccessMask accessMask, AccessMask accessMaskToTriggerBreak)
        {
            clientAfterFailover.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);

            FILEID fileIdBeforeFailover;
            uint treeIdBeforeFailover;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "BeforeFailover: Connect to general file server {0}.", TestConfig.ClusteredFileServerName);
            ConnectGeneralFileServerBeforeFailover(TestConfig.ClusteredFileServerName, out treeIdBeforeFailover);

            #region CREATE a durable open with flag DHANDLE_FLAG_PERSISTENT
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "BeforeFailover: CREATE a durable open with flag DHANDLE_FLAG_PERSISTENT.");
            Smb2CreateContextResponse[] serverCreateContexts;
            createGuid = Guid.NewGuid();
            leaseKey = Guid.NewGuid();
            status = clientBeforeFailover.Create(
                treeIdBeforeFailover,
                isDirectory ? testDirectory : fileName,
                isDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeFailover,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                         Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                         Timeout = 3600000,
                    },
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = leaseKey,
                        LeaseState = requestedLeaseState,
                    }
                },
                accessMask: accessMask);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Do failover of the file server.");
            FailoverServer(currentAccessIp, TestConfig.ClusteredFileServerName, FileServerType.GeneralFileServer);

            FILEID fileIdAfterFailover = FILEID.Zero;
            uint treeIdAfterFailover;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: Reconnect to the same general file server {0}.", TestConfig.ClusteredFileServerName);
            ReconnectServerAfterFailover(TestConfig.ClusteredFileServerName, FileServerType.GeneralFileServer, out treeIdAfterFailover);

            #region CREATE to reconnect previous duarable open with flag DHANDLE_FLAG_PERSISTENT
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: CREATE to reconnect previous duarable open with flag DHANDLE_FLAG_PERSISTENT.");
            status = DoUntilSucceed(
                () => clientAfterFailover.Create(
                treeIdAfterFailover,
                isDirectory ? testDirectory : fileName,
                isDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterFailover,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleReconnectV2
                    {
                        FileId = new FILEID { Persistent = fileIdBeforeFailover.Persistent },
                        CreateGuid = createGuid,
                        Flags = CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags.DHANDLE_FLAG_PERSISTENT
                    },
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = leaseKey,
                        LeaseState = requestedLeaseState,
                    }
                },
                accessMask: accessMask,
                checker: (header, response) => { }),
                TestConfig.FailoverTimeout,
                "Retry Create until succeed within timeout span");
            #endregion

            // Create a timer that signals the delegate to invoke AckLeaseBreakNotification
            Timer timer = new Timer(AckLeaseBreakNotification, treeIdAfterFailover, 0, Timeout.Infinite);
            base.clientToAckLeaseBreak = clientAfterFailover;

            Smb2FunctionalClient clientTriggeringBreak = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            clientTriggeringBreak.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.ClusteredFileServerName, currentAccessIp);

            // Request CREATE from Client3 to trigger lease break notification and the operation will be blocked until notification is acknowledged or 35 seconds timeout
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "AfterFailover: Request CREATE from Client3 to trigger lease break notification and the operation will be blocked until notification is acknowledged or 35 seconds timeout.");
            TriggerBreakFromClient(
                clientTriggeringBreak,
                TestConfig.RequestDialects,
                TestConfig.ClusteredFileServerName,
                isDirectory,
                LeaseStateValues.SMB2_LEASE_NONE,
                accessMaskToTriggerBreak);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "AfterFailover: Sleep 1 second to wait completion of LeaseBreakNotification check and acknowledgement in background thread");
            Thread.Sleep(1000);

            status = clientAfterFailover.Close(treeIdAfterFailover, fileIdAfterFailover);

            status = clientAfterFailover.TreeDisconnect(treeIdAfterFailover);

            status = clientAfterFailover.LogOff();
        }

        /// <summary>
        /// Attempt to trigger LeaseBreakNotification from a separate client
        /// </summary>
        /// <param name="client">Client to trigger LeaseBreakNotification</param>
        /// <param name="requestDialect">Negotiate dialects</param>
        /// <param name="serverName">Name of file server to access</param>
        /// <param name="isDirectory">True value indicating to open a directory, false for a file</param>
        /// <param name="requestedLeaseState">LeaseState when open the directory or file</param>
        /// <param name="accessMask">AccessMask when open the directory or file</param>
        private void TriggerBreakFromClient(
            Smb2FunctionalClient client,
            DialectRevision[] requestDialect,
            string serverName,
            bool isDirectory,
            LeaseStateValues requestedLeaseState,
            AccessMask accessMask)
        {
            BaseTestSite.Log.Add(LogEntryKind.Debug, "AfterFailover: Attempt to access same file/directory to trigger LeaseBreakNotification from a separate client with LeaseState {0} and AccessMask {1}", requestedLeaseState, accessMask);
            #region Negotiate
            status = client.Negotiate(
                requestDialect,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                });
            #endregion

            #region SESSION_SETUP
            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                serverName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            #endregion

            #region TREE_CONNECT to share
            uint treeId;
            status = client.TreeConnect(uncSharePath, out treeId);
            #endregion

            #region CREATE
            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fileId;
            status = client.Create(
                treeId, // To break lease on directory, create a new child item
                isDirectory ? testDirectory + @"\child.txt" : fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
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
                accessMask: accessMask,
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => { });
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "AfterFailover: Finish triggering a LeaseBreakNotification from a separate client.");
        }

        /// <summary>
        /// Timer Callback to be executed to acknowledge Lease Break Notification
        /// </summary>
        /// <param name="obj"></param>
        protected void AckLeaseBreakNotification(object obj)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Check if client received lease break notification");
            BaseTestSite.Assert.IsTrue(
                // Wait for notification arrival
                notificationReceived.WaitOne(TestConfig.FailoverTimeout),
                "LeaseBreakNotification should be raised.");

            uint treeId = (uint)obj;
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Client attempts to acknowledge the lease break");
            AcknowledgeLeaseBreak(clientToAckLeaseBreak, treeId, receivedLeaseBreakNotify);
        }
        #endregion

    }
}
