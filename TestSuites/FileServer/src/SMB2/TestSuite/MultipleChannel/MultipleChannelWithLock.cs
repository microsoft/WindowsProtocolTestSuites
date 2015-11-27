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
    public partial class MultipleChannelExtendedTest : SMB2TestBase
    {
        #region Test Case
        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.MultipleChannel)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate file via multi-channel with lock operation on different channels.")]
        public void MultipleChannel_LockUnlockOnDiffChannel()
        {
            MultipleChannelTestWithLock(false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.MultipleChannel)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate file via multi-channel with lock operation on same channel.")]
        public void MultipleChannel_LockUnlockOnSameChannel()
        {
            MultipleChannelTestWithLock(true);
        }
        #endregion

        #region Common Methods
        /// <summary>
        /// Establish alternative channel and lock/unlock byte range of a file from same or different channel client
        /// </summary>
        /// <param name="lockUnlockOnSameChannel">Set this parameter to true if the unlock operation is taken from the same channel client</param>
        private void MultipleChannelTestWithLock(bool lockUnlockOnSameChannel)
        {
            uint treeId;
            FILEID fileId;

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Establish main channel with client {0} and server {1}.", clientIps[0].ToString(), serverIps[0].ToString());
            EstablishMainChannel(
                TestConfig.RequestDialects,
                serverIps[0],
                clientIps[0],
                out treeId);
            
            #region CREATE to open file for WRITE and LOCK
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Main channel: CREATE file.");
            Smb2CreateContextResponse[] serverCreateContexts;
            status = mainChannelClient.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);
            #endregion

            #region WRITE content to file
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Main channel: WRITE content to file.");
            status = mainChannelClient.Write(treeId, fileId, contentWrite);
            #endregion

            #region Request byte lock range
            //Construct LOCK_ELEMENT
            LOCK_ELEMENT[] locks = new LOCK_ELEMENT[1];
            uint lockSequence = 0;
            locks[0].Offset = 0;
            locks[0].Length = (ulong)TestConfig.WriteBufferLengthInKb * 1024;
            locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_SHARED_LOCK;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Main channel client starts to lock a byte range for file \"{0}\" with parameters offset:{1}, length:{2}, flags: {3})",
                fileName, locks[0].Offset, locks[0].Length, locks[0].Flags.ToString());
            status = mainChannelClient.Lock(treeId, lockSequence++, fileId, locks);
            #endregion

            #region WRITE content within the locking range
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Main channel: attempts to write content within the locking range.");
            status = mainChannelClient.Write(
                treeId,
                fileId,
                contentWrite,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Write should not success when shared lock is taken");

                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_FILE_LOCK_CONFLICT,
                        header.Status,
                        RequirementCategory.STATUS_FILE_LOCK_CONFLICT.Id,
                        RequirementCategory.STATUS_FILE_LOCK_CONFLICT.Description);
                });
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Establish alternative channel with client {0} and server {1}.", clientIps[1].ToString(), serverIps[1].ToString());
            EstablishAlternativeChannel(
                TestConfig.RequestDialects,
                serverIps[1],
                clientIps[1],
                treeId);

            #region READ/WRITE from alternative channel within the locking range
            Random random = new Random();
            uint offset = (uint)random.Next(0, TestConfig.WriteBufferLengthInKb * 1024 - 1);
            uint length = (uint)random.Next(0, (int)(TestConfig.WriteBufferLengthInKb * 1024 - offset));
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Alternative channel: attempts to randomly read the locking range with offset: {0} and length: {1}", offset, length);
            status = alternativeChannelClient.Read(treeId, fileId, offset, length, out contentRead);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Alternative channel: attempts to write content to the locking range");
            status = alternativeChannelClient.Write(
                treeId,
                fileId,
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
                "Read and write the locking range from Client3 when the file is locked");
            ValidateByteLockRangeFromAnotherClient(true);

            locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK;
            if (lockUnlockOnSameChannel)
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.TestStep,
                    "From main channel client unlock the range");
                status = mainChannelClient.Lock(treeId, lockSequence++, fileId, locks);
            }
            else
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.TestStep,
                    "From alternative channel client unlock the range");
                status = alternativeChannelClient.Lock(treeId, lockSequence++, fileId, locks);
            }

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Read and write the locking range from Client3 when the file is unlocked");
            ValidateByteLockRangeFromAnotherClient(false);

            ClientTearDown(mainChannelClient, treeId, fileId);
        }

        /// <summary>
        /// Read and write the file within the locking byte range when lock or unlock is taken
        /// </summary>
        /// <param name="isLocked">Set true to indicate that access the file when lock operation is taken</param>
        private void ValidateByteLockRangeFromAnotherClient(bool isLocked)
        {
            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            status = client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);

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
                    "Write content in file failed with error code=" + Smb2Status.GetStatusCode(status));
            }
        }
        #endregion
    }
}
