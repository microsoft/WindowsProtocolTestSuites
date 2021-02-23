// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.Leasing
{
    [TestClass]
    public class CompareLeaseKeys : SMB2TestBase
    {
        #region Variables

        private Smb2FunctionalClient client1;
        private Smb2FunctionalClient client2;
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
            sharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
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

        #region Test Cases

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.LeaseV2)]
        [Description("Compare the server returned lease keys of two opens of the same file using the same lease key")]
        public void Comparing_Same_LeaseKeys()
        {
            CheckLeaseApplicability(FileType.DataFile);

            Smb2CreateRequestLeaseV2 leaseRequest = new Smb2CreateRequestLeaseV2
            {
                LeaseKey = Guid.NewGuid(),
                LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING,
            };
            
            Compare_LeaseKeys(leaseRequest, leaseRequest);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.LeaseV2)]
        [Description("Compare the returned lease keys of two opens of a parent directory file and a child file")]
        public void DirectoryComparing_ParentLeaseKey_ChildLeaseKey()
        {
            CheckLeaseApplicability(FileType.DirectoryFile);

            Guid leaseKey = Guid.NewGuid();
            Smb2CreateRequestLeaseV2 client1LeaseRequest = new Smb2CreateRequestLeaseV2
            {
                LeaseKey = leaseKey,
                LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING,
            };

            Smb2CreateRequestLeaseV2 client2LeaseRequest = new Smb2CreateRequestLeaseV2
            {
                LeaseKey = Guid.NewGuid(),
                LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING,
                LeaseFlags = (uint) LeaseFlagsValues.SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET,
                ParentLeaseKey = leaseKey
            };

            Compare_LeaseKeys(client1LeaseRequest, client2LeaseRequest, FileType.DirectoryFile);
        }

        #endregion

        #region Utility

        public void Compare_LeaseKeys(Smb2CreateRequestLeaseV2 client1LeaseRequest, Smb2CreateRequestLeaseV2 client2LeaseRequest, FileType client1FileType = FileType.DataFile)
        {
            string client1FileName;
            string client2FileName;
            GenerateFileNames(client1FileType, out client1FileName, out client2FileName);
            
            #region Add Event Handler

            client1.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(base.OnLeaseBreakNotificationReceived);
            clientToAckLeaseBreak = client1;
            
            #endregion

            #region Client1 (LeaseOpen) Open a File with Lease RWH or Directory with RH
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "Start the first client to create a file by sending the following requests: 1. NEGOTIATE; 2. SESSION_SETUP; 3. TREE_CONNECT; 4. CREATE (with LeaseV2 context)");
            uint client1TreeId;
            SetupClientConnection(client1, out client1TreeId);
            FILEID client1FileId;
            Smb2CreateContextResponse[] createContextResponse;
            client1.Create(client1TreeId, client1FileName, client1FileType == FileType.DataFile ? CreateOptions_Values.FILE_NON_DIRECTORY_FILE : CreateOptions_Values.FILE_DIRECTORY_FILE, 
                out client1FileId, out createContextResponse, RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]
                {
                    client1LeaseRequest
                }
                );
            
            #endregion

            // Get response lease of Client1 (LeaseOpen)
            Smb2CreateResponseLeaseV2 client1LeaseResponse = (Smb2CreateResponseLeaseV2) createContextResponse.First();
            

            // Create a task to invoke CheckBreakNotification to check whether server will send out lease break notification or not, no acknowledgement required
            var checkFirstBreakNotificationTask = Task.Run(() => CheckBreakNotification(client1TreeId, false));

            #region Start a second client (OperationOpen) to request lease by using the same lease key with the first client
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a second client to create the same file with the first client by sending the following requests: 1. NEGOTIATE; 2. SESSION_SETUP; 3. TREE_CONNECT; 4. CREATE");
            uint client2TreeId;
            SetupClientConnection(client2, out client2TreeId);
            FILEID client2FileId;
            client2.Create(client2TreeId, client2FileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out client2FileId, out createContextResponse, RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]
                {
                    client2LeaseRequest
                }
                );
            
            #endregion

            // Get response lease of Client2 (OperationOpen)
            Smb2CreateResponseLeaseV2 client2LeaseResponse = (Smb2CreateResponseLeaseV2) createContextResponse.First();

            #region Lease Keys Comparison
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Comparing the lease keys responses of client1 (LeaseOpen) and client2 (OperationOpen)");

            BaseTestSite.Assert.IsTrue(client1LeaseResponse.LeaseKey != Guid.Empty || client1LeaseResponse.ParentLeaseKey != Guid.Empty,
                    "Both LeaseOpen.LeaseKey and LeaseOpen.ParentLeaseKey can not be empty");

            BaseTestSite.Assert.IsTrue(client2LeaseResponse.LeaseKey != Guid.Empty || client2LeaseResponse.ParentLeaseKey != Guid.Empty,
                "Both OperationOpen.LeaseKey and OperationOpen.ParentLeaseKey can not be empty");

            BaseTestSite.Assert.IsTrue(client1LeaseResponse.LeaseKey != Guid.Empty,
                "LeaseOpen.LeaseKey can not be empty");

            if (client2LeaseRequest.LeaseFlags == (uint)LeaseFlagsValues.SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET)
            {
                BaseTestSite.Assert.IsTrue(client2LeaseResponse.ParentLeaseKey != Guid.Empty, "OperationOpen.ParentLeaseKey CANNOT be empty when SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET Flags is set");

                BaseTestSite.Assert.AreEqual(client1LeaseResponse.LeaseKey, client2LeaseResponse.ParentLeaseKey, "LeaseOpen.LeaseKey MUST be equal to OperationOpen.ParentLeasekey");
            } else
            {
                BaseTestSite.Assert.AreEqual(client1LeaseResponse.LeaseKey, client2LeaseResponse.LeaseKey,
                   "LeaseOpen.LeaseKey and OperationOpen.LeaseKey MUST be the same");
            }

            #endregion

            #region Tear Down Clients
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the first client by sending the following requests: 1. CLOSE; 2. TREE_DISCONNECT; 3. LOG_OFF");
            TearDownClient(client1, client1TreeId, client1FileId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the second client by sending the following requests: 1. CLOSE; 2. TREE_DISCONNECT; 3. LOG_OFF");
            TearDownClient(client2, client2TreeId, client2FileId);
            
            #endregion
        }

        private void SetupClientConnection(Smb2FunctionalClient client, out uint clientTreeId)
        {
            client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            client.TreeConnect(sharePath, out clientTreeId);
        }

        private void CheckLeaseApplicability(FileType fileType)
        {
            TestConfig.CheckDialect(DialectRevision.Smb30);
            if (fileType == FileType.DataFile)
            {
                TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            }
            else
            {
                TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING);
            }
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);
        }

        private void TearDownClient(Smb2FunctionalClient client, uint clientTreeId, FILEID clientFileId)
        {
            client.Close(clientTreeId, clientFileId);
            client.TreeDisconnect(clientTreeId);
            client.LogOff();
        }

        private void GenerateFileNames(FileType client1FileType, out string client1FileName, out string client2FileName)
        {
            if (client1FileType == FileType.DirectoryFile)
            {
                client1FileName = GetTestDirectoryName(sharePath);
                client2FileName = client1FileName + "\\" + Guid.NewGuid().ToString();
                AddTestFileName(sharePath, client2FileName);
            }
            else
            {
                client1FileName = GetTestFileName(sharePath);
                client2FileName = client1FileName;
            }
        }

        #endregion
    }
}
