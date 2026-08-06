// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class Signing : SMB2TestBase
    {
        #region Variables
        private const string WrongSessionKeyRejectedDescription =
            "This test case is designed to verify that when the server receives a signed, unencrypted " +
            "SMB2 request whose signature fails verification, it fails the request with STATUS_ACCESS_DENIED per MS-SMB2 3.3.5.2.4.";
        private Smb2FunctionalClient client;
        #endregion

        #region Initialization and Cleanup

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
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
        }

        protected override void TestCleanup()
        {
            client.Disconnect();
            base.TestCleanup();
        }

        #endregion

        #region Test cases

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Signing)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE and SESSION_SETUP requests with NEGOTIATE_SIGNING_REQUIRED set.")]
        public void BVT_Signing()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb2002);
            testConfig.CheckServerEncrypt();
            TestConfig.CheckSigning();
            #endregion
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends NEGOTIATE request with NEGOTIATE_SIGNING_REQUIRED flag set.");
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SESSION_SETUP request with NEGOTIATE_SIGNING_REQUIRED flag set.");
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken,
                SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);

            string uncSharepath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT request.");
            client.TreeConnect(
                uncSharepath,
                out treeId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: TREE_DISCONNECT; LOG_OFF");
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Signing)]
        [Description("This test case is designed to test whether server set the Signature field to zero in Encrypted message.")]
        public void Signing_VerifySignatureWhenEncrypted()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION);
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends NEGOTIATE with the capability GLOBAL_CAP_ENCRYPTION.");
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_ENCRYPTION
                );

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SESSION_SETUP request and expects response.");
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken,
                SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED);

            string uncSharepath =
                Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.EncryptedFileShare);
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT to share: {0}", uncSharepath);
            client.TreeConnect(
                uncSharepath,
                out treeId,
                (Packet_Header header, TREE_CONNECT_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "TreeConnect should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    BaseTestSite.Assert.AreEqual(
                    ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA,
                    ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA & response.ShareFlags,
                    "Server should set SMB2_SHAREFLAG_ENCRYPT_DATA for ShareFlags field in TREE_CONNECT response");

                });

            // After calling this method, client will send encrypted message after tree connect.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client enables per share encryption: TreeId=0x{0:x}", treeId);
            client.SetTreeEncryption(treeId, true);

            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends encrypted CREATE request and expects success.");
            client.Create(
                treeId,
                GetTestFileName(uncSharepath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                null,
                shareAccess: ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.IsTrue(
                        !header.Signature.Any(e => e != 0),
                        "[MS-SMB2] 3.3.4.1.1 If the server encrypts the message, as specified in section 3.1.4.3, the server MUST set the Signature field of the SMB2 header to zero, actually the Signature field is [{0}].", string.Join(", ", header.Signature));
                });

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: CLOSE; TREE_DISCONNECT; LOG_OFF");
            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Signing)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether outgoing and incoming messages " +
            "are correctly signed and verified using aes-gmac signing algorithm.")]
        public void Signing_VerifyAesGmacSigning()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb311);
            TestConfig.CheckSigning();
            #endregion

            var signingAlgorithms = new SigningAlgorithm[] { SigningAlgorithm.AES_GMAC };
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends NEGOTIATE with the Aes-GMAC signing algorithm and NEGOTIATE_SIGNING_REQUIRED.");
            client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                Smb2Utility.GetDialects(DialectRevision.Smb311),
                SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED,
                preauthHashAlgs: new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 },
                compressionFlags: SMB2_COMPRESSION_CAPABILITIES_Flags.SMB2_COMPRESSION_CAPABILITIES_FLAG_NONE,
                signingAlgorithms: signingAlgorithms,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status, "SUT MUST return STATUS_SUCCESS if the negotiation finished successfully.");
                });

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SESSION_SETUP request and expects response.");
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken,
                SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED);

            string uncSharepath =
                Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT to share: {0}", uncSharepath);
            client.TreeConnect(
                uncSharepath,
                out treeId,
                (Packet_Header header, TREE_CONNECT_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "TreeConnect should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    BaseTestSite.Assert.AreEqual(
                    Packet_Header_Flags_Values.FLAGS_SIGNED,
                    Packet_Header_Flags_Values.FLAGS_SIGNED & header.Flags,
                    "Server should set SMB2_FLAGS_SIGNED bit in the Flags field of the SMB2 header");

                });

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: TREE_DISCONNECT; LOG_OFF");
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Signing)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description(WrongSessionKeyRejectedDescription)]
        public void Signing_WrongSessionKey_Rejected_Smb2002_HmacSha256()
        {
            RunSigningWrongSessionKeyRejected(DialectRevision.Smb2002, SigningAlgorithm.HMAC_SHA256);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.Signing)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description(WrongSessionKeyRejectedDescription)]
        public void Signing_WrongSessionKey_Rejected_Smb21_HmacSha256()
        {
            RunSigningWrongSessionKeyRejected(DialectRevision.Smb21, SigningAlgorithm.HMAC_SHA256);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Signing)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description(WrongSessionKeyRejectedDescription)]
        public void Signing_WrongSessionKey_Rejected_Smb30_AesCmac()
        {
            RunSigningWrongSessionKeyRejected(DialectRevision.Smb30, SigningAlgorithm.AES_CMAC);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb302)]
        [TestCategory(TestCategories.Signing)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description(WrongSessionKeyRejectedDescription)]
        public void Signing_WrongSessionKey_Rejected_Smb302_AesCmac()
        {
            RunSigningWrongSessionKeyRejected(DialectRevision.Smb302, SigningAlgorithm.AES_CMAC);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Signing)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description(WrongSessionKeyRejectedDescription)]
        public void Signing_WrongSessionKey_Rejected_Smb311_HmacSha256()
        {
            RunSigningWrongSessionKeyRejected(DialectRevision.Smb311, SigningAlgorithm.HMAC_SHA256);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Signing)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description(WrongSessionKeyRejectedDescription)]
        public void Signing_WrongSessionKey_Rejected_Smb311_AesCmac()
        {
            RunSigningWrongSessionKeyRejected(DialectRevision.Smb311, SigningAlgorithm.AES_CMAC);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Signing)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description(WrongSessionKeyRejectedDescription)]
        public void Signing_WrongSessionKey_Rejected_Smb311_AesGmac()
        {
            RunSigningWrongSessionKeyRejected(DialectRevision.Smb311, SigningAlgorithm.AES_GMAC);
        }

        private void RunSigningWrongSessionKeyRejected(DialectRevision dialect, SigningAlgorithm signingAlgorithm)
        {
            #region Check Applicability
            TestConfig.CheckDialect(dialect);
            TestConfig.CheckSigning();
            TestConfig.CheckSigningAlgorithm(signingAlgorithm);
            // This case requires a signed, unencrypted request to reach server-side signature verification.
            // When the session/global data is encrypted, the SMB2 header Signature field is zeroed and the
            // request is validated via the transform header instead, so this negative signing case is not applicable.
            BaseTestSite.Assume.IsFalse(
                TestConfig.IsGlobalEncryptDataEnabled,
                "This test case requires a signed, unencrypted session; it is not applicable when global encryption is enabled.");
            #endregion

            // Step 1: Establish a signing-required, non-encrypted SMB session using exactly the dialect
            // and effective signing algorithm represented by this test case.
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Test case configuration: Dialect={0}, SigningAlgorithm={1}. Client sends NEGOTIATE with NEGOTIATE_SIGNING_REQUIRED.",
                dialect,
                signingAlgorithm);

            if (dialect == DialectRevision.Smb311)
            {
                client.NegotiateWithContexts(
                    Packet_Header_Flags_Values.NONE,
                    new[] { dialect },
                    SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED,
                    capabilityValue: Capabilities_Values.NONE,
                    preauthHashAlgs: new[] { PreauthIntegrityHashID.SHA_512 },
                    ifAddGLOBAL_CAP_ENCRYPTION: false,
                    signingAlgorithms: new[] { signingAlgorithm },
                    responseChecker: (header, response) =>
                    {
                        BaseTestSite.Assert.IsNotNull(
                            response.NegotiateContext_SIGNING,
                            "An applicable SMB 3.1.1 signing-algorithm row must receive an SMB2_SIGNING_CAPABILITIES response context.");
                        BaseTestSite.Assert.AreEqual(
                            1,
                            (int)response.NegotiateContext_SIGNING.Value.SigningAlgorithmCount,
                            "The SMB2_SIGNING_CAPABILITIES response must select exactly one signing algorithm.");
                        BaseTestSite.Assert.AreEqual(
                            signingAlgorithm,
                            response.NegotiateContext_SIGNING.Value.SigningAlgorithms[0],
                            "The SUT must select the signing algorithm offered by this SMB 3.1.1 test case.");
                    });
            }
            else
            {
                client.Negotiate(
                    new[] { dialect },
                    false,
                    SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED,
                    capabilityValue: Capabilities_Values.NONE,
                    ifAddGLOBAL_CAP_ENCRYPTION: false);
            }

            BaseTestSite.Assert.AreEqual(
                dialect,
                client.Dialect,
                "The SUT must select the exact dialect requested by this test case.");

            if (dialect == DialectRevision.Smb311)
            {
                BaseTestSite.Assert.AreEqual(
                    signingAlgorithm,
                    client.Smb2Client.SelectedSigningAlgorithm,
                    "The SUT must select the SMB 3.1.1 signing algorithm offered by this applicable test case.");
            }
            else
            {
                SigningAlgorithm dialectDefinedAlgorithm =
                    dialect == DialectRevision.Smb2002 || dialect == DialectRevision.Smb21
                    ? SigningAlgorithm.HMAC_SHA256
                    : SigningAlgorithm.AES_CMAC;
                BaseTestSite.Assert.AreEqual(
                    signingAlgorithm,
                    dialectDefinedAlgorithm,
                    "The test case must use the signing algorithm defined by the negotiated pre-SMB 3.1.1 dialect.");
            }

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Negotiation evidence: selected Dialect={0}, effective SigningAlgorithm={1}; encryption capability/context was not requested.",
                client.Dialect,
                signingAlgorithm);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SESSION_SETUP request with NEGOTIATE_SIGNING_REQUIRED flag set.");
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken,
                SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);

            // Use a non-encrypted share so that requests are signed but not encrypted.
            string uncSharepath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends TREE_CONNECT request to a non-encrypted share: {0}.", uncSharepath);
            client.TreeConnect(uncSharepath, out treeId);

            // Step 2: Send a correctly signed control ECHO and require success.
            // ECHO is a no-op keep-alive, so there is no protocol operation side effect to verify beyond the status codes.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a correctly signed ECHO request as a control and expects STATUS_SUCCESS.");
            client.Echo(
                treeId,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "The correctly signed control ECHO should succeed, actually server returns {0}.",
                        Smb2Status.GetStatusCode(header.Status));
                });

            // Step 3: Add the smallest post-signature mutation hook. It runs after normal signing (Smb2Crypto.SignCompressAndEncrypt)
            // and deterministically flips one signature byte, keeping the request correctly framed but with an invalid signature.
            bool signatureMutationApplied = false;
            bool transportSendCompleted = false;
            bool onWireEvidenceVerified = false;
            bool rejectionResponseReceived = false;
            ulong mutatedMessageId = 0;
            byte[] mutatedSignature = null;
            Action<Smb2Packet> signatureCorrupter = (packet) =>
            {
                var singlePacket = packet as Smb2SinglePacket;
                BaseTestSite.Assert.IsNotNull(
                    singlePacket,
                    "The post-signing packet must be an unencrypted SMB2 single packet.");
                BaseTestSite.Assert.AreEqual(
                    Smb2Command.ECHO,
                    singlePacket.Header.Command,
                    "The packet selected for signature mutation must be an ECHO request.");
                BaseTestSite.Assert.AreEqual(
                    Packet_Header_Flags_Values.FLAGS_SIGNED,
                    singlePacket.Header.Flags & Packet_Header_Flags_Values.FLAGS_SIGNED,
                    "The ECHO request must have SMB2_FLAGS_SIGNED set before its signature is mutated.");

                // Packet_Header.Signature returns a copy of the two signature backing fields. Mutate the
                // copy and assign it back so that the serialized SMB2 header contains the corrupted byte.
                byte[] corruptedSignature = singlePacket.Header.Signature;
                BaseTestSite.Assert.AreEqual(
                    16,
                    corruptedSignature.Length,
                    "The SMB2 header signature must be 16 bytes before mutation.");
                byte originalSignatureByte = corruptedSignature[0];
                corruptedSignature[0] ^= 0xFF;
                singlePacket.Header.Signature = corruptedSignature;

                BaseTestSite.Assert.AreEqual(
                    (byte)(originalSignatureByte ^ 0xFF),
                    singlePacket.Header.Signature[0],
                    "The corrupted signature byte must be written back to the SMB2 header.");
                mutatedMessageId = singlePacket.Header.MessageId;
                mutatedSignature = singlePacket.Header.Signature.ToArray();
                signatureMutationApplied = true;

                BaseTestSite.Log.Add(
                    LogEntryKind.TestStep,
                    "The signature of the outgoing signed ECHO request has been deterministically corrupted (one byte flipped) after signing. " +
                    "The serialized transport payload will be verified after the underlying send completes.");
            };

            Action<byte[]> malformedPacketSent = (data) =>
            {
                // OnWirePacketSent runs only after the underlying SendBytes call succeeds. Verify the exact
                // serialized SMB2 payload without logging the payload or any session/signing key material.
                transportSendCompleted = true;
                BaseTestSite.Assert.IsTrue(
                    data.Length >= 64,
                    "The transport-confirmed SMB2 payload must contain a complete 64-byte SMB2 header.");
                BaseTestSite.Assert.AreEqual(
                    0x424D53FEu,
                    BitConverter.ToUInt32(data, 0),
                    "The transport-confirmed payload must be an unencrypted SMB2 message, not a transform header.");
                BaseTestSite.Assert.AreEqual(
                    Smb2Command.ECHO,
                    (Smb2Command)BitConverter.ToUInt16(data, 12),
                    "The transport-confirmed malformed request must be ECHO.");
                BaseTestSite.Assert.AreEqual(
                    Packet_Header_Flags_Values.FLAGS_SIGNED,
                    (Packet_Header_Flags_Values)BitConverter.ToUInt32(data, 16) & Packet_Header_Flags_Values.FLAGS_SIGNED,
                    "The transport-confirmed malformed request must have SMB2_FLAGS_SIGNED set.");
                BaseTestSite.Assert.AreEqual(
                    mutatedMessageId,
                    BitConverter.ToUInt64(data, 24),
                    "The transport-confirmed request MessageId must match the post-signature mutation evidence.");

                byte[] serializedSignature = data.Skip(48).Take(16).ToArray();
                BaseTestSite.Assert.IsTrue(
                    mutatedSignature != null && mutatedSignature.SequenceEqual(serializedSignature),
                    "The complete serialized signature handed to the transport must match the signature captured after mutation.");

                string payloadDigest = Convert.ToHexString(SHA256.HashData(data));
                onWireEvidenceVerified = true;
                BaseTestSite.Log.Add(
                    LogEntryKind.TestStep,
                    "Transport evidence: the underlying send completed for malformed unencrypted SMB2 ECHO MessageId={0}, " +
                    "Dialect={1}, SigningAlgorithm={2}, PayloadSHA256={3}. The full serialized signature matches the post-signing mutation; no key material is logged.",
                    mutatedMessageId,
                    dialect,
                    signingAlgorithm,
                    payloadDigest);
            };

            // Step 4: Transmit the malformed request and require a response with STATUS_ACCESS_DENIED.
            BaseTestSite.Log.Add(LogEntryKind.TestStep,
                "Client sends a signed ECHO request whose signature is corrupted after signing and expects STATUS_ACCESS_DENIED.");
            client.Smb2Client.ProcessedPacketModifier += signatureCorrupter;
            client.Smb2Client.OnWirePacketSent += malformedPacketSent;
            try
            {
                client.Echo(
                    treeId,
                    checker: (header, response) =>
                    {
                        rejectionResponseReceived = true;
                        BaseTestSite.Assert.AreEqual(
                            mutatedMessageId,
                            header.MessageId,
                            "The rejection response must correspond to the transport-confirmed malformed request.");
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_ACCESS_DENIED,
                            header.Status,
                            "[MS-SMB2] 3.3.5.2.4: When signature verification of a signed, unencrypted request fails, the server MUST fail the request with STATUS_ACCESS_DENIED. Actually server returns {0}.",
                            Smb2Status.GetStatusCode(header.Status));
                    });
            }
            catch (Exception ex) when (
                !transportSendCompleted &&
                !(ex is AssertFailedException) &&
                !(ex is AssertInconclusiveException))
            {
                BaseTestSite.Assert.Fail(
                    "The client failed before the underlying transport confirmed sending the mutated request. " +
                    "This is a local serialization/send failure and cannot satisfy server-side signature-verification coverage. " +
                    "Exception={0}: {1}",
                    ex.GetType().FullName,
                    ex.Message);
            }
            catch (InvalidOperationException ex) when (
                transportSendCompleted &&
                string.Equals(ex.Message, "Underlying connection has been closed.", StringComparison.Ordinal))
            {
                // MS-SMB2 3.3.5.2.4 permits a disconnect only in addition to the required rejection response.
                // A send-confirmed connection closure before that response is therefore a distinct server behavior failure.
                BaseTestSite.Assert.Fail(
                    "The transport confirmed sending the malformed request, but the server closed the connection without " +
                    "returning the required STATUS_ACCESS_DENIED response. A disconnect is optional only after the rejection response.");
            }
            catch (TimeoutException ex) when (transportSendCompleted)
            {
                BaseTestSite.Assert.Fail(
                    "The transport confirmed sending the malformed request, but no rejection response was received before timeout. " +
                    "A timeout cannot substitute for STATUS_ACCESS_DENIED. Transport detail: {0}",
                    ex.Message);
            }
            finally
            {
                client.Smb2Client.ProcessedPacketModifier -= signatureCorrupter;
                client.Smb2Client.OnWirePacketSent -= malformedPacketSent;
            }

            BaseTestSite.Assert.IsTrue(
                signatureMutationApplied,
                "The outgoing signed ECHO request must be mutated after normal signing.");
            BaseTestSite.Assert.IsTrue(
                transportSendCompleted,
                "The underlying transport must confirm successful submission of the mutated request.");
            BaseTestSite.Assert.IsTrue(
                onWireEvidenceVerified,
                "The serialized payload sent by the transport must match the post-signature mutation evidence.");
            BaseTestSite.Assert.IsTrue(
                rejectionResponseReceived,
                "The SUT must return a response for the malformed request; timeout or connection closure is not a pass criterion.");

            // Step 5: The server MAY disconnect after the rejection response, so connection survival is only a portable
            // pass criterion for an authoritatively identified Windows SUT, where the documented product behavior is that
            // the mismatched signature does not disconnect the connection.
            if (TestConfig.IsWindowsPlatform)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep,
                    "SUT is a Windows implementation: client sends a valid signed ECHO follow-up and expects STATUS_SUCCESS to prove the connection was retained (the mismatched signature does not disconnect the connection).");
                client.Echo(
                    treeId,
                    checker: (header, response) =>
                    {
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "A valid signed follow-up ECHO should succeed on a Windows SUT, proving the connection was retained after the invalid-signature rejection. Actually server returns {0}.",
                            Smb2Status.GetStatusCode(header.Status));
                    });

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client by sending the following requests: TREE_DISCONNECT; LOG_OFF");
                client.TreeDisconnect(treeId);
                client.LogOff();
            }
            else
            {
                // On non-Windows implementations, a disconnect after the rejection response is an optional post-response
                // action. Tear-down is therefore best-effort and any resulting exception is acceptable.
                BaseTestSite.Log.Add(LogEntryKind.TestStep,
                    "SUT is authoritatively configured as a non-Windows implementation: the required STATUS_ACCESS_DENIED response was received; " +
                    "connection survival is not asserted because a later disconnect is optional. Tear-down is best-effort.");
                try
                {
                    client.TreeDisconnect(treeId);
                    client.LogOff();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug,
                        "Best-effort tear-down after the rejection encountered an exception, which is acceptable because a post-rejection disconnect is optional for non-Windows implementations: {0}", ex.Message);
                }
            }
        }
        #endregion
    }
}
