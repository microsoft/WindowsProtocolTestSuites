// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class LeasingExtendedTest : SMB2TestBase
    {
        #region Variables
        private uint status;
        private string uncSharePath;
        private string testDirectory;
        private string fileName;

        /// <summary>
        /// Client collection used in test and indexed by string as client name
        /// </summary>
        private Dictionary<Guid, Smb2FunctionalClient> testClients;

        /// <summary>
        /// LEASE_BREAK_Notification packet collection received from server and indexed by LeaseKey Guid
        /// </summary>
        private Dictionary<Guid, LEASE_BREAK_Notification_Packet> breakNotifications;

        /// <summary>
        /// Collection for expected new lease state in LEASE_BREAK_Notification and indexed by LeaseKey Guid
        /// </summary>
        private Dictionary<Guid, LeaseStateValues> expectedNewLeaseStates;

        /// <summary>
        /// ClientGuid list for clients used in test, and is also used as LeaseKey when request a lease
        /// </summary>
        private List<Guid> clientGuids;

        /// <summary>
        /// Manual event signal collection indicate the arrival of LEASE_BREAK_Notification packet and indexed by the LeaseKey Guid
        /// </summary>
        private Dictionary<Guid, ManualResetEvent> notificationsReceived;
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

            notificationsReceived = new Dictionary<Guid, ManualResetEvent>();
            clientGuids = new List<Guid>();
            testClients = new Dictionary<Guid, Smb2FunctionalClient>();
            breakNotifications = new Dictionary<Guid, LEASE_BREAK_Notification_Packet>();
            expectedNewLeaseStates = new Dictionary<Guid, LeaseStateValues>();

            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
        }

        protected override void TestCleanup()
        {
            foreach (var client in testClients.Values)
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
        [Description("Test whether server can handle lease break notification when multiple clients request caching lease.")]
        public void BVT_DirectoryLeasing_LeaseBreakOnMultiClients()
        {
            #region Prepare test directory
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create test directory.");
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            #endregion

            #region Initialize test clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize test clients.");

            Guid client1GuidRequestingLease = Guid.NewGuid();
            clientGuids.Add(client1GuidRequestingLease);
            notificationsReceived.Add(client1GuidRequestingLease, new ManualResetEvent(false));
            testClients.Add(client1GuidRequestingLease, new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite));

            Guid client2GuidRequestingLease = Guid.NewGuid();
            clientGuids.Add(client2GuidRequestingLease);
            notificationsReceived.Add(client2GuidRequestingLease, new ManualResetEvent(false));
            testClients.Add(client2GuidRequestingLease, new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite));

            Guid clientGuidTirggeringBreak = Guid.NewGuid();
            testClients.Add(clientGuidTirggeringBreak, new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite));

            testClients[client1GuidRequestingLease].Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(this.OnLeaseBreakNotificationReceived);
            testClients[client2GuidRequestingLease].Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(this.OnLeaseBreakNotificationReceived);

            testClients[client1GuidRequestingLease].ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            testClients[client2GuidRequestingLease].ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            testClients[clientGuidTirggeringBreak].ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            #endregion

            #region CREATE an open from one client to request lease
            uint treeIdClient1RequestingLease;
            FILEID fileIdClient1RequestingLease;
            LeaseStateValues client1RequestedLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;
            AccessMask client1AccessMask = AccessMask.GENERIC_READ;

            // Add expected NewLeaseState
            expectedNewLeaseStates.Add(client1GuidRequestingLease, LeaseStateValues.SMB2_LEASE_READ_CACHING);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 attempts to request lease {0} on directory {1}", client1RequestedLeaseState, testDirectory);
            status = CreateOpenFromClient(testClients[client1GuidRequestingLease], client1GuidRequestingLease, testDirectory, true, client1RequestedLeaseState, client1AccessMask, out treeIdClient1RequestingLease, out fileIdClient1RequestingLease);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));
            #endregion

            #region CREATE an open from a 2nd client to request lease
            uint treeIdClient2RequestingLease;
            FILEID fileIdClient2RequestingLease;
            LeaseStateValues client2RequestedLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;
            AccessMask client2AccessMask = AccessMask.GENERIC_READ;

            // Add expected NewLeaseState
            expectedNewLeaseStates.Add(client2GuidRequestingLease, LeaseStateValues.SMB2_LEASE_READ_CACHING);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client2 attempts to request lease {0} on directory {1}", client2RequestedLeaseState, testDirectory);
            status = CreateOpenFromClient(testClients[client2GuidRequestingLease], client2GuidRequestingLease, testDirectory, true, client2RequestedLeaseState, client2AccessMask, out treeIdClient2RequestingLease, out fileIdClient2RequestingLease);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));
            #endregion

            Dictionary<Guid, uint> treeIds = new Dictionary<Guid, uint>();
            treeIds.Add(client1GuidRequestingLease, treeIdClient1RequestingLease);
            treeIds.Add(client2GuidRequestingLease, treeIdClient2RequestingLease);
            // Create a timer that signals the delegate to invoke CheckBreakNotification
            Timer timer = new Timer(CheckBreakNotification, treeIds, 0, Timeout.Infinite);

            #region Attemp to trigger lease break from a separate client
            uint treeIdClientTriggerBreak;
            FILEID fileIdClientTriggerBreak;
            LeaseStateValues breakTriggerRequestedLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;
            AccessMask breakTriggerAccessMask = AccessMask.DELETE;
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client3 attempts to access directory {0} to trigger lease break with AccessMask {1}", testDirectory, breakTriggerAccessMask.ToString());
            // Request CREATE from Client3 to trigger lease break notification and the operation will be blocked until notification is acknowledged or 35 seconds timeout
            status = CreateOpenFromClient(testClients[clientGuidTirggeringBreak], clientGuidTirggeringBreak, testDirectory, true, breakTriggerRequestedLeaseState, breakTriggerAccessMask, out treeIdClientTriggerBreak, out fileIdClientTriggerBreak);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client3 attempts to access directory {0} to trigger lease break with AccessMask {1}", testDirectory, breakTriggerAccessMask.ToString());
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down Client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            ClientTearDown(testClients[client1GuidRequestingLease], treeIdClient1RequestingLease, fileIdClient1RequestingLease);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down Client2 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            ClientTearDown(testClients[client2GuidRequestingLease], treeIdClient2RequestingLease, fileIdClient2RequestingLease);

            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Ready to tear down Client3 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            ClientTearDown(testClients[clientGuidTirggeringBreak], treeIdClientTriggerBreak, fileIdClientTriggerBreak);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test whether server can handle READ lease break notification triggered by deleting child item on a directory.")]
        public void DirectoryLeasing_BreakReadCachingByChildDeleted()
        {
            #region Prepare test directory and test file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create test directory and test file.");

            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            fileName = "DirectoryLeasing_BreakReadCachingByChildDeleted_" + Guid.NewGuid().ToString() + ".txt";
            testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            sutProtocolController.CreateFile(uncSharePath + "\\" + testDirectory, fileName, string.Empty);
            #endregion

            #region Initialize test clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize test clients.");

            Guid clientGuidRequestingLease = Guid.NewGuid();
            Smb2FunctionalClient clientRequestingLease = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            Guid clientGuidTriggeringBreak = Guid.NewGuid();
            Smb2FunctionalClient clientTriggeringBreak = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            clientRequestingLease.Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);

            clientRequestingLease.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            clientTriggeringBreak.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            #endregion

            #region CREATE an open to request lease
            uint treeIdClientRequestingLease;
            FILEID fileIdClientRequestingLease;
            LeaseStateValues requestedLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;

            // Add expected NewLeaseState
            expectedNewLeaseStates.Add(clientGuidRequestingLease, LeaseStateValues.SMB2_LEASE_NONE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client attempts to request lease {0} on directory {1}", requestedLeaseState.ToString(), testDirectory);
            status = CreateOpenFromClient(clientRequestingLease, clientGuidRequestingLease, testDirectory, true, requestedLeaseState, AccessMask.GENERIC_READ, out treeIdClientRequestingLease, out fileIdClientRequestingLease);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));
            #endregion

            // Create a timer that signals the delegate to invoke CheckBreakNotification
            Timer timer = new Timer(base.CheckBreakNotification, treeIdClientRequestingLease, 0, Timeout.Infinite);
            base.clientToAckLeaseBreak = clientRequestingLease;

            #region Attempt to trigger lease break by deleting child item
            uint treeIdClientTriggeringBreak;
            FILEID fileIdClientTriggeringBreak;
            AccessMask accessMaskTrigger = AccessMask.DELETE;
            string targetName = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "A separate client attempts to access directory {0} to trigger lease break by deleting inner file ", testDirectory);
            status = CreateOpenFromClient(clientTriggeringBreak, clientGuidTriggeringBreak, targetName, false, LeaseStateValues.SMB2_LEASE_NONE, accessMaskTrigger, out treeIdClientTriggeringBreak, out fileIdClientTriggeringBreak);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", targetName, Smb2Status.GetStatusCode(status));

            ClientTearDown(clientTriggeringBreak, treeIdClientTriggeringBreak, fileIdClientTriggeringBreak);
            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test whether server can handle READ lease break notification triggered by adding child item on a directory.")]
        public void DirectoryLeasing_BreakReadCachingByChildAdded()
        {
            #region Prepare test directory and test file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create test directory and test file.");

            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            fileName = "DirectoryLeasing_BreakReadCachingByChildAdded_" + Guid.NewGuid().ToString() + ".txt";
            testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            sutProtocolController.CreateFile(uncSharePath + "\\" + testDirectory, fileName, string.Empty);
            #endregion

            #region Initialize test clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize test clients.");

            Guid clientGuidRequestingLease = Guid.NewGuid();
            Smb2FunctionalClient clientRequestingLease = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            Guid clientGuidTriggeringBreak = Guid.NewGuid();
            Smb2FunctionalClient clientTriggeringBreak = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            clientRequestingLease.Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);

            clientRequestingLease.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            clientTriggeringBreak.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            #endregion

            #region CREATE an open to request lease
            uint treeIdClientRequestingLease;
            FILEID fileIdClientRequestingLease;
            LeaseStateValues requestedLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;

            // Add expected NewLeaseState
            expectedNewLeaseStates.Add(clientGuidRequestingLease, LeaseStateValues.SMB2_LEASE_NONE);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client attempts to request lease {0} on directory {1}", requestedLeaseState.ToString(), testDirectory);
            status = CreateOpenFromClient(clientRequestingLease, clientGuidRequestingLease, testDirectory, true, requestedLeaseState, AccessMask.GENERIC_READ, out treeIdClientRequestingLease, out fileIdClientRequestingLease);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));
            #endregion

            // Create a timer that signals the delegate to invoke CheckBreakNotification
            Timer timer = new Timer(base.CheckBreakNotification, treeIdClientRequestingLease, 0, Timeout.Infinite);
            base.clientToAckLeaseBreak = clientRequestingLease;

            #region Attempt to trigger lease break by deleting child item
            uint treeIdClientTriggeringBreak;
            FILEID fileIdClientTriggeringBreak;
            AccessMask accessMaskTrigger = AccessMask.FILE_ADD_FILE;
            string targetName = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "A separate client attempts to access directory {0} to trigger lease break by adding inner file ", testDirectory);
            status = CreateOpenFromClient(clientTriggeringBreak, clientGuidTriggeringBreak, targetName, false, LeaseStateValues.SMB2_LEASE_NONE, accessMaskTrigger, out treeIdClientTriggeringBreak, out fileIdClientTriggeringBreak);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", targetName, Smb2Status.GetStatusCode(status));

            ClientTearDown(clientTriggeringBreak, treeIdClientTriggeringBreak, fileIdClientTriggeringBreak);
            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test whether server can handle READ lease break notification triggered by modifying child item on a directory.")]
        public void DirectoryLeasing_BreakReadCachingByChildModified()
        {
            #region Prepare test directory and test file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create test directory and test file.");
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            fileName = "DirectoryLeasing_BreakReadCachingByChildModified_" + Guid.NewGuid().ToString() + ".txt";
            testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            sutProtocolController.CreateFile(uncSharePath + "\\" + testDirectory, fileName, string.Empty);
            #endregion

            #region Initialize test clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize test clients.");

            Guid clientGuidRequestingLease = Guid.NewGuid();
            Smb2FunctionalClient clientRequestingLease = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            Guid clientGuidTriggeringBreak = Guid.NewGuid();
            Smb2FunctionalClient clientTriggeringBreak = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            clientRequestingLease.Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);

            clientRequestingLease.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            clientTriggeringBreak.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            #endregion

            #region CREATE an open to request lease
            uint treeIdClientRequestingLease;
            FILEID fileIdClientRequestingLease;
            LeaseStateValues requestedLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;

            // Add expected NewLeaseState
            expectedNewLeaseState = LeaseStateValues.SMB2_LEASE_NONE;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client attempts to request lease {0} on directory {1}", requestedLeaseState, testDirectory);
            status = CreateOpenFromClient(clientRequestingLease, clientGuidRequestingLease, testDirectory, true, requestedLeaseState, AccessMask.GENERIC_READ, out treeIdClientRequestingLease, out fileIdClientRequestingLease);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));
            #endregion
            // Create a timer that signals the delegate to invoke CheckBreakNotification
            Timer timer = new Timer(base.CheckBreakNotification, treeIdClientRequestingLease, 0, Timeout.Infinite);
            base.clientToAckLeaseBreak = clientRequestingLease;

            #region Attempt to trigger lease break by modifying child item
            uint treeIdClientTriggeringBreak;
            FILEID fileIdClientTriggeringBreak;
            AccessMask accessMaskTrigger = AccessMask.GENERIC_WRITE;
            string targetName = testDirectory + "\\" + fileName;
            string contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "A separate client attempts to access directory {0} to trigger lease break by modifying inner file", testDirectory);
            status = CreateOpenFromClient(clientTriggeringBreak, clientGuidTriggeringBreak, targetName, false, LeaseStateValues.SMB2_LEASE_NONE, accessMaskTrigger, out treeIdClientTriggeringBreak, out fileIdClientTriggeringBreak);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));

            status = clientTriggeringBreak.Write(treeIdClientTriggeringBreak, fileIdClientTriggeringBreak, contentWrite);

            ClientTearDown(clientTriggeringBreak, treeIdClientTriggeringBreak, fileIdClientTriggeringBreak);
            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test whether server can handle READ lease break notification triggered by renaming child item on a directory.")]
        public void DirectoryLeasing_BreakReadCachingByChildRenamed()
        {
            #region Prepare test directory and test file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create test directory and test file.");
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            fileName = "DirectoryLeasing_BreakReadCachingByChildRenamed_" + Guid.NewGuid().ToString() + ".txt";
            sutProtocolController.CreateFile(uncSharePath + "\\" + testDirectory, fileName, string.Empty);
            #endregion

            #region Initialize test clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize test clients.");

            Guid clientGuidRequestingLease = Guid.NewGuid();
            Smb2FunctionalClient clientRequestingLease = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            Guid clientGuidTriggeringBreak = Guid.NewGuid();
            Smb2FunctionalClient clientTriggeringBreak = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            clientRequestingLease.Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);

            clientRequestingLease.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            clientTriggeringBreak.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            #endregion

            #region CREATE an open to request lease
            uint treeIdClientRequestingLease;
            FILEID fileIdClientRequestingLease;
            LeaseStateValues requestedLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;

            // Add expected NewLeaseState
            expectedNewLeaseState = LeaseStateValues.SMB2_LEASE_NONE;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client attempts to request lease {0} on directory {1}", requestedLeaseState, testDirectory);
            status = CreateOpenFromClient(clientRequestingLease, clientGuidRequestingLease, testDirectory, true, requestedLeaseState, AccessMask.GENERIC_READ, out treeIdClientRequestingLease, out fileIdClientRequestingLease);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));
            #endregion
            // Create a timer that signals the delegate to invoke CheckBreakNotification
            Timer timer = new Timer(base.CheckBreakNotification, treeIdClientRequestingLease, 0, Timeout.Infinite);
            base.clientToAckLeaseBreak = clientRequestingLease;

            #region Attempt to trigger lease break by renaming child item
            uint treeIdClientTriggeringBreak;
            FILEID fileIdClientTriggeringBreak;
            AccessMask accessMaskTrigger = AccessMask.DELETE;
            string targeName = testDirectory + "\\" + fileName;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "A separate client attempts to access directory {0} to trigger lease break by renaming it", testDirectory);
            status = CreateOpenFromClient(clientTriggeringBreak, clientGuidTriggeringBreak, targeName, false, LeaseStateValues.SMB2_LEASE_NONE, accessMaskTrigger, out treeIdClientTriggeringBreak, out fileIdClientTriggeringBreak);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", targeName, Smb2Status.GetStatusCode(status));

            #region SetFileAttributes with FileRenameInformation to rename child item
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SetFileAttributes with FileRenameInformation to rename child item.");

            string newName = "Renamed_" + fileName;
            FileRenameInformation fileRenameInfo;
            fileRenameInfo.ReplaceIfExists = TypeMarshal.ToBytes(false)[0];
            fileRenameInfo.Reserved = new byte[7];
            fileRenameInfo.RootDirectory = FileRenameInformation_RootDirectory_Values.V1;
            fileRenameInfo.FileName = Encoding.Unicode.GetBytes(newName);
            fileRenameInfo.FileNameLength = (uint)fileRenameInfo.FileName.Length;

            byte[] inputBuffer;
            inputBuffer = TypeMarshal.ToBytes<FileRenameInformation>(fileRenameInfo);

            status = clientTriggeringBreak.SetFileAttributes(
                        treeIdClientTriggeringBreak,
                        (byte)FileInformationClasses.FileRenameInformation,
                        fileIdClientTriggeringBreak,
                        inputBuffer,
                        (header, response) => { });
            #endregion

            ClientTearDown(clientTriggeringBreak, treeIdClientTriggeringBreak, fileIdClientTriggeringBreak);
            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test whether server can handle HANDLE lease break notification triggered by renaming parent directory.")]
        public void DirectoryLeasing_BreakHandleCachingByParentRenamed()
        {
            #region Prepare test directory
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create test directory.");
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            string parentDirectory = CreateTestDirectory(uncSharePath);
            testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare + "\\" + parentDirectory);
            #endregion

            #region Initialize test clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize test clients.");

            Guid clientGuidRequestingLease = Guid.NewGuid();
            Smb2FunctionalClient clientRequestingLease = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            Guid clientGuidTriggeringBreak = Guid.NewGuid();
            Smb2FunctionalClient clientTriggeringBreak = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            clientRequestingLease.Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);

            clientRequestingLease.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            clientTriggeringBreak.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            #endregion

            #region CREATE an open to request lease
            uint treeIdClientRequestingLease;
            FILEID fileIdClientRequestingLease;
            string targetName = parentDirectory + "\\" + testDirectory;
            LeaseStateValues requestedLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;

            // Add expected NewLeaseState
            expectedNewLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client attempts to request lease {0} on directory {1}", requestedLeaseState, testDirectory);
            status = CreateOpenFromClient(clientRequestingLease, clientGuidRequestingLease, targetName, true, requestedLeaseState, AccessMask.GENERIC_READ, out treeIdClientRequestingLease, out fileIdClientRequestingLease);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));
            #endregion
            // Create a timer that signals the delegate to invoke CheckBreakNotification
            Timer timer = new Timer(base.CheckBreakNotification, treeIdClientRequestingLease, 0, Timeout.Infinite);
            base.clientToAckLeaseBreak = clientRequestingLease;

            #region Attempt to trigger lease break by renaming parent directory
            uint treeIdClientTriggeringBreak;
            FILEID fileIdClientTriggeringBreak;
            AccessMask accessMaskTrigger = AccessMask.DELETE;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "A separate client attempts to access directory {0} to trigger lease break by renaming its parent directory", testDirectory);
            status = CreateOpenFromClient(clientTriggeringBreak, clientGuidTriggeringBreak, parentDirectory, true, LeaseStateValues.SMB2_LEASE_NONE, accessMaskTrigger, out treeIdClientTriggeringBreak, out fileIdClientTriggeringBreak);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));

            #region SetFileAttributes with FileRenameInformation to rename parent directory
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "SetFileAttributes with FileRenameInformation to rename parent directory.");

            string newName = "Renamed" + parentDirectory;
            FileRenameInformation fileRenameInfo;
            fileRenameInfo.ReplaceIfExists = TypeMarshal.ToBytes(false)[0];
            fileRenameInfo.Reserved = new byte[7];
            fileRenameInfo.RootDirectory = FileRenameInformation_RootDirectory_Values.V1;
            fileRenameInfo.FileName = Encoding.Unicode.GetBytes(newName);
            fileRenameInfo.FileNameLength = (uint)fileRenameInfo.FileName.Length;

            byte[] inputBuffer;
            inputBuffer = TypeMarshal.ToBytes<FileRenameInformation>(fileRenameInfo);

            status = clientTriggeringBreak.SetFileAttributes(
                        treeIdClientTriggeringBreak,
                        (byte)FileInformationClasses.FileRenameInformation,
                        fileIdClientTriggeringBreak,
                        inputBuffer,
                        (header, response) =>
                        {
                            BaseTestSite.Assert.AreNotEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "{0} should succeed, actually server returns {1}", header.Command, Smb2Status.GetStatusCode(header.Status));
                            BaseTestSite.CaptureRequirementIfAreEqual(
                                Smb2Status.STATUS_ACCESS_DENIED,
                                header.Status,
                                RequirementCategory.STATUS_ACCESS_DENIED.Id,
                                RequirementCategory.STATUS_ACCESS_DENIED.Description);
                        });
            #endregion

            #endregion

            ClientTearDown(clientRequestingLease, treeIdClientRequestingLease, fileIdClientRequestingLease);
            ClientTearDown(clientTriggeringBreak, treeIdClientTriggeringBreak, fileIdClientTriggeringBreak);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test whether server can handle HANDLE lease break notification triggered by deleting parent directory.")]
        public void DirectoryLeasing_BreakHandleCachingByParentDeleted()
        {
            #region Prepare test directory
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create test directory.");
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            string parentDirectory = CreateTestDirectory(uncSharePath);
            testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare + "\\" + parentDirectory);
            #endregion

            #region Initialize test clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize test clients.");

            Guid clientGuidRequestingLease = Guid.NewGuid();
            Smb2FunctionalClient clientRequestingLease = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            Guid clientGuidTriggeringBreak = Guid.NewGuid();
            Smb2FunctionalClient clientTriggeringBreak = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            clientRequestingLease.Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);

            clientRequestingLease.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            clientTriggeringBreak.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            #endregion

            #region CREATE an open to request lease
            uint treeIdClientRequestingLease;
            FILEID fileIdClientRequestingLease;
            string targetName = parentDirectory + "\\" + testDirectory;
            LeaseStateValues requestedLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;

            // Add expected NewLeaseState
            expectedNewLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client attempts to request lease {0} on directory {1}", requestedLeaseState, testDirectory);
            status = CreateOpenFromClient(clientRequestingLease, clientGuidRequestingLease, targetName, true, requestedLeaseState, AccessMask.GENERIC_READ, out treeIdClientRequestingLease, out fileIdClientRequestingLease);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));
            #endregion
            // Create a timer that signals the delegate to invoke CheckBreakNotification
            Timer timer = new Timer(base.CheckBreakNotification, treeIdClientRequestingLease, 0, Timeout.Infinite);
            base.clientToAckLeaseBreak = clientRequestingLease;

            #region Attempt to trigger lease break by deleting parent directory
            uint treeIdClientTriggeringBreak;
            FILEID fileIdClientTriggeringBreak;
            AccessMask accessMaskTrigger = AccessMask.DELETE;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "A separate client attempts to trigger lease break by deleting its parent directory");
            status = CreateOpenFromClient(clientTriggeringBreak, clientGuidTriggeringBreak, parentDirectory, true, LeaseStateValues.SMB2_LEASE_NONE, accessMaskTrigger, out treeIdClientTriggeringBreak, out fileIdClientTriggeringBreak);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed", parentDirectory);

            #region set FileDispositionInformation for deletion
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Set FileDispositionInformation for deletion.");

            FileDispositionInformation fileDispositionInfo;
            fileDispositionInfo.DeletePending = 1;  // Set 1 to indicate directory SHOULD be delted when the open closed
            byte[] inputBuffer = TypeMarshal.ToBytes<FileDispositionInformation>(fileDispositionInfo);
            status = clientTriggeringBreak.SetFileAttributes(
                        treeIdClientTriggeringBreak,
                        (byte)FileInformationClasses.FileDispositionInformation,
                        fileIdClientTriggeringBreak,
                        inputBuffer,
                        (header, response) =>
                        {
                            BaseTestSite.Assert.AreNotEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "Setting FileDispositionInformation to the parent directory for deletion when child is opened by others is not expected to SUCCESS. " +
                                "Actually server returns with {0}.", Smb2Status.GetStatusCode(header.Status));
                            BaseTestSite.CaptureRequirementIfAreEqual(
                                Smb2Status.STATUS_DIRECTORY_NOT_EMPTY,
                                header.Status,
                                RequirementCategory.STATUS_DIRECTORY_NOT_EMPTY.Id,
                                RequirementCategory.STATUS_DIRECTORY_NOT_EMPTY.Description);
                        });

            status = clientTriggeringBreak.Close(treeIdClientTriggeringBreak, fileIdClientTriggeringBreak);
            #endregion

            #region CREATE an open to parent directory again
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "CREATE an open to parent directory again.");

            // Currently we need an additional CREATE to open the parent directory to trigger the lease break
            // which is the same way when Windows attempt to delete the parent directory when child is opened by others
            Smb2CreateContextResponse[] serverCreateContexts;
            status = clientTriggeringBreak.Create(
                treeIdClientTriggeringBreak,
                targetName,
                CreateOptions_Values.FILE_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileIdClientTriggeringBreak,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]
                {
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = clientGuidTriggeringBreak,
                        LeaseState = LeaseStateValues.SMB2_LEASE_NONE
                    }
                },
                accessMask: accessMaskTrigger);
            #endregion
            #endregion

            ClientTearDown(clientRequestingLease, treeIdClientRequestingLease, fileIdClientRequestingLease);
            ClientTearDown(clientTriggeringBreak, treeIdClientTriggeringBreak, fileIdClientTriggeringBreak);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test whether server can handle HANDLE lease break notification triggered by a conflict open.")]
        public void DirectoryLeasing_BreakHandleCachingByConflictOpen()
        {
            #region Prepare test directory
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create test directory.");
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            testDirectory = CreateTestDirectory(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            #endregion

            #region Initialize test clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize test clients.");

            Guid clientGuidRequestingLease = Guid.NewGuid();
            Smb2FunctionalClient clientRequestingLease = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            Guid clientGuidTriggeringBreak = Guid.NewGuid();
            Smb2FunctionalClient clientTriggeringBreak = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            clientRequestingLease.Smb2Client.LeaseBreakNotificationReceived +=
                new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);

            clientRequestingLease.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            clientTriggeringBreak.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            #endregion

            #region CREATE an open to request lease
            uint treeIdClientRequestingLease;
            FILEID fileIdClientRequestingLease;
            LeaseStateValues requestedLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;

            // Add expected NewLeaseState
            expectedNewLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client attempts to request lease {0} on directory {1}", requestedLeaseState, testDirectory);
            status = CreateOpenFromClient(clientRequestingLease, clientGuidRequestingLease, testDirectory, true, requestedLeaseState, AccessMask.GENERIC_READ, out treeIdClientRequestingLease, out fileIdClientRequestingLease, // Doesn't allow share access then another open will be conflict
                ShareAccess_Values.NONE);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} should succeed, actual status is {1}", testDirectory, Smb2Status.GetStatusCode(status));
            #endregion
            // Create a timer that signals the delegate to invoke CheckBreakNotification
            Timer timer = new Timer(base.CheckBreakNotification, treeIdClientRequestingLease, 0, Timeout.Infinite);
            base.clientToAckLeaseBreak = clientRequestingLease;

            #region Attempt to trigger lease break by a conflict open
            uint treeIdClientTriggeringBreak;
            FILEID fileIdClientTriggeringBreak;
            AccessMask accessMaskTrigger = AccessMask.GENERIC_WRITE;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "A separate client attempts to access directory {0} to trigger lease break by making access conflict.", testDirectory);
            status = CreateOpenFromClient(clientTriggeringBreak, clientGuidTriggeringBreak, testDirectory, true, LeaseStateValues.SMB2_LEASE_NONE, accessMaskTrigger, out treeIdClientTriggeringBreak, out fileIdClientTriggeringBreak);
            BaseTestSite.Assert.AreNotEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open to {0} is not expected to SUCCESS when it is conflict with another one", testDirectory, Smb2Status.GetStatusCode(status));
            BaseTestSite.CaptureRequirementIfAreEqual(
                Smb2Status.STATUS_SHARING_VIOLATION,
                status,
                RequirementCategory.STATUS_SHARING_VIOLATION.Id,
                RequirementCategory.STATUS_SHARING_VIOLATION.Description);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Tear down Client1 by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            ClientTearDown(clientRequestingLease, treeIdClientRequestingLease, fileIdClientRequestingLease);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("Verify only SMB2_LEASE_READ_CACHING granted when client request SMB2_LEASE_READ_CACHING | SMB2_LEASE_WRITE_CACHING.")]
        public void DirectoryLeasing_RWGrantedAsR()
        {
            VerifyGrantedLeaseState(LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING, LeaseStateValues.SMB2_LEASE_READ_CACHING);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DirectoryLeasing)]
        [TestCategory(TestCategories.Positive)]
        [Description("Verify only SMB2_ SMB2_LEASE_READ_CACHING | SMB2_LEASE_HANDLE_CACHING granted when client request SMB2_LEASE_READ_CACHING | SMB2_LEASE_WRITE_CACHING | SMB2_LEASE_HANDLE_CACHING.")]
        public void DirectoryLeasing_RWHGrantedAsRH()
        {
            VerifyGrantedLeaseState(LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING, LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING);
        }
        #endregion

        #region Common Method
        /// <summary>
        /// Create an open from client, this include NEGOTIATE and SESSION_SETUP with server, TREE_CONNECT to the share and CREATE an open to file/directory
        /// </summary>
        /// <param name="client">Client used to take the operation</param>
        /// <param name="clientGuid">Client GUID for negotiation</param>
        /// <param name="targetName">File/directory name for the open</param>
        /// <param name="isDirectory">Set true if create open to a directory, set false if create open to a file</param>
        /// <param name="accessMask">Desired access when create the open</param>
        /// <param name="treeId">Out param for tree id used to connect to the share</param>
        /// <param name="fileId">Out param for file id that is associated with the open</param>
        /// <param name="shareAccess">Optional param for share access when create the open</param>
        /// <returns>Status value returned from CREATE request</returns>
        private uint CreateOpenFromClient(Smb2FunctionalClient client, Guid clientGuid, string targetName, bool isDirectory, LeaseStateValues requestLeaseState, AccessMask accessMask, out uint treeId, out FILEID fileId, ShareAccess_Values shareAccess = ShareAccess_Values.FILE_SHARE_DELETE | ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE)
        {
            #region Negotiate
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client {0} sends NEGOTIATE request with the following capabilities: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES.", clientGuid.ToString());
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                clientGuid: clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Negotiation is expected success, actually server returns {0}", Smb2Status.GetStatusCode(header.Status));

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                    TestConfig.CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING, response);
                });
            #endregion

            #region SESSION_SETUP
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client {0} sends SESSION_SETUP request.", clientGuid.ToString());
            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            #endregion

            #region TREE_CONNECT to share
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client {0} sends TREE_CONNECT request.", clientGuid.ToString());
            status = client.TreeConnect(uncSharePath, out treeId);
            #endregion

            #region CREATE
            Smb2CreateContextResponse[] serverCreateContexts;
            CreateOptions_Values createOptions;

            if (isDirectory)
            {
                createOptions = CreateOptions_Values.FILE_DIRECTORY_FILE;
            }
            else
            {
                createOptions = CreateOptions_Values.FILE_NON_DIRECTORY_FILE;
            }

            // Include FILE_DELETE_ON_CLOSE if accessMask has DELETE
            if ((accessMask & AccessMask.DELETE) == AccessMask.DELETE)
            {
                createOptions = createOptions | CreateOptions_Values.FILE_DELETE_ON_CLOSE;
            }

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client {0} sends CREATE request with the lease state in SMB2_CREATE_REQUEST_LEASE_V2 set to {1}.", clientGuid.ToString(), requestLeaseState);
            status = client.Create(
                treeId,
                targetName,
                createOptions,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]
                {
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = clientGuid,
                        LeaseState = requestLeaseState
                    }
                },
                accessMask: accessMask,
                shareAccess: shareAccess,
                checker: (header, response) => { });

            return status;
            #endregion
        }

        /// <summary>
        /// Callback invoked by timer to check if LeaseBreakNotification received
        /// </summary>
        /// <param name="obj">Dictionary stores clientGuid and treeId KeyValuePairs</param>
        protected override void CheckBreakNotification(object obj)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Check if client received lease break notification");
            foreach (var manualEvent in notificationsReceived.Values)
            {
                BaseTestSite.Assert.AreEqual(
                    true,
                    // Wait for notification arrival
                    manualEvent.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                    "LeaseBreakNotification should be raised.");
            }
            BaseTestSite.Assert.AreEqual(
                notificationsReceived.Count,
                breakNotifications.Count,
                "Number of received lease break notifications should be {0})", notificationsReceived.Count);

            Dictionary<Guid, uint> treeIds = (Dictionary<Guid, uint>)obj;
            foreach (KeyValuePair<Guid, uint> pair in treeIds)
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.Comment,
                    "Client with clientGuid {0} attempts to acknowledge the lease break", pair.Key);
                base.AcknowledgeLeaseBreak(testClients[pair.Key], pair.Value, breakNotifications[pair.Key]);
            }
        }

        /// <summary>
        /// Handler when receive a LeaseBreakNotification
        /// </summary>
        /// <param name="respHeader">Packet header of LeaseBreakNotification</param>
        /// <param name="leaseBreakNotify">Received LeaseBreakNotification</param>
        protected override void OnLeaseBreakNotificationReceived(Packet_Header respHeader, LEASE_BREAK_Notification_Packet leaseBreakNotify)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "LeaseBreakNotification with LeaseKey \"{0}\" was received from server", leaseBreakNotify.LeaseKey.ToString());
            lock (breakNotifications)
            {
                breakNotifications.Add(leaseBreakNotify.LeaseKey, leaseBreakNotify);
            }

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
                expectedNewLeaseStates[leaseBreakNotify.LeaseKey],
                leaseBreakNotify.NewLeaseState,
                "NewLeaseState in LeaseBreakNotification from server should be {0}", expectedNewLeaseStates[leaseBreakNotify.LeaseKey]);
            // NewEpoch should be 2 based on assumption that no lease state change before this break since server initially granted
            BaseTestSite.Assert.AreEqual(
                2,
                leaseBreakNotify.NewEpoch,
                "NewEpoch in LeaseBreakNotification from server should be 2");
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

            notificationsReceived[leaseBreakNotify.LeaseKey].Set();
        }

        /// <summary>
        /// Tear down the client by Close file, Disconnect treeconnect and Logoff session
        /// </summary>
        /// <param name="client">Client to be torn down</param>
        /// <param name="treeId">Tree id to be disconnected</param>
        /// <param name="fileId">File id to be closed</param>
        private void ClientTearDown(Smb2FunctionalClient client, uint treeId, FILEID fileId)
        {
            status = client.Close(treeId, fileId);

            status = client.TreeDisconnect(treeId);

            status = client.LogOff();
        }

        /// <summary>
        /// Verify if server grant lease state as expected
        /// </summary>
        /// <param name="requestedLeaseState">Requested lease state from client</param>
        /// <param name="expectedGrantedLeaseState">Expected lease state that server granted</param>
        private void VerifyGrantedLeaseState(LeaseStateValues requestedLeaseState, LeaseStateValues expectedGrantedLeaseState)
        {
            Guid clientGuid = Guid.NewGuid();

            #region Connect to share
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to share {0}.", uncSharePath);

            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            #region Negotiate
            Capabilities_Values clientCapabilities =
                Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU
                | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                clientCapabilities,
                clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "NEGOTIATE should succeed.");

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                    TestConfig.CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING, response);
                });

            #endregion

            #region SessionSetup
            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "SessionSetup should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            #endregion

            #region TreeConnect
            uint treeId;
            status = client.TreeConnect(uncSharePath, out treeId);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "TreeConnect should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            #endregion
            #endregion

            #region CREATE open to directory with lease
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "CREATE open to directory with lease.");
            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fileId;
            status = client.Create(
                treeId,
                GetTestDirectoryName(uncSharePath),
                CreateOptions_Values.FILE_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]
                {
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = clientGuid,
                        LeaseState = requestedLeaseState
                    }
                },
                accessMask: AccessMask.GENERIC_ALL,
                shareAccess: ShareAccess_Values.FILE_SHARE_READ,
                checker: (header, response) => { });


            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "Create an open on directory should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            #endregion

            #region Verify server granted lease state
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server granted lease state.");

            BaseTestSite.Assert.AreNotEqual(
                null,
                serverCreateContexts,
                "Server should return granted lease state");
            foreach (Smb2CreateContextResponse serverCreateContext in serverCreateContexts)
            {
                Smb2CreateResponseLeaseV2 createResponseLeaseV2 = serverCreateContext as Smb2CreateResponseLeaseV2;
                if (createResponseLeaseV2 != null)
                {
                    BaseTestSite.Assert.AreEqual(
                        expectedGrantedLeaseState,
                        createResponseLeaseV2.LeaseState,
                        "Server granted lease state {0} should be the same as {1}", createResponseLeaseV2.LeaseState, expectedGrantedLeaseState);
                    break;
                }
            }
            #endregion

            ClientTearDown(client, treeId, fileId);
        }

        #endregion
    }
}
