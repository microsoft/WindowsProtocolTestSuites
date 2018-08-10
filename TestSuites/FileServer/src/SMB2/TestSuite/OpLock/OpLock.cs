// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.OpLock
{
    [TestClass]
    public class OpLock : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client1;
        private Smb2FunctionalClient client2;
        private string fileName;
        private string sharePath;
        private uint treeId;
        private FILEID fileId;
        private OPLOCK_BREAK_Notification_Packet OpLockBreakNotifyReceived;
        private ManualResetEvent OpLockNotificationReceived;
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
            sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            fileName = GetTestFileName(sharePath);

            client1 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client2 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client1.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            client2.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            OpLockNotificationReceived = new ManualResetEvent(false);
        }

        protected override void TestCleanup()
        {
            if (client1 != null)
            {
                client1.Disconnect();
            }
            if (client2 != null)
            {
                client2.Disconnect();
            }
            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.OplockOnShareWithoutForceLevel2OrSOFS)]
        [Description("This test case is designed to test whether sever can handle OplockBreak correctly.")]
        public void BVT_OpLockBreak()
        {            
            #region Add Event Handler
            client1.Smb2Client.OplockBreakNotificationReceived += new Action<Packet_Header, OPLOCK_BREAK_Notification_Packet>(OnOpLockBreakNotificationReceived);
            #endregion

            #region Client1 Open a File with BatchOpLock
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start the first client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT.");
            client1.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client1.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            client1.TreeConnect(sharePath, out treeId);

            Smb2CreateContextResponse[] createContextResponse;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends CREATE request with BatchOplock.");
            client1.Create(treeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out fileId, out createContextResponse, RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                       "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));

                    if (response.OplockLevel == OplockLevel_Values.OPLOCK_LEVEL_NONE)
                    {
                        BaseTestSite.Assert.Inconclusive("OpLock request not succeeded.");
                    }
                });
            #endregion

            #region Oplock Break
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start the second client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT.");
            client2.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client2.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            uint client2TreeId;
            client2.TreeConnect(sharePath, out client2TreeId);

            FILEID client2FileId;
            ulong createRequestId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The second client sends CREATE request on the same file with the first client.");
            client2.CreateRequest(client2TreeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out createRequestId);

            BaseTestSite.Assert.IsTrue(OpLockNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "The expected OPLOCK_BREAK_NOTIFY should be received within {0} milliseconds", TestConfig.WaitTimeoutInMilliseconds);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends OPLOCK_ACKNOWLEDGEMENT request after received OPLOCK_BREAK_NOTIFICATION response from server.");
            OplockBreakAcknowledgment(client1);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The second client sends CREATE response");
            client2.CreateResponse(createRequestId, out client2FileId, out createContextResponse);
            #endregion

            #region Tear Down Clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the first client by sending CLOSE, TREE_DISCONNECT, and LOG_OFF.");
            client1.Close(treeId, fileId);
            client1.TreeDisconnect(treeId);
            client1.LogOff();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending CLOSE, TREE_DISCONNECT, and LOG_OFF.");
            client2.Close(client2TreeId, client2FileId);
            client2.TreeDisconnect(client2TreeId);
            client2.LogOff();
            #endregion
        }
        #endregion

        #region Private Methods
        private void OnOpLockBreakNotificationReceived(Packet_Header respHeader, OPLOCK_BREAK_Notification_Packet OpLockBreakNotify)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "OpLockBreakNotification was received from server");
            OpLockBreakNotifyReceived = OpLockBreakNotify;

            ///The FileId field of the response structure MUST be set to the values from the Open structure, 
            ///with the volatile part set to Open.FileId and the persistent part set to Open.DurableFileId.
            BaseTestSite.Assert.AreEqual(fileId, OpLockBreakNotify.FileId, "FileId should be identical");

            BaseTestSite.Assert.IsTrue(
                OpLockBreakNotify.OplockLevel == OPLOCK_BREAK_Notification_Packet_OplockLevel_Values.OPLOCK_LEVEL_II ||
                OpLockBreakNotify.OplockLevel == OPLOCK_BREAK_Notification_Packet_OplockLevel_Values.OPLOCK_LEVEL_NONE,
                "The new oplock level MUST be either SMB2_OPLOCK_LEVEL_NONE or SMB2_OPLOCK_LEVEL_II.");
            OpLockNotificationReceived.Set();
        }

        private void OplockBreakAcknowledgment(Smb2FunctionalClient client)
        {
            OPLOCK_BREAK_Acknowledgment_OplockLevel_Values acknowledgementOplockLevel = default(OPLOCK_BREAK_Acknowledgment_OplockLevel_Values);

            //OpLockBreakNotifyReceived.OplockLevel can only be one of the next two values, otherwise AssertionError will be got in OnOpLockBreakNotificationReceived
            if (OpLockBreakNotifyReceived.OplockLevel == OPLOCK_BREAK_Notification_Packet_OplockLevel_Values.OPLOCK_LEVEL_II)
            {
                acknowledgementOplockLevel = OPLOCK_BREAK_Acknowledgment_OplockLevel_Values.OPLOCK_LEVEL_II;
            }
            else if (OpLockBreakNotifyReceived.OplockLevel == OPLOCK_BREAK_Notification_Packet_OplockLevel_Values.OPLOCK_LEVEL_NONE)
            {
                acknowledgementOplockLevel = OPLOCK_BREAK_Acknowledgment_OplockLevel_Values.OPLOCK_LEVEL_NONE;
            }

            client.OplockAcknowledgement(treeId, fileId, acknowledgementOplockLevel);
        }
        #endregion
    }
}
