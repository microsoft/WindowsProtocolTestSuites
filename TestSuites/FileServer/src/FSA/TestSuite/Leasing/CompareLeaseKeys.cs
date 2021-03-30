// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Smb2 = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite.Leasing
{
    public partial class LeasingTestCases : PtfTestClassBase
    {
        #region Variables

        private Smb2FunctionalClient client1;
        private Smb2FunctionalClient client2;
        private string sharePath;
        private uint client1TreeId;
        private Smb2.FILEID client1FileId;
        private uint client2TreeId;
        private Smb2.FILEID client2FileId;

        #endregion

        #region Test Case Initialize and Clean up

        protected override void TestInitialize()
        {
            initializeAdapter();
            testConfig = fsaAdapter.TestConfig;
            sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, fsaAdapter.ShareName);
            client1 = new Smb2FunctionalClient(testConfig.Timeout, testConfig, BaseTestSite);
            client2 = new Smb2FunctionalClient(testConfig.Timeout, testConfig, BaseTestSite);
            client1.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);
            client2.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);
        }

        protected override void TestCleanup()
        {
            #region Tear Down Clients

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the first client by sending the following requests: 1. CLOSE; 2. TREE_DISCONNECT; 3. LOG_OFF");
            TearDownClient(client1, client1TreeId, client1FileId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending the following requests: 1. CLOSE; 2. TREE_DISCONNECT; 3. LOG_OFF");
            TearDownClient(client2, client2TreeId, client2FileId);

            #endregion

            if (client1 != null)
            {
                client1.Disconnect();
            }
            if (client2 != null)
            {
                client2.Disconnect();
            }

            base.TestCleanup();

            fsaAdapter.Dispose();
            base.TestCleanup();
            CleanupTestManager();
        }

        #endregion

        #region Test Cases

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.LeaseV1)]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Positive)]
        [Description("Compare the server returned lease keys of two opens of the same file using the same lease key")]
        public void Comparing_Same_File_LeaseKeysV1()
        {
            CheckLeaseApplicability(DialectRevision.Smb21);

            Smb2CreateRequestLease leaseRequest = createLeaseRequestContext();

            Smb2CreateResponseLease client1ResponseLease;
            Smb2CreateResponseLease client2ResponseLease;

            InitializeClientsConnections(leaseRequest, leaseRequest, out client1ResponseLease, out client2ResponseLease);

            #region Test Cases

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Comparing lease keys");
            BaseTestSite.Assert.AreEqual(leaseRequest.LeaseKey, client1ResponseLease.LeaseKey, "Client 1 Request Lease Key MUST be the same its Response Lease key");
            BaseTestSite.Assert.AreEqual(leaseRequest.LeaseKey, client2ResponseLease.LeaseKey, "Client 2 Request Lease Key MUST be the same its Response Lease key");
            BaseTestSite.Assert.AreEqual(client1ResponseLease.LeaseKey, client2ResponseLease.LeaseKey, "LeaseOpen.leaseKey MUST be the same with OperationOpen.LeaseKey");

            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.LeaseV1)]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Positive)]
        [Description("Compare the server returned lease keys of two opens of the same directory file using the same lease key")]
        public void Comparing_Same_Directory_LeaseKeysV1()
        {
            CheckLeaseApplicability(DialectRevision.Smb21, checkFileTypeSupport: false, checkDirectoryTypeSupport: true);

            Smb2CreateRequestLease leaseRequest = createLeaseRequestContext(leaseState: LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING);

            Smb2CreateResponseLease client1ResponseLease;
            Smb2CreateResponseLease client2ResponseLease; 

            InitializeClientsConnections(leaseRequest, leaseRequest, out client1ResponseLease, out client2ResponseLease, isBothClientDirectory: true);

            #region Test Cases

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Comparing lease keys");
            BaseTestSite.Assert.AreEqual(leaseRequest.LeaseKey, client1ResponseLease.LeaseKey, "Client 1 Request Lease Key MUST be the same with its Response Lease key");
            BaseTestSite.Assert.AreEqual(leaseRequest.LeaseKey, client2ResponseLease.LeaseKey, "Client 2 Request Lease Key MUST be the same with its Response Lease key");
            BaseTestSite.Assert.AreEqual(client1ResponseLease.LeaseKey, client2ResponseLease.LeaseKey, "LeaseOpen.leaseKey MUST be the same with OperationOpen.LeaseKey");

            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.LeaseV2)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Positive)]
        [Description("Compare the server returned lease keys of two opens of the same file using the same lease key")]
        public void Comparing_Same_LeaseKeysV2()
        {
            CheckLeaseApplicability();

            Smb2CreateRequestLeaseV2 leaseRequest = createLeaseV2RequestContext();

            Smb2CreateResponseLeaseV2 client1ResponseLease;
            Smb2CreateResponseLeaseV2 client2ResponseLease;

            InitializeClientsConnections(leaseRequest, leaseRequest, out client1ResponseLease, out client2ResponseLease);

            #region Test Cases

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Comparing lease keys");
            BaseTestSite.Assert.AreEqual(leaseRequest.LeaseKey, client1ResponseLease.LeaseKey, "Client 1 Request Lease Key MUST be the same its Response Lease key");
            BaseTestSite.Assert.AreEqual(leaseRequest.LeaseKey, client2ResponseLease.LeaseKey, "Client 2 Request Lease Key MUST be the same its Response Lease key");
            BaseTestSite.Assert.AreEqual(client1ResponseLease.LeaseKey, client2ResponseLease.LeaseKey, "LeaseOpen.leaseKey MUST be the same with OperationOpen.LeaseKey");
            BaseTestSite.Assert.AreNotSame(client1ResponseLease.LeaseKey, Guid.Empty, "LeaseOpen.LeaseKey can not be empty");
            BaseTestSite.Assert.IsTrue(client1ResponseLease.LeaseKey != Guid.Empty || client1ResponseLease.ParentLeaseKey != Guid.Empty,
                    "Both LeaseOpen.LeaseKey and LeaseOpen.ParentLeaseKey can not be empty");

            BaseTestSite.Assert.IsTrue(client2ResponseLease.LeaseKey != Guid.Empty || client2ResponseLease.ParentLeaseKey != Guid.Empty,
                "Both OperationOpen.LeaseKey and OperationOpen.ParentLeaseKey can not be empty");

            BaseTestSite.Assert.IsTrue(client1ResponseLease.LeaseKey != Guid.Empty,
                "LeaseOpen.LeaseKey can not be empty");

            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.LeaseV2)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("The server should return an empty parentLeaseKey when leaseFlag is set to zero")]
        public void Compare_Zero_LeaseFlag_ParentLeaseKey()
        {
            CheckLeaseApplicability();

            Smb2CreateRequestLeaseV2 leaseRequest = createLeaseV2RequestContext(leaseFlag: LeaseFlagsValues.NONE);

            Smb2CreateResponseLeaseV2 client1ResponseLease;
            Smb2CreateResponseLeaseV2 client2ResponseLease;

            InitializeClientsConnections(leaseRequest, leaseRequest, out client1ResponseLease, out client2ResponseLease);

            #region Test Cases

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Comparing lease keys");
            BaseTestSite.Assert.AreNotEqual(leaseRequest.ParentLeaseKey, Guid.Empty,
                "LeaseRequest.ParentLeaseKey should not be empty");
            BaseTestSite.Assert.AreEqual(client1ResponseLease.ParentLeaseKey, Guid.Empty,
                "LeaseOpen.ParentLeaseKey MUST be empty if LeaseFlag is set to Zero");

            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.LeaseV2)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Positive)]
        [Description("Compare the returned lease keys of two opens to the same directory file")]
        public void DirectoryComparing_LeaseKeysV2()
        {
            CheckLeaseApplicability(checkDirectoryTypeSupport: true);

            Guid leaseKey = Guid.NewGuid();
            Smb2CreateRequestLeaseV2 client1RequestLease = createLeaseV2RequestContext(leaseKey, leaseState: LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING);

            Smb2CreateRequestLeaseV2 client2RequestLease = createLeaseV2RequestContext(parentLeaseKey: leaseKey);

            Smb2CreateResponseLeaseV2 client1ResponseLease;
            Smb2CreateResponseLeaseV2 client2ResponseLease;

            InitializeClientsConnections(client1RequestLease, client2RequestLease, out client1ResponseLease, out client2ResponseLease, isBothClientDirectory: true);

            #region Test Cases

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Comparing lease keys");
            BaseTestSite.Assert.IsTrue(client2ResponseLease.ParentLeaseKey != Guid.Empty, "OperationOpen.ParentLeaseKey CANNOT be empty when SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET Flags is set");
            BaseTestSite.Assert.AreEqual(client1ResponseLease.LeaseKey, client2ResponseLease.ParentLeaseKey, "LeaseOpen.LeaseKey MUST be equal to OperationOpen.ParentLeasekey");

            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.LeaseV2)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Positive)]
        [Description("Compare the returned lease keys of two opens of a parent directory file and a child file")]
        public void DirectoryComparing_ParentLeaseKey_ChildLeaseKey()
        {
            CheckLeaseApplicability(checkDirectoryTypeSupport: true);

            Guid leaseKey = Guid.NewGuid();
            Smb2CreateRequestLeaseV2 client1RequestLease = createLeaseV2RequestContext(leaseKey, leaseState: LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING);

            Smb2CreateRequestLeaseV2 client2RequestLease = createLeaseV2RequestContext(parentLeaseKey: leaseKey);

            Smb2CreateResponseLeaseV2 client1ResponseLease;
            Smb2CreateResponseLeaseV2 client2ResponseLease;

            InitializeClientsConnections(client1RequestLease, client2RequestLease, out client1ResponseLease, out client2ResponseLease, isClient1ParentDirectory: true);

            #region Test Cases

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Comparing lease keys");
            BaseTestSite.Assert.IsTrue(client2ResponseLease.ParentLeaseKey != Guid.Empty, "OperationOpen.ParentLeaseKey CANNOT be empty when SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET Flags is set");
            BaseTestSite.Assert.AreEqual(client1ResponseLease.LeaseKey, client2ResponseLease.ParentLeaseKey, "LeaseOpen.LeaseKey MUST be equal to OperationOpen.ParentLeasekey");

            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.LeaseV2)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Compare lease keys of parent directory file and a child file with an invalid parent lease key")]
        public void DirectoryComparing_Child_Invalid_ParentLeaseKeys()
        {
            CheckLeaseApplicability(checkDirectoryTypeSupport: true);

            Guid leaseKey = Guid.NewGuid();
            Smb2CreateRequestLeaseV2 client1RequestLease = createLeaseV2RequestContext(leaseKey, leaseState: LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING);

            Smb2CreateRequestLeaseV2 client2RequestLease = createLeaseV2RequestContext();
            client2RequestLease.ParentLeaseKey = new Guid(); // Invalid Key

            Smb2CreateResponseLeaseV2 client1ResponseLease;
            Smb2CreateResponseLeaseV2 client2ResponseLease;

            InitializeClientsConnections(client1RequestLease, client2RequestLease, out client1ResponseLease, out client2ResponseLease, isClient1ParentDirectory: true, expectServerBreakNotification: true);

            #region Test Cases

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Comparing lease keys");
            BaseTestSite.Assert.AreEqual(client2RequestLease.ParentLeaseKey, Guid.Empty, "request.ParentLeaseKey should be empty");
            BaseTestSite.Assert.AreEqual(client2ResponseLease.ParentLeaseKey, Guid.Empty, "open.ParentLeaseKey should be empty");
 
            #endregion
        }

        #endregion

        #region Utility

        private void InitializeClientsConnections(Smb2CreateRequestLease client1RequestLease, Smb2CreateRequestLease client2RequestLease, out Smb2CreateResponseLease client1ResponseLease, 
            out Smb2CreateResponseLease client2ResponseLease, bool isBothClientDirectory = false, bool isClient1ParentDirectory = false, bool expectServerBreakNotification = false)
        {
            Smb2CreateContextResponse client1ResponseContext;
            Smb2CreateContextResponse client2ResponseContext;

            SendRequestContext(client1RequestLease, client2RequestLease, out client1ResponseContext, out client2ResponseContext, isBothClientDirectory, isClient1ParentDirectory, expectServerBreakNotification);

            client1ResponseLease = client1ResponseContext as Smb2CreateResponseLease;
            client2ResponseLease = client2ResponseContext as Smb2CreateResponseLease;
        }

        private void InitializeClientsConnections(Smb2CreateRequestLeaseV2 client1RequestLease, Smb2CreateRequestLeaseV2 client2RequestLease, out Smb2CreateResponseLeaseV2 client1ResponseLease, 
            out Smb2CreateResponseLeaseV2 client2ResponseLease, bool isBothClientDirectory = false, bool isClient1ParentDirectory = false, bool expectServerBreakNotification = false)
        {
            Smb2CreateContextResponse client1ResponseContext;
            Smb2CreateContextResponse client2ResponseContext;

            SendRequestContext(client1RequestLease, client2RequestLease, out client1ResponseContext, out client2ResponseContext, isBothClientDirectory, isClient1ParentDirectory, expectServerBreakNotification);

            client1ResponseLease = client1ResponseContext as Smb2CreateResponseLeaseV2;
            client2ResponseLease = client2ResponseContext as Smb2CreateResponseLeaseV2;
        }

        public void SendRequestContext(Smb2CreateContextRequest client1RequestLease, Smb2CreateContextRequest client2RequestLease, out Smb2CreateContextResponse client1ResponseLease, 
            out Smb2CreateContextResponse client2ResponseLease, bool isBothClientDirectory = false, bool isClient1ParentDirectory = false, bool expectServerBreakNotification = false)
        {
            string client1FileName;
            string client2FileName;
            GenerateFileNames(isBothClientDirectory, isClient1ParentDirectory, out client1FileName, out client2FileName);

            #region Add Event Handler

            client1.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(OnLeaseBreakNotificationReceived);
            clientToAckLeaseBreak = client1;

            #endregion

            #region Client1 (LeaseOpen) Open a File with Lease RWH or Directory with RH

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "Start the first client to create a file by sending the following requests: 1. NEGOTIATE; 2. SESSION_SETUP; 3. TREE_CONNECT; 4. CREATE (with LeaseV2 context)");
            SetupClientConnection(client1, out client1TreeId);
            Smb2CreateContextResponse[] createContextResponse;
            client1.Create(client1TreeId, client1FileName, isBothClientDirectory || isClient1ParentDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                 out client1FileId, out createContextResponse, RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                 new Smb2CreateContextRequest[]
                 {
                    client1RequestLease
                 },
                 checker: (Packet_Header header, CREATE_Response response) =>
                 {
                     if (response.OplockLevel != OplockLevel_Values.OPLOCK_LEVEL_LEASE)
                     {
                         BaseTestSite.Assume.Inconclusive("Server OPLOCK Level is: {0}, expected: {1} indicating support for leasing.", response.OplockLevel, OplockLevel_Values.OPLOCK_LEVEL_LEASE);
                     }
                 }
                 );

            #endregion

            // Get response lease for Client1 (LeaseOpen)
            client1ResponseLease = (createContextResponse != null) ? createContextResponse.First() : new Smb2CreateResponseLease();


            #region Start a second client (OperationOpen) to request lease by using the same lease key with the first client
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a second client to create the same file with the first client by sending the following requests: 1. NEGOTIATE; 2. SESSION_SETUP; 3. TREE_CONNECT; 4. CREATE");
            SetupClientConnection(client2, out client2TreeId);
            client2.Create(client2TreeId, client2FileName, isBothClientDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out client2FileId, out createContextResponse, RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]
                {
                    client2RequestLease
                },
                checker: (Packet_Header header, CREATE_Response response) =>
                {
                    if (response.OplockLevel != OplockLevel_Values.OPLOCK_LEVEL_LEASE)
                    {
                        BaseTestSite.Assume.Inconclusive("Server OPLOCK Level is: {0}, expected: {1} indicating support for leasing.", response.OplockLevel, OplockLevel_Values.OPLOCK_LEVEL_LEASE);
                    }

                }
                );

            #endregion

            // Check whether server will send out lease break notification or not
            CheckBreakNotification(expectServerBreakNotification);

            // Get response lease for Client2 (OperationOpen)
            client2ResponseLease = createContextResponse.First();   
            
        }

        private Smb2CreateRequestLease createLeaseRequestContext(Guid leaseKey = new Guid(), LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING)
        {
            return new Smb2CreateRequestLease
            {
                LeaseKey = leaseKey == Guid.Empty ? Guid.NewGuid() : leaseKey,
                LeaseState = leaseState
            };
        }

        private Smb2CreateRequestLeaseV2 createLeaseV2RequestContext(Guid leaseKey = new Guid(), LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING,
            Guid parentLeaseKey = new Guid(), LeaseFlagsValues leaseFlag = LeaseFlagsValues.SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET)
        {
            return new Smb2CreateRequestLeaseV2
            {
                LeaseKey = leaseKey == Guid.Empty ? Guid.NewGuid() : leaseKey,
                LeaseState = leaseState,
                ParentLeaseKey = parentLeaseKey == Guid.Empty ? Guid.NewGuid() : parentLeaseKey,
                LeaseFlags = (uint) leaseFlag
            };
        }

        private void SetupClientConnection(Smb2FunctionalClient client, out uint clientTreeId)
        {
            client.Negotiate(testConfig.RequestDialects, testConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(testConfig.DefaultSecurityPackage, testConfig.SutComputerName, testConfig.AccountCredential, false);
            client.TreeConnect(sharePath, out clientTreeId);
        }

        private void CheckLeaseApplicability(DialectRevision dialect = DialectRevision.Smb30, bool checkFileTypeSupport = true, bool checkDirectoryTypeSupport = false)
        {
            testConfig.CheckDialect(dialect);
            if (checkFileTypeSupport)
            {
                testConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            }
            if (checkDirectoryTypeSupport)
            {
                testConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING);
            }
            if (dialect >= DialectRevision.Smb30)
            {
                testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);
            } else
            {
                testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            }
        }

        private void TearDownClient(Smb2FunctionalClient client, uint clientTreeId, FILEID clientFileId)
        {
            if (ClientTreeConnectSessionExists(clientTreeId))
            {
                client.Close(clientTreeId, clientFileId);
                client.TreeDisconnect(clientTreeId);
                client.LogOff();
            }
        }

        // Checks that there's an existing Tree_Connect session 
        private bool ClientTreeConnectSessionExists(uint clientTreeId)
        {
             return clientTreeId != 0;
        }

        private void GenerateFileNames(bool isBothClientDirectory, bool isClient1ParentDirectory, out string client1FileName, out string client2FileName)
        {
            client1FileName = "";
            client2FileName = "";

            if (isBothClientDirectory)
            {
                client1FileName = fsaAdapter.ComposeRandomFileName(10);
                client2FileName = client1FileName;
            }
            
            if (isClient1ParentDirectory)
            {
                client1FileName = fsaAdapter.ComposeRandomFileName(10);
                client2FileName = fsaAdapter.ComposeRandomFileName(10, opt: isBothClientDirectory ? CreateOptions.DIRECTORY_FILE : CreateOptions.NON_DIRECTORY_FILE, parentDirectoryName: client1FileName);
            }
            else if (!isBothClientDirectory)
            {
                client1FileName = fsaAdapter.ComposeRandomFileName(10, opt: CreateOptions.NON_DIRECTORY_FILE);
                client2FileName = client1FileName;
            }
        }

        #endregion
    }
}
