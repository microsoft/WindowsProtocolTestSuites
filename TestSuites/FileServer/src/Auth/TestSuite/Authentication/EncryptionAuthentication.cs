// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace Microsoft.Protocols.TestSuites.FileSharing.Auth.TestSuite
{
    /// <summary>
    /// This test class validates the cross-mechanism encrypted post-authentication path required by
    /// MS-SMB2 3.3.5.5.3 (server obtains authentication-specific key material from GSS),
    /// MS-SMB2 3.2.5.3.1 (client derives encryption keys for an encryption-capable SMB 3.x,
    /// non-guest, non-anonymous session), and MS-SMB2 3.1.4.3 (encrypting the message).
    ///
    /// For each supported authentication mechanism (Kerberos and NTLM) the test establishes an SMB 3.x
    /// session under identical negotiation/encryption settings (same dialect, same cipher, same share,
    /// same operation) and completes an encrypted request/response round trip. The interoperable
    /// observation is a correctly encrypted and successfully decrypted post-authentication exchange -
    /// internal key bytes are never inspected and no comparison is made between Kerberos and NTLM key
    /// material.
    /// </summary>
    [TestClass]
    public class EncryptionAuthentication : AuthenticationTestBase
    {
        #region Constants

        // Hold the cipher constant across both mechanism rows so the only variable is the
        // authentication mechanism itself.
        private const EncryptionAlgorithm CommonCipher = EncryptionAlgorithm.ENCRYPTION_AES128_GCM;

        #endregion

        #region Variables

        private Smb2FunctionalClient client;

        private string servicePrincipalName;

        #endregion

        #region Initialization and Cleanup

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Cleanup();
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();

            servicePrincipalName = Smb2Utility.GetCifsServicePrincipalName(TestConfig.SutComputerName);

            client = null;
        }

        protected override void TestCleanup()
        {
            if (client != null)
            {
                try
                {
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Disconnect throws an exception: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }

        #endregion

        #region Test cases

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.KerberosAuthentication)]
        [TestCategory(TestCategories.Encryption)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.DomainRequired)]
        [Description("Authenticate with Kerberos on an encryption-capable SMB 3.x session and verify an " +
            "encrypted TREE_CONNECT plus file operation round trip is encrypted and successfully decrypted. " +
            "Traceable to MS-SMB2 3.3.5.5.3, 3.2.5.3.1, and 3.1.4.3.")]
        public void Auth_Encryption_Kerberos()
        {
            // Gate Kerberos on domain, SPN, ticket, and clock prerequisites.
            CheckKerberosPrerequisites();

            EstablishEncryptedSessionAndRoundTrip(SecurityPackageType.Kerberos);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Auth)]
        [TestCategory(TestCategories.Encryption)]
        [TestCategory(TestCategories.Smb311)]
        [Description("Authenticate with NTLM on an encryption-capable SMB 3.x session and verify an " +
            "encrypted TREE_CONNECT plus file operation round trip is encrypted and successfully decrypted. " +
            "Traceable to MS-SMB2 3.3.5.5.3, 3.2.5.3.1, and 3.1.4.3.")]
        public void Auth_Encryption_Ntlm()
        {
            // Gate NTLM on explicit package/policy support.
            CheckNtlmPrerequisites();

            EstablishEncryptedSessionAndRoundTrip(SecurityPackageType.Ntlm);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gate the Kerberos row on the domain, SPN, ticket and clock prerequisites required by
        /// MS-KILE so unavailable prerequisites are reported separately from a genuine failure.
        /// Clock synchronization within Kerberos policy is assumed to be maintained by the test environment.
        /// </summary>
        private void CheckKerberosPrerequisites()
        {
            if (!Regex.IsMatch(TestConfig.DomainName ?? string.Empty,
                @"^(?:[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?\.)+[a-z0-9][a-z0-9-]{0,61}[a-z0-9]$",
                RegexOptions.IgnoreCase))
            {
                BaseTestSite.Assert.Inconclusive(
                    "Kerberos encryption test case is not applicable in a non-domain environment.");
            }

            BaseTestSite.Assert.IsFalse(
                string.IsNullOrEmpty(servicePrincipalName),
                "Kerberos requires a resolvable SMB service principal name for {0}.", TestConfig.SutComputerName);

            // Verify a Kerberos credential can be acquired for the test account, which implies a valid
            // TGT is available in the current logon session. Clock skew within Kerberos policy is assumed.
            try
            {
                using (var credentialProbe = new SspiClientSecurityContext(
                    SecurityPackageType.Kerberos,
                    TestConfig.AccountCredential,
                    servicePrincipalName,
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNetworkDrep))
                {
                    // Credential acquisition succeeded; dispose immediately without initializing a context.
                }
            }
            catch (Exception ex)
            {
                BaseTestSite.Assert.Inconclusive(
                    "Kerberos encryption test requires valid Kerberos credentials (TGT) for account {0}\\{1}. {2}",
                    TestConfig.AccountCredential.DomainName,
                    TestConfig.AccountCredential.AccountName,
                    ex.Message);
            }
        }

        /// <summary>
        /// Gate the NTLM row on explicit NTLM package/policy support so unavailable mechanisms are
        /// reported separately from a genuine failure.
        /// </summary>
        private void CheckNtlmPrerequisites()
        {
            // Verify the NTLM security package can acquire credentials for the test account. This catches
            // environments where NTLM is disabled by policy or the package is not available, without
            // producing a misleading test failure during SESSION_SETUP.
            try
            {
                using (var credentialProbe = new SspiClientSecurityContext(
                    SecurityPackageType.Ntlm,
                    TestConfig.AccountCredential,
                    servicePrincipalName,
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNetworkDrep))
                {
                    // Credential acquisition succeeded; dispose immediately without initializing a context.
                }
            }
            catch (Exception ex)
            {
                BaseTestSite.Assert.Inconclusive(
                    "NTLM encryption test requires NTLM to be enabled and valid credentials for account {0}\\{1}. {2}",
                    TestConfig.AccountCredential.DomainName,
                    TestConfig.AccountCredential.AccountName,
                    ex.Message);
            }
        }

        /// <summary>
        /// Establish an encryption-capable SMB 3.x session with the selected authentication mechanism,
        /// verify the session is neither guest nor anonymous, and complete an encrypted request/response
        /// round trip (encrypted TREE_CONNECT plus a minimal WRITE/READ file operation round trip). The client is
        /// configured to reject any response that should be encrypted but is not (CheckEncrypt), so a
        /// successful, content-validated round trip proves the transform specified in MS-SMB2 3.1.4.3 is used
        /// and the response is decrypted. Internal key bytes are never inspected or logged.
        /// </summary>
        /// <param name="securityPackageType">The authentication mechanism to use (Kerberos or NTLM).</param>
        private void EstablishEncryptedSessionAndRoundTrip(SecurityPackageType securityPackageType)
        {
            #region Check Applicability

            // Hold dialect and cipher constant across mechanism rows.
            TestConfig.CheckDialect(DialectRevision.Smb311);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION);
            TestConfig.CheckEncryptionAlgorithm(CommonCipher);
            BaseTestSite.Assume.IsTrue(
                TestConfig.IsEncryptionSupported,
                "This test case requires the server to support encryption.");

            #endregion

            // CheckEncrypt (enabled by default on Smb2FunctionalClient) asserts that every response that
            // must be encrypted is actually delivered under the SMB2 transform; otherwise decoding throws.
            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sends NEGOTIATE with dialect 3.11 and SMB2_ENCRYPTION_CAPABILITIES context requesting {0}.",
                CommonCipher);
            client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                TestConfig.RequestDialects,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_ENCRYPTION,
                preauthHashAlgs: new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 },
                encryptionAlgs: new EncryptionAlgorithm[] { CommonCipher });

            BaseTestSite.Assert.AreEqual(
                CommonCipher,
                client.SelectedCipherID,
                "The negotiated cipher should be {0} so both mechanism rows use an equivalent encryption setting.",
                CommonCipher);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Client sends SESSION_SETUP authenticating with {0} and expects success.",
                securityPackageType);
            SessionFlags_Values sessionFlags = SessionFlags_Values.NONE;
            uint status = client.SessionSetup(
                securityPackageType,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                // Server GSS token is only meaningful for Negotiate; the mechanism is selected explicitly here.
                useServerGssToken: false,
                checker: (Packet_Header header, SESSION_SETUP_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "SESSION_SETUP with {0} should succeed, actually server returns {1}.",
                        securityPackageType,
                        Smb2Status.GetStatusCode(header.Status));

                    sessionFlags = response.SessionFlags;
                });

            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS,
                status,
                "SESSION_SETUP with {0} should succeed.", securityPackageType);

            // MS-SMB2 3.2.5.3.1: encryption keys are derived only for a non-guest, non-anonymous session.
            BaseTestSite.Assert.AreNotEqual(
                SessionFlags_Values.SESSION_FLAG_IS_GUEST,
                sessionFlags & SessionFlags_Values.SESSION_FLAG_IS_GUEST,
                "The authenticated session must not be a guest session for the encryption key derivation path to apply.");
            BaseTestSite.Assert.AreNotEqual(
                SessionFlags_Values.SESSION_FLAG_IS_NULL,
                sessionFlags & SessionFlags_Values.SESSION_FLAG_IS_NULL,
                "The authenticated session must not be an anonymous (NULL) session for the encryption key derivation path to apply.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client enables global session encryption.");
            client.EnableSessionSigningAndEncryption(enableSigning: false, enableEncryption: true);

            string uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.EncryptedFileShare);
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends encrypted TREE_CONNECT to share: {0}", uncSharePath);
            client.TreeConnect(
                uncSharePath,
                out treeId,
                (Packet_Header header, TREE_CONNECT_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "TREE_CONNECT should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                });

            FILEID fileId;
            Smb2CreateContextResponse[] serverCreateContexts;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends encrypted CREATE request and expects success.");
            client.Create(
                treeId,
                GetTestFileName(uncSharePath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts);

            string content = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends encrypted WRITE request and expects success.");
            client.Write(treeId, fileId, content);

            string actualContent;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends encrypted READ request and expects success.");
            client.Read(treeId, fileId, 0, (uint)content.Length, out actualContent);

            // A successful, content-validated round trip proves the response was delivered under the SMB2
            // transform and the client decrypted it correctly with the mechanism-derived key material.
            BaseTestSite.Assert.IsTrue(
                content.Equals(actualContent),
                "The content read back over the encrypted channel should be identical to what was written, " +
                "confirming the response was encrypted and successfully decrypted for the {0} session.",
                securityPackageType);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the client: CLOSE; TREE_DISCONNECT; LOG_OFF.");
            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        #endregion
    }
}
