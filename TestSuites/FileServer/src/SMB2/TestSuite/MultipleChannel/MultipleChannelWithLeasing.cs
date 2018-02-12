// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public partial class MultipleChannelExtendedTest : SMB2TestBase
    {
        #region Test Case
        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate file via multi-channel with file lease and the lease break is expected from main channel.")]
        public void MultipleChannel_FileLease()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);
            #endregion

            mainChannelClient.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);

            uint treeId;
            FILEID fileId;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Establish main channel with client {0} and server {1}.", clientIps[0].ToString(), serverIps[0].ToString());
            EstablishMainChannel(
                TestConfig.RequestDialects,
                serverIps[0],
                clientIps[0],
                out treeId);

            #region CREATE to open file to request lease
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Main channel: CREATE to open file to request lease.");

            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;

            // Add expected NewLeaseState
            expectedNewLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;
            Smb2CreateContextResponse[] serverCreateContexts;
            status = mainChannelClient.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] {
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = Guid.NewGuid(),
                        LeaseState = leaseState,
                    }
                },
                accessMask: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE);
            #endregion

            #region WRITE content
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Main channel: WRITE content.");
            status = mainChannelClient.Write(treeId, fileId, contentWrite);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Establish alternative channel with client {0} and server {1}.", clientIps[1].ToString(), serverIps[1].ToString());
            EstablishAlternativeChannel(
                TestConfig.RequestDialects,
                serverIps[1],
                clientIps[1],
                treeId);

            // Create a timer that signals the delegate to invoke CheckBreakNotification after 5 seconds
            Timer timer = new Timer(CheckBreakNotification, treeId, 0, Timeout.Infinite);

            Smb2FunctionalClient clientTriggeringBreak = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client3 connect to same server.");
            clientTriggeringBreak.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, serverIps[1], clientIps[1]);

            base.clientToAckLeaseBreak = mainChannelClient;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Request CREATE from Client3 to trigger lease break notification.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "The operation will be blocked until notification is acknowledged or 35 seconds timeout.");
            TriggerBreakFromClient(
                clientTriggeringBreak,
                TestConfig.RequestDialects,
                false,
                LeaseStateValues.SMB2_LEASE_NONE,
                AccessMask.GENERIC_WRITE);

            ClientTearDown(alternativeChannelClient, treeId, fileId);
        }
        #endregion

        #region Common Method
        /// <summary>
        /// Attempt to trigger LeaseBreakNotification from a separate client
        /// </summary>
        /// <param name="client">Client to trigger LeaseBreakNotification</param>
        /// <param name="requestDialect">Negotiate dialects</param>
        /// <param name="isDirectory">True value indicating to open a directory, false for a file</param>
        /// <param name="requestedLeaseState">LeaseState when open the directory or file</param>
        /// <param name="accessMask">AccessMask when open the directory or file</param>
        private void TriggerBreakFromClient(
            Smb2FunctionalClient client,
            DialectRevision[] requestDialect,
            bool isDirectory,
            LeaseStateValues requestedLeaseState,
            AccessMask accessMask)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Trigger a lease break notification by accessing same file/directory from a separate client with LeaseState {0} and AccessMask {1}", requestedLeaseState, accessMask);
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
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Finish triggering lease break notification");
        }
        #endregion
    }
}
