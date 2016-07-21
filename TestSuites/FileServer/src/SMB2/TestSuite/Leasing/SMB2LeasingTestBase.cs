// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public partial class SMB2TestBase
    {
        #region Variables
        /// <summary>
        /// Received LEASE_BREAK_Notification
        /// </summary>
        protected LEASE_BREAK_Notification_Packet receivedLeaseBreakNotify;

        /// <summary>
        /// ManualResetEvent instance for signal
        /// </summary>
        protected ManualResetEvent notificationReceived = new ManualResetEvent(false);

        /// <summary>
        /// Expected new lease state in LEASE_BREAK_Notification
        /// </summary>
        protected LeaseStateValues expectedNewLeaseState;

        /// <summary>
        /// The client from which to send LeaseBreakAcknowledgment
        /// </summary>
        protected Smb2FunctionalClient clientToAckLeaseBreak;
        #endregion

        #region Methods
        /// <summary>
        /// Handler when receive LeaseBreakNotification
        /// </summary>
        /// <param name="respHeader">Packet header in LeaseBreakNotification</param>
        /// <param name="leaseBreakNotify">Received LeaseBreakNotification</param>
        protected virtual void OnLeaseBreakNotificationReceived(Packet_Header respHeader, LEASE_BREAK_Notification_Packet leaseBreakNotify)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "LeaseBreakNotification was received from server");
            receivedLeaseBreakNotify = leaseBreakNotify;

            BaseTestSite.Assert.AreEqual<ulong>(
                0xFFFFFFFFFFFFFFFF,
                respHeader.MessageId,
                "Expect that the field MessageId is set to 0xFFFFFFFFFFFFFFFF.");
            BaseTestSite.Assert.AreEqual<ulong>(
                0,
                respHeader.SessionId,
                "Expect that the field SessionId is set to 0.");
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                respHeader.TreeId,
                "Expect that the field TreeId is set to 0.");
            BaseTestSite.Assert.AreEqual(
                expectedNewLeaseState,
                leaseBreakNotify.NewLeaseState,
                "NewLeaseState in LeaseBreakNotification from server should be {0}", expectedNewLeaseState);
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                leaseBreakNotify.BreakReason,
                "Expect that the field BreakReason is set to 0.");
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                leaseBreakNotify.AccessMaskHint,
                "Expect that the field AccessMaskHint is set to 0.");
            BaseTestSite.Assert.AreEqual<uint>(
                0,
                leaseBreakNotify.ShareMaskHint,
                "Expect that the field ShareMaskHint is set to 0.");

            notificationReceived.Set();
        }

        /// <summary>
        /// Acknowledge LeaseBreakNotification received from server
        /// </summary>
        /// <param name="client">Client to send the acknowledgement</param>
        /// <param name="treeId">TreeId associated to send the acknowledgement</param>
        /// <param name="leaseBreakNotify">LeaseBreakNotification received from server</param>
        protected virtual void AcknowledgeLeaseBreak(Smb2FunctionalClient client, uint treeId, LEASE_BREAK_Notification_Packet leaseBreakNotify)
        {
            if (receivedLeaseBreakNotify.Flags == LEASE_BREAK_Notification_Packet_Flags_Values.SMB2_NOTIFY_BREAK_LEASE_FLAG_ACK_REQUIRED)
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Server requires an LEASE_BREAK_ACK on this LEASE_BREAK_NOTIFY");
                // Will add verification for response after SDK change
                uint status = client.LeaseBreakAcknowledgment(treeId, leaseBreakNotify.LeaseKey, leaseBreakNotify.NewLeaseState);
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "LeaseBreakAcknowledgement should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            }
            else
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Server does not require an LEASE_BREAK_ACK on this LEASE_BREAK_NOTIFY");
            }
        }

        /// <summary>
        /// TimerCallback to be executed to acknowledge LeaseBreakNotification
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void CheckBreakNotification(object obj)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Check if client received lease break notification");
            BaseTestSite.Assert.IsTrue(
                // Wait for notification arrival
                notificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
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
