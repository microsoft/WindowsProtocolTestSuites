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

        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion1)]
        [Description("Check if the server supports opening/closing a shared virtual disk file with SVHDX_OPEN_DEVICE_CONTEXT.")]
        public void BVT_OpenCloseSharedVHD_V1()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file with SMB2 create context SVHDX_OPEN_DEVICE_CONTEXT and expects success.");
            client.Connect(
                TestConfig.FileServerNameContainingSharedVHD,
                TestConfig.FileServerIPContainingSharedVHD,
                TestConfig.DomainName,
                TestConfig.UserName,
                TestConfig.UserPassword,
                TestConfig.DefaultSecurityPackage,
                TestConfig.UseServerGssToken,
                TestConfig.ShareContainingSharedVHD);

            Smb2CreateContextRequest[] contexts = new Smb2CreateContextRequest[]
            {
                new Smb2CreateSvhdxOpenDeviceContext 
                {
                    Version = TestConfig.ServerServiceVersion,
                    OriginatorFlags = (uint)OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER, 
                    InitiatorHostName = TestConfig.InitiatorHostName,
                    InitiatorHostNameLength = (ushort)(TestConfig.InitiatorHostName.Length * 2)  // InitiatorHostName is a null-terminated Unicode UTF-16 string 
                }
            };

            CREATE_Response response;
            Smb2CreateContextResponse[] serverCreateContexts;
            uint status = client.OpenSharedVirtualDisk(
                TestConfig.NameOfSharedVHDX + fileNameSuffix,
                FsCreateOption.NONE,
                contexts,
                out serverCreateContexts,
                out response);

            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Open shared virtual disk file should succeed, actual status: {0}",
                GetStatus(status));

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.RsvdVersion2)]
        [Description("Check if the server supports opening/closing a shared virtual disk file with SVHDX_OPEN_DEVICE_CONTEXT_V2.")]
        public void BVT_OpenCloseSharedVHD_V2()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1.	Client opens a shared virtual disk file with SMB2 create context SVHDX_OPEN_DEVICE_CONTEXT_V2 and expects success.");
            client.Connect(
                TestConfig.FileServerNameContainingSharedVHD,
                TestConfig.FileServerIPContainingSharedVHD,
                TestConfig.DomainName,
                TestConfig.UserName,
                TestConfig.UserPassword,
                TestConfig.DefaultSecurityPackage,
                TestConfig.UseServerGssToken,
                TestConfig.ShareContainingSharedVHD);

            Smb2CreateContextRequest[] contexts = new Smb2CreateContextRequest[]
            {
                new Smb2CreateSvhdxOpenDeviceContextV2
                {
                    Version = TestConfig.ServerServiceVersion,
                    OriginatorFlags = (uint)OriginatorFlag.SVHDX_ORIGINATOR_PVHDPARSER, 
                    InitiatorHostName = TestConfig.InitiatorHostName,
                    InitiatorHostNameLength = (ushort)(TestConfig.InitiatorHostName.Length * 2),  // InitiatorHostName is a null-terminated Unicode UTF-16 string 
                    VirtualDiskPropertiesInitialized = 0,
                    ServerServiceVersion = 0,
                    VirtualSectorSize = 0,
                    PhysicalSectorSize = 0,
                    VirtualSize = 0
                }
            };

            CREATE_Response response;
            Smb2CreateContextResponse[] serverCreateContexts;
            uint status = client.OpenSharedVirtualDisk(
                TestConfig.NameOfSharedVHDX + fileNameSuffix,
                FsCreateOption.NONE,
                contexts,
                out serverCreateContexts,
                out response);

            BaseTestSite.Assert.AreEqual(
                (uint)Smb2Status.STATUS_SUCCESS,
                status,
                "Open shared virtual disk file should succeed, actual status: {0}",
                GetStatus(status));

            CheckOpenDeviceContext(serverCreateContexts);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2.	Client closes the file.");
            client.CloseSharedVirtualDisk();
        }

        [TestMethod]
        [TestCategory(TestCategories.RsvdVersion1)]
        [TestCategory(TestCategories.Positive)]
        [Description("Check if the client can reconnect the persistent handle to the shared virtual disk file without carrying device context.")]
        public void ReconnectSharedVHDWithoutDeviceContext()
        {
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep, 
                "1.	Client opens a shared virtual disk file with SMB2 create contexts " + 
                "SVHDX_OPEN_DEVICE_CONTEXT and SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 (persistent bit is set). ");

            Smb2FunctionalClient clientBeforeDisconnect = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            string uncShareName = Smb2Utility.GetUncPath(TestConfig.FileServerNameContainingSharedVHD, TestConfig.ShareContainingSharedVHD);
            uint treeId;
            Guid clientGuid = Guid.NewGuid();
            ConnectToShare(clientBeforeDisconnect, clientGuid, uncShareName, out treeId);

            Guid createGuid = Guid.NewGuid();
            Guid initiatorId = Guid.NewGuid();
            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fileIdBeforeDisconnect;
            clientBeforeDisconnect.Create
                (treeId,
                TestConfig.NameOfSharedVHDX + fileNameSuffix,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeDisconnect,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[]
                {
                    new Smb2CreateSvhdxOpenDeviceContext 
                    {
                        Version = TestConfig.ServerServiceVersion,
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

            Smb2FunctionalClient clientAfterDisconnect = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            ConnectToShare(clientAfterDisconnect, clientGuid, uncShareName, out treeId);
            FILEID fileIdAfterDisconnect;
            uint status = clientAfterDisconnect.Create
                (treeId,
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

            clientAfterDisconnect.Close(treeId, fileIdAfterDisconnect);
            clientAfterDisconnect.TreeDisconnect(treeId);
            clientAfterDisconnect.LogOff();
            clientAfterDisconnect.Disconnect();
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
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            client.TreeConnect(Smb2Utility.GetUncPath(TestConfig.FileServerNameContainingSharedVHD, TestConfig.ShareContainingSharedVHD), out treeId);
        }

        private void CheckOpenDeviceContext(Smb2CreateContextResponse[] servercreatecontexts)
        {
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
