// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public partial class AppInstanceIdExtendedTest : SMB2TestBase
    {
        #region Test Case
        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.AppInstanceId)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Operate file with lock during client failover and expect no lock is maintained after failover.")]
        public void AppInstanceId_Lock_ExpectNoLockInReOpen()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "InitialOpen: Connect to share via Nic1.");
            FILEID fileIdForInitialOpen;
            uint treeIdForInitialOpen;
            ConnectShare(TestConfig.SutIPAddress, TestConfig.ClientNic1IPAddress, clientForInitialOpen, out treeIdForInitialOpen);

            #region Create an open with AppInstanceId
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "InitialOpen: Create an open with AppInstanceId.");
            Guid appInstanceId = Guid.NewGuid();
            Smb2CreateContextResponse[] serverCreateContexts;
            status = clientForInitialOpen.Create(
                treeIdForInitialOpen,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdForInitialOpen,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = Guid.NewGuid(),
                         Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                    },
                    new Smb2CreateAppInstanceId
                    {
                         AppInstanceId = appInstanceId
                    }
                },
                accessMask: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE,
                shareAccess: ShareAccess_Values.NONE);
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "InitialOpen: Write contents to file.");
            status = clientForInitialOpen.Write(treeIdForInitialOpen, fileIdForInitialOpen, contentWrite);

            #region Request ByteRangeLock
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "InitialOpen: Request ByteRangeLock.");
            LOCK_ELEMENT[] locks = new LOCK_ELEMENT[1];
            uint lockSequence = 0;
            locks[0].Offset = 0;
            locks[0].Length = (ulong)TestConfig.WriteBufferLengthInKb * 1024;
            locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_SHARED_LOCK;

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Client starts to lock a byte range for file \"{0}\" with parameters offset:{1}, length:{2}, flags: {3}",
                fileName, locks[0].Offset, locks[0].Length, locks[0].Flags.ToString());
            status = clientForInitialOpen.Lock(treeIdForInitialOpen, lockSequence++, fileIdForInitialOpen, locks);
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "ReOpen: Connect to share via Nic2.");
            FILEID fileIdForReOpen;
            uint treeIdForReOpen;
            ConnectShare(TestConfig.SutIPAddress, TestConfig.ClientNic2IPAddress, clientForReOpen, out treeIdForReOpen);

            #region Create an open with same AppInstanceId
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "ReOpen: Create an open with same AppInstanceId.");
            status = clientForReOpen.Create(
                treeIdForReOpen,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdForReOpen,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = Guid.NewGuid(),
                         Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                    },
                    new Smb2CreateAppInstanceId
                    {
                        // Use the same application instance id to force the server close all files
                        // and will clear previous ByteRangeLock
                        AppInstanceId = appInstanceId
                    }
                },
                accessMask: AccessMask.GENERIC_READ);
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "ReOpen: Read the contents written by InitialOpen.");
            status = clientForReOpen.Read(treeIdForReOpen, fileIdForReOpen, 0, (uint)contentWrite.Length, out contentRead);

            BaseTestSite.Assert.IsTrue(
                contentRead.Equals(contentWrite),
                "The written content should equal to read content.");

            #region AfterFailover: Attempt to access same file with previous byte lock range
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "ReOpen: Attempt to access same file with previous byte lock range.");
            ValidateByteLockRangeFromAnotherClient(false);
            #endregion

            #region Client tear down
            BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Close file.");
            status = clientForReOpen.Close(treeIdForReOpen, fileIdForReOpen);

            BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Disconnect from the share.");
            status = clientForReOpen.TreeDisconnect(treeIdForReOpen);

            BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Log off.");
            status = clientForReOpen.LogOff();

            #endregion
        }
        #endregion

        #region Common Method
        /// <summary>
        /// Read and write file within byte lock range when the file is locked or unlocked
        /// </summary>
        /// <param name="isLocked">Set true to indicate that byte lock range is taken on the file</param>
        private void ValidateByteLockRangeFromAnotherClient(bool isLocked)
        {
            uint status = 0;

            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress, TestConfig.ClientNic2IPAddress);

            status = client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled);

            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            uint treeId;
            status = client.TreeConnect(uncSharePath, out treeId);

            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fileId;
            status = client.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);

            string data;
            Random random = new Random();
            uint offset = (uint)random.Next(0, TestConfig.WriteBufferLengthInKb * 1024 - 1);
            uint length = (uint)random.Next(0, (int)(TestConfig.WriteBufferLengthInKb * 1024 - offset));
            status = client.Read(treeId, fileId, offset, length, out data);

            status = client.Write(treeId, fileId, contentWrite, checker:(header, response) => { });
            if (isLocked)
            {
                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "Write content to locked range of file from different client is not expected to success");
                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_FILE_LOCK_CONFLICT,
                    status,
                    RequirementCategory.STATUS_FILE_LOCK_CONFLICT.Id,
                    RequirementCategory.STATUS_FILE_LOCK_CONFLICT.Description);
            }
            else
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "Write content in file should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            }

            status = client.Close(treeId, fileId);

            status = client.TreeDisconnect(treeId);

            status = client.LogOff();

            client.Disconnect();
        }
        #endregion
    }
}
