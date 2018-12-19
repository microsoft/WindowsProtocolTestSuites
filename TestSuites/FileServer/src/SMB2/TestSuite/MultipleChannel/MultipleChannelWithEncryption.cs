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
        #region Test Cases
        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate file via multi-channel with encryption on both channels.")]
        public void MultipleChannel_EncryptionOnBothChannels()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION | NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);
            #endregion

            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.EncryptedFileShare);

            uint treeId;
            FILEID fileId;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Establish main channel with client {0} and server {1}, encryption is enabled.", clientIps[0].ToString(), serverIps[0].ToString());
            EstablishMainChannel(
                TestConfig.RequestDialects,
                serverIps[0],
                clientIps[0],
                out treeId,
                true);

            #region CREATE
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Main channel: Create file {0}.", fileName);
            Smb2CreateContextResponse[] serverCreateContexts;
            status = mainChannelClient.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts);
            #endregion

            #region WRITE
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Main channel: Write content to file.");
            status = mainChannelClient.Write(treeId, fileId, contentWrite);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Establish alternative channel with client {0} and server {1}, encryption is enabled.", clientIps[1].ToString(), serverIps[1].ToString());
            EstablishAlternativeChannel(
                TestConfig.RequestDialects,
                serverIps[1],
                clientIps[1],
                treeId,
                true);

            #region READ
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Alternative channel: Read content from file.");
            status = alternativeChannelClient.Read(treeId, fileId, 0, (uint)contentWrite.Length, out contentRead);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Verify the contents read from alternative channel are the same as the one written by main channel.");
            BaseTestSite.Assert.IsTrue(
                contentRead.Equals(contentWrite),
                "Read content should be consistent with written content");

            ClientTearDown(alternativeChannelClient, treeId, fileId);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Operate file via multi-channel only with encryption on main channel.")]
        public void MultipleChannel_Negative_EncryptionOnMainChannel()
        {
            #region Check Applicability
            TestConfig.CheckServerEncrypt();
            #endregion
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.EncryptedFileShare);

            uint treeId;
            FILEID fileId;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Establish main channel with client {0} and server {1}, encryption is enabled.", clientIps[0].ToString(), serverIps[0].ToString());
            EstablishMainChannel(
                TestConfig.RequestDialects,
                serverIps[0],
                clientIps[0],
                out treeId,
                true);

            #region CREATE
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Main channel: Create file {0}.", fileName);
            Smb2CreateContextResponse[] serverCreateContexts;
            status = mainChannelClient.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts);
            #endregion

            #region WRITE
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Main channel: Write content to file.");
            status = mainChannelClient.Write(treeId, fileId, contentWrite);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Establish alternative channel with client {0} and server {1}, encryption is NOT enabled.", clientIps[1].ToString(), serverIps[1].ToString());
            EstablishAlternativeChannel(
                TestConfig.RequestDialects,
                serverIps[1],
                clientIps[1],
                treeId,
                false);

            #region READ
            // Skip the verification of signature when sending a non-encrypted CREATE request to an encrypted share
            alternativeChannelClient.Smb2Client.DisableVerifySignature = true;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Alternative channel: Read content from file.");
            status = alternativeChannelClient.Read(
                treeId,
                fileId,
                0,
                (uint)contentWrite.Length,
                out contentRead,
                checker: (header, response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify Server response indicates that read content failed.");
                    BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Read data from file should not success");
                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_ACCESS_DENIED,
                        header.Status,
                        RequirementCategory.STATUS_ACCESS_DENIED.Id,
                        RequirementCategory.STATUS_ACCESS_DENIED.Description);
                });
            #endregion

        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Operate file via multi-channel only with encryption on alternative channel.")]
        public void MultipleChannel_Negative_EncryptionOnAlternativeChannel()
        {
            #region Check Applicability
            TestConfig.CheckServerEncrypt();
            #endregion
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.EncryptedFileShare);

            uint treeId;
            FILEID fileId;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Establish main channel with client {0} and server {1}, encryption is NOT enabled.", clientIps[0].ToString(), serverIps[0].ToString());
            EstablishMainChannel(
                TestConfig.RequestDialects,
                serverIps[0],
                clientIps[0],
                out treeId,
                false);

            #region CREATE
            // Skip the verification of signature when sending a non-encrypted CREATE request to an encrypted share
            mainChannelClient.Smb2Client.DisableVerifySignature = true;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Main channel: Create file {0}.", fileName);
            Smb2CreateContextResponse[] serverCreateContexts;
            status = mainChannelClient.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Create file without encryption on encrypted share is expected to fail");
                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_ACCESS_DENIED,
                        header.Status,
                        RequirementCategory.STATUS_ACCESS_DENIED.Id,
                        RequirementCategory.STATUS_ACCESS_DENIED.Description);
                });
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Establish alternative channel with client {0} and server {1}, encryption is enabled.", clientIps[1].ToString(), serverIps[1].ToString());
            EstablishAlternativeChannel(
                TestConfig.RequestDialects,
                serverIps[1],
                clientIps[1],
                treeId,
                true);

            #region READ
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Alternative channel: Read content from file.");
            status = alternativeChannelClient.Read(
                treeId,
                fileId,
                0,
                (uint)contentWrite.Length,
                out contentRead,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreNotEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Read data from file should not success");
                    BaseTestSite.CaptureRequirementIfAreEqual(
                        Smb2Status.STATUS_FILE_CLOSED,
                        header.Status,
                        RequirementCategory.STATUS_FILE_CLOSED.Id,
                        RequirementCategory.STATUS_FILE_CLOSED.Description);
                });
            #endregion
        }
        #endregion
    }
}
