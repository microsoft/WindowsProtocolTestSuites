// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class DurableHandleV2 : SMB2TestBase
    {
        #region Variables
        private uint status;
        private Smb2FunctionalClient clientBeforeDisconnection;
        private Smb2FunctionalClient clientAfterDisconnection;
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
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DurableHandleV2LeaseV2)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test reconnect with DurableHandleV2 but without persistent flag.")]
        public void DurableHandleV2_Reconnect_WithoutPersistence()
        {
            /// 1. Client requests a durable handle V2 without persistent flag
            /// 2. Lose connection by disabling NIC
            /// 3. Client reconnects the durable handle V2 without persistent flag.

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            Guid clientGuid = Guid.NewGuid();
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb30, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdBeforeDisconnection, null);

            Guid createGuid = Guid.NewGuid();
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
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                    },
                    new Smb2CreateRequestLeaseV2
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
                    CheckCreateContextResponses(serverCreateContexts, new DefaultDurableHandleV2ResponseChecker(BaseTestSite, 0, uint.MaxValue));
                });

            clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);
            #endregion

            clientBeforeDisconnection.Disconnect();

            #region client reconnect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client opens the same file and reconnects the durable handle");

            uint treeIdAfterDisconnection;
            Connect(DialectRevision.Smb30, clientAfterDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdAfterDisconnection, clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;
            clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleReconnectV2
                    {
                         CreateGuid = createGuid,
                         FileId = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                    },
                    new Smb2CreateRequestLeaseV2
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
            #endregion

            clientAfterDisconnection.Close(treeIdAfterDisconnection, fileIdAfterDisconnection);
            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.PersistentHandleNonClusterRequired)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test reconnect with DurableHandleV2 when no persistent handle is granted by the server and the share does not have CA capability.")]
        public void DurableHandleV2_NoPersistenceGrantedOnNonCAShare()
        {
            /// 1. Client requests a durable handle V2 to a Non-CA share
            /// 2. Expect the create response contains Smb2CreateDurableHandleResponseV2 context but no persistent flag is set.

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            #endregion

            durableHandleUncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);
            Guid clientGuid = Guid.NewGuid();

            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            #region client connect to server
            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb30, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShare, out treeIdBeforeDisconnection, null);

            Smb2CreateContextResponse[] serverCreateContexts = null;
            FILEID fileIdBeforeDisconnection;
            Guid createGuid = Guid.NewGuid();
            clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                         Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                    },
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                });

            BaseTestSite.Assert.AreEqual(
                null,
                serverCreateContexts,
                "The server should ignore the create context when TreeConnect.Share.IsCA is FALSE.");
            #endregion

            clientBeforeDisconnection.Close(treeIdBeforeDisconnection, fileIdBeforeDisconnection);
            clientBeforeDisconnection.TreeDisconnect(treeIdBeforeDisconnection);
            clientBeforeDisconnection.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DurableHandleV2LeaseV2)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Test reconnect with DurableHandleV2 and different durable owner.")]
        public void DurableHandleV2_Reconnect_WithDifferentDurableOwner()
        {
            /// 1. Client requests a durable handle V2 with LeaseV1 context
            /// 2. Client disconnects
            /// 3. Client reconnects the durable handle V2 with a different durable owner, and expects that server will return STATUS_ACCESS_DENIED.

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            Guid clientGuid = Guid.NewGuid();
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb30, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdBeforeDisconnection, null);

            Guid createGuid = Guid.NewGuid();
            Guid leaseKey = Guid.NewGuid();
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            status = clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                    },
                    new Smb2CreateRequestLeaseV2
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
                    CheckCreateContextResponses(serverCreateContexts, new DefaultDurableHandleV2ResponseChecker(BaseTestSite, 0, uint.MaxValue));
                });

            status = clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);
            #endregion

            clientBeforeDisconnection.Disconnect();

            #region client reconnect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client opens the same file and reconnects the durable handle");

            uint treeIdAfterDisconnection;
            Connect(DialectRevision.Smb30, clientAfterDisconnection, clientGuid, testConfig.NonAdminAccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdAfterDisconnection, clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;
            clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleReconnectV2
                    {
                         CreateGuid = createGuid,
                         FileId = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                    },
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState,
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should not be successful if the DurableOwner is different, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_ACCESS_DENIED,
                        header.Status,
                        RequirementCategory.STATUS_ACCESS_DENIED.Id,
                        RequirementCategory.STATUS_ACCESS_DENIED.Description);
                });

            #endregion

            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DurableHandleV2BatchOplock)]
        [Description("Test reconnect with DurableHandleV2 and BatchOplock.")]
        public void BVT_DurableHandleV2_Reconnect_WithBatchOplock()
        {
            /// 1. Client requests a durable handle V2 with BatchOplock
            /// 2. Client disconnects
            /// 3. Client reconnects the durable handle V2 with BatchOplock, and expects success.

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2);
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            Guid clientGuid = Guid.NewGuid();
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle");

            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb30, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdBeforeDisconnection, null);

            Guid createGuid = Guid.NewGuid();
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            status = clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    CheckCreateContextResponses(serverCreateContexts, new DefaultDurableHandleV2ResponseChecker(BaseTestSite, 0, uint.MaxValue));
                });

            status = clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);
            #endregion

            clientBeforeDisconnection.Disconnect();

            #region client reconnect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client opens the same file and reconnects the durable handle");

            uint treeIdAfterDisconnection;
            Connect(DialectRevision.Smb30, clientAfterDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.BasicShareWithoutAssert, out treeIdAfterDisconnection, clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;
            status = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleReconnectV2
                    {
                         CreateGuid = createGuid,
                         FileId = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                    }
                },
                shareAccess: ShareAccess_Values.NONE);

            string readContent;
            status = clientAfterDisconnection.Read(treeIdAfterDisconnection, fileIdAfterDisconnection, 0, (uint)content.Length, out readContent);

            BaseTestSite.Assert.IsTrue(
                readContent.Equals(content),
                "The written content should equal to read content.");
            #endregion

            clientAfterDisconnection.Close(treeIdAfterDisconnection, fileIdAfterDisconnection);
            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DurableHandleV2LeaseV1)]
        [Description("Test reconnect with DurableHandleV2 and LeaseV1 context.")]
        public void BVT_DurableHandleV2_Reconnect_WithLeaseV1()
        {
            /// 1. Client requests a durable handle V2 with LeaseV1 context
            /// 2. Client disconnects from the server
            /// 3. Client reconnects the durable handle V2 with LeaseV1 context, and expects success.

            DurableHandleV2_Reconnect_WithLeaseV1(sameFileName:true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DurableHandleV2LeaseV1)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Test reconnect with DurableHandleV2 and LeaseV1 context, but the file name is different.")]
        public void DurableHandleV2_Reconnect_WithLeaseV1_WithDifferentFileName()
        {
            /// 1. Client requests a durable handle V2 with LeaseV1 context
            /// 2. Client disconnects from the server
            /// 3. Client reconnects the durable handle V2 with LeaseV1 context, but the file name is different, and expects STATUS_INVALID_PARAMETER.

            DurableHandleV2_Reconnect_WithLeaseV1(sameFileName:false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DurableHandleV2LeaseV2)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test no durable handle is granted if requesting with DurableHandleV2 and LeaseV2 context, but SMB2_LEASE_HANDLE_CACHING bit is not set in LeaseState.")]
        public void DurableHandleV2_WithLeaseV2_WithoutHandleCaching()
        {
            /// 1. Client requests a durable handle with LeaseV2 context, SMB2_LEASE_HANDLE_CACHING bit is not set in LeaseState.
            /// 2. Durable Handle v2 is not granted.
            /// 
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            Guid clientGuid = Guid.NewGuid();
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a durable handle v2, SMB2_LEASE_HANDLE_CACHING bit is not set in LeaseState.");

            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb30, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential,
                ConnectShareType.BasicShareWithoutAssert, out treeIdBeforeDisconnection, null);

            Guid createGuid = Guid.NewGuid();
            Guid leaseKey = Guid.NewGuid();

            // SMB2_LEASE_HANDLE_CACHING bit is not set in LeaseState
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            status = clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                    },
                    new Smb2CreateRequestLeaseV2
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
                    CheckCreateContextResponsesNotExist(
                        serverCreateContexts,
                        new DefaultDurableHandleV2ResponseChecker(
                            BaseTestSite,
                            0,
                            uint.MaxValue));
                });
            #endregion

            clientBeforeDisconnection.TreeDisconnect(treeIdBeforeDisconnection);
            clientBeforeDisconnection.LogOff();
            clientBeforeDisconnection.Disconnect();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.PersistentHandle)]
        [Description("Test reconnect with persistent handle.")]
        public void BVT_PersistentHandle_Reconnect()
        {
            /// 1. Client requests a persistent handle
            /// 2. Client disconnects from the server
            /// 3. Client reconnects the persistent handle, and expects success.

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING | NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2);
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            Guid clientGuid = Guid.NewGuid();
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.CAShareServerName, testConfig.CAShareName);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a persistent handle");

            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb30, clientBeforeDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.CAShare, out treeIdBeforeDisconnection, null);

            Guid createGuid = Guid.NewGuid();
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            status = clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                         Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    CheckCreateContextResponses(serverCreateContexts, new DefaultDurableHandleV2ResponseChecker(BaseTestSite, CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags.DHANDLE_FLAG_PERSISTENT, uint.MaxValue));
                });

            status = clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);
            #endregion

            clientBeforeDisconnection.Disconnect();

            #region client reconnect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client opens the same file and reconnects the persistent handle");

            uint treeIdAfterDisconnection;
            Connect(DialectRevision.Smb30, clientAfterDisconnection, clientGuid, testConfig.AccountCredential, ConnectShareType.CAShare, out treeIdAfterDisconnection, clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;
            status = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleReconnectV2
                    {
                         CreateGuid = createGuid,
                         Flags = CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                         FileId = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                    }
                },
                shareAccess: ShareAccess_Values.NONE);

            string readContent;
            status = clientAfterDisconnection.Read(treeIdAfterDisconnection, fileIdAfterDisconnection, 0, (uint)content.Length, out readContent);

            BaseTestSite.Assert.IsTrue(
                readContent.Equals(content),
                "The written content should equal to read content.");
            #endregion

            clientAfterDisconnection.Close(treeIdAfterDisconnection, fileIdAfterDisconnection);
            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
        }
        #endregion

        #region private method
        private void DurableHandleV2_Reconnect_WithLeaseV1(bool sameFileName, bool persistent = false)
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(
                persistent ? 
                NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING | NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES:
                NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            Guid clientGuid = Guid.NewGuid();
            durableHandleUncSharePath = 
                persistent ? Smb2Utility.GetUncPath(testConfig.CAShareServerName, testConfig.CAShareName) : Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            #region client connect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client connects to server and opens file with a {0} handle", persistent ? "persistent" : "durable");

            uint treeIdBeforeDisconnection;
            Connect(
                DialectRevision.Smb30, 
                clientBeforeDisconnection, 
                clientGuid, 
                testConfig.AccountCredential, 
                persistent ? ConnectShareType.CAShare : ConnectShareType.BasicShareWithoutAssert, 
                out treeIdBeforeDisconnection, 
                null);

            Guid createGuid = Guid.NewGuid();
            Guid leaseKey = Guid.NewGuid();
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            status = clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                         Flags = persistent? CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT : 0,
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
                    CheckCreateContextResponses(
                        serverCreateContexts, 
                        new DefaultDurableHandleV2ResponseChecker(
                            BaseTestSite, 
                            persistent ? CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags.DHANDLE_FLAG_PERSISTENT : 0, 
                            uint.MaxValue));
                });

            status = clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);
            #endregion

            clientBeforeDisconnection.Disconnect();

            #region client reconnect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client opens the same file and reconnects the {0} handle", persistent ? "durable" : "persistent");

            uint treeIdAfterDisconnection;
            Connect(
                DialectRevision.Smb30, 
                clientAfterDisconnection, 
                clientGuid, 
                testConfig.AccountCredential,
                persistent ? ConnectShareType.CAShare : ConnectShareType.BasicShareWithoutAssert, 
                out treeIdAfterDisconnection, 
                clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;
            status = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                sameFileName ? fileName : GetTestFileName(durableHandleUncSharePath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleReconnectV2
                    {
                         CreateGuid = createGuid,
                         Flags = persistent ? CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags.DHANDLE_FLAG_PERSISTENT : 0,
                         FileId = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                    },
                    new Smb2CreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState,
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => { });

            if (sameFileName)
            {
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Reconnect a durable handle should be successful");
                string readContent;
                status = clientAfterDisconnection.Read(treeIdAfterDisconnection, fileIdAfterDisconnection, 0, (uint)content.Length, out readContent);

                BaseTestSite.Assert.IsTrue(
                    readContent.Equals(content),
                    "The written content should equal to read content.");
                clientAfterDisconnection.Close(treeIdAfterDisconnection, fileIdAfterDisconnection);
            }
            else
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_INVALID_PARAMETER, status,
                    "If Open.Lease is not NULL and Open.FileName does not match the file name specified in the Buffer field of the SMB2 CREATE request, " +
                    "the server MUST fail the request with STATUS_INVALID_PARAMETER.");
            }

            #endregion

            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
        }
        #endregion
    }
}
