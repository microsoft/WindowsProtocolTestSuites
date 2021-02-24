// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite.Leasing
{
    [TestClassAttribute()]
    public partial class LeasingTestCases : PtfTestClassBase
    {
        #region Variables

        private FSAAdapter fsaAdapter;
        private FSATestConfig testConfig;
        /// <summary>
        /// Received LEASE_BREAK_Notification
        /// </summary>
        protected LEASE_BREAK_Notification_Packet receivedLeaseBreakNotify;

        /// <summary>
        /// ManualResetEvent instance for signal
        /// </summary>
        protected ManualResetEvent notificationReceived = new ManualResetEvent(false);

        /// <summary>
        /// The client from which to send LeaseBreakAcknowledgment
        /// </summary>
        protected Smb2FunctionalClient clientToAckLeaseBreak;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected void initializeAdapter()
        {
            this.InitializeTestManager();
            this.fsaAdapter = new FSAAdapter();
            this.fsaAdapter.Initialize(BaseTestSite);
            this.fsaAdapter.LogTestCaseDescription(BaseTestSite);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Test environment:");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 1. File System: " + this.fsaAdapter.FileSystem.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 2. Transport: " + this.fsaAdapter.Transport.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 3. Share Path: " + this.fsaAdapter.UncSharePath);
        }
        
        #endregion

        #region Common Utility For FSCTL test cases

        /// <summary>
        /// To check if current transport supports Integrity.
        /// </summary>
        /// <param name="status">NTStatus code to compare with expected status.</param>
        /// <returns>Return True if supported, False if not supported.</returns>
        private bool IsCurrentTransportSupportIntegrity(MessageStatus status)
        {
            if (this.fsaAdapter.Transport == Transport.SMB)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "This operation is not supported for SMB transport.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// To check if current transport supports CopyOffload.
        /// </summary>
        /// <param name="status">NTStatus code to compare with expected status.</param>
        /// <returns>Return True if supported, False if not supported.</returns>
        private bool IsCurrentTransportSupportCopyOffload(MessageStatus status)
        {
            if (this.fsaAdapter.Transport == Transport.SMB)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "This operation is not supported for SMB transport.");
                return false;
            }
            return true;
        }

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
        /// TimerCallback to be executed to acknowledge LeaseBreakNotification
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isNotificationExpected"></param>
        protected virtual void CheckBreakNotification(bool isNotificationExpected = true)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Check if client received lease break notification");
            // Wait for notification arrival
            bool isNotificationReceived = notificationReceived.WaitOne(testConfig.LeaseBreakNotificationWaitTimeout);

            if (isNotificationExpected)
            {
                BaseTestSite.Assert.IsTrue(
                isNotificationReceived,
                "LeaseBreakNotification should be raised.");
            }
            else
            {
                BaseTestSite.Assert.IsFalse(
                    isNotificationReceived,
                    "Server should not send lease break notification");
            }
        }

        #endregion
    }
}

