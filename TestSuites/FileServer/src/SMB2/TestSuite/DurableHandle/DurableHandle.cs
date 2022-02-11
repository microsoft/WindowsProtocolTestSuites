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
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context.")]
        public void BVT_DurableHandleV1_Reconnect_WithLeaseV1()
        {
            /// 1. Client requests a durable handle with LeaseV1 context
            /// 2. Client sends Disconnect to lose connection
            /// 3. Client reconnects the durable handle with LeaseV1 context, and expects success.
            DurableHandleV1_Reconnect_WithLeaseV1(sameFileName:true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Test reconnect with DurableHandleV1 and LeaseV1 context, but the file name is different.")]
        public void DurableHandleV1_Reconnect_WithLeaseV1_WithDifferentFileName()
        {
            /// 1. Client requests a durable handle with LeaseV1 context
            /// 2. Client sends Disconnect to lose connection
            /// 3. Client reconnects the durable handle with LeaseV1 context but different file name, and expects STATUS_INVALID_PARAMETER.
            DurableHandleV1_Reconnect_WithLeaseV1(sameFileName:false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.DurableHandleV1LeaseV1)]
        [TestCategory(TestCategories.Positive)]
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
        #endregion

        #region Common Methods

        private void DurableHandleV1_Reconnect_WithLeaseV1(bool sameFileName)
        {
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
            uint status = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                sameFileName ? fileName : GetTestFileName(durableHandleUncSharePath),
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
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => {});

            if (sameFileName)
            {
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Reconnect a durable handle should be successful");
                string readContent;
                clientAfterDisconnection.Read(treeIdAfterDisconnection, fileIdAfterDisconnection, 0, (uint)content.Length, out readContent);

                BaseTestSite.Assert.IsTrue(
                    content.Equals(readContent),
                    "The written content is expected to be equal to read content.");
                clientAfterDisconnection.Close(treeIdAfterDisconnection, fileIdAfterDisconnection);
            }
            else
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_INVALID_PARAMETER, status,
                    "If Open.Lease is not NULL and Open.FileName does not match the file name specified in the Buffer field of the SMB2 CREATE request, " +
                    "the server MUST fail the request with STATUS_INVALID_PARAMETER.");
            }

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
