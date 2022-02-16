// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.Leasing
{
    [TestClass]
    public class FileLeasing : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client1;
        private Smb2FunctionalClient client2;
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
            notificationReceived = new AutoResetEvent(false);
            sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            fileName = GetTestFileName(sharePath);
            client1 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client2 = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client1.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            client2.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
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

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.LeaseV1)]
        [Description("This test case is designed to test whether server can handle LeaseV1 context correctly on a file.")]
        public void BVT_Leasing_FileLeasingV1()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb21);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            #endregion

            Smb2CreateContextRequest leaseContext = new Smb2CreateRequestLease
            {
                LeaseKey = Guid.NewGuid(),
                LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING
            };

            TestFileLeasing(leaseContext);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.LeaseV2)]
        [Description("This test case is designed to test whether server can handle LeaseV2 context correctly on a file.")]
        public void BVT_Leasing_FileLeasingV2()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);
            #endregion

            Smb2CreateContextRequest leaseContext = new Smb2CreateRequestLeaseV2
            {
                LeaseKey = Guid.NewGuid(),
                LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING
            };

            TestFileLeasing(leaseContext);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.LeaseV1)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("This test case is designed to test whether server can handle LeaseV1 context with the same lease key")]
        public void Leasing_FileLeasingV1_SameLeaseKey()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb21);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            #endregion

            Smb2CreateContextRequest leaseContext = new Smb2CreateRequestLease
            {
                LeaseKey = Guid.NewGuid(),
                LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING
            };

            TestFileLeasingKey(leaseContext);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.LeaseV2)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("This test case is designed to test whether server can handle LeaseV2 context with the same lease key")]
        public void Leasing_FileLeasingV2_SameLeaseKey()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);
            #endregion

            Smb2CreateContextRequest leaseContext = new Smb2CreateRequestLeaseV2
            {
                LeaseKey = Guid.NewGuid(),
                LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING
            };

            TestFileLeasingKey(leaseContext);
        }

        #region Common Methods
        public void TestFileLeasing(Smb2CreateContextRequest leaseContext)
        {
            #region Add Event Handler
            client1.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);
            clientToAckLeaseBreak = client1;
            #endregion

            #region Client1 Open a File with Lease RWH
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "Start the first client to create a file by sending the following requests: 1. NEGOTIATE; 2. SESSION_SETUP; 3. TREE_CONNECT; 4. CREATE (with context: {0})",
                leaseContext is Smb2CreateRequestLease ? "LeaseV1" : "LeaseV2");
            client1.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client1.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            uint client1TreeId;
            client1.TreeConnect(sharePath, out client1TreeId);
            FILEID client1FileId;
            Smb2CreateContextResponse[] createContextResponse;
            client1.Create(client1TreeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out client1FileId, out createContextResponse, RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]
                {
                    leaseContext
                }
                );
            #endregion

            #region Check Response Contexts

            if (leaseContext is Smb2CreateRequestLease)
            {
                Smb2CreateRequestLease leaseRequest = leaseContext as Smb2CreateRequestLease;
                CheckCreateContextResponses(createContextResponse, new DefaultLeaseResponseChecker(BaseTestSite, leaseRequest.LeaseKey, leaseRequest.LeaseState, LeaseFlagsValues.NONE));
            }
            else if (leaseContext is Smb2CreateRequestLeaseV2)
            {
                Smb2CreateRequestLeaseV2 leaseRequest = leaseContext as Smb2CreateRequestLeaseV2;
                CheckCreateContextResponses(createContextResponse, new DefaultLeaseV2ResponseChecker(BaseTestSite, leaseRequest.LeaseKey, leaseRequest.LeaseState, LeaseFlagsValues.NONE));
            }
            #endregion

            // Create a task to invoke CheckBreakNotification
            var checkFirstBreakNotificationTask = Task.Run(() => CheckBreakNotification(client1TreeId));

            #region Lease Break RWH => RH
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a second client to create the same file with the first client by sending the following requests: 1. NEGOTIATE; 2. SESSION_SETUP; 3. TREE_CONNECT; 4. CREATE");
            expectedNewLeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;
            client2.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client2.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            uint client2TreeId;
            client2.TreeConnect(sharePath, out client2TreeId);
            FILEID client2FileId;
            client2.Create(client2TreeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out client2FileId, out createContextResponse);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends LEASE_BREAK_ACKNOWLEDGEMENT request to break lease state to RH after receiving LEASE_BREAK_NOTIFICATION response from server.");
            checkFirstBreakNotificationTask.Wait(TestConfig.WaitTimeoutInMilliseconds);
            #endregion

            #region Lease BreakRH => NONE           
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The second client sends WRITE request.");
            expectedNewLeaseState = LeaseStateValues.SMB2_LEASE_NONE;
            byte[] data = { 0 };
            client2.Write(client2TreeId, client2FileId, data);
            var checkSecondBreakNotificationTask = Task.Run(() => CheckBreakNotification(client1TreeId));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends LEASE_BREAK_ACKNOWLEDGEMENT request to break lease state to NONE after receiving LEASE_BREAK_NOTIFICATION response from server.");
            checkSecondBreakNotificationTask.Wait(TestConfig.WaitTimeoutInMilliseconds);
            #endregion

            #region Tear Down Clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the first client by sending the following requests: 1. CLOSE; 2. TREE_DISCONNECT; 3. LOG_OFF");
            client1.Close(client1TreeId, client1FileId);
            client1.TreeDisconnect(client1TreeId);
            client1.LogOff();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending the following requests: 1. CLOSE; 2. TREE_DISCONNECT; 3. LOG_OFF");
            client2.Close(client2TreeId, client2FileId);
            client2.TreeDisconnect(client2TreeId);
            client2.LogOff();
            #endregion           
        }

        public void TestFileLeasingKey(Smb2CreateContextRequest leaseContext)
        {
            #region Add Event Handler
            client1.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);
            clientToAckLeaseBreak = client1;
            #endregion

            #region Client1 Open a File with Lease RWH
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "Start the first client to create a file by sending the following requests: 1. NEGOTIATE; 2. SESSION_SETUP; 3. TREE_CONNECT; 4. CREATE (with context: {0})",
                leaseContext is Smb2CreateRequestLease ? "LeaseV1" : "LeaseV2");
            client1.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client1.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            uint client1TreeId;
            client1.TreeConnect(sharePath, out client1TreeId);
            FILEID client1FileId;
            Smb2CreateContextResponse[] createContextResponse;
            client1.Create(client1TreeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out client1FileId, out createContextResponse, RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]
                {
                    leaseContext
                }
                );
            #endregion

            #region Check Response Contexts of Client1

            if (leaseContext is Smb2CreateRequestLease)
            {
                Smb2CreateRequestLease leaseRequest = leaseContext as Smb2CreateRequestLease;
                CheckCreateContextResponses(createContextResponse, new DefaultLeaseResponseChecker(BaseTestSite, leaseRequest.LeaseKey, leaseRequest.LeaseState, LeaseFlagsValues.NONE));
            }
            else if (leaseContext is Smb2CreateRequestLeaseV2)
            {
                Smb2CreateRequestLeaseV2 leaseRequest = leaseContext as Smb2CreateRequestLeaseV2;
                CheckCreateContextResponses(createContextResponse, new DefaultLeaseV2ResponseChecker(BaseTestSite, leaseRequest.LeaseKey, leaseRequest.LeaseState, LeaseFlagsValues.NONE));
            }
            #endregion

            // Create a task to invoke CheckBreakNotification to check whether server will send out lease break notification or not, no acknowledgement required
            var checkFirstBreakNotificationTask = Task.Run(() => CheckBreakNotification(client1TreeId, false));

            #region Start a second client to request lease by using the same lease key with the first client
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a second client to create the same file with the first client by sending the following requests: 1. NEGOTIATE; 2. SESSION_SETUP; 3. TREE_CONNECT; 4. CREATE");
            client2.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client2.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            uint client2TreeId;
            client2.TreeConnect(sharePath, out client2TreeId);
            FILEID client2FileId;
            client2.Create(client2TreeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out client2FileId, out createContextResponse, RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]
                {
                    leaseContext
                }
                );
            #endregion

            #region Check Response Contexts of Client2
            if (leaseContext is Smb2CreateRequestLease)
            {
                Smb2CreateRequestLease leaseRequest = leaseContext as Smb2CreateRequestLease;
                CheckCreateContextResponses(createContextResponse, new DefaultLeaseResponseChecker(BaseTestSite, leaseRequest.LeaseKey, leaseRequest.LeaseState, LeaseFlagsValues.NONE));
            }
            else if (leaseContext is Smb2CreateRequestLeaseV2)
            {
                Smb2CreateRequestLeaseV2 leaseRequest = leaseContext as Smb2CreateRequestLeaseV2;
                CheckCreateContextResponses(createContextResponse, new DefaultLeaseV2ResponseChecker(BaseTestSite, leaseRequest.LeaseKey, leaseRequest.LeaseState, LeaseFlagsValues.NONE));
            }
            #endregion

            #region The first client send write request          
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client sends WRITE request.");
            byte[] data = { 0 };
            client1.Write(client1TreeId, client1FileId, data);
            #endregion

            #region The second client send write request          
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The second client sends WRITE request.");
            client2.Write(client2TreeId, client2FileId, data);
            #endregion

            #region Check whether the server will send lease break notification
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The first client will not receive lease break notification since the second client is using the same lease key");
            checkFirstBreakNotificationTask.Wait(TestConfig.WaitTimeoutInMilliseconds);
            #endregion

            #region Tear Down Clients
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the first client by sending the following requests: 1. CLOSE; 2. TREE_DISCONNECT; 3. LOG_OFF");
            client1.Close(client1TreeId, client1FileId);
            client1.TreeDisconnect(client1TreeId);
            client1.LogOff();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending the following requests: 1. CLOSE; 2. TREE_DISCONNECT; 3. LOG_OFF");
            client2.Close(client2TreeId, client2FileId);
            client2.TreeDisconnect(client2TreeId);
            client2.LogOff();
            #endregion           
        }
        #endregion
    }
}
