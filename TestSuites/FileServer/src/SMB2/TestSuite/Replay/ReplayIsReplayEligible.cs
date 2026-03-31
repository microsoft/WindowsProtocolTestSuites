// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.Replay
{
    /// <summary>
    /// Tests for the IsReplayEligible condition change in MS-SMB2 sections 3.3.5.11 through 3.3.5.21.
    ///
    /// Spec change: The condition for clearing Open.IsReplayEligible changed from
    ///   "If Open.IsPersistent is FALSE and Open.IsReplayEligible is TRUE"
    /// to
    ///   "If server implements SMB 3.x dialect family and Open.IsReplayEligible is TRUE"
    ///
    /// This means persistent handles on SMB 3.x connections now also have their replay
    /// eligibility cleared after the first successful replay.
    /// </summary>
    [TestClass]
    public class ReplayIsReplayEligible : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client;
        private string fileName;
        private Packet_Header receivedChangeNotifyHeader;
        private CHANGE_NOTIFY_Response receivedChangeNotify;
        private FILE_NOTIFY_INFORMATION[] receivedFileNotifyInfo;
        private AutoResetEvent changeNotificationReceived = new AutoResetEvent(false);
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

        protected override void TestInitialize()
        {
            base.TestInitialize();
            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
        }

        protected override void TestCleanup()
        {
            if (client != null)
            {
                try { client.Disconnect(); }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Disconnect exception: {0}", ex.ToString());
                }
            }
            changeNotificationReceived?.Dispose();
            base.TestCleanup();
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Connect to the CA share and create a file with a persistent DurableHandleV2.
        /// </summary>
        private void SetupPersistentHandle(out uint treeId, out FILEID fileId, CreateOptions_Values createOptions = CreateOptions_Values.FILE_NON_DIRECTORY_FILE)
        {
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);
            BaseTestSite.Assume.IsTrue(TestConfig.IsPersistentHandlesSupported, "Test requires persistent handle support.");

            Guid clientGuid = Guid.NewGuid();
            Capabilities_Values capabilities = Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES |
                Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU;

            string caSharePath = Smb2Utility.GetUncPath(TestConfig.CAShareServerName, TestConfig.CAShareName);
            bool isDirectory = createOptions.HasFlag(CreateOptions_Values.FILE_DIRECTORY_FILE);
            fileName = isDirectory
                ? string.Format("ReplayEligible_{0}", Guid.NewGuid())
                : string.Format("ReplayEligible_{0}.txt", Guid.NewGuid());

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to CA share and negotiate SMB 3.x.");
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.CAShareServerName, TestConfig.CAShareServerIP);
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: capabilities,
                clientGuid: clientGuid);
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.CAShareServerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            client.TreeConnect(caSharePath, out treeId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create file with persistent DurableHandleV2.");
            Smb2CreateContextResponse[] createContextResponse;
            client.Create(
                treeId,
                fileName,
                createOptions,
                out fileId,
                out createContextResponse,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[]
                {
                    new Smb2CreateDurableHandleRequestV2
                    {
                        CreateGuid = Guid.NewGuid(),
                        Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                    }
                },
                shareAccess: ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE);

            CheckCreateContextResponses(createContextResponse,
                new DefaultDurableHandleV2ResponseChecker(BaseTestSite,
                    CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags.DHANDLE_FLAG_PERSISTENT, uint.MaxValue));
        }

        /// <summary>
        /// Connect to the basic share and create a file with a non-persistent DurableHandleV2.
        /// Uses Batch oplock for files; uses a lease with RH caching for directories
        /// (servers do not grant batch oplocks on directory opens per MS-SMB2 3.3.5.9.10).
        /// </summary>
        private void SetupNonPersistentHandle(out uint treeId, out FILEID fileId, CreateOptions_Values createOptions = CreateOptions_Values.FILE_NON_DIRECTORY_FILE)
        {
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);

            Guid clientGuid = Guid.NewGuid();
            bool isDirectory = createOptions.HasFlag(CreateOptions_Values.FILE_DIRECTORY_FILE);
            Capabilities_Values capabilities = Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU;
            if (isDirectory)
            {
                TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING);
                capabilities |= Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING;
            }

            string sharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            fileName = isDirectory
                ? string.Format("ReplayEligible_{0}", Guid.NewGuid())
                : string.Format("ReplayEligible_{0}.txt", Guid.NewGuid());

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to basic share and negotiate SMB 3.x.");
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: capabilities,
                clientGuid: clientGuid);
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            client.TreeConnect(sharePath, out treeId);

            Smb2CreateContextResponse[] createContextResponse;
            if (isDirectory)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create directory with non-persistent DurableHandleV2 and RH lease.");
                Guid leaseKey = Guid.NewGuid();
                client.Create(
                    treeId,
                    fileName,
                    createOptions,
                    out fileId,
                    out createContextResponse,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                    new Smb2CreateContextRequest[]
                    {
                        new Smb2CreateDurableHandleRequestV2
                        {
                            CreateGuid = Guid.NewGuid(),
                        },
                        new Smb2CreateRequestLeaseV2
                        {
                            LeaseKey = leaseKey,
                            LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING,
                        }
                    },
                    shareAccess: ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE);

                // Per MS-SMB2 3.3.5.9.7, the server only grants DurableHandleV2 for non-persistent opens
                // when the lease includes handle caching. Verify handle caching was granted.
                bool handleCachingGranted = false;
                if (createContextResponse != null)
                {
                    foreach (var ctx in createContextResponse)
                    {
                        if (ctx is Smb2CreateResponseLeaseV2 leaseResponse)
                        {
                            handleCachingGranted = leaseResponse.LeaseState.HasFlag(LeaseStateValues.SMB2_LEASE_HANDLE_CACHING);
                            break;
                        }
                    }
                }
                BaseTestSite.Assume.IsTrue(handleCachingGranted,
                    "Server must grant handle caching in the lease for non-persistent DurableHandleV2 on directory opens. " +
                    "If the server does not grant handle caching, DurableHandleV2 cannot be established.");
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create file with non-persistent DurableHandleV2 and Batch oplock.");
                client.Create(
                    treeId,
                    fileName,
                    createOptions,
                    out fileId,
                    out createContextResponse,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                    new Smb2CreateContextRequest[]
                    {
                        new Smb2CreateDurableHandleRequestV2
                        {
                            CreateGuid = Guid.NewGuid(),
                        }
                    },
                    shareAccess: ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE);
            }

            if (isDirectory)
            {
                // Log what the server actually returned for diagnostics.
                if (createContextResponse == null || createContextResponse.Length == 0)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Warning, "Server returned no create context responses for directory open.");
                }
                else
                {
                    foreach (var ctx in createContextResponse)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "Create context response: {0}", ctx.GetType().Name);
                        if (ctx is Smb2CreateResponseLeaseV2 leaseV2)
                        {
                            BaseTestSite.Log.Add(LogEntryKind.Debug,
                                "LeaseV2 response — LeaseState: {0}, LeaseFlags: {1}",
                                leaseV2.LeaseState, leaseV2.Flags);
                        }
                    }
                }

                // Not all servers grant DurableHandleV2 on directory opens with RH lease.
                // Skip the test as inconclusive if the server did not grant it.
                bool hasDurableHandleV2 = createContextResponse != null &&
                    Array.Exists(createContextResponse, r => r is Smb2CreateDurableHandleResponseV2);
                BaseTestSite.Assume.IsTrue(hasDurableHandleV2,
                    "Server must grant non-persistent DurableHandleV2 on directory open with RH lease. " +
                    "Test requires DurableHandleV2 to set Open.IsReplayEligible.");
            }

            CheckCreateContextResponses(createContextResponse,
                new DefaultDurableHandleV2ResponseChecker(BaseTestSite, 0, uint.MaxValue));
        }

        private void OnChangeNotifyResponseReceived(FILE_NOTIFY_INFORMATION[] fileNotifyInfo, Packet_Header respHeader, CHANGE_NOTIFY_Response changeNotify)
        {
            BaseTestSite.Log.Add(LogEntryKind.Debug, "CHANGE_NOTIFY response received, status: {0}", Smb2Status.GetStatusCode(respHeader.Status));
            receivedChangeNotifyHeader = respHeader;
            receivedChangeNotify = changeNotify;
            receivedFileNotifyInfo = fileNotifyInfo;
            changeNotificationReceived.Set();
        }

        /// <summary>
        /// Create a new file inside the specified directory to trigger a CHANGE_NOTIFY response.
        /// </summary>
        private void TriggerFileChangeInDirectory(uint treeId, string directoryName)
        {
            string newFilePath = directoryName + "\\" + Guid.NewGuid().ToString();
            FILEID newFileId;
            Smb2CreateContextResponse[] ctx;
            client.Create(treeId, newFilePath, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out newFileId, out ctx);
            client.Close(treeId, newFileId);
        }

        #endregion

        #region Persistent Handle Tests

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [TestCategory(TestCategories.PersistentHandle)]
        [Description("Verify that after a successful replay of SMB2 WRITE on a persistent handle " +
            "over SMB 3.x, Open.IsReplayEligible is set to FALSE (MS-SMB2 section 3.3.5.13).")]
        public void Replay_PersistentHandle_Write_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupPersistentHandle(out treeId, out fileId);

            string originalContent = "OriginalContent_";
            string modifiedContent = "ModifiedContent_";

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. WRITE original content to file.");
            client.Write(treeId, fileId, originalContent);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay WRITE (same content) — should return cached response.");
            client.Write(treeId, fileId, originalContent, isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay WRITE (different content) — IsReplayEligible should be FALSE, " +
                "so server processes as new request.");
            client.Write(treeId, fileId, modifiedContent, isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. READ back file content to verify the second write was processed.");
            string readContent;
            client.Read(treeId, fileId, 0, (uint)modifiedContent.Length, out readContent);

            BaseTestSite.Assert.AreEqual(
                modifiedContent,
                readContent,
                "[MS-SMB2] 3.3.5.13: On SMB 3.x, after first successful replay sets Open.IsReplayEligible to FALSE, " +
                "the second replay WRITE should be processed as a new request. " +
                "Expected file content '{0}', actual '{1}'.",
                modifiedContent, readContent);

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [TestCategory(TestCategories.PersistentHandle)]
        [Description("Verify that replay of SMB2 READ on a persistent handle over SMB 3.x " +
            "clears Open.IsReplayEligible (MS-SMB2 section 3.3.5.12).")]
        public void Replay_PersistentHandle_Read_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupPersistentHandle(out treeId, out fileId);

            string firstHalf = "FirstHalfOfData!";
            string secondHalf = "SecondPartData!!";
            client.Write(treeId, fileId, firstHalf + secondHalf);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. READ first 16 bytes from offset 0.");
            string data;
            client.Read(treeId, fileId, 0, 16, out data);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay READ (offset 0) — cached response.");
            client.Read(treeId, fileId, 0, 16, out data, isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay READ (offset 16) — IsReplayEligible cleared, processed as new request.");
            client.Read(treeId, fileId, 16, 16, out data, isReplay: true);

            BaseTestSite.Assert.AreEqual(
                secondHalf,
                data,
                "[MS-SMB2] 3.3.5.12: On SMB 3.x, after first replay clears IsReplayEligible, " +
                "second replay READ with different offset should return data from the new offset, " +
                "not the cached response from offset 0.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [TestCategory(TestCategories.PersistentHandle)]
        [Description("Verify that replay of SMB2 FLUSH on a persistent handle over SMB 3.x " +
            "clears Open.IsReplayEligible (MS-SMB2 section 3.3.5.11).")]
        public void Replay_PersistentHandle_Flush_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupPersistentHandle(out treeId, out fileId);

            client.Write(treeId, fileId, "FlushTestData");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. FLUSH file.");
            client.Flush(treeId, fileId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay FLUSH — cached response.");
            client.Flush(treeId, fileId, isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay FLUSH — IsReplayEligible cleared, processed as new request.");
            uint status = client.Flush(treeId, fileId, checker: (header, response) => { }, isReplay: true);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "[MS-SMB2] 3.3.5.11: Server should process the second replay FLUSH successfully.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [TestCategory(TestCategories.PersistentHandle)]
        [Description("Verify that replay of SMB2 IOCTL on a persistent handle over SMB 3.x " +
            "clears Open.IsReplayEligible (MS-SMB2 section 3.3.5.15).")]
        public void Replay_PersistentHandle_IoCtl_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupPersistentHandle(out treeId, out fileId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Send IOCTL (FSCTL_LMR_REQUEST_RESILIENCY).");
            Packet_Header ioCtlHeader;
            IOCTL_Response ioCtlResponse;
            byte[] inputInResponse;
            byte[] outputInResponse;
            client.ResiliencyRequest(
                treeId, fileId, 0,
                (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                out ioCtlHeader, out ioCtlResponse,
                out inputInResponse, out outputInResponse);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay IOCTL — cached response.");
            client.ResiliencyRequest(
                treeId, fileId, 0,
                (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                out ioCtlHeader, out ioCtlResponse,
                out inputInResponse, out outputInResponse,
                isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay IOCTL — IsReplayEligible cleared, processed as new request.");
            uint status = client.ResiliencyRequest(
                treeId, fileId, 0,
                (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                out ioCtlHeader, out ioCtlResponse,
                out inputInResponse, out outputInResponse,
                checker: (header, response) => { },
                isReplay: true);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "[MS-SMB2] 3.3.5.15: Server should process the second replay IOCTL successfully.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [TestCategory(TestCategories.PersistentHandle)]
        [Description("Verify that replay of SMB2 SET_INFO on a persistent handle over SMB 3.x " +
            "clears Open.IsReplayEligible (MS-SMB2 section 3.3.5.21).")]
        public void Replay_PersistentHandle_SetInfo_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupPersistentHandle(out treeId, out fileId);

            FileEndOfFileInformation endOfFile = new FileEndOfFileInformation();
            endOfFile.EndOfFile = 1024;
            byte[] inputBuffer = TypeMarshal.ToBytes<FileEndOfFileInformation>(endOfFile);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. SET_INFO (EndOfFile = 1024).");
            client.SetFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileEndOfFileInformation,
                fileId,
                inputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay SET_INFO — cached response.");
            client.SetFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileEndOfFileInformation,
                fileId,
                inputBuffer,
                isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay SET_INFO — IsReplayEligible cleared, processed as new request.");
            endOfFile.EndOfFile = 2048;
            byte[] newInputBuffer = TypeMarshal.ToBytes<FileEndOfFileInformation>(endOfFile);
            uint status = client.SetFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileEndOfFileInformation,
                fileId,
                newInputBuffer,
                checker: (header, response) => { },
                isReplay: true);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "[MS-SMB2] 3.3.5.21: Server should process the second replay SET_INFO successfully.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "4. QUERY_INFO FileStandardInformation to verify EndOfFile was updated to 2048.");
            byte[] queryBuffer;
            client.QueryFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileStandardInformation,
                QUERY_INFO_Request_Flags_Values.V1,
                fileId,
                new byte[0],
                out queryBuffer);
            FileStandardInformation stdInfo = TypeMarshal.ToStruct<FileStandardInformation>(queryBuffer);
            BaseTestSite.Assert.AreEqual(
                2048L,
                stdInfo.EndOfFile,
                "[MS-SMB2] 3.3.5.21: EndOfFile should be 2048, confirming the second replay " +
                "SET_INFO was processed as a new request, not returned from cache.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [TestCategory(TestCategories.PersistentHandle)]
        [Description("Verify that SMB2 LOCK on a persistent handle over SMB 3.x " +
            "clears Open.IsReplayEligible and the lock sequence idempotency " +
            "mechanism works correctly (MS-SMB2 section 3.3.5.14).")]
        public void Replay_PersistentHandle_Lock_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupPersistentHandle(out treeId, out fileId);

            // Use a valid non-zero lock sequence so the server's lock sequence
            // idempotency mechanism recognizes replays.
            // Lock sequence format per MS-SMB2 3.3.5.14:
            //   LockSequenceIndex = LockSequence >> 4  (valid range: 1-64)
            //   LockSequenceNumber = LockSequence & 0xF (lower 4 bits)
            uint lockSequence = (1u << 4) | 1u; // LockSequenceIndex=1, LockSequenceNumber=1

            LOCK_ELEMENT[] locks = new LOCK_ELEMENT[1];
            locks[0].Offset = 0;
            locks[0].Length = 1024;
            locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_EXCLUSIVE_LOCK | LOCK_ELEMENT_Flags_Values.LOCKFLAG_FAIL_IMMEDIATELY;

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "1. LOCK byte range [0, 1024) with exclusive lock. " +
                "Per MS-SMB2 3.3.5.14, this clears Open.IsReplayEligible on SMB 3.x.");
            client.Lock(treeId, lockSequence, fileId, locks);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay LOCK with new lock sequence — IsReplayEligible cleared, processed as new request.");
            uint newLockSequence = (2u << 4) | 1u; // LockSequenceIndex=2, LockSequenceNumber=1
            uint status = client.Lock(treeId, newLockSequence, fileId, locks, checker: (header, response) => { }, isReplay: true);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_LOCK_NOT_GRANTED,
                status,
                "[MS-SMB2] 3.3.5.14: A new lock request on an already-locked range " +
                "should fail with STATUS_LOCK_NOT_GRANTED, confirming the lock was acquired.");

            // Unlock before cleanup
            LOCK_ELEMENT[] unlocks = new LOCK_ELEMENT[1];
            unlocks[0].Offset = 0;
            unlocks[0].Length = 1024;
            unlocks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK;
            client.Lock(treeId, (3u << 4) | 1u, fileId, unlocks, checker: (header, response) => { });

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [TestCategory(TestCategories.PersistentHandle)]
        [Description("Verify that SMB2 QUERY_DIRECTORY on a persistent handle over SMB 3.x " +
            "clears Open.IsReplayEligible and subsequent queries work correctly " +
            "(MS-SMB2 section 3.3.5.18).")]
        public void Replay_PersistentHandle_QueryDirectory_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupPersistentHandle(out treeId, out fileId, CreateOptions_Values.FILE_DIRECTORY_FILE);

            // QUERY_DIRECTORY does not support cached response replay.
            // Per MS-SMB2 3.3.5.18, it clears IsReplayEligible and then processes
            // the query normally (advancing the enumeration cursor).

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "1. QUERY_DIRECTORY on directory. " +
                "Per MS-SMB2 3.3.5.18, this clears Open.IsReplayEligible on SMB 3.x.");
            byte[] outputBuffer;
            client.QueryDirectory(
                treeId,
                FileInformationClass_Values.FileIdBothDirectoryInformation,
                QUERY_DIRECTORY_Request_Flags_Values.NONE,
                0,
                fileId,
                out outputBuffer);
            int originalOutputLength = outputBuffer.Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "2. Create a file in the directory to change directory contents.");
            TriggerFileChangeInDirectory(treeId, fileName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. QUERY_DIRECTORY with RESTART_SCANS — should return updated listing " +
                "including the newly created file.");
            client.QueryDirectory(
                treeId,
                FileInformationClass_Values.FileIdBothDirectoryInformation,
                QUERY_DIRECTORY_Request_Flags_Values.RESTART_SCANS,
                0,
                fileId,
                out outputBuffer);

            BaseTestSite.Assert.IsTrue(
                outputBuffer.Length > originalOutputLength,
                "[MS-SMB2] 3.3.5.18: After IsReplayEligible is cleared, subsequent " +
                "QUERY_DIRECTORY with RESTART_SCANS should return updated directory listing. " +
                "Original output: {0} bytes, Updated output: {1} bytes.",
                originalOutputLength, outputBuffer.Length);

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [TestCategory(TestCategories.PersistentHandle)]
        [Description("Verify that replay of SMB2 QUERY_INFO on a persistent handle over SMB 3.x " +
            "clears Open.IsReplayEligible (MS-SMB2 section 3.3.5.20).")]
        public void Replay_PersistentHandle_QueryInfo_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupPersistentHandle(out treeId, out fileId);

            client.Write(treeId, fileId, "QueryInfoTestData");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. QUERY_INFO (FileBasicInformation).");
            byte[] outputBuffer;
            client.QueryFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileBasicInformation,
                QUERY_INFO_Request_Flags_Values.V1,
                fileId,
                new byte[0],
                out outputBuffer);
            int cachedResponseLength = outputBuffer.Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay QUERY_INFO (FileBasicInformation) — cached response.");
            client.QueryFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileBasicInformation,
                QUERY_INFO_Request_Flags_Values.V1,
                fileId,
                new byte[0],
                out outputBuffer,
                isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay QUERY_INFO (FileStandardInformation) — IsReplayEligible cleared, processed as new request.");
            uint status = client.QueryFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileStandardInformation,
                QUERY_INFO_Request_Flags_Values.V1,
                fileId,
                new byte[0],
                out outputBuffer,
                checker: (header, response) => { },
                isReplay: true);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "[MS-SMB2] 3.3.5.20: Server should process the second replay QUERY_INFO successfully.");
            BaseTestSite.Assert.AreNotEqual(
                cachedResponseLength,
                outputBuffer.Length,
                "[MS-SMB2] 3.3.5.20: Second replay with FileStandardInformation should return " +
                "a different-sized response than the cached FileBasicInformation ({0} bytes), " +
                "confirming it was processed as a new request. Actual: {1} bytes.",
                cachedResponseLength, outputBuffer.Length);

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [TestCategory(TestCategories.PersistentHandle)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("Verify that SMB2 CHANGE_NOTIFY on a persistent handle over SMB 3.x " +
            "clears Open.IsReplayEligible and subsequent watches work correctly " +
            "(MS-SMB2 section 3.3.5.19).")]
        public void Replay_PersistentHandle_ChangeNotify_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupPersistentHandle(out treeId, out fileId, CreateOptions_Values.FILE_DIRECTORY_FILE);

            // CHANGE_NOTIFY is async and does not support cached response replay.
            // Per MS-SMB2 3.3.5.19, it clears IsReplayEligible and then registers
            // the watch normally. Verify the open remains functional for watches
            // after IsReplayEligible is cleared.

            client.Smb2Client.ChangeNotifyResponseReceived +=
                new Action<FILE_NOTIFY_INFORMATION[], Packet_Header, CHANGE_NOTIFY_Response>(OnChangeNotifyResponseReceived);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "1. Register CHANGE_NOTIFY watch on directory. " +
                "Per MS-SMB2 3.3.5.19, this clears Open.IsReplayEligible on SMB 3.x.");
            client.ChangeNotify(treeId, fileId, CompletionFilter_Values.FILE_NOTIFY_CHANGE_FILE_NAME,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Trigger filesystem change to complete the first watch.");
            TriggerFileChangeInDirectory(treeId, fileName);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "First CHANGE_NOTIFY response should be received within {0} milliseconds.", TestConfig.WaitTimeoutInMilliseconds);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "First CHANGE_NOTIFY should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Register a second CHANGE_NOTIFY watch (IsReplayEligible is now FALSE).");
            changeNotificationReceived.Reset();
            client.ChangeNotify(treeId, fileId, CompletionFilter_Values.FILE_NOTIFY_CHANGE_FILE_NAME,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "4. Trigger another filesystem change to complete the second watch.");
            TriggerFileChangeInDirectory(treeId, fileName);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "[MS-SMB2] 3.3.5.19: After IsReplayEligible is cleared, subsequent " +
                "CHANGE_NOTIFY should still register watches and complete successfully " +
                "within {0} milliseconds.", TestConfig.WaitTimeoutInMilliseconds);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "[MS-SMB2] 3.3.5.19: Second CHANGE_NOTIFY watch should complete with STATUS_SUCCESS.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        #endregion

        #region Non-Persistent Handle Tests

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [Description("Verify that after a successful replay of SMB2 WRITE on a non-persistent " +
            "DurableHandleV2 over SMB 3.x, Open.IsReplayEligible is set to FALSE (MS-SMB2 section 3.3.5.13).")]
        public void Replay_NonPersistentHandle_Write_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupNonPersistentHandle(out treeId, out fileId);

            string originalContent = "OriginalContent_";
            string modifiedContent = "ModifiedContent_";

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. WRITE original content to file.");
            client.Write(treeId, fileId, originalContent);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay WRITE — cached response.");
            client.Write(treeId, fileId, originalContent, isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay WRITE — IsReplayEligible cleared, processed as new request.");
            client.Write(treeId, fileId, modifiedContent, isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. READ back to verify second write was processed.");
            string readContent;
            client.Read(treeId, fileId, 0, (uint)modifiedContent.Length, out readContent);

            BaseTestSite.Assert.AreEqual(
                modifiedContent,
                readContent,
                "[MS-SMB2] 3.3.5.13: On SMB 3.x with non-persistent handle, after first replay " +
                "clears IsReplayEligible, second replay WRITE should be processed as new request.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [Description("Verify that replay of SMB2 READ on a non-persistent DurableHandleV2 " +
            "over SMB 3.x clears Open.IsReplayEligible (MS-SMB2 section 3.3.5.12).")]
        public void Replay_NonPersistentHandle_Read_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupNonPersistentHandle(out treeId, out fileId);

            string firstHalf = "FirstHalfOfData!";
            string secondHalf = "SecondPartData!!";
            client.Write(treeId, fileId, firstHalf + secondHalf);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. READ first 16 bytes from offset 0.");
            string data;
            client.Read(treeId, fileId, 0, 16, out data);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay READ (offset 0) — cached response.");
            client.Read(treeId, fileId, 0, 16, out data, isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay READ (offset 16) — IsReplayEligible cleared, processed as new request.");
            client.Read(treeId, fileId, 16, 16, out data, isReplay: true);

            BaseTestSite.Assert.AreEqual(
                secondHalf,
                data,
                "[MS-SMB2] 3.3.5.12: On SMB 3.x, after first replay clears IsReplayEligible, " +
                "second replay READ with different offset should return data from the new offset, " +
                "not the cached response from offset 0.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [Description("Verify that replay of SMB2 FLUSH on a non-persistent DurableHandleV2 " +
            "over SMB 3.x clears Open.IsReplayEligible (MS-SMB2 section 3.3.5.11).")]
        public void Replay_NonPersistentHandle_Flush_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupNonPersistentHandle(out treeId, out fileId);

            client.Write(treeId, fileId, "FlushTestData");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. FLUSH file.");
            client.Flush(treeId, fileId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay FLUSH — cached response.");
            client.Flush(treeId, fileId, isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay FLUSH — IsReplayEligible cleared, processed as new request.");
            uint status = client.Flush(treeId, fileId, checker: (header, response) => { }, isReplay: true);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "[MS-SMB2] 3.3.5.11: Server should process the second replay FLUSH successfully.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [Description("Verify that replay of SMB2 IOCTL on a non-persistent DurableHandleV2 " +
            "over SMB 3.x clears Open.IsReplayEligible (MS-SMB2 section 3.3.5.15).")]
        public void Replay_NonPersistentHandle_IoCtl_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupNonPersistentHandle(out treeId, out fileId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. IOCTL (FSCTL_LMR_REQUEST_RESILIENCY).");
            Packet_Header ioCtlHeader;
            IOCTL_Response ioCtlResponse;
            byte[] inputInResponse;
            byte[] outputInResponse;
            client.ResiliencyRequest(
                treeId, fileId, 0,
                (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                out ioCtlHeader, out ioCtlResponse,
                out inputInResponse, out outputInResponse);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay IOCTL — cached response.");
            client.ResiliencyRequest(
                treeId, fileId, 0,
                (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                out ioCtlHeader, out ioCtlResponse,
                out inputInResponse, out outputInResponse,
                isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay IOCTL — IsReplayEligible cleared, processed as new request.");
            uint status = client.ResiliencyRequest(
                treeId, fileId, 0,
                (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                out ioCtlHeader, out ioCtlResponse,
                out inputInResponse, out outputInResponse,
                checker: (header, response) => { },
                isReplay: true);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "[MS-SMB2] 3.3.5.15: Server should process the second replay IOCTL successfully.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [Description("Verify that replay of SMB2 SET_INFO on a non-persistent DurableHandleV2 " +
            "over SMB 3.x clears Open.IsReplayEligible (MS-SMB2 section 3.3.5.21).")]
        public void Replay_NonPersistentHandle_SetInfo_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupNonPersistentHandle(out treeId, out fileId);

            FileEndOfFileInformation endOfFile = new FileEndOfFileInformation();
            endOfFile.EndOfFile = 1024;
            byte[] inputBuffer = TypeMarshal.ToBytes<FileEndOfFileInformation>(endOfFile);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. SET_INFO (EndOfFile = 1024).");
            client.SetFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileEndOfFileInformation,
                fileId,
                inputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay SET_INFO — cached response.");
            client.SetFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileEndOfFileInformation,
                fileId,
                inputBuffer,
                isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay SET_INFO — IsReplayEligible cleared, processed as new request.");
            endOfFile.EndOfFile = 2048;
            byte[] newInputBuffer = TypeMarshal.ToBytes<FileEndOfFileInformation>(endOfFile);
            uint status = client.SetFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileEndOfFileInformation,
                fileId,
                newInputBuffer,
                checker: (header, response) => { },
                isReplay: true);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "[MS-SMB2] 3.3.5.21: Server should process the second replay SET_INFO successfully.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "4. QUERY_INFO FileStandardInformation to verify EndOfFile was updated to 2048.");
            byte[] queryBuffer;
            client.QueryFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileStandardInformation,
                QUERY_INFO_Request_Flags_Values.V1,
                fileId,
                new byte[0],
                out queryBuffer);
            FileStandardInformation stdInfo = TypeMarshal.ToStruct<FileStandardInformation>(queryBuffer);
            BaseTestSite.Assert.AreEqual(
                2048L,
                stdInfo.EndOfFile,
                "[MS-SMB2] 3.3.5.21: EndOfFile should be 2048, confirming the second replay " +
                "SET_INFO was processed as a new request, not returned from cache.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [Description("Verify that SMB2 LOCK on a non-persistent DurableHandleV2 " +
            "over SMB 3.x clears Open.IsReplayEligible and lock sequence idempotency " +
            "works correctly (MS-SMB2 section 3.3.5.14).")]
        public void Replay_NonPersistentHandle_Lock_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupNonPersistentHandle(out treeId, out fileId);

            uint lockSequence = (1u << 28) | 1u; // BucketIndex=1, SequenceNumber=1

            LOCK_ELEMENT[] locks = new LOCK_ELEMENT[1];
            locks[0].Offset = 0;
            locks[0].Length = 1024;
            locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_EXCLUSIVE_LOCK | LOCK_ELEMENT_Flags_Values.LOCKFLAG_FAIL_IMMEDIATELY;

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "1. LOCK byte range [0, 1024) with exclusive lock. " +
                "Per MS-SMB2 3.3.5.14, this clears Open.IsReplayEligible on SMB 3.x.");
            client.Lock(treeId, lockSequence, fileId, locks);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay LOCK with new lock sequence — IsReplayEligible cleared, processed as new request.");
            uint newLockSequence = (2u << 4) | 1u; // LockSequenceIndex=2, LockSequenceNumber=1
            uint status = client.Lock(treeId, newLockSequence, fileId, locks, checker: (header, response) => { }, isReplay: true);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_LOCK_NOT_GRANTED,
                status,
                "[MS-SMB2] 3.3.5.14: A new lock request on an already-locked range " +
                "should fail with STATUS_LOCK_NOT_GRANTED, confirming the lock was acquired.");

            // Unlock before cleanup
            LOCK_ELEMENT[] unlocks = new LOCK_ELEMENT[1];
            unlocks[0].Offset = 0;
            unlocks[0].Length = 1024;
            unlocks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK;
            client.Lock(treeId, (3u << 4) | 1u, fileId, unlocks, checker: (header, response) => { });

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [Description("Verify that SMB2 QUERY_DIRECTORY on a non-persistent DurableHandleV2 " +
            "over SMB 3.x clears Open.IsReplayEligible and subsequent queries work correctly " +
            "(MS-SMB2 section 3.3.5.18).")]
        public void Replay_NonPersistentHandle_QueryDirectory_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupNonPersistentHandle(out treeId, out fileId, CreateOptions_Values.FILE_DIRECTORY_FILE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "1. QUERY_DIRECTORY on directory. " +
                "Per MS-SMB2 3.3.5.18, this clears Open.IsReplayEligible on SMB 3.x.");
            byte[] outputBuffer;
            client.QueryDirectory(
                treeId,
                FileInformationClass_Values.FileIdBothDirectoryInformation,
                QUERY_DIRECTORY_Request_Flags_Values.NONE,
                0,
                fileId,
                out outputBuffer);
            int originalOutputLength = outputBuffer.Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "2. Create a file in the directory to change directory contents.");
            TriggerFileChangeInDirectory(treeId, fileName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. QUERY_DIRECTORY with RESTART_SCANS — should return updated listing.");
            client.QueryDirectory(
                treeId,
                FileInformationClass_Values.FileIdBothDirectoryInformation,
                QUERY_DIRECTORY_Request_Flags_Values.RESTART_SCANS,
                0,
                fileId,
                out outputBuffer);

            BaseTestSite.Assert.IsTrue(
                outputBuffer.Length > originalOutputLength,
                "[MS-SMB2] 3.3.5.18: After IsReplayEligible is cleared, subsequent " +
                "QUERY_DIRECTORY with RESTART_SCANS should return updated directory listing. " +
                "Original output: {0} bytes, Updated output: {1} bytes.",
                originalOutputLength, outputBuffer.Length);

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [Description("Verify that replay of SMB2 QUERY_INFO on a non-persistent DurableHandleV2 " +
            "over SMB 3.x clears Open.IsReplayEligible (MS-SMB2 section 3.3.5.20).")]
        public void Replay_NonPersistentHandle_QueryInfo_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupNonPersistentHandle(out treeId, out fileId);

            client.Write(treeId, fileId, "QueryInfoTestData");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. QUERY_INFO (FileBasicInformation).");
            byte[] outputBuffer;
            client.QueryFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileBasicInformation,
                QUERY_INFO_Request_Flags_Values.V1,
                fileId,
                new byte[0],
                out outputBuffer);
            int cachedResponseLength = outputBuffer.Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. First replay QUERY_INFO (FileBasicInformation) — cached response.");
            client.QueryFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileBasicInformation,
                QUERY_INFO_Request_Flags_Values.V1,
                fileId,
                new byte[0],
                out outputBuffer,
                isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Second replay QUERY_INFO (FileStandardInformation) — IsReplayEligible cleared, processed as new request.");
            uint status = client.QueryFileAttributes(
                treeId,
                (byte)FileInformationClasses.FileStandardInformation,
                QUERY_INFO_Request_Flags_Values.V1,
                fileId,
                new byte[0],
                out outputBuffer,
                checker: (header, response) => { },
                isReplay: true);

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "[MS-SMB2] 3.3.5.20: Server should process the second replay QUERY_INFO successfully.");
            BaseTestSite.Assert.AreNotEqual(
                cachedResponseLength,
                outputBuffer.Length,
                "[MS-SMB2] 3.3.5.20: Second replay with FileStandardInformation should return " +
                "a different-sized response than the cached FileBasicInformation ({0} bytes), " +
                "confirming it was processed as a new request. Actual: {1} bytes.",
                cachedResponseLength, outputBuffer.Length);

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Replay)]
        [TestCategory(TestCategories.ChangeNotify)]
        [Description("Verify that SMB2 CHANGE_NOTIFY on a non-persistent DurableHandleV2 " +
            "over SMB 3.x clears Open.IsReplayEligible and subsequent watches work correctly " +
            "(MS-SMB2 section 3.3.5.19).")]
        public void Replay_NonPersistentHandle_ChangeNotify_ReplayEligibleCleared()
        {
            uint treeId;
            FILEID fileId;
            SetupNonPersistentHandle(out treeId, out fileId, CreateOptions_Values.FILE_DIRECTORY_FILE);

            client.Smb2Client.ChangeNotifyResponseReceived +=
                new Action<FILE_NOTIFY_INFORMATION[], Packet_Header, CHANGE_NOTIFY_Response>(OnChangeNotifyResponseReceived);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "1. Register CHANGE_NOTIFY watch on directory. " +
                "Per MS-SMB2 3.3.5.19, this clears Open.IsReplayEligible on SMB 3.x.");
            client.ChangeNotify(treeId, fileId, CompletionFilter_Values.FILE_NOTIFY_CHANGE_FILE_NAME,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Trigger filesystem change to complete the first watch.");
            TriggerFileChangeInDirectory(treeId, fileName);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "First CHANGE_NOTIFY response should be received within {0} milliseconds.", TestConfig.WaitTimeoutInMilliseconds);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "First CHANGE_NOTIFY should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "3. Register a second CHANGE_NOTIFY watch (IsReplayEligible is now FALSE).");
            changeNotificationReceived.Reset();
            client.ChangeNotify(treeId, fileId, CompletionFilter_Values.FILE_NOTIFY_CHANGE_FILE_NAME,
                flags: CHANGE_NOTIFY_Request_Flags_Values.WATCH_TREE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "4. Trigger another filesystem change to complete the second watch.");
            TriggerFileChangeInDirectory(treeId, fileName);

            BaseTestSite.Assert.IsTrue(
                changeNotificationReceived.WaitOne(TestConfig.WaitTimeoutInMilliseconds),
                "[MS-SMB2] 3.3.5.19: After IsReplayEligible is cleared, subsequent " +
                "CHANGE_NOTIFY should still register watches and complete successfully " +
                "within {0} milliseconds.", TestConfig.WaitTimeoutInMilliseconds);
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                receivedChangeNotifyHeader.Status,
                "[MS-SMB2] 3.3.5.19: Second CHANGE_NOTIFY watch should complete with STATUS_SUCCESS.");

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        #endregion
    }
}
