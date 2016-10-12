// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.ContentServer
{
    [TestClass]
    public class SmbServer : BranchCacheTestClassBase
    {
        #region Test Suite Initialization

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();

            ResetContentServer();
        }

        protected override void TestCleanup()
        {
            ResetContentServer();

            base.TestCleanup();
        }

        #endregion

        #region ContentServer_SmbServer_HashTypeInvalid

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("SMB2")]
        public void ContentServer_SmbServer_HashTypeInvalid()
        {
            CheckApplicability();

            contentInformationUtility.RetrieveContentData();

            using (Smb2ClientTransport smb2Client = new Smb2ClientTransport(testConfig.Timeout))
            {
                smb2Client.OpenFile(
                    testConfig.ContentServerComputerName,
                    testConfig.SharedFolderName,
                    testConfig.NameOfFileWithMultipleBlocks,
                    testConfig.SecurityPackageType,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword,
                    AccessMask.GENERIC_READ);

                HASH_HEADER hashHeader;
                byte[] hashData;
                uint smb2Status = smb2Client.ReadHash(
                    (SRV_READ_HASH_Request_HashType_Values)0xFEFEFEFE,
                    SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_1,
                    SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED,
                    0,
                    uint.MaxValue,
                    out hashHeader,
                    out hashData);

                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    smb2Status,
                    "The content server should return error for invalid hash type");

                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_INVALID_PARAMETER,
                    smb2Status,
                    RequirementCategory.InvalidParameter,
                    RequirementCategory.InvalidParameterMessage);

                smb2Client.CloseFile();
            }
        }

        #endregion

        #region ContentServer_SmbServer_HashVersionInvalid

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("SMB2")]
        public void ContentServer_SmbServer_HashVersionInvalid()
        {
            CheckApplicability();

            contentInformationUtility.RetrieveContentData();

            using (Smb2ClientTransport smb2Client = new Smb2ClientTransport(testConfig.Timeout))
            {
                smb2Client.OpenFile(
                    testConfig.ContentServerComputerName,
                    testConfig.SharedFolderName,
                    testConfig.NameOfFileWithMultipleBlocks,
                    testConfig.SecurityPackageType,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword,
                    AccessMask.GENERIC_READ);

                HASH_HEADER hashHeader;
                byte[] hashData;
                uint smb2Status = smb2Client.ReadHash(
                    SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                    (SRV_READ_HASH_Request_HashVersion_Values)0xFEFEFEFE,
                    SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED,
                    0,
                    uint.MaxValue,
                    out hashHeader,
                    out hashData);

                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    smb2Status,
                    "The content server should return error for invalid hash type");

                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_INVALID_PARAMETER,
                    smb2Status,
                    RequirementCategory.InvalidParameter,
                    RequirementCategory.InvalidParameterMessage);

                smb2Client.CloseFile();
            }
        }

        #endregion

        #region ContentServer_SmbServer_HashRetrivalTypeInvalid

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("SMB2")]
        public void ContentServer_SmbServer_HashRetrivalTypeInvalid()
        {
            CheckApplicability();

            contentInformationUtility.RetrieveContentData();

            using (Smb2ClientTransport smb2Client = new Smb2ClientTransport(testConfig.Timeout))
            {
                smb2Client.OpenFile(
                    testConfig.ContentServerComputerName,
                    testConfig.SharedFolderName,
                    testConfig.NameOfFileWithMultipleBlocks,
                    testConfig.SecurityPackageType,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword,
                    AccessMask.GENERIC_READ);

                HASH_HEADER hashHeader;
                byte[] hashData;
                uint smb2Status = smb2Client.ReadHash(
                    SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                    SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_1,
                    (SRV_READ_HASH_Request_HashRetrievalType_Values)0xFEFEFEFE,
                    0,
                    uint.MaxValue,
                    out hashHeader,
                    out hashData);

                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    smb2Status,
                    "The content server should return error for invalid hash retrieval type");

                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_INVALID_PARAMETER,
                    smb2Status,
                    RequirementCategory.InvalidParameter,
                    RequirementCategory.InvalidParameterMessage);

                smb2Client.CloseFile();
            }
        }

        #endregion

        #region ContentServer_SmbServer_OffsetInvalid

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("SMB2")]
        public void ContentServer_SmbServer_OffsetInvalid()
        {
            CheckApplicability();

            contentInformationUtility.RetrieveContentData();

            using (Smb2ClientTransport smb2Client = new Smb2ClientTransport(testConfig.Timeout))
            {
                smb2Client.OpenFile(
                    testConfig.ContentServerComputerName,
                    testConfig.SharedFolderName,
                    testConfig.NameOfFileWithMultipleBlocks,
                    testConfig.SecurityPackageType,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword,
                    AccessMask.GENERIC_READ);

                HASH_HEADER hashHeader;
                byte[] hashData;

                uint smb2Status = 0;
                TestUtility.DoUntilSucceed(
                    () => (smb2Status = smb2Client.ReadHash(
                        SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                        SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_1,
                        SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED,
                        uint.MaxValue,
                        uint.MaxValue,
                        out hashHeader,
                        out hashData)) != Smb2Status.STATUS_HASH_NOT_PRESENT,
                        testConfig.Timeout,
                        testConfig.RetryInterval);

                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    smb2Status,
                    "The content server should return error for invalid offset");

                /// If the HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED and Offset field of the SRV_READ_HASH request is equal to or beyond the end of the file 
                /// represented by Open.LocalOpen, the server MUST fail the SRV_READ_HASH request with error code STATUS_END_OF_FILE.
                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_END_OF_FILE,
                    smb2Status,
                    RequirementCategory.EndOfFile,
                    RequirementCategory.EndOfFileMessage);

                smb2Client.CloseFile();
            }
        }

        #endregion

        #region ContentServer_SmbServer_HashV1FileBased

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("SMB2")]
        public void ContentServer_SmbServer_HashV1HashBased()
        {
            CheckApplicability();

            contentInformationUtility.RetrieveContentData();

            using (Smb2ClientSupportDialect smb2Client= new Smb2ClientSupportDialect(testConfig.Timeout, DialectRevision.Smb30))
            {
                smb2Client.OpenFile(
                    testConfig.ContentServerComputerName,
                    testConfig.SharedFolderName,
                    testConfig.NameOfFileWithMultipleBlocks,
                    testConfig.SecurityPackageType,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword,
                    AccessMask.GENERIC_READ);

                HASH_HEADER hashHeader;
                byte[] hashData;
                uint smb2Status = 0;
                TestUtility.DoUntilSucceed(
                    () => (smb2Status = smb2Client.ReadHash(
                        SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                        SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_1,
                        SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED,
                        0,
                        uint.MaxValue,
                        out hashHeader,
                        out hashData)) != Smb2Status.STATUS_HASH_NOT_PRESENT,
                        testConfig.Timeout,
                        testConfig.RetryInterval);

                if (testConfig.HashLevelType == ServerHashLevel.HashDisableAll)
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_HASH_NOT_SUPPORTED,
                        smb2Status,
                        "Server MUST fail the SRV_READ_HASH request with STATUS_HASH_NOT_SUPPORTED if ServerHashLevel is HashDisableAll, but the actual status is {0}.", Smb2Status.GetStatusCode(smb2Status));
                }
                else if (testConfig.HashLevelType == ServerHashLevel.HashEnableShare && !smb2Client.isShareHashEnabled)
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_HASH_NOT_SUPPORTED,
                        smb2Status,
                        "Server MUST fail the SRV_READ_HASH request with STATUS_HASH_NOT_SUPPORTED if ServerHashLevel is HashEnableShare and Open.TreeConnect.Share.Enabled is FALSE, but the actual status is {0}.", Smb2Status.GetStatusCode(smb2Status));
                }
                else
                {
                    BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    smb2Status,
                    "Dialect 3.0 should support SRV_HASH_RETRIEVE_FILE_BASED, but the actual status is {0}.", Smb2Status.GetStatusCode(smb2Status));
                }

                smb2Client.CloseFile();
            }
        }

        #endregion

        #region ContentServer_SmbServer_HashV2HashBased

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("SMB2")]
        public void ContentServer_SmbServer_HashV2FileBased()
        {
            CheckApplicability();

            contentInformationUtility.RetrieveContentData();

            using (Smb2ClientSupportDialect smb2Client = new Smb2ClientSupportDialect(testConfig.Timeout, DialectRevision.Smb30))
            {
                smb2Client.OpenFile(
                    testConfig.ContentServerComputerName,
                    testConfig.SharedFolderName,
                    testConfig.NameOfFileWithMultipleBlocks,
                    testConfig.SecurityPackageType,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword,
                    AccessMask.GENERIC_READ);

                HASH_HEADER hashHeader;
                byte[] hashData;

                uint smb2Status = 0;
                TestUtility.DoUntilSucceed(
                    () => (smb2Status = smb2Client.ReadHash(
                        SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                        SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_2,
                        SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_FILE_BASED,
                        0,
                        uint.MaxValue,
                        out hashHeader,
                        out hashData)) != Smb2Status.STATUS_HASH_NOT_PRESENT,
                        testConfig.Timeout,
                        testConfig.RetryInterval);               

                if (testConfig.HashLevelType == ServerHashLevel.HashDisableAll)
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_HASH_NOT_SUPPORTED,
                        smb2Status,
                        "Server MUST fail the SRV_READ_HASH request with STATUS_HASH_NOT_SUPPORTED if ServerHashLevel is HashDisableAll, but the actual status is {0}.", Smb2Status.GetStatusCode(smb2Status));
                }
                else if (testConfig.HashLevelType == ServerHashLevel.HashEnableShare && !smb2Client.isShareHashEnabled)
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_HASH_NOT_SUPPORTED,
                        smb2Status,
                        "Server MUST fail the SRV_READ_HASH request with STATUS_HASH_NOT_SUPPORTED if ServerHashLevel is HashEnableShare and Open.TreeConnect.Share.Enabled is FALSE, but the actual status is {0}.", Smb2Status.GetStatusCode(smb2Status));
                }
                else
                {
                    BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    smb2Status,
                    "Dialect 3.0 should support SRV_HASH_RETRIEVE_HASH_BASED, but the actual status is {0}.", Smb2Status.GetStatusCode(smb2Status));
                }

                smb2Client.CloseFile();
            }
        }

        #endregion

        #region ContentServer_SmbServer_HashV1FileBased_Smb2002

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("SMB2")]
        public void ContentServer_SmbServer_HashV1FileBased_Smb2002()
        {
            CheckApplicability();

            contentInformationUtility.RetrieveContentData();

            using (Smb2ClientSupportDialect smb2Client = new Smb2ClientSupportDialect(testConfig.Timeout, DialectRevision.Smb2002))
            {
                smb2Client.OpenFile(
                    testConfig.ContentServerComputerName,
                    testConfig.SharedFolderName,
                    testConfig.NameOfFileWithMultipleBlocks,
                    testConfig.SecurityPackageType,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword,
                    AccessMask.GENERIC_READ);

                HASH_HEADER hashHeader;
                byte[] hashData;
                uint smb2Status = smb2Client.ReadHash(
                    SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                    SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_1,
                    SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_FILE_BASED,
                    0,
                    uint.MaxValue,
                    out hashHeader,
                    out hashData);

                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    smb2Status,
                    "The content server should return error for invalid hash type");

                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_INVALID_PARAMETER,
                    smb2Status,
                    RequirementCategory.InvalidParameter,
                    RequirementCategory.InvalidParameterMessage);

                smb2Client.CloseFile();
            }
        }

        #endregion

        #region ContentServer_SmbServer_HashV2HashBased_Smb21

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("SMB2")]
        public void ContentServer_SmbServer_HashV2HashBased_Smb21()
        {
            CheckApplicability();

            contentInformationUtility.RetrieveContentData();

            using (Smb2ClientSupportDialect smb2Client = new Smb2ClientSupportDialect(testConfig.Timeout, DialectRevision.Smb21))
            {
                smb2Client.OpenFile(
                    testConfig.ContentServerComputerName,
                    testConfig.SharedFolderName,
                    testConfig.NameOfFileWithMultipleBlocks,
                    testConfig.SecurityPackageType,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword,
                    AccessMask.GENERIC_READ);

                HASH_HEADER hashHeader;
                byte[] hashData;

                uint smb2Status = 0;
                TestUtility.DoUntilSucceed(
                    () => (smb2Status = smb2Client.ReadHash(
                        SRV_READ_HASH_Request_HashType_Values.SRV_HASH_TYPE_PEER_DIST,
                        SRV_READ_HASH_Request_HashVersion_Values.SRV_HASH_VER_2,
                        SRV_READ_HASH_Request_HashRetrievalType_Values.SRV_HASH_RETRIEVE_HASH_BASED,
                        0,
                        uint.MaxValue,
                        out hashHeader,
                        out hashData)) != Smb2Status.STATUS_HASH_NOT_PRESENT,
                        testConfig.Timeout,
                        testConfig.RetryInterval);    

                BaseTestSite.Assert.AreNotEqual(
                    Smb2Status.STATUS_SUCCESS,
                    smb2Status,
                    "The content server should return error for invalid hash type");

                BaseTestSite.CaptureRequirementIfAreEqual(
                    Smb2Status.STATUS_INVALID_PARAMETER,
                    smb2Status,
                    RequirementCategory.InvalidParameter,
                    RequirementCategory.InvalidParameterMessage);

                smb2Client.CloseFile();
            }
        }

        #endregion

        class Smb2ClientSupportDialect : Smb2ClientTransport
        {
            private DialectRevision dialect;
            public bool isShareHashEnabled;

            public Smb2ClientSupportDialect(TimeSpan timeout, DialectRevision dialect)
                : base(timeout)
            {
                this.dialect = dialect;
            }

            protected override uint Negotiate(ushort creditCharge, ushort creditRequest, ulong messageId, Guid clientGuid,
                out DialectRevision selectedDialect, out byte[] gssToken, out Packet_Header responseHeader, out NEGOTIATE_Response responsePayload)
            {
                return client.Negotiate(
                    creditCharge,
                    creditRequest,
                    Packet_Header_Flags_Values.NONE,
                    messageId,
                    new DialectRevision[] { this.dialect },
                    SecurityMode_Values.NONE,
                    Capabilities_Values.NONE,
                    clientGuid,
                    out selectedDialect,
                    out gssToken,
                    out responseHeader,
                    out responsePayload);
            }

            protected override uint TreeConnect(ushort creditCharge, ushort creditRequest, Packet_Header_Flags_Values flags, ulong messageId, ulong sessionId,
                string server, string share, out Packet_Header header, out TREE_CONNECT_Response response)
            {
                uint treeConnectResponseCode = base.TreeConnect(
                    creditCharge, 
                    creditRequest, 
                    flags, 
                    messageId, 
                    sessionId, 
                    server, 
                    share, 
                    out header, 
                    out response);

                this.isShareHashEnabled = response.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_ENABLE_HASH_V1)
                    || response.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_ENABLE_HASH_V2);
                BaseTestSite.Assert.AreEqual(
                    true,
                    this.isShareHashEnabled,
                    "The share folder should enable hash V1 or hash V2");

                return treeConnectResponseCode;
            }
        }
    }
}
