// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.TestSuite
{
    public partial class FileServerFailoverExtendedTest : ServerFailoverTestBase
    {
        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Failover)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate files with lock during failover and expect the lock is maintained after failover.")]
        public void FileServerFailover_Lock()
        {
            FILEID fileIdBeforeFailover;
            uint treeIdBeforeFailover;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "BeforeFailover: Connect to general file server {0}.", TestConfig.ClusteredFileServerName);
            ConnectGeneralFileServerBeforeFailover(TestConfig.ClusteredFileServerName, out treeIdBeforeFailover);            

            #region CREATE a durable open with flag DHANDLE_FLAG_PERSISTENT
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "BeforeFailover: CREATE a durable open with flag DHANDLE_FLAG_PERSISTENT.");
            Smb2CreateContextResponse[] serverCreateContexts;
            createGuid = Guid.NewGuid();

            status = clientBeforeFailover.Create(
                treeIdBeforeFailover,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdBeforeFailover,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                new Smb2CreateContextRequest[] { 
                    new Smb2CreateDurableHandleRequestV2
                    {
                         CreateGuid = createGuid,
                         Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                         Timeout = 3600000,
                    },
                    new Smb2CreateQueryOnDiskId
                    {
                    },
                });
            #endregion

            #region WRITE content to file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "BeforeFailover: WRITE content to file.");
            status = clientBeforeFailover.Write(treeIdBeforeFailover, fileIdBeforeFailover, contentWrite);
            #endregion

            #region Request byte range lock
            LOCK_ELEMENT[] locks = new LOCK_ELEMENT[1];
            uint lockSequence = 0;
            locks[0].Offset = 0;
            locks[0].Length = (ulong)TestConfig.WriteBufferLengthInKb * 1024;
            locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_SHARED_LOCK;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client1 start to lock a byte range for file \"{0}\" with parameters offset:{1}, length:{2}, flags: {3}",
                fileName, locks[0].Offset, locks[0].Length, locks[0].Flags.ToString());
            status = clientBeforeFailover.Lock(treeIdBeforeFailover, lockSequence++, fileIdBeforeFailover, locks);

            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Do failover of the file server.");
            FailoverServer(currentAccessIp, TestConfig.ClusteredFileServerName, FileServerType.GeneralFileServer);

            FILEID fileIdAfterFailover = FILEID.Zero;
            uint treeIdAfterFailover;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: Reconnect to the same general file server {0}.", TestConfig.ClusteredFileServerName);
            ReconnectServerAfterFailover(TestConfig.ClusteredFileServerName, FileServerType.GeneralFileServer, out treeIdAfterFailover);

            #region CREATE to reconnect previous duarable open with flag DHANDLE_FLAG_PERSISTENT
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: CREATE to reconnect previous duarable open with flag DHANDLE_FLAG_PERSISTENT.");
            status = DoUntilSucceed(
                () => clientAfterFailover.Create(
                    treeIdAfterFailover,
                    fileName,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    out fileIdAfterFailover,
                    out serverCreateContexts,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    new Smb2CreateContextRequest[] { 
                        new Smb2CreateDurableHandleReconnectV2
                        {
                            FileId = new FILEID { Persistent = fileIdBeforeFailover.Persistent },
                            CreateGuid = createGuid,
                            Flags = CREATE_DURABLE_HANDLE_RECONNECT_V2_Flags.DHANDLE_FLAG_PERSISTENT
                        },
                    },
                    checker: (header, response) => { }),
                TestConfig.FailoverTimeout,
                "Retry Create until succeed within timeout span");
            #endregion

            #region READ and WRITE
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: Read the contents written before failover.");
            status = clientAfterFailover.Read(
                treeIdAfterFailover,
                fileIdAfterFailover,
                0,
                (uint)contentWrite.Length,
                out contentRead);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: Verify the contents are the same as the one written before failover.");
            BaseTestSite.Assert.IsTrue(
                contentWrite.Equals(contentRead),
                "Content read after failover should be identical to that written before failover.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: Write contents to the locking range, it MUST NOT be allowed.");
            status = clientAfterFailover.Write(
                treeIdAfterFailover,
                fileIdAfterFailover,
                contentWrite,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "All opens MUST NOT be allowed to write within the range when SMB2_LOCKFLAG_SHARED_LOCK set");
                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_FILE_LOCK_CONFLICT,
                        header.Status,
                        RequirementCategory.STATUS_FILE_LOCK_CONFLICT.Id,
                        RequirementCategory.STATUS_FILE_LOCK_CONFLICT.Description);
                });
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "From client3: Read and write the locking range when the file is locked, it should fail.");
            ValidateByteLockRangeFromAnotherClient(true, TestConfig.ClusteredFileServerName, fileName);

            #region Unlock byte range
            locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "AfterFailover: Client2 attempts to unlock the range");
            status = clientAfterFailover.Lock(treeIdAfterFailover, lockSequence++, fileIdAfterFailover, locks);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "From client3: Read and write the locking range from a separate client when the file is unlocked, it should succeed.");
            ValidateByteLockRangeFromAnotherClient(false, TestConfig.ClusteredFileServerName, fileName);

            status = clientAfterFailover.Close(treeIdAfterFailover, fileIdAfterFailover);

            status = clientAfterFailover.TreeDisconnect(treeIdAfterFailover);

            status = clientAfterFailover.LogOff();
        }
        #endregion

        #region Common Methods
        /// <summary>
        /// Read and write file within byte lock range when the file is locked or unlocked
        /// </summary>
        /// <param name="isLocked">Set true to indicate that byte lock range is taken on the file</param>
        /// <param name="serverName">Name of file server to access</param>
        /// <param name="targetFileName">Target file name to read and write</param>
        private void ValidateByteLockRangeFromAnotherClient(bool isLocked, string serverName, string targetFileName)
        {
            uint status = 0;

            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, serverName, currentAccessIp);

            status = client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);

            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                serverName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            uint treeId;
            status = client.TreeConnect(uncSharePath, out treeId);

            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fileId;
            status = client.Create(
                treeId,
                targetFileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);

            string data;
            Random random = new Random();
            uint offset = (uint)random.Next(0, TestConfig.WriteBufferLengthInKb * 1024 - 1);
            uint length = (uint)random.Next(0, (int)(TestConfig.WriteBufferLengthInKb * 1024 - offset));
            status = client.Read(treeId, fileId, offset, length, out data);

            status = client.Write(treeId, fileId, contentWrite, checker: (header, response) => { });
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
        }
        #endregion
    }
}
