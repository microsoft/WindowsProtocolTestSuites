// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public partial class AppInstanceIdExtendedTest : SMB2TestBase
    {
        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate files with encrypted message before and after client failover.")]
        public void AppInstanceId_Encryption()
        {
            AppInstanceIdTestWithEncryption(encryptionInInitialOpen: true, encryptionInReOpen: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Operate files with encrypted message before failover but with unencrypted message after failover.")]
        public void AppInstanceId_Negative_EncryptionInInitialOpen_NoEncryptionInReOpen()
        {
            #region Check Applicability
            TestConfig.CheckServerEncrypt();
            #endregion
            AppInstanceIdTestWithEncryption(encryptionInInitialOpen: true, encryptionInReOpen: false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Operate files with encrypted message before client failover but with unencrypted message after failover.")]
        public void AppInstanceId_Negative_NoEncryptionInInitialOpen_EncryptionInReOpen()
        {
            #region Check Applicability
            TestConfig.CheckServerEncrypt();
            #endregion
            AppInstanceIdTestWithEncryption(encryptionInInitialOpen: false, encryptionInReOpen: true);
        }
        #endregion

        #region Common Methods
        /// <summary>
        /// Test AppInstanceId with encryption
        /// </summary>
        /// <param name="encryptionInInitialOpen">Set true if encryption is enabled in initial open, otherwise set false</param>
        /// <param name="encryptionInReOpen">Set true if encryption is enabled in re-open, otherwise set false</param>
        private void AppInstanceIdTestWithEncryption(bool encryptionInInitialOpen, bool encryptionInReOpen)
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION
                | NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING
                | NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID, CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2, CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);
            #endregion

            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.EncryptedFileShare);

            BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "InitialOpen: Connect to share via Nic1.");
            FILEID fileIdForInitialOpen;
            uint treeIdForInitialOpen;
            ConnectShare(TestConfig.SutIPAddress, TestConfig.ClientNic1IPAddress, clientForInitialOpen, out treeIdForInitialOpen, encryptionInInitialOpen);

            // Skip the verification of signature when sending a non-encrypted CREATE request to an encrypted share
            if (!encryptionInInitialOpen)
            {
                clientForInitialOpen.Smb2Client.DisableVerifySignature = true;
            }

            #region CREATE an open with AppInstanceId
            BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "InitialOpen: CREATE an open with AppInstanceId.");

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
                    },
                    new Smb2CreateAppInstanceId
                    {
                         AppInstanceId = appInstanceId
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => { });
            #endregion

            if (!encryptionInInitialOpen)
            {
                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "Create durable handle request to encrypted share without encryption should not SUCCESS ");
                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_ACCESS_DENIED,
                    status,
                    RequirementCategory.STATUS_ACCESS_DENIED.Id,
                    RequirementCategory.STATUS_ACCESS_DENIED.Description);
            }
            else
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "Create durable handle request V2 should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));

                BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "InitialOpen: Write contents to file.");
                status = clientForInitialOpen.Write(treeIdForInitialOpen, fileIdForInitialOpen, contentWrite);
            }
            
            FILEID fileIdForReOpen;
            uint treeIdForReOpen;
            BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Connect to same share via Nic2.");
            ConnectShare(TestConfig.SutIPAddress, TestConfig.ClientNic2IPAddress, clientForReOpen, out treeIdForReOpen, encryptionInReOpen);

            // Skip the verification of signature when sending a non-encrypted CREATE request to an encrypted share
            if (!encryptionInReOpen)
            {
                clientForReOpen.Smb2Client.DisableVerifySignature = true;
            }
            #region CREATE an open with same AppInstanceId for reopen
            BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: CREATE an open with same AppInstanceId for reopen.");
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
                    },
                    new Smb2CreateAppInstanceId
                    {
                        // Use the same application instance id to force the server close all files
                        AppInstanceId = appInstanceId
                    }
                },
                shareAccess: ShareAccess_Values.NONE,
                checker: (header, response) => { });
            #endregion

            if (encryptionInInitialOpen && encryptionInReOpen)
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "Create durable handle request should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));

                BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Read the contents written by InitialOpen.");
                status = clientForReOpen.Read(treeIdForReOpen, fileIdForReOpen, 0, (uint)contentWrite.Length, out contentRead);

                BaseTestSite.Assert.IsTrue(
                    contentRead.Equals(contentWrite),
                    "The written content should equal to read content.");

                BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Close file.");
                status = clientForReOpen.Close(treeIdForReOpen, fileIdForReOpen);

                BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Disconnect the share.");
                status = clientForReOpen.TreeDisconnect(treeIdForReOpen);
            }
            else if (encryptionInInitialOpen && !encryptionInReOpen)
            {
                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "Create durable handle request to encrypted share without encryption should not SUCCESS ");
                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_ACCESS_DENIED,
                    status,
                    RequirementCategory.STATUS_ACCESS_DENIED.Id,
                    RequirementCategory.STATUS_ACCESS_DENIED.Description);

                BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Disconnect the share.");
                status = clientForReOpen.TreeDisconnect(treeIdForReOpen, (header, response) => { });
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
            else if (!encryptionInInitialOpen && encryptionInReOpen)
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "Create durable handle request should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));

                BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Close file.");
                status = clientForReOpen.Close(treeIdForReOpen, fileIdForReOpen);

                BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Disconnect from the share.");
                status = clientForReOpen.TreeDisconnect(treeIdForReOpen);
            }

            BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "ReOpen: Log off.");
            status = clientForReOpen.LogOff();
        }
        #endregion
    }
}
