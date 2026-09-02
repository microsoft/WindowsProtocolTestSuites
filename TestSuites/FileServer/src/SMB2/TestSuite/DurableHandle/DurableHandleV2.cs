// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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

            if (clientAfterDisconnection != null)
            {
                try
                {
                    clientAfterDisconnection.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect clientAfterDisconnection: {0}", ex.ToString());
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
                        Epoch = GetCreateResponseEpoch(serverCreateContexts),
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
                        Epoch = GetCreateResponseEpoch(serverCreateContexts),
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
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DurableHandleV2BatchOplock)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("Test reconnect with DurableHandleV2 and an additional SMB2_CREATE_DURABLE_HANDLE_REQUEST create context.")]
        public void DurableHandleV2_Reconnect_IncludeDurableHandleRequest()
        {
            /// 1. Client requests a durable handle V2 with BatchOplock.
            /// 2. Client disconnects.
            /// 3. Client reconnects with the correct FileId.Persistent and includes SMB2_CREATE_DURABLE_HANDLE_REQUEST.
            /// 4. Server is expected to return STATUS_OBJECT_NAME_NOT_FOUND.

            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            DurableHandleV2_Reconnect_WithAdditionalDurableHandleContext(
                DurableHandleV2ReconnectAdditionalContext.DurableHandleRequest);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DurableHandleV2BatchOplock)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("Test reconnect with DurableHandleV2 and an additional SMB2_CREATE_DURABLE_HANDLE_RECONNECT create context.")]
        public void DurableHandleV2_Reconnect_IncludeDurableHandleReconnect()
        {
            /// 1. Client requests a durable handle V2 with BatchOplock.
            /// 2. Client disconnects.
            /// 3. Client reconnects with the correct FileId.Persistent and includes SMB2_CREATE_DURABLE_HANDLE_RECONNECT.
            /// 4. Server is expected to return STATUS_OBJECT_NAME_NOT_FOUND.

            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            DurableHandleV2_Reconnect_WithAdditionalDurableHandleContext(
                DurableHandleV2ReconnectAdditionalContext.DurableHandleReconnect);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DurableHandleV2BatchOplock)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("Test reconnect with DurableHandleV2 and an additional SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context.")]
        public void DurableHandleV2_Reconnect_IncludeDurableHandleRequestV2()
        {
            /// 1. Client requests a durable handle V2 with BatchOplock.
            /// 2. Client disconnects.
            /// 3. Client reconnects with the correct FileId.Persistent and includes SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2.
            /// 4. Server is expected to return STATUS_OBJECT_NAME_NOT_FOUND.

            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            DurableHandleV2_Reconnect_WithAdditionalDurableHandleContext(
                DurableHandleV2ReconnectAdditionalContext.DurableHandleRequestV2);
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

            DurableHandleV2_Reconnect_WithLeaseV1(sameFileName: true);
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
            /// 3. Client reconnects the durable handle V2 with LeaseV1 context, but the file name is different, and expects STATUS_OBJECT_NAME_NOT_FOUND.

            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            DurableHandleV2_Reconnect_WithLeaseV1(sameFileName: false);
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

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.PersistentHandle)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test reconnect of a persistent handle that is resumed through the CreateGuid lookup path when the FileId.Persistent lookup fails.")]
        public void PersistentHandle_Reconnect_ViaCreateGuid()
        {
            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            PersistentHandleReconnectViaCreateGuid(PersistentHandleReconnectViaCreateGuidScenario.SuccessWithoutLease);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.PersistentHandle)]
        [TestCategory(TestCategories.InvalidIdentifier)]
        [Description("Test that reconnect fails when both the FileId.Persistent lookup and the CreateGuid lookup fail.")]
        public void PersistentHandle_Reconnect_ViaCreateGuid_CreateGuidNotFound()
        {
            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            PersistentHandleReconnectViaCreateGuid(PersistentHandleReconnectViaCreateGuidScenario.CreateGuidNotFound);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.PersistentHandle)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Test that reconnect through the CreateGuid lookup path fails when SMB2_DHANDLE_FLAG_PERSISTENT is not set.")]
        public void PersistentHandle_Reconnect_ViaCreateGuid_WithoutPersistentFlag()
        {
            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            PersistentHandleReconnectViaCreateGuid(PersistentHandleReconnectViaCreateGuidScenario.WithoutPersistentFlag);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.PersistentHandle)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Test that reconnect through the CreateGuid lookup path fails when Open.Session is not NULL.")]
        public void PersistentHandle_Reconnect_ViaCreateGuid_SessionNotNull()
        {
            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            PersistentHandleReconnectViaCreateGuid(PersistentHandleReconnectViaCreateGuidScenario.SessionNotNull);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.PersistentHandle)]
        [TestCategory(TestCategories.FileAccessCheck)]
        [Description("Test that reconnect through the CreateGuid lookup path fails when the durable owner is different.")]
        public void PersistentHandle_Reconnect_ViaCreateGuid_WithDifferentDurableOwner()
        {

            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }
            
            PersistentHandleReconnectViaCreateGuid(PersistentHandleReconnectViaCreateGuidScenario.DifferentDurableOwner);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.PersistentHandle)]
        [TestCategory(TestCategories.DurableHandleV2LeaseV1)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Test that reconnect through the CreateGuid lookup path fails when the Lease V1 key does not match the lease key recreated during resume.")]
        public void PersistentHandle_Reconnect_ViaCreateGuid_WithDifferentLeaseKeyV1()
        {
            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            PersistentHandleReconnectViaCreateGuid(PersistentHandleReconnectViaCreateGuidScenario.DifferentLeaseKeyV1);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.PersistentHandle)]
        [TestCategory(TestCategories.DurableHandleV2LeaseV2)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Test that reconnect through the CreateGuid lookup path fails when the Lease V2 key does not match the lease key recreated during resume.")]
        public void PersistentHandle_Reconnect_ViaCreateGuid_WithDifferentLeaseKeyV2()
        {
            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            PersistentHandleReconnectViaCreateGuid(PersistentHandleReconnectViaCreateGuidScenario.DifferentLeaseKeyV2);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.PersistentHandle)]
        [TestCategory(TestCategories.DurableHandleV2LeaseV1)]
        [TestCategory(TestCategories.Positive)]
        [Description("Test that the lease-presence validations do not apply when a persistent handle with a Lease V1 is resumed through the CreateGuid lookup path without a lease context in the reconnect request.")]
        public void PersistentHandle_Reconnect_ViaCreateGuid_WithoutReconnectLeaseContext()
        {
            if (TestConfig.Platform >= Platform.WindowsServer2022 || TestConfig.Platform == Platform.NonWindows)
            {
                BaseTestSite.Assert.Inconclusive("This test case is currently under investigation and will be revisited once additional information becomes available.");
            }

            PersistentHandleReconnectViaCreateGuid(PersistentHandleReconnectViaCreateGuidScenario.WithoutReconnectLeaseContext);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Compatibility)]
        [TestCategory(TestCategories.PersistentHandle)]
        [Description("Verify that whether opening a file will fail if a previous client already had the persistent handle to this file.")]
        public void PersistentHandle_ReOpenFromDiffClient()
        {
            /// 1. A client requests a persistent handle and succeeds
            /// 2. The client disconnects from the server
            /// 3. Another client (different client guid) opens the same file
            /// 4. The expected result of the second OPEN is STATUS_FILE_NOT_AVAILABLE according to section 3.3.5.9:
            ///    If Connection.Dialect belongs to the SMB 3.x dialect family and the request does not contain SMB2_CREATE_DURABLE_HANDLE_RECONNECT 
            ///    Create Context or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context, the server MUST look up an existing open in the GlobalOpenTable 
            ///    where Open.FileName matches the file name in the Buffer field of the request. 
            ///    If an Open entry is found, and if all the following conditions are satisfied, the server SHOULD fail the request with STATUS_FILE_NOT_AVAILABLE.
            ///       Open.IsPersistent is TRUE
            ///       Open.Connection is NULL

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
            #endregion

            Smb2FunctionalClient clientBeforeDisconnection = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.CAShareServerName, testConfig.CAShareName);

            // It will cost 3 minutes to delete this file, so do not add it to testFiles to skip auto-clean.
            string fileName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. A client requests a persistent handle and succeeds");
            uint treeIdBeforeDisconnection;
            Connect(DialectRevision.Smb30, clientBeforeDisconnection, Guid.NewGuid(), testConfig.AccountCredential, ConnectShareType.CAShare, out treeIdBeforeDisconnection, null);

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
                         CreateGuid = Guid.NewGuid(),
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


            // Disconnect
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "2. The client disconnects from the server");
            clientBeforeDisconnection.Disconnect();

            //If an Open entry is found, and if all the following conditions are satisfied, the server SHOULD<276> fail the request with STATUS_FILE_NOT_AVAILABLE.
            // Open.IsPersistent is TRUE
            // Open.Connection is NULL
            //<276> Section 3.3.5.9:  If Open.ClientGuid is not equal to the ClientGuid of the connection that received this request, Open.Lease.LeaseState is equal to RWH, or Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH,
            // Windows-based servers will attempt to break the lease/oplock and return STATUS_PENDING to process the create request asynchronously. Otherwise, if Open.Lease.LeaseState does not include SMB2_LEASE_HANDLE_CACHING and
            // Open.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH, Windows-based servers return STATUS_FILE_NOT_AVAILABLE.

            // Open from another client
            Smb2FunctionalClient anotherClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "3. Open the same file from different client (different client guid), the expected result of OPEN is STATUS_FILE_NOT_AVAILABLE");
            uint treeIdAfterDisconnection;
            Connect(DialectRevision.Smb30, anotherClient, Guid.NewGuid(), testConfig.AccountCredential, ConnectShareType.CAShare, out treeIdAfterDisconnection, null);

            FILEID fileIdAfterDisconnection;
            status = anotherClient.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[] {
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = Guid.NewGuid(),
                         Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    if (TestConfig.Platform == Platform.NonWindows)
                    {
                        BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "The server SHOULD fail the request with STATUS_FILE_NOT_AVAILABLE.");
                    }
                    else
                    {
                        BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_FILE_NOT_AVAILABLE,
                        header.Status,
                        "The server SHOULD fail the request with STATUS_FILE_NOT_AVAILABLE.");
                    }
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.DurableHandleV2LeaseV2)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Test reconnect with DurableHandleV2, LeaseV1 and LeaseV2 context, but with different LeaseKey.")]
        public void DurableHandleV2_WithLeaseV1AndV2_WithDifferentLeaseKey()
        {
            DurableHandleV2_Reconnect_ObjectNotFound();
        }
        #endregion

        #region private method

        private enum DurableHandleV2ReconnectAdditionalContext
        {
            DurableHandleRequest,
            DurableHandleReconnect,
            DurableHandleRequestV2,
        }

        private enum PersistentHandleReconnectViaCreateGuidScenario
        {
            SuccessWithoutLease,
            CreateGuidNotFound,
            WithoutPersistentFlag,
            SessionNotNull,
            DifferentDurableOwner,
            DifferentLeaseKeyV1,
            DifferentLeaseKeyV2,
            WithoutReconnectLeaseContext,
        }

        private void DurableHandleV2_Reconnect_WithAdditionalDurableHandleContext(
            DurableHandleV2ReconnectAdditionalContext additionalContext)
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);

            CreateContextTypeValue additionalContextType;
            switch (additionalContext)
            {
                case DurableHandleV2ReconnectAdditionalContext.DurableHandleRequest:
                    additionalContextType = CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST;
                    break;
                case DurableHandleV2ReconnectAdditionalContext.DurableHandleReconnect:
                    additionalContextType = CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT;
                    break;
                case DurableHandleV2ReconnectAdditionalContext.DurableHandleRequestV2:
                    additionalContextType = CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(additionalContext), additionalContext, null);
            }

            if (additionalContext == DurableHandleV2ReconnectAdditionalContext.DurableHandleRequestV2)
            {
                TestConfig.CheckCreateContext(
                    CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2,
                    CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2);
            }
            else
            {
                TestConfig.CheckCreateContext(
                    CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2,
                    CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2,
                    additionalContextType);
            }
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            Guid clientGuid = Guid.NewGuid();
            Guid createGuid = Guid.NewGuid();
            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "1. Client connects to the server and opens a file with a durable handle V2 and a batch oplock.");

            uint treeIdBeforeDisconnection;
            Connect(
                DialectRevision.Smb30,
                clientBeforeDisconnection,
                clientGuid,
                testConfig.AccountCredential,
                ConnectShareType.BasicShareWithoutAssert,
                out treeIdBeforeDisconnection,
                null);

            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            status = clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                new Smb2CreateContextRequest[]
                {
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
                        "{0} should be successful, actually server returns {1}.",
                        header.Command,
                        Smb2Status.GetStatusCode(header.Status));
                    CheckCreateContextResponses(
                        serverCreateContexts,
                        new DefaultDurableHandleV2ResponseChecker(BaseTestSite, 0, uint.MaxValue));
                });

            status = clientBeforeDisconnection.Write(
                treeIdBeforeDisconnection,
                fileIdBeforeDisconnection,
                content);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "2. Client disconnects from the server.");
            clientBeforeDisconnection.Disconnect();

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "3. Client reconnects with the correct FileId.Persistent and an additional {0} create context.",
                additionalContextType);

            uint treeIdAfterDisconnection;
            Connect(
                DialectRevision.Smb30,
                clientAfterDisconnection,
                clientGuid,
                testConfig.AccountCredential,
                ConnectShareType.BasicShareWithoutAssert,
                out treeIdAfterDisconnection,
                clientBeforeDisconnection);

            var reconnectCreateContexts = new List<Smb2CreateContextRequest>
            {
                new Smb2CreateDurableHandleReconnectV2
                {
                    CreateGuid = createGuid,
                    FileId = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent },
                }
            };

            switch (additionalContext)
            {
                case DurableHandleV2ReconnectAdditionalContext.DurableHandleRequest:
                    reconnectCreateContexts.Add(
                        new Smb2CreateDurableHandleRequest
                        {
                            DurableRequest = Guid.Empty,
                        });
                    break;
                case DurableHandleV2ReconnectAdditionalContext.DurableHandleReconnect:
                    reconnectCreateContexts.Add(
                        new Smb2CreateDurableHandleReconnect
                        {
                            Data = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent },
                        });
                    break;
                case DurableHandleV2ReconnectAdditionalContext.DurableHandleRequestV2:
                    reconnectCreateContexts.Add(
                        new Smb2CreateDurableHandleRequestV2
                        {
                            CreateGuid = Guid.NewGuid(),
                        });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(additionalContext), additionalContext, null);
            }

            FILEID fileIdAfterDisconnection;
            status = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                reconnectCreateContexts.ToArray(),
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND,
                        header.Status,
                        "[MS-SMB2] section 3.3.5.9.12: When the FileId.Persistent lookup succeeds and the " +
                        "SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 request also contains {0}, the server MUST fail " +
                        "the request with STATUS_OBJECT_NAME_NOT_FOUND. Actually server returns {1}.",
                        additionalContextType,
                        Smb2Status.GetStatusCode(header.Status));
                });

            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
        }

        private void PersistentHandleReconnectViaCreateGuid(PersistentHandleReconnectViaCreateGuidScenario scenario)
        {
            bool useLeaseV1 =
                scenario == PersistentHandleReconnectViaCreateGuidScenario.DifferentLeaseKeyV1 ||
                scenario == PersistentHandleReconnectViaCreateGuidScenario.WithoutReconnectLeaseContext;
            bool useLeaseV2 = scenario == PersistentHandleReconnectViaCreateGuidScenario.DifferentLeaseKeyV2;
            bool includeLeaseInReconnect =
                scenario == PersistentHandleReconnectViaCreateGuidScenario.DifferentLeaseKeyV1 ||
                scenario == PersistentHandleReconnectViaCreateGuidScenario.DifferentLeaseKeyV2;
            bool keepOriginalSession = scenario == PersistentHandleReconnectViaCreateGuidScenario.SessionNotNull;
            bool useDifferentDurableOwner = scenario == PersistentHandleReconnectViaCreateGuidScenario.DifferentDurableOwner;

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);

            NEGOTIATE_Response_Capabilities_Values requiredCapabilities =
                NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
            if (useLeaseV1 || useLeaseV2)
            {
                requiredCapabilities |= NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING;
            }
            if (useLeaseV2)
            {
                requiredCapabilities |= NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING;
            }
            TestConfig.CheckCapabilities(requiredCapabilities);

            if (useLeaseV1)
            {
                TestConfig.CheckCreateContext(
                    CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2,
                    CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2,
                    CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            }
            else if (useLeaseV2)
            {
                TestConfig.CheckCreateContext(
                    CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2,
                    CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2,
                    CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);
            }
            else
            {
                TestConfig.CheckCreateContext(
                    CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2,
                    CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2);
            }
            #endregion

            string content = Smb2Utility.CreateRandomString(testConfig.WriteBufferLengthInKb);
            Guid clientGuid = Guid.NewGuid();
            Guid createGuid = Guid.NewGuid();
            Guid leaseKey = Guid.NewGuid();
            LeaseStateValues requestedLeaseState =
                LeaseStateValues.SMB2_LEASE_READ_CACHING |
                LeaseStateValues.SMB2_LEASE_HANDLE_CACHING |
                LeaseStateValues.SMB2_LEASE_WRITE_CACHING;

            durableHandleUncSharePath = Smb2Utility.GetUncPath(testConfig.CAShareServerName, testConfig.CAShareName);
            string fileName = GetTestFileName(durableHandleUncSharePath);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "1. Client connects to a CA share and opens a file with a persistent handle{0}.",
                useLeaseV1 ? " and a Lease V1" : useLeaseV2 ? " and a Lease V2" : string.Empty);

            uint treeIdBeforeDisconnection;
            Connect(
                DialectRevision.Smb30,
                clientBeforeDisconnection,
                clientGuid,
                testConfig.AccountCredential,
                ConnectShareType.CAShare,
                out treeIdBeforeDisconnection,
                null);

            var initialCreateContexts = new List<Smb2CreateContextRequest>
            {
                new Smb2CreateDurableHandleRequestV2
                {
                    CreateGuid = createGuid,
                    Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                }
            };

            if (useLeaseV1)
            {
                initialCreateContexts.Add(
                    new Smb2CreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = requestedLeaseState,
                    });
            }
            else if (useLeaseV2)
            {
                initialCreateContexts.Add(
                    new Smb2CreateRequestLeaseV2
                    {
                        LeaseKey = leaseKey,
                        LeaseState = requestedLeaseState,
                    });
            }

            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] initialServerCreateContexts = null;
            status = clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out initialServerCreateContexts,
                useLeaseV1 || useLeaseV2
                    ? RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE
                    : RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                initialCreateContexts.ToArray(),
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should be successful, actually server returns {1}.",
                        header.Command,
                        Smb2Status.GetStatusCode(header.Status));
                    CheckCreateContextResponses(
                        initialServerCreateContexts,
                        new DefaultDurableHandleV2ResponseChecker(
                            BaseTestSite,
                            CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                            uint.MaxValue));
                });

            LeaseStateValues grantedLeaseState = default(LeaseStateValues);
            if (useLeaseV1 || useLeaseV2)
            {
                grantedLeaseState = GetGrantedLeaseState(initialServerCreateContexts, useLeaseV2);
            }
            ushort leaseEpoch = useLeaseV2 ? GetCreateResponseEpoch(initialServerCreateContexts) : (ushort)0;

            status = clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);

            FILEID invalidFileId = FILEID.Invalid;
            BaseTestSite.Assert.AreNotEqual(
                fileIdBeforeDisconnection.Persistent,
                invalidFileId.Persistent,
                "The FileId.Persistent used to force the Step 1 lookup failure must differ from the FileId.Persistent returned by the server.");

            if (!keepOriginalSession)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Client disconnects from the server.");
                clientBeforeDisconnection.Disconnect();
            }
            else
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.TestStep,
                    "2. The original client remains connected so that Open.Session is not NULL.");
            }

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "3. Another client sends SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 with an invalid FileId.Persistent to exercise the CreateGuid lookup path. Scenario: {0}.",
                scenario);

            uint treeIdAfterDisconnection;
            Connect(
                DialectRevision.Smb30,
                clientAfterDisconnection,
                clientGuid,
                useDifferentDurableOwner ? testConfig.NonAdminAccountCredential : testConfig.AccountCredential,
                ConnectShareType.CAShare,
                out treeIdAfterDisconnection,
                keepOriginalSession || useDifferentDurableOwner ? null : clientBeforeDisconnection);

            Guid reconnectCreateGuid = createGuid;
            if (scenario == PersistentHandleReconnectViaCreateGuidScenario.CreateGuidNotFound)
            {
                do
                {
                    reconnectCreateGuid = Guid.NewGuid();
                }
                while (reconnectCreateGuid == createGuid);
            }

            var reconnectContexts = new List<Smb2CreateContextRequest>
            {
                new Smb2CreateDurableHandleReconnectV2
                {
                    CreateGuid = reconnectCreateGuid,
                    Flags = scenario == PersistentHandleReconnectViaCreateGuidScenario.WithoutPersistentFlag
                        ? 0
                        : CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                    FileId = invalidFileId,
                }
            };

            if (includeLeaseInReconnect)
            {
                Guid mismatchedLeaseKey;
                do
                {
                    mismatchedLeaseKey = Guid.NewGuid();
                }
                while (mismatchedLeaseKey == leaseKey);

                if (useLeaseV1)
                {
                    reconnectContexts.Add(
                        new Smb2CreateRequestLease
                        {
                            LeaseKey = mismatchedLeaseKey,
                            LeaseState = grantedLeaseState,
                        });
                }
                else
                {
                    reconnectContexts.Add(
                        new Smb2CreateRequestLeaseV2
                        {
                            LeaseKey = mismatchedLeaseKey,
                            LeaseState = grantedLeaseState,
                            Epoch = leaseEpoch,
                        });
                }
            }

            FILEID fileIdAfterDisconnection;
            Smb2CreateContextResponse[] reconnectServerCreateContexts;
            uint reconnectStatus = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out reconnectServerCreateContexts,
                includeLeaseInReconnect
                    ? RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE
                    : RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                reconnectContexts.ToArray(),
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => { });

            uint expectedStatus = GetExpectedStatus(scenario);
            uint readStatus = uint.MaxValue;
            string readContent = null;
            uint closeStatus = uint.MaxValue;

            if (reconnectStatus == Smb2Status.STATUS_SUCCESS)
            {
                if (expectedStatus == Smb2Status.STATUS_SUCCESS)
                {
                    readStatus = clientAfterDisconnection.Read(
                        treeIdAfterDisconnection,
                        fileIdAfterDisconnection,
                        0,
                        (uint)content.Length,
                        out readContent,
                        checker: (header, response) => { });
                }

                closeStatus = clientAfterDisconnection.Close(
                    treeIdAfterDisconnection,
                    fileIdAfterDisconnection,
                    checker: (header, response) => { });
            }
            else if (keepOriginalSession)
            {
                closeStatus = clientBeforeDisconnection.Close(
                    treeIdBeforeDisconnection,
                    fileIdBeforeDisconnection,
                    checker: (header, response) => { });
            }
            else
            {
                if (useDifferentDurableOwner)
                {
                    DisconnectClientBestEffort(clientAfterDisconnection, treeIdAfterDisconnection);
                    clientAfterDisconnection = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
                    Connect(
                        DialectRevision.Smb30,
                        clientAfterDisconnection,
                        clientGuid,
                        testConfig.AccountCredential,
                        ConnectShareType.CAShare,
                        out treeIdAfterDisconnection,
                        clientBeforeDisconnection);
                }

                ReconnectAndClosePersistentHandleBestEffort(
                    clientAfterDisconnection,
                    treeIdAfterDisconnection,
                    fileName,
                    createGuid,
                    fileIdBeforeDisconnection,
                    useLeaseV1,
                    useLeaseV2,
                    leaseKey,
                    grantedLeaseState,
                    leaseEpoch);
            }

            if (keepOriginalSession)
            {
                DisconnectClientBestEffort(clientBeforeDisconnection, treeIdBeforeDisconnection);
            }
            DisconnectClientBestEffort(clientAfterDisconnection, treeIdAfterDisconnection);

            BaseTestSite.Assert.AreEqual(
                expectedStatus,
                reconnectStatus,
                "[MS-SMB2] section 3.3.5.9.12 CreateGuid reconnect scenario {0} should return {1}, actually server returns {2}.",
                scenario,
                Smb2Status.GetStatusCode(expectedStatus),
                Smb2Status.GetStatusCode(reconnectStatus));

            if (expectedStatus == Smb2Status.STATUS_SUCCESS)
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    readStatus,
                    "The resumed persistent handle should be usable for READ.");
                BaseTestSite.Assert.AreEqual(
                    content,
                    readContent,
                    "The written content should equal the content read through the resumed persistent handle.");
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    closeStatus,
                    "The resumed persistent handle should be closed successfully.");

                if (scenario == PersistentHandleReconnectViaCreateGuidScenario.WithoutReconnectLeaseContext)
                {
                    CheckCreateContextResponses(
                        reconnectServerCreateContexts,
                        new DefaultLeaseResponseChecker(
                            BaseTestSite,
                            leaseKey,
                            grantedLeaseState,
                            LeaseFlagsValues.NONE));
                }
            }
            else if (keepOriginalSession)
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    closeStatus,
                    "The original persistent handle should remain valid after the rejected reconnect attempt.");
            }
        }

        private uint GetExpectedStatus(PersistentHandleReconnectViaCreateGuidScenario scenario)
        {
            switch (scenario)
            {
                case PersistentHandleReconnectViaCreateGuidScenario.SuccessWithoutLease:
                case PersistentHandleReconnectViaCreateGuidScenario.WithoutReconnectLeaseContext:
                    return Smb2Status.STATUS_SUCCESS;

                case PersistentHandleReconnectViaCreateGuidScenario.DifferentDurableOwner:
                    return Smb2Status.STATUS_ACCESS_DENIED;

                default:
                    return Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND;
            }
        }

        private LeaseStateValues GetGrantedLeaseState(Smb2CreateContextResponse[] createContextResponses, bool leaseV2)
        {
            if (createContextResponses != null)
            {
                foreach (Smb2CreateContextResponse response in createContextResponses)
                {
                    if (!leaseV2 && response is Smb2CreateResponseLease leaseResponse)
                    {
                        return leaseResponse.LeaseState;
                    }
                    if (leaseV2 && response is Smb2CreateResponseLeaseV2 leaseV2Response)
                    {
                        return leaseV2Response.LeaseState;
                    }
                }
            }

            BaseTestSite.Assert.Fail(
                "The server should return a {0} response create context when granting the persistent handle.",
                leaseV2 ? "Lease V2" : "Lease V1");
            return default(LeaseStateValues);
        }

        private void ReconnectAndClosePersistentHandleBestEffort(
            Smb2FunctionalClient client,
            uint treeId,
            string fileName,
            Guid createGuid,
            FILEID fileIdBeforeDisconnection,
            bool useLeaseV1,
            bool useLeaseV2,
            Guid leaseKey,
            LeaseStateValues leaseState,
            ushort leaseEpoch)
        {
            try
            {
                var cleanupContexts = new List<Smb2CreateContextRequest>
                {
                    new Smb2CreateDurableHandleReconnectV2
                    {
                        CreateGuid = createGuid,
                        Flags = CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                        FileId = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent },
                    }
                };

                if (useLeaseV1)
                {
                    cleanupContexts.Add(
                        new Smb2CreateRequestLease
                        {
                            LeaseKey = leaseKey,
                            LeaseState = leaseState,
                        });
                }
                else if (useLeaseV2)
                {
                    cleanupContexts.Add(
                        new Smb2CreateRequestLeaseV2
                        {
                            LeaseKey = leaseKey,
                            LeaseState = leaseState,
                            Epoch = leaseEpoch,
                        });
                }

                FILEID cleanupFileId;
                Smb2CreateContextResponse[] cleanupResponseContexts;
                uint cleanupStatus = client.Create(
                    treeId,
                    fileName,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    out cleanupFileId,
                    out cleanupResponseContexts,
                    useLeaseV1 || useLeaseV2
                        ? RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE
                        : RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    cleanupContexts.ToArray(),
                    shareAccess: ShareAccess_Values.NONE,
                    checker: (header, response) => { });

                if (cleanupStatus == Smb2Status.STATUS_SUCCESS)
                {
                    client.Close(treeId, cleanupFileId, checker: (header, response) => { });
                }
                else
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Best-effort persistent-handle cleanup reconnect returned {0}.",
                        Smb2Status.GetStatusCode(cleanupStatus));
                }
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Unexpected exception during best-effort persistent-handle cleanup: {0}",
                    ex.ToString());
            }
        }

        private void DisconnectClientBestEffort(Smb2FunctionalClient client, uint treeId)
        {
            if (client == null)
            {
                return;
            }

            try
            {
                client.TreeDisconnect(treeId, checker: (header, response) => { });
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception during TREE_DISCONNECT cleanup: {0}", ex.ToString());
            }

            try
            {
                client.LogOff(checker: (header, response) => { });
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception during LOGOFF cleanup: {0}", ex.ToString());
            }

            try
            {
                client.Disconnect();
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception during connection cleanup: {0}", ex.ToString());
            }
        }

        private ushort GetCreateResponseEpoch(Smb2CreateContextResponse[] createContextResponses)
        {
            ushort leaseEpoch = 0;

            if(createContextResponses != null)
            {
                foreach(Smb2CreateContextResponse response in createContextResponses)
                {
                    if(response is Smb2CreateResponseLeaseV2)
                    {
                        //If Connection.Dialect belongs to the SMB 3.x dialect family, the Epoch field MUST be set to 
                        //File.LeaseEpoch of the file being opened.
                        Smb2CreateResponseLeaseV2 clientResponseLease = response as Smb2CreateResponseLeaseV2;
                        leaseEpoch = clientResponseLease.Epoch;
                        break;
                    }
                }
            }
            
            return leaseEpoch;
        }

        private void DurableHandleV2_Reconnect_WithLeaseV1(bool sameFileName, bool persistent = false)
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(
                persistent ?
                NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING | NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES :
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
                    Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND, status,
                    "If Open.Lease is not NULL and Open.FileName does not match the file name specified in the Buffer field of the SMB2 CREATE request, " +
                    "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.");
            }

            #endregion

            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
        }

        private void DurableHandleV2_Reconnect_ObjectNotFound()
        {
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

            Guid createGuid = Guid.NewGuid();
            Guid leaseKey = Guid.NewGuid();
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
            FILEID fileIdBeforeDisconnection;
            Smb2CreateContextResponse[] serverCreateContexts = null;
            uint treeIdBeforeDisconnection;

            Connect_To_Server_WithDurableHandleV2(fileName, content, createGuid, clientGuid, leaseKey, leaseState, out treeIdBeforeDisconnection, out fileIdBeforeDisconnection, out serverCreateContexts);

            #endregion

            clientBeforeDisconnection.Disconnect();

            #region client reconnect to server
            BaseTestSite.Log.Add(
                LogEntryKind.Comment,
                "Client opens the same file and reconnects the durable handle");

            uint treeIdAfterDisconnection;
            Connect(
                DialectRevision.Smb30,
                clientAfterDisconnection,
                clientGuid,
                testConfig.AccountCredential,
                ConnectShareType.BasicShareWithoutAssert,
                out treeIdAfterDisconnection,
                clientBeforeDisconnection);

            FILEID fileIdAfterDisconnection;
            Smb2CreateContextRequest[] createContextRequest = new Smb2CreateContextRequest[] {
                new Smb2CreateDurableHandleReconnectV2
                {
                        CreateGuid = createGuid,
                        Flags = 0,
                        FileId = new FILEID { Persistent = fileIdBeforeDisconnection.Persistent }
                },
                new Smb2CreateRequestLeaseV2
                {
                    LeaseKey = Guid.NewGuid(),
                    LeaseState = leaseState,
                    Epoch = GetCreateResponseEpoch(serverCreateContexts),
                }
            };
            status = clientAfterDisconnection.Create(
                treeIdAfterDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnection,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                createContextRequest,
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => { });

            BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND, status,
                    "[MS-SMB2] Section 3.3.5.9.7  If any of the following conditions is TRUE, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND: " +
                    "The SMB2_CREATE_REQUEST_LEASE_V2 create context is also present in the request, Connection.Dialect belongs to the SMB 3.x dialect family," +
                    "the server supports directory leasing, Open.Lease is not NULL, and Open.Lease.LeaseKey does not match the LeaseKey provided " +
                    "in the SMB2_CREATE_REQUEST_LEASE_V2 create context.");

            #endregion

            clientAfterDisconnection.TreeDisconnect(treeIdAfterDisconnection);
            clientAfterDisconnection.LogOff();
            clientAfterDisconnection.Disconnect();
        }

        private void Connect_To_Server_WithDurableHandleV2(string fileName, string content, Guid createGuid, Guid clientGuid, Guid leaseKey, LeaseStateValues leaseState, 
            out uint treeIdBeforeDisconnection, out FILEID fileIdBeforeDisconnection, out Smb2CreateContextResponse[] serverCreateContexts)
        {
            Smb2CreateContextResponse[] serverCreateContextsResponse = null;
            Connect(
                DialectRevision.Smb30,
                clientBeforeDisconnection,
                clientGuid,
                testConfig.AccountCredential,
                ConnectShareType.BasicShareWithoutAssert,
                out treeIdBeforeDisconnection,
                null);

            status = clientBeforeDisconnection.Create(
                treeIdBeforeDisconnection,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnection,
                out serverCreateContextsResponse,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[] {
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                         Flags = 0,
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
                        serverCreateContextsResponse,
                        new DefaultDurableHandleV2ResponseChecker(
                            BaseTestSite,
                            0,
                            uint.MaxValue));
                });
            serverCreateContexts = serverCreateContextsResponse;
            status = clientBeforeDisconnection.Write(treeIdBeforeDisconnection, fileIdBeforeDisconnection, content);
        }
        #endregion
    }
}
