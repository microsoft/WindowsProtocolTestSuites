// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.RSVD.TestSuite
{
    [TestClass]
    public class OpenCloseSharedVHD : RSVDTestBase
    {

        #region Test Suite Initialization

        #region Variables
        Smb2FunctionalClient clientBeforeDisconnect = null;
        Smb2FunctionalClient clientAfterDisconnect = null;
        uint treeIdBeforeDisconnect;
        uint treeIdAfterDisconnect;
        FILEID fileIdBeforeDisconnect;
        FILEID fileIdAfterDisconnect;
        #endregion

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        protected override void TestCleanup()
        {
            // The persistent handle requested by the clientBeforeDisconnect needs to be closed t make sure that server will not preserve the handle.
            // Or else others test cases execution will be affected 
            if (clientBeforeDisconnect != null)
            {
                try
                {
                    clientBeforeDisconnect.Close(treeIdBeforeDisconnect, fileIdBeforeDisconnect);
                    clientBeforeDisconnect.TreeDisconnect(treeIdBeforeDisconnect);
                    clientBeforeDisconnect.LogOff();
                    clientBeforeDisconnect.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when release clientBeforeDisconnect: {0}", ex.ToString());
                }
            }

            if (clientAfterDisconnect != null)
            {
                try
                {
                    clientAfterDisconnect.Close(treeIdBeforeDisconnect, fileIdBeforeDisconnect);
                    clientAfterDisconnect.TreeDisconnect(treeIdBeforeDisconnect);
                    clientAfterDisconnect.LogOff();
                    clientAfterDisconnect.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when release clientAfterDisconnect: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }

        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if the server supports opening/closing a shared virtual disk file with SVHDX_OPEN_DEVICE_CONTEXT.")]
        public void BVT_OpenCloseSharedVHD_V1()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file with SMB2 create context SVHDX_OPEN_DEVICE_CONTEXT and expects success.");
            Smb2CreateContextResponse[] serverContextResponse;
            OpenSharedVHD(TestConfig.NameOfSharedVHDX, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1, null, true, null, out serverContextResponse, null);
            CheckOpenDeviceContext(serverContextResponse);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion2)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if the server supports opening/closing a shared virtual disk file with SVHDX_OPEN_DEVICE_CONTEXT_V2.")]
        public void BVT_OpenCloseSharedVHD_V2()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file with SMB2 create context SVHDX_OPEN_DEVICE_CONTEXT_V2 and expects success.");
            Smb2CreateContextResponse[] serverContextResponse;
            OpenSharedVHD(TestConfig.NameOfSharedVHDS, RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_2, null, true, null, out serverContextResponse, null);
            CheckOpenDeviceContext(serverContextResponse);            

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Check if the client can reconnect the persistent handle to the shared virtual disk file without carrying device context.")]
        public void ReconnectSharedVHDWithoutDeviceContext()
        {
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "1.	Client opens a shared virtual disk file with SMB2 create contexts " +
                "SVHDX_OPEN_DEVICE_CONTEXT and SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 (persistent bit is set). ");

            clientBeforeDisconnect = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            Guid clientGuid = Guid.NewGuid();
            ConnectToShare(clientBeforeDisconnect, clientGuid, TestConfig.FullPathShareContainingSharedVHD, out treeIdBeforeDisconnect);

            Guid createGuid = Guid.NewGuid();
            Guid initiatorId = Guid.NewGuid();
            Smb2CreateContextResponse[] serverCreateContexts;
            clientBeforeDisconnect.Create
                (treeIdBeforeDisconnect,
                TestConfig.NameOfSharedVHDX + fileNameSuffix,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnect,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[]
                {
                    new Smb2CreateSvhdxOpenDeviceContext
                    {
                        Version = (uint)RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1,
                        OriginatorFlags = (uint)OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER,
                        InitiatorHostName = TestConfig.InitiatorHostName,
                        InitiatorHostNameLength = (ushort)(TestConfig.InitiatorHostName.Length * 2),  // InitiatorHostName is a null-terminated Unicode UTF-16 string 
                        InitiatorId = initiatorId
                    },
                    new Smb2CreateDurableHandleRequestV2
                    {
                        CreateGuid = createGuid,
                        Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT
                    }
                });

            bool persistentHandleReturned = false;
            if (serverCreateContexts != null && serverCreateContexts[0] is Smb2CreateDurableHandleResponseV2)
            {
                var durableResponse = serverCreateContexts[0] as Smb2CreateDurableHandleResponseV2;
                if (durableResponse.Flags.HasFlag(CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags.DHANDLE_FLAG_PERSISTENT))
                {
                    persistentHandleReturned = true;
                }
            }

            BaseTestSite.Assert.IsTrue(persistentHandleReturned, "Server should return a persistent handle.");

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "2.	Client disconnects from the server.");
            clientBeforeDisconnect.Disconnect();

            clientAfterDisconnect = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            ConnectToShare(clientAfterDisconnect, clientGuid, TestConfig.FullPathShareContainingSharedVHD, out treeIdAfterDisconnect);
            uint status = clientAfterDisconnect.Create
                (treeIdAfterDisconnect,
                TestConfig.NameOfSharedVHDX + fileNameSuffix,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdAfterDisconnect,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[]
                {
                    new Smb2CreateDurableHandleReconnectV2
                    {
                        CreateGuid = createGuid,
                        Flags = CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                        FileId = new FILEID { Persistent = fileIdBeforeDisconnect.Persistent }
                    }
                },
                checker: (header, response) => { });

            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "3. Client reconnects the persistent handle without create context SVHDX_OPEN_DEVICE_CONTEXT and expects success. Actual status is: {0}",
                GetStatus(status));
        }

        private void ConnectToShare(Smb2FunctionalClient client, Guid clientGuid, string uncShareName, out uint treeId)
        {
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.FileServerNameContainingSharedVHD, TestConfig.FileServerIPContainingSharedVHD);
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                clientGuid);
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.FileServerNameContainingSharedVHD, TestConfig.AccountCredential, false);
            client.TreeConnect(uncShareName, out treeId);
        }

        private void CheckOpenDeviceContext(Smb2CreateContextResponse[] servercreatecontexts)
        {
            // <9> Section 3.2.5.1:  Windows Server 2012 R2 without [MSKB-3025091] doesn't return SVHDX_OPEN_DEVICE_CONTEXT_RESPONSE.
            if (TestConfig.Platform == Platform.WindowsServer2012R2)
            { 
                return;
            }
            
            foreach (var context in servercreatecontexts)
            {
                Type type = context.GetType();
                if (type.Name == "Smb2CreateSvhdxOpenDeviceContext")
                {
                    Smb2CreateSvhdxOpenDeviceContextResponse openDeviceContext = context as Smb2CreateSvhdxOpenDeviceContextResponse;
                    VerifyFieldInResponse("ServerVersion", TestConfig.ServerServiceVersion, openDeviceContext.Version);
                }

                if (type.Name == "Smb2CreateSvhdxOpenDeviceContextResponseV2")
                {
                    Smb2CreateSvhdxOpenDeviceContextResponseV2 openDeviceContext = context as Smb2CreateSvhdxOpenDeviceContextResponseV2;

                    VerifyFieldInResponse("ServerVersion", TestConfig.ServerServiceVersion, openDeviceContext.Version);
                    VerifyFieldInResponse("SectorSize", TestConfig.PhysicalSectorSize, openDeviceContext.PhysicalSectorSize);
                    VerifyFieldInResponse("VirtualSize", TestConfig.VirtualSize, openDeviceContext.VirtualSize);
                }
            }          
                        
        }
    }
}
