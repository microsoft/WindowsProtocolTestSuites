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
        [Description("Operate files with encryption before server failover and after failover.")]
        public void FileServerFailover_Encryption()
        {
            FileServerFailoverWithEncryption(true, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Failover)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Operate files with encryption before server failover but without encryption after failover")]
        public void FileServerFailover_Negative_EncryptionBeforeFailover_NoEncryptionAfterFailover()
        {
            FileServerFailoverWithEncryption(true, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Failover)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Operate files without encryption before failover but with encryption after failover.")]
        public void FileServerFailover_Negative_NoEncryptionBeforeFailover_EncryptionAfterFailover()
        {
            FileServerFailoverWithEncryption(false, true);
        }
        #endregion

        #region Commond Method
        /// <summary>
        /// Test server failover with encryption
        /// </summary>
        /// <param name="encryptionBeforeFailover">Set true if encryption is enabled before server failover, otherwise set false</param>
        /// <param name="encryptionAfterFailover">Set true if encryption is enabled after server failover, otherwise set false</param>
        private void FileServerFailoverWithEncryption(bool encryptionBeforeFailover, bool encryptionAfterFailover)
        {
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.ClusteredFileServerName, TestConfig.ClusteredEncryptedFileShare);

            FILEID fileIdBeforeFailover;
            uint treeIdBeforeFailover;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "BeforeFailover: Connect to general file server {0}.", TestConfig.ClusteredFileServerName);
            ConnectGeneralFileServerBeforeFailover(TestConfig.ClusteredFileServerName, out treeIdBeforeFailover, encryptionBeforeFailover);

            #region CREATE a durable open with flag DHANDLE_FLAG_PERSISTENT
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "BeforeFailover: CREATE a durable open with flag DHANDLE_FLAG_PERSISTENT.");
            Smb2CreateContextResponse[] serverCreateContexts;
            createGuid = Guid.NewGuid();

            // Skip the verification of signature when sending a non-encrypted CREATE request to an encrypted share
            if (!encryptionBeforeFailover)
            {
                clientBeforeFailover.Smb2Client.DisableVerifySignature = true;
            }
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
                },
                checker: (header, response) => { });
            #endregion

            if (!encryptionBeforeFailover)
            {
                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "CreateDurableHandleRequestV2 to encrypted share without encryption should not SUCCESS ");
                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_ACCESS_DENIED,
                    status,
                    RequirementCategory.STATUS_ACCESS_DENIED.Id,
                    RequirementCategory.STATUS_ACCESS_DENIED.Description);
            }
            else if (encryptionBeforeFailover)
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "CreateDurableHandleRequestV2 for file {0} should succeed, actual status is {1}", fileName, Smb2Status.GetStatusCode(status));

                status = clientBeforeFailover.Write(treeIdBeforeFailover, fileIdBeforeFailover, contentWrite);
            }

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Do failover of the file server.");
            FailoverServer(currentAccessIp, TestConfig.ClusteredFileServerName, FileServerType.GeneralFileServer);

            FILEID fileIdAfterFailover = FILEID.Zero;
            uint treeIdAfterFailover;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: Connect to the same general file server {0}.", TestConfig.ClusteredFileServerName);
            ReconnectServerAfterFailover(TestConfig.ClusteredFileServerName, FileServerType.GeneralFileServer, out treeIdAfterFailover, encryptionAfterFailover);

            // Skip the verification of signature when sending a non-encrypted CREATE request to an encrypted share
            if (!encryptionAfterFailover)
            {
                clientAfterFailover.Smb2Client.DisableVerifySignature = true;
            }
            if (encryptionBeforeFailover && !encryptionAfterFailover)
            {
                #region CREATE to reconnect previous duarable open with flag DHANDLE_FLAG_PERSISTENT
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: CREATE to reconnect previous duarable open with flag DHANDLE_FLAG_PERSISTENT.");
                status = clientAfterFailover.Create(
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
                        checker: (header, response) => { });
                #endregion

                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "CreatDurableHandleReconnectV2 to encrypted share without encryption should not SUCCESS ");
                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_ACCESS_DENIED,
                    status,
                    RequirementCategory.STATUS_ACCESS_DENIED.Id,
                    RequirementCategory.STATUS_ACCESS_DENIED.Description);

                status = clientAfterFailover.TreeDisconnect(treeIdAfterFailover, (header, response) => { });
                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "TreeDisconnect should not SUCCESS");
                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_ACCESS_DENIED,
                    status,
                    RequirementCategory.STATUS_ACCESS_DENIED.Id,
                    RequirementCategory.STATUS_ACCESS_DENIED.Description);
            }
            else if (!encryptionBeforeFailover && encryptionAfterFailover)
            {
                #region CREATE to reconnect previous duarable open with flag DHANDLE_FLAG_PERSISTENT
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: CREATE to reconnect previous duarable open with flag DHANDLE_FLAG_PERSISTENT.");
                status = clientAfterFailover.Create(
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
                        checker: (header, response) => { });
                #endregion

                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "CreatDurableHandleReconnectV2 should not SUCCESS ");

                if (TestConfig.Platform == Platform.WindowsServer2012)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug,
                        @"[MS-SMB2] Section 3.3.5.9.12: Windows Server 2012 with [KB2770917] and Windows 8 with [KB2770917] fail the CREATE request with STATUS_INVALID_PARAMETER 
                        if the request includes the SMB2_DHANDLE_FLAG_PERSISTENT bit in the Flags field of the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context.");

                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_INVALID_PARAMETER,
                        status,
                        RequirementCategory.STATUS_INVALID_PARAMETER.Id,
                        RequirementCategory.STATUS_INVALID_PARAMETER.Description);
                }
                else
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, 
                        @"[MS-SMB2] 3.3.5.9.12   Handling the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context.
                        The server MUST look up an existing Open in the GlobalOpenTable by doing a lookup with the FileId.Persistent portion of the create context.
                        If the lookup fails, the server SHOULD (285) fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in Failed Open Handling in section 3.3.5.9.");

                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_OBJECT_NAME_NOT_FOUND,
                        status,
                        RequirementCategory.STATUS_OBJECT_NAME_NOT_FOUND.Id,
                        RequirementCategory.STATUS_OBJECT_NAME_NOT_FOUND.Description);
                }
                status = clientAfterFailover.TreeDisconnect(treeIdAfterFailover);
            }
            else if (encryptionBeforeFailover && encryptionAfterFailover)
            {
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
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "CreatDurableHandleReconnectV2 should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: Read contents written before failover.");
                status = clientAfterFailover.Read(treeIdAfterFailover, fileIdAfterFailover, 0, (uint)contentWrite.Length, out contentRead);

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "AfterFailover: Verify the contents are the same as the one written before failover.");
                BaseTestSite.Assert.IsTrue(
                    contentWrite.Equals(contentRead),
                    "Content read after failover should be identical to that written before failover");

                status = clientAfterFailover.Close(treeIdAfterFailover, fileIdAfterFailover);

                status = clientAfterFailover.TreeDisconnect(treeIdAfterFailover);
            }

            status = clientAfterFailover.LogOff();
        }
        #endregion
    }
}
