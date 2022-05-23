// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.DurableHandle
{
    [TestClass]
    public class DurableHandle : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient clientBeforeDisconnection;
        private Smb2FunctionalClient clientAfterDisconnection;
        private Guid clientGuid;
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

            clientBeforeDisconnection = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            clientAfterDisconnection = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            clientGuid = Guid.NewGuid();
        }

        protected override void TestCleanup()
        {
            if (clientBeforeDisconnection != null)
            {
                try
                {
                    clientBeforeDisconnection.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect clientBeforeFailover: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test reconnect with DurableHandleV1 after server has been disconnected.")]
        public void DurableHandleV1_Reconnect_AfterServerDisconnect()
        {
            /// 1. Client requests a durable handle with LeaseV1 context
            /// 2. Client sends Negotiate request after the Create operation to trigger server terminates the connection
            /// 3. Client reconnects the durable handle with LeaseV1 context, and expects success.

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb21);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);
            
            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb21, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdBeforeDisconnection, null);

            Guid leaseKey = Guid.NewGuid();
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequest
                    {
                         DurableRequest = Guid.Empty,
                    },
                    new Smb2CreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState,
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    CheckCreateContextResponses(serverCreateContexts, new DefaultDurableHandleResponseChecker(BaseTestSite));
                });

            clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);
            #endregion

            /// Send negotiate after create to make server disconnect the connection
            TriggerServerTerminateConnection();

            #region client reconnect to server
            BaseTestSite.Log.Add(
            LogEntryKind.Comment,
            "Client opens the same file and reconnects the durable handle");

            uint treeIdAfterDisconnection;
            Connect(DialectRevision.Smb21, clientAfterDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdAfterDisconnection, clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;
            clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleReconnect
                    {
                        Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                    },
                    new Smb2CreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState,
                    }
                },
                shareAccess: ShareAccess_Values.NONE);

            string readContent;
            clientAfterDisconnection.Read(treeIdAfterDisconnection, fileIdAfterDisconnection, 0, (uint)content.Length, out readContent);

            BaseTestSite.Assert.IsTrue(
                content.Equals(readContent),
                "The written content is expected to be equal to read content.");

            clientAfterDisconnection.Close(treeIdAfterDisconnection, fileIdAfterDisconnection);
            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.DurableHandleV1BatchOplock)]
        [TestCategory(TestCategories.InvalidIdentifier)]
        [Description("Test reconnect with DurableHandleV1, without log off or disconnect and without previous session id.")]
        public void DurableHandleV1_Reconnect_WithoutLogoffOrDisconnect_WithoutPreviousSessionIdSet()
        {
            DurableHandleV1_Reconnect_WithoutLogoff(false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.DurableHandleV1BatchOplock)]
        [Description("Test reconnect with DurableHandleV1 and BatchOplock.")]
        public void BVT_DurableHandleV1_Reconnect_WithBatchOplock()
        {
            /// 1. Client requests a durable handle with BatchOplock
            /// 2. Client sends Disconnect to lose connection
            /// 3. Client reconnects the durable handle with BatchOplock, and expects success.
            
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb2002);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT);
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb21, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdBeforeDisconnection, null);

            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The client with clientGuid {0} sends CREATE request, with OPLOCK_LEVEL_BATCH and SMB2_CREATE_DURABLE_HANDLE_REQUEST.", clientGuid);
            clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequest
                    {
                         DurableRequest = Guid.Empty,
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    CheckCreateContextResponses(serverCreateContexts, new DefaultDurableHandleResponseChecker(BaseTestSite));
                });

            clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "The client with clientGuid {0} connects to server and opens file with a durable handle.", clientGuid.ToString());
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "The client with clientGuid {0} sends DISCONECT request.", clientGuid.ToString());
            clientBeforeDisconnection.Disconnect();

            #region client reconnect to server
            BaseTestSite.Log.Add(
            LogEntryKind.Comment,
            "The client with clientGuid {0} opens the same file and reconnects the durable handle.", clientGuid);

            uint treeIdAfterDisconnection;
            Connect(DialectRevision.Smb21, clientAfterDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdAfterDisconnection, clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The client with clientGuid {0} sends CREATE request, with OPLOCK_LEVEL_BATCH and SMB2_CREATE_DURABLE_HANDLE_RECONNECT.", clientGuid);
            clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleReconnect
                    {
                        Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                    }
                },
                shareAccess: ShareAccess_Values.NONE);

            string readContent;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The client with clientGuid {0} sends READ request.", clientGuid.ToString());
            clientAfterDisconnection.Read(treeIdAfterDisconnection, fileIdAfterDisconnection, 0, (uint)content.Length, out readContent);

            BaseTestSite.Assert.IsTrue(
                content.Equals(readContent),
                "The written content is expected to be equal to read content.");
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client opens the same file and reconnects the durable handle");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client with clientGuid {0} by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF; DISCONNECT.", clientGuid.ToString());
            clientAfterDisconnection.Close(treeIdAfterDisconnection, fileIdAfterDisconnection);
            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("Test no durable handle is granted if requesting with DurableHandleV1 and LeaseV1 context, but SMB2_LEASE_HANDLE_CACHING bit is not set in LeaseState.")]
        public void DurableHandleV1_WithLeaseV1_WithoutHandleCaching()
        {
            /// 1. Client requests a durable handle with LeaseV1 context, SMB2_LEASE_HANDLE_CACHING bit is not set in LeaseState.
            /// 2. Durable Handle is not granted.
            
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb21);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle, SMB2_LEASE_HANDLE_CACHING bit is not set in LeaseState.");

            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb21, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdBeforeDisconnection, null);
            Guid leaseKey = Guid.NewGuid();

            // SMB2_LEASE_HANDLE_CACHING bit is not set in LeaseState
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequest
                    {
                         DurableRequest = Guid.Empty,
                    },
                    new Smb2CreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState,
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    // Durable Handle should not be granted.
                    CheckCreateContextResponsesNotExist(serverCreateContexts, new DefaultDurableHandleResponseChecker(BaseTestSite));
                });

            clientBeforeDisconnection.TreeDisconnect(treeIdBeforeDisconnection);
            clientBeforeDisconnection.LogOff();
            clientBeforeDisconnection.Disconnect();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context.")]
        public void BVT_DurableHandleV1_Reconnect_WithLeaseV1()
        {
            /// 1. Client requests a durable handle with LeaseV1 context
            /// 2. Client sends Disconnect to close connection
            /// 3. Client reconnects the durable handle with LeaseV1 context, and expects success.
            DurableHandleV1_Reconnect_WithLeaseV1();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context and include create durable request.")]
        public void DurableHandleV1_Reconnect_IncludeCreateDurableHandle()
        {
            //Client requests a durable handle with LeaseV1.
            //Client sends Disconnect to close connection.
            //Client sends reconnect request with LeaseV1 and include CreateDurableHandle and expects STATUS_SUCCESS.
            DurableHandleV1_Reconnect_WithLeaseV1(includeDurableHandleRequest: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context, but the file name is different.")]
        public void DurableHandleV1_Reconnect_WithLeaseV1_WithDifferentFileName()
        {
            /// 1. Client requests a durable handle with LeaseV1 context
            /// 2. Client sends Disconnect to close connection
            /// 3. Client reconnects the durable handle with LeaseV1 context but different file name, and expects STATUS_INVALID_PARAMETER.
            DurableHandleV1_Reconnect_InvalidParameter();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context and include SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2.")]
        public void DurableHandleV1_Reconnect_IncludeRequestV2()
        {
            //Client requests a durable handle with LeaseV1.
            //Client sends Disconnect to close connection.
            //Client sends reconnect request with LeaseV1 and include Smb2CreateDurableHandleRequestV2 then expects STATUS_INVALID_PARAMETER.
            DurableHandleV1_Reconnect_InvalidParameter(includeRequestV2: true);

        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context and include SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2")]
        public void DurableHandleV1_Reconnect_IncludeReconnectV2()
        {
            //Client requests a durable handle with LeaseV1.
            //Client sends Disconnect to close connection.
            //Client sends reconnect request with LeaseV1 and include Smb2CreateDurableHandleReconnectV2 and expects STATUS_INVALID_PARAMETER.
            DurableHandleV1_Reconnect_InvalidParameter(includeReconnectV2: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context without FileId.Persistent")]
        public void DurableHandleV1_Reconnect_WithoutFileIdPersistent()
        {
            //Client requests a durable handle with LeaseV1.
            //Client sends Disconnect to close connection.
            //Client sends reconnect request with LeaseV1 and without FileId.Persistent and expects STATUS_OBJECT_NAME_NOT_FOUND.
            DurableHandleV1_Reconnect_ObjectNameNotFound();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context but with a different LeaseKey")]
        public void DurableHandleV1_Reconnect_WithDifferentLeaseKey()
        {
            //Client requests a durable handle with LeaseV1.
            //Client sends Disconnect to close connection.
            //Client sends reconnect request with LeaseV1 but different lease key and expects STATUS_OBJECT_NAME_NOT_FOUND.
            DurableHandleV1_Reconnect_ObjectNameNotFound(differentLeaseKey: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context when request Lease is Null")]
        public void DurableHandleV1_Reconnect_LeaseIsNull()
        {
            //Client requests a durable handle with no LeaseV1.
            //Client sends Disconnect to close connection.
            //Client sends reconnect request with LeaseV1 and expects STATUS_OBJECT_NAME_NOT_FOUND.
            DurableHandleV1_Reconnect_ObjectNameNotFound(leaseIsNull: true);

        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context when Open.Session is not NULL")]
        public void DurableHandleV1_Reconnect_SessionIsNotNull()
        {
            //Client requests a durable handle with LeaseV1.
            //Client does not disconnect
            //Client sends reconnect request with LeaseV1 and expects STATUS_OBJECT_NAME_NOT_FOUND.
            DurableHandleV1_Reconnect_ObjectNameNotFound(sessionIsNull: false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context without Smb2CreateRequestLease")]
        public void DurableHandleV1_Reconnect_WithoutRequestLease()
        {
            //Client requests a durable handle with LeaseV1.
            //Client sends Disconnect to close connection.
            //Client sends reconnect request without SMB2_CREATE_REQUEST_LEASE and expects STATUS_OBJECT_NAME_NOT_FOUND.
            DurableHandleV1_Reconnect_ObjectNameNotFound(requestLease: false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.InvalidIdentifier)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context and different client guid.")]
        public void DurableHandleV1_Reconnect_WithDifferentClientGuid()
        {
            //Client requests a durable handle with LeaseV1.
            //Client sends Disconnect to close connection.
            //Client sends reconnect request with LeaseV1 but different client GUID then expects STATUS_OBJECT_NAME_NOT_FOUND.
            DurableHandleV1_Reconnect_ObjectNameNotFound(differentClientGuid: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with LeaseV1 context without DurableHandleV1.")]
        public void DurableHandleV1_Reconnect_WithoutDurableHandle()
        {
            //Client requests with LeaseV1 without durable handle.
            //Client sends Disconnect to close connection.
            //Client sends reconnect request with LeaseV1 and expects STATUS_OBJECT_NAME_NOT_FOUND.
            DurableHandleV1_Reconnect_ObjectNameNotFound(includeDurableHandle: false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.FileAccessCheck)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context and different client credential account.")]
        public void DurableHandleV1_Reconnect_WithDifferentDurableOwner()
        {
            //Client requests with LeaseV1 with durable handle.
            //Client sends Disconnect to close connection.
            //Client sends reconnect request with LeaseV1 context but a different durable owner, and expects STATUS_ACCESS_DENIED.
            DurableHandleV1_Reconnect_AccessDenied();
        }

        #endregion

        #region Common Methods

        private void Check_DurableHandle_Applicability()
        {
            TestConfig.CheckDialect(DialectRevision.Smb21);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST,
                CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
        }

        private void Client_Connect_Server_WithDurableHandleV1(string content, string fileName, Guid leaseKey, LeaseStateValues leaseState,
            out uint treeIdBeforeDisconnection, out FILEID fileIdBeforeDisconnection, out Smb2CreateContextResponse[] serverCreateContexts, 
            bool requestWithoutDurableHande = false, bool requestWithoutLease = false, bool contextResponseExist = true)
        {
            Smb2CreateContextResponse[] serverCreateContextResponse = null;
            Smb2CreateContextRequest[] smb2CreateContextRequests;

            if (requestWithoutDurableHande)
            {
                smb2CreateContextRequests = new Smb2CreateContextRequest[] {
                    new Smb2CreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState,
                    }
                };
            }
            else if(requestWithoutLease)
            {
                smb2CreateContextRequests = new Smb2CreateContextRequest[] {
                    new Smb2CreateDurableHandleRequest
                    {
                        DurableRequest = Guid.Empty,
                    }
                };
            }
            else
            {
                smb2CreateContextRequests = new Smb2CreateContextRequest[] {
                    new Smb2CreateDurableHandleRequest
                    {
                        DurableRequest = Guid.Empty,
                    },
                    new Smb2CreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState,
                    }
                };
            }

            Connect(DialectRevision.Smb21, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdBeforeDisconnection, null);

            clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContextResponse,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                smb2CreateContextRequests,
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    checkCreateContextResponse(serverCreateContextResponse, contextResponseExist);
                });

            serverCreateContexts = serverCreateContextResponse;
            clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);
        }

        private void checkCreateContextResponse(Smb2CreateContextResponse[] serverCreateContextResponse, bool contextResponseExist)
        {
            if(contextResponseExist)
            {
                CheckCreateContextResponses(serverCreateContextResponse, new DefaultDurableHandleResponseChecker(BaseTestSite));
            }
            else
            {
                CheckCreateContextResponsesNotExist(serverCreateContextResponse, new DefaultDurableHandleResponseChecker(BaseTestSite));
            }
        }

        private void DurableHandleV1_Reconnect_ObjectNameNotFound(bool includeDurableHandle = true, bool leaseIsNull = false, bool sessionIsNull = true, bool requestLease = true, bool differentClientGuid = false, bool differentLeaseKey = false)
        {
            #region Check Applicability
            Check_DurableHandle_Applicability();
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);
            string objectNameNotFoundComment = "";

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            uint treeIdBeforeDisconnection;
            Guid leaseKey = Guid.NewGuid();
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;

            if (leaseIsNull)
            {
                Client_Connect_Server_WithDurableHandleV1(content, fileName, leaseKey, leaseState, out treeIdBeforeDisconnection, out fileIdBeforeDisconnection, out serverCreateContexts, requestWithoutLease: true, contextResponseExist: false);
            }
            else if (includeDurableHandle == false)
            {
                Client_Connect_Server_WithDurableHandleV1(content, fileName, leaseKey, leaseState, out treeIdBeforeDisconnection, out fileIdBeforeDisconnection, out serverCreateContexts, requestWithoutDurableHande: true, contextResponseExist: false);
            }
            else
            {
                Client_Connect_Server_WithDurableHandleV1(content, fileName, leaseKey, leaseState, out treeIdBeforeDisconnection, out fileIdBeforeDisconnection, out serverCreateContexts);
            }
            #endregion

            if (sessionIsNull)
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.TestStep,
                    "The client with clientGuid {0} sends DISCONNECT request.", clientGuid.ToString());
                clientBeforeDisconnection.Disconnect();
            }
            else
            {
                clientBeforeDisconnection = null;
            }

            #region client reconnect to server
            BaseTestSite.Log.Add(
            LogEntryKind.Comment,
            "Client opens the same file and reconnects the durable handle");

            uint treeIdAfterDisconnection;

            if(differentClientGuid)
            {
                clientGuid = Guid.NewGuid();
            }

            if(differentLeaseKey)
            {
                leaseKey = Guid.NewGuid();
            }

            Connect(DialectRevision.Smb21, clientAfterDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdAfterDisconnection, clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;

            Smb2CreateContextRequest[] createContextRequests = new Smb2CreateContextRequest[] {
                new Smb2CreateDurableHandleReconnect
                {
                    Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                },
                new Smb2CreateRequestLease
                {
                    LeaseKey = leaseKey,
                    LeaseState = leaseState
                },
            };

            Smb2CreateContextRequest[] createContextWithNoFileIdPersistent = new Smb2CreateContextRequest[] {
                new Smb2CreateDurableHandleReconnect
                {
                    Data = new FILEID { }
                },
                new Smb2CreateRequestLease
                {
                    LeaseKey = leaseKey,
                    LeaseState = leaseState
                },
            };

            Smb2CreateContextRequest[] createContextWithoutRequestLease = new Smb2CreateContextRequest[] {
                new Smb2CreateDurableHandleReconnect
                {
                    Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                }

            };

            if (leaseIsNull)
            {
                objectNameNotFoundComment = "[MS-SMB2] Section 3.3.5.9.7 If any of the following conditions is TRUE, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND: " +
                    "Open.Lease is NULL and the SMB2_CREATE_REQUEST_LEASE_V2 or the SMB2_CREATE_REQUEST_LEASE create context is present";
            }
            else if (sessionIsNull == false)
            {
                objectNameNotFoundComment = "[MS-SMB2] Section 3.3.5.9.7 If any of the following conditions is TRUE, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND: " +
                    "Open.Session is not NULL.";
            }
            else if (requestLease == false)
            {
                createContextRequests = createContextWithoutRequestLease;
                objectNameNotFoundComment = "[MS-SMB2] Section 3.3.5.9.7  If Open.Lease is not NULL, the server supports leasing and if Lease.Version is 1 and the request " +
                    "does not contain the SMB2_CREATE_REQUEST_LEASE create context or if Lease.Version is 2 and " +
                    "the request does not contain the SMB2_CREATE_REQUEST_LEASE_V2 create context, the server " +
                    "SHOULD < 316 > fail the request with STATUS_OBJECT_NAME_NOT_FOUND.";
            }
            else if (differentClientGuid)
            {
                objectNameNotFoundComment = "[MS-SMB2] Section 3.3.5.9.7 if any Open.Lease is not NULL and Open.clientGuid is not equal " +
                    "to the ClientGuid of the connection that received this request, the server MUST fail this " +
                    "request with STATUS_OBJECT_NAME_NOT_FOUND.";
            }
            else if (differentLeaseKey)
            {
                objectNameNotFoundComment = "[MS-SMB2] Section 3.3.5.9.7 If any of the following conditions is TRUE, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND: " +
                    "The SMB2_CREATE_REQUEST_LEASE create context is also present in the request, Connection.Dialect is \"2.1\" " +
                    "or belongs to the SMB 3.x dialect family, the server supports leasing, Open.Lease is not NULL, " +
                    "and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE create context.";
            }
            else if (includeDurableHandle == false)
            {
                objectNameNotFoundComment = "[MS-SMB2] Section 3.3.5.9.7  If any of the following conditions is TRUE, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND: " +
                    "Open.IsDurable is FALSE and Open.IsResilient is FALSE or unimplemented.";
            }
            else
            {
                createContextRequests = createContextWithNoFileIdPersistent;
                objectNameNotFoundComment = "[MS-SMB2] Section 3.3.5.9.7 The server MUST look up an existing open in the GlobalOpenTable by doing a lookup with the " +
                    "FileId.Persistent portion of the create context.If the lookup fails, the server SHOULD < 315 > fail " +
                    "the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.";
            }

            uint status = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                createContextRequests,
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => { });

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND, status,
                objectNameNotFoundComment);
            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
            #endregion
        }

        private void DurableHandleV1_Reconnect_InvalidParameter(bool includeRequestV2 = false, bool includeReconnectV2 = false)
        {
            #region Check Applicability
            Check_DurableHandle_Applicability();
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);
            string invalidParameterComment = "";

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            uint treeIdBeforeDisconnection;
            Guid leaseKey = Guid.NewGuid();
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            Client_Connect_Server_WithDurableHandleV1(content, fileName, leaseKey, leaseState, out treeIdBeforeDisconnection, out fileIdBeforeDisconnection, out serverCreateContexts);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "The client with clientGuid {0} sends DISCONNECT request.", clientGuid.ToString());
            clientBeforeDisconnection.Disconnect();

            #region client reconnect to server
            BaseTestSite.Log.Add(
            LogEntryKind.Comment,
            "Client opens the same file and reconnects the durable handle");

            uint treeIdAfterDisconnection;

            Connect(DialectRevision.Smb21, clientAfterDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdAfterDisconnection, clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;

            Smb2CreateContextRequest[] createContextRequests = new Smb2CreateContextRequest[] {
                new Smb2CreateDurableHandleReconnect
                {
                    Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                },
                new Smb2CreateRequestLease
                {
                    LeaseKey = leaseKey,
                    LeaseState = leaseState
                },
            };

            Smb2CreateContextRequest[] createContextIncludeRequestV2 = new Smb2CreateContextRequest[] {
                new Smb2CreateDurableHandleReconnect
                {
                    Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                },
                new Smb2CreateRequestLease
                {
                    LeaseKey = leaseKey,
                    LeaseState = leaseState
                },
                new Smb2CreateDurableHandleRequestV2
                {
                        CreateGuid = Guid.Empty,
                },
            };

            Smb2CreateContextRequest[] createContextIncludeReconnectV2 = new Smb2CreateContextRequest[] {
                new Smb2CreateDurableHandleReconnect
                {
                    Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                },
                new Smb2CreateRequestLease
                {
                    LeaseKey = leaseKey,
                    LeaseState = leaseState
                },
                new Smb2CreateDurableHandleReconnectV2
                {
                        CreateGuid = Guid.Empty,
                        FileId = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                },
            };

            if (includeRequestV2)
            {
                createContextRequests = createContextIncludeRequestV2;
                invalidParameterComment = "[MS-SMB2] Section 3.3.5.9.7 If the create request also contains an SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 or " +
                                        "SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, the server SHOULD < 314 > fail" +
                                        "the request with STATUS_INVALID_PARAMETER.";
            }
            else if (includeReconnectV2)
            {
                createContextRequests = createContextIncludeReconnectV2;
                invalidParameterComment = "[MS-SMB2] Section 3.3.5.9.7 If the create request also contains an SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 or " +
                                        "SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, the server SHOULD < 314 > fail" +
                                        "the request with STATUS_INVALID_PARAMETER.";
            }
            else
            {
                fileName = "invalidFileName";
                invalidParameterComment = "[MS-SMB2] Section 3.3.5.9.7  If Open.Lease is not NULL, Open.Lease.FileDeleteOnClose is FALSE, and " +
                                        "Open.Lease.FileName does not match the file name specified in the Buffer field of the SMB2 " +
                                        "CREATE request, the server MUST fail the request with STATUS_INVALID_PARAMETER.";
            }

            uint status = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                createContextRequests,
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => { });

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_INVALID_PARAMETER, status,
                invalidParameterComment);
            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
            #endregion
        }

        private void DurableHandleV1_Reconnect_AccessDenied()
        {
            #region Check Applicability
            Check_DurableHandle_Applicability();
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            uint treeIdBeforeDisconnection;
            Guid leaseKey = Guid.NewGuid();
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            Client_Connect_Server_WithDurableHandleV1(content, fileName, leaseKey, leaseState, out treeIdBeforeDisconnection, out fileIdBeforeDisconnection, out serverCreateContexts);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "The client with clientGuid {0} sends DISCONNECT request.", clientGuid.ToString());
            clientBeforeDisconnection.Disconnect();

            #region client reconnect to server
            BaseTestSite.Log.Add(
            LogEntryKind.Comment,
            "Client opens the same file and reconnects the durable handle");

            uint treeIdAfterDisconnection;
            clientBeforeDisconnection = null;

            Connect(DialectRevision.Smb21, clientAfterDisconnection, clientGuid, testConfig.NonAdminAccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdAfterDisconnection, clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;

            Smb2CreateContextRequest[] createContextRequests = new Smb2CreateContextRequest[] {
                new Smb2CreateDurableHandleReconnect
                {
                    Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                },
                new Smb2CreateRequestLease
                {
                    LeaseKey = leaseKey,
                    LeaseState = leaseState
                },
            };

            string accessDeniedComment = "[MS-SMB2] Section 3.3.5.9.7 If the user represented by Session.SecurityContext is not the same user denoted by " +
                "Open.DurableOwner, the server MUST fail the request with STATUS_ACCESS_DENIED and " +
                "proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.";            

            uint status = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                createContextRequests,
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => { });

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_ACCESS_DENIED, status,
                accessDeniedComment);
            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
            #endregion
        }

        private void DurableHandleV1_Reconnect_WithLeaseV1(bool includeDurableHandleRequest = false)
        {
            #region Check Applicability
            Check_DurableHandle_Applicability();
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            uint treeIdBeforeDisconnection;
            Guid leaseKey = Guid.NewGuid();
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            Client_Connect_Server_WithDurableHandleV1(content, fileName, leaseKey, leaseState, out treeIdBeforeDisconnection, out fileIdBeforeDisconnection, out serverCreateContexts);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "The client with clientGuid {0} sends DISCONNECT request.", clientGuid.ToString());
            clientBeforeDisconnection.Disconnect();

            #region client reconnect to server
            BaseTestSite.Log.Add(
            LogEntryKind.Comment,
            "Client opens the same file and reconnects the durable handle");

            uint treeIdAfterDisconnection;
            Connect(DialectRevision.Smb21, clientAfterDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdAfterDisconnection, clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;

            Smb2CreateContextRequest[] createContextRequests;

            string successComment = "Reconnect a durable handle should be successful";

            if (includeDurableHandleRequest)
            {
                createContextRequests = new Smb2CreateContextRequest[] {
                    new Smb2CreateDurableHandleReconnect
                    {
                        Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                    },
                    new Smb2CreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState
                    },
                    new Smb2CreateDurableHandleRequest
                    {
                            DurableRequest = Guid.Empty,
                    },
                };

                successComment = "[MS-SMB2] Section 3.3.5.9.7  If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_REQUEST create" +
                "context, the server MUST ignore the SMB2_CREATE_DURABLE_HANDLE_REQUEST create context.";
            }
            else
            {
                createContextRequests = new Smb2CreateContextRequest[] {
                    new Smb2CreateDurableHandleReconnect
                    {
                        Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                    },
                    new Smb2CreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState,
                    }
                };
            }

            uint status = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                createContextRequests,
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => {});

            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, successComment);

            string readContent;
            clientAfterDisconnection.Read(treeIdAfterDisconnection, fileIdAfterDisconnection, 0, (uint)content.Length, out readContent);
            BaseTestSite.Assert.IsTrue(content.Equals(readContent),
                "The written content is expected to be equal to read content.");

            clientAfterDisconnection.Close(treeIdAfterDisconnection, fileIdAfterDisconnection);
            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
            #endregion
        }

        private void DurableHandleV1_Reconnect_WithoutLogoff(bool isPreviousSessionIdSet)
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb2002);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT);
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb21, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdBeforeDisconnection, null);

            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                new Smb2CreateContextRequest[] {
                    new Smb2CreateDurableHandleRequest
                    {
                        DurableRequest = Guid.Empty
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    CheckCreateContextResponses(serverCreateContexts, new DefaultDurableHandleResponseChecker(BaseTestSite));
                });

            clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);
            #endregion

            #region client reconnect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client opens the same file and reconnects the durable handle");

            uint treeIdAfterDisconnection;
            Connect(DialectRevision.Smb21, clientAfterDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert,
                out treeIdAfterDisconnection, isPreviousSessionIdSet ? clientBeforeDisconnection : null);

            FILEID fileIdAfterDisconnection;

            ResponseChecker<CREATE_Response> createResponseChecker = null;
            if (isPreviousSessionIdSet)
            {
                if (testConfig.Platform == Platform.WindowsServer2012R2)
                {
                    // 6   Appendix A: Product Behavior
                    // <265> Section 3.3.5.9.7: If the Session was established by specifying PreviousSessionId in the SMB2 SESSION_SETUP request, 
                    // therefore invalidating the previous session, Windows 8.1 and Windows Server 2012 R2 close the durable opens established on the previous session.
                    createResponseChecker = (header, response) =>
                    {
                        BaseTestSite.Assert.AreNotEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                           "{0} should not be successful if Server is Windows Server 2012R2 and the Session was established by specifying PreviousSessionId in the SMB2 SESSION_SETUP request. " +
                            "Actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));

                        BaseTestSite.CaptureRequirementIfAreEqual(
                            Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND,
                            header.Status,
                            RequirementCategory.STATUS_OBJECT_NAME_NOT_FOUND.Id,
                            RequirementCategory.STATUS_OBJECT_NAME_NOT_FOUND.Description);
                    };
                }
                else
                {
                    createResponseChecker = (header, response) =>
                    {
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    };
                }
            }
            else
            {
                createResponseChecker = (header, response) =>
                {
                    BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                       "{0} should not be successful if Open.Session is not NULL, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));

                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND,
                        header.Status,
                        RequirementCategory.STATUS_OBJECT_NAME_NOT_FOUND.Id,
                        RequirementCategory.STATUS_OBJECT_NAME_NOT_FOUND.Description);
                };
            }

            clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                new Smb2CreateContextRequest[] {
                    new Smb2CreateDurableHandleReconnect
                    {
                         Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: createResponseChecker);
            #endregion

            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);

            clientAfterDisconnection.LogOff();

            clientAfterDisconnection.Disconnect();
        }

        private void TriggerServerTerminateConnection()
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client sends negotiate to make server disconnect the connection");
            try
            {
                DialectRevision[] requestDialect = new DialectRevision[] { DialectRevision.Smb2002, DialectRevision.Smb21 };
                Capabilities_Values clientCapabilities = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
                SecurityMode_Values clientSecurityMode = SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED;
                clientBeforeDisconnection.Negotiate(requestDialect, TestConfig.IsSMB1NegotiateEnabled, clientSecurityMode, clientCapabilities, clientGuid);
            }
            catch
            {
            }

            BaseTestSite.Assert.IsTrue(
                clientBeforeDisconnection.Smb2Client.IsServerDisconnected,
                "If Negotiate after Create, the Connection.NegotiateDialect in server is 0x0202, 0x0210, or 0x0300, the server MUST disconnect the connection and not reply");
        }
        #endregion
    }
}
