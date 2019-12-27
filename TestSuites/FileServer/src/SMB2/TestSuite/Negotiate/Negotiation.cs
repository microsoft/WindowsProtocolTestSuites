// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    /// <summary>
    /// This test class is for testing negotiation using SMB3.0.
    /// </summary>
    [TestClass]
    public class Negotiation : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client;
        private uint status;
        public static List<DialectRevision> allDialects;
        #endregion

        #region Initilization
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
            status = 0;
            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Connect to server:" + TestConfig.SutComputerName);
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
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with Smb2 dialect wildcard.")]
        public void BVT_Negotiate_Compatible_Wildcard()
        {
            //Send wildcard revision number to verify if the server supports SMB2.1 or future dialect revisions.
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Send MultiProtocolNegotiate request with dialects: SMB 2.002, SMB 2.???");
            string[] dialects = new string[] { "SMB 2.002", "SMB 2.???" };
            bool isSmb2002Selected = false;
            status = client.MultiProtocolNegotiate(
                dialects,
                (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "The selected dialect is " + response.DialectRevision);
                    if (TestConfig.MaxSmbVersionSupported == DialectRevision.Smb2002)
                    {
                        BaseTestSite.Assert.AreEqual(
                            DialectRevision.Smb2002,
                            response.DialectRevision,
                            "The server is expected to use dialect: {0}", DialectRevision.Smb2002);
                        isSmb2002Selected = true;
                    }
                    else
                    {
                        BaseTestSite.Assert.AreEqual(
                            DialectRevision.Smb2Wildcard,
                            response.DialectRevision,
                            "The server is expected to use dialect: {0}", DialectRevision.Smb2Wildcard);
                    }
                });

            if (isSmb2002Selected)
            {
                return;
            }

            //According to server response, send new dialects for negotiation.
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                $"Send Negotiate request with dialects: {string.Join(",", TestConfig.RequestDialects)}");
            status = client.Negotiate(
                Packet_Header_Flags_Values.NONE,
                TestConfig.RequestDialects,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    CheckServerCapabilities(response);
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb21)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle compatible NEGOTIATE with dialect Smb 2.002.")]
        public void BVT_Negotiate_Compatible_2002()
        {
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Send MultiProtocolNegotiate request with dialects: SMB 2.002");
            string[] dialects = new string[] { "SMB 2.002" };
            status = client.MultiProtocolNegotiate(
                dialects,
                (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "The selected dialect is " + response.DialectRevision);
                    BaseTestSite.Assert.AreEqual(
                        DialectRevision.Smb2002,
                        response.DialectRevision,
                        "The server is expected to use dialect: {0}", DialectRevision.Smb2002);
                    CheckServerCapabilities(response);
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with Signing Required.")]
        public void BVT_Negotiate_SigningEnabled()
        {
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                $"Send Negotiate request with dialects: {string.Join(",", TestConfig.RequestDialects)} and Signing Required.");
            status = client.Negotiate(
                Packet_Header_Flags_Values.NONE,
                TestConfig.RequestDialects,
                securityMode: SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    CheckServerCapabilities(response);
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with Smb 2.002 dialect.")]
        public void BVT_Negotiate_SMB2002()
        {
            NegotiateWithSpecificDialect(DialectRevision.Smb2002);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with Smb 2.1 dialect.")]
        public void BVT_Negotiate_SMB21()
        {
            NegotiateWithSpecificDialect(DialectRevision.Smb21);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with Smb 3.0 dialect.")]
        public void BVT_Negotiate_SMB30()
        {
            NegotiateWithSpecificDialect(DialectRevision.Smb30);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with Smb 3.02 dialect.")]
        public void BVT_Negotiate_SMB302()
        {
            NegotiateWithSpecificDialect(DialectRevision.Smb302);
        }


        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server (including the server doesn't implement dialect 3.11) " +
            " can handle NEGOTIATE with SMB 3.11 dialect and Negotiate Contexts.")]
        public void BVT_Negotiate_SMB311()
        {
            DialectRevision clientMaxDialectSupported = DialectRevision.Smb311;
            PreauthIntegrityHashID[] preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
            EncryptionAlgorithm[] encryptionAlgs = new EncryptionAlgorithm[] {
                EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                EncryptionAlgorithm.ENCRYPTION_AES128_CCM };

            BaseTestSite.Log.Add(
               LogEntryKind.TestStep,
               "Send Negotiate request with dialect SMB 3.11, SMB2_PREAUTH_INTEGRITY_CAPABILITIES context and " +
               "SMB2_ENCRYPTION_CAPABILITIES context.");
            NegotiateWithNegotiateContexts(
                clientMaxDialectSupported,
                preauthHashAlgs,
                encryptionAlgs,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    CheckNegotiateResponse(header, response, clientMaxDialectSupported, encryptionAlgs);
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with " +
            "Smb 3.11 dialect and SMB2_PREAUTH_INTEGRITY_CAPABILITIES context")]
        public void BVT_Negotiate_SMB311_Preauthentication()
        {
            DialectRevision clientMaxDialectSupported = DialectRevision.Smb311;
            PreauthIntegrityHashID[] preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };

            if (TestConfig.MaxSmbVersionSupported < DialectRevision.Smb311)
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured server max dialect is lower than 3.11.");

            BaseTestSite.Log.Add(
               LogEntryKind.TestStep,
               "Send Negotiate request with dialect SMB 3.11 and SMB2_PREAUTH_INTEGRITY_CAPABILITIES context.");
            NegotiateWithNegotiateContexts(
                clientMaxDialectSupported,
                preauthHashAlgs,
                null,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    CheckNegotiateResponse(header, response, clientMaxDialectSupported, null);
                });
        }


        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with " +
            "Smb 3.11 dialect and SMB2_PREAUTH_INTEGRITY_CAPABILITIES context and " +
            "SMB2_ENCRYPTION_CAPABILITIES context with AES-128-CCM preferred.")]
        public void BVT_Negotiate_SMB311_Preauthentication_Encryption_CCM()
        {
            if (TestConfig.MaxSmbVersionSupported < DialectRevision.Smb311)
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured server max dialect is lower than 3.11.");

            DialectRevision clientMaxDialectSupported = DialectRevision.Smb311;
            PreauthIntegrityHashID[] preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
            EncryptionAlgorithm[] encryptionAlgs = new EncryptionAlgorithm[] {
                EncryptionAlgorithm.ENCRYPTION_AES128_CCM,
                EncryptionAlgorithm.ENCRYPTION_AES128_GCM };

            BaseTestSite.Log.Add(
               LogEntryKind.TestStep,
               "Send Negotiate request with dialect SMB 3.11, SMB2_PREAUTH_INTEGRITY_CAPABILITIES context and " +
               "SMB2_ENCRYPTION_CAPABILITIES context with AES-128-CCM preferred.");
            NegotiateWithNegotiateContexts(
                clientMaxDialectSupported,
                preauthHashAlgs,
                encryptionAlgs,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    CheckNegotiateResponse(header, response, clientMaxDialectSupported, encryptionAlgs);
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with " +
            "Smb 3.11 dialect and SMB2_PREAUTH_INTEGRITY_CAPABILITIES context and " +
            "SMB2_ENCRYPTION_CAPABILITIES context with AES-128-GCM preferred.")]
        public void BVT_Negotiate_SMB311_Preauthentication_Encryption_GCM()
        {
            if (TestConfig.MaxSmbVersionSupported < DialectRevision.Smb311)
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured server max dialect is lower than 3.11.");

            DialectRevision clientMaxDialectSupported = DialectRevision.Smb311;
            PreauthIntegrityHashID[] preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
            EncryptionAlgorithm[] encryptionAlgs = new EncryptionAlgorithm[] {
                EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                EncryptionAlgorithm.ENCRYPTION_AES128_CCM };

            BaseTestSite.Log.Add(
               LogEntryKind.TestStep,
               "Send Negotiate request with dialect SMB 3.11, SMB2_PREAUTH_INTEGRITY_CAPABILITIES context and " +
               "SMB2_ENCRYPTION_CAPABILITIES context with AES-128-GCM preferred.");
            NegotiateWithNegotiateContexts(
                clientMaxDialectSupported,
                preauthHashAlgs,
                encryptionAlgs,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    CheckNegotiateResponse(header, response, clientMaxDialectSupported, encryptionAlgs);
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with " +
            "Smb 3.11 dialect and with all the three Negotiate Contexts.")]
        public void Negotiate_SMB311_WithAllContexts()
        {
            if (TestConfig.MaxSmbVersionSupported < DialectRevision.Smb311)
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured server max dialect is lower than 3.11.");

            var encryptionAlgs = new EncryptionAlgorithm[] {
                EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                EncryptionAlgorithm.ENCRYPTION_AES128_CCM };
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send Negotiate request with dialect SMB 3.11, and with all the three Negotiate Contexts.");
            client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                Smb2Utility.GetDialects(DialectRevision.Smb311),
                preauthHashAlgs: new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 },
                encryptionAlgs: encryptionAlgs,
                compressionAlgorithms: TestConfig.SupportedCompressionAlgorithmList.ToArray(),
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    CheckNegotiateResponse(header, response, DialectRevision.Smb311, encryptionAlgs, TestConfig.IsCompressionSupported());
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with " +
            "Smb 3.11 dialect and without any Negotiate Contexts.")]
        public void Negotiate_SMB311_WithoutAnyContexts()
        {
            if (TestConfig.MaxSmbVersionSupported < DialectRevision.Smb311)
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured server max dialect is lower than 3.11.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send Negotiate request with dialect SMB 3.11, and without any Negotiate Contexts.");
            client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                Smb2Utility.GetDialects(DialectRevision.Smb311),
                preauthHashAlgs: null,
                encryptionAlgs: null,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server fails the negotiate request with STATUS_INVALID_PARAMETER.");
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_INVALID_PARAMETER, header.Status,
                        "[MS-SMB2] 3.3.5.4 If the negotiate context list does not contain exactly one SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context, " +
                        "then the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.");
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with " +
            "Smb 3.11 dialect, with SMB2_ENCRYPTION_CAPABILITIES Context and without SMB2_PREAUTH_INTEGRITY_CAPABILITIES Context.")]
        public void Negotiate_SMB311_WithEncryptionContextWithoutIntegrityContext()
        {
            if (TestConfig.MaxSmbVersionSupported < DialectRevision.Smb311)
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured server max dialect is lower than 3.11.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send Negotiate request with dialect SMB 3.11, with SMB2_ENCRYPTION_CAPABILITIES context, " +
                "without SMB2_PREAUTH_INTEGRITY_CAPABILITIES context.");
            client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                Smb2Utility.GetDialects(DialectRevision.Smb311),
                preauthHashAlgs: null,
                encryptionAlgs: new EncryptionAlgorithm[] {
                EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                EncryptionAlgorithm.ENCRYPTION_AES128_CCM },
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server fails the negotiate request with STATUS_INVALID_PARAMETER.");
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_INVALID_PARAMETER, header.Status,
                        "[MS-SMB2] 3.3.5.4 If the negotiate context list does not contain exactly one SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context, " +
                        "then the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.");
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with " +
            "Smb 3.11 dialect, with SMB2_COMPRESSION_CAPABILITIES Context and without SMB2_PREAUTH_INTEGRITY_CAPABILITIES Context.")]
        public void Negotiate_SMB311_WithCompressionContextWithoutIntegrityContext()
        {
            if (TestConfig.MaxSmbVersionSupported < DialectRevision.Smb311)
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured server max dialect is lower than 3.11.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send Negotiate request with dialect SMB 3.11, with SMB2_COMPRESSION_CAPABILITIES context, " +
                "without SMB2_PREAUTH_INTEGRITY_CAPABILITIES context.");
            client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                Smb2Utility.GetDialects(DialectRevision.Smb311),
                preauthHashAlgs: null,
                encryptionAlgs: null,
                compressionAlgorithms: TestConfig.SupportedCompressionAlgorithmList.ToArray(),
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server fails the negotiate request with STATUS_INVALID_PARAMETER.");
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_INVALID_PARAMETER, header.Status,
                        "[MS-SMB2] 3.3.5.4 If the negotiate context list does not contain exactly one SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context, " +
                        "then the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.");
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with " +
            "Smb 3.11 dialect, with SMB2_ENCRYPTION_CAPABILITIES and SMB2_COMPRESSION_CAPABILITIES Contexts and without SMB2_PREAUTH_INTEGRITY_CAPABILITIES Context.")]
        public void Negotiate_SMB311_WithEncryptionAndCompressionContextsWithoutIntegrityContext()
        {
            if (TestConfig.MaxSmbVersionSupported < DialectRevision.Smb311)
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured server max dialect is lower than 3.11.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send Negotiate request with dialect SMB 3.11, with SMB2_ENCRYPTION_CAPABILITIES and SMB2_COMPRESSION_CAPABILITIES context, " +
                "without SMB2_PREAUTH_INTEGRITY_CAPABILITIES context.");
            client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                Smb2Utility.GetDialects(DialectRevision.Smb311),
                preauthHashAlgs: null,
                encryptionAlgs: new EncryptionAlgorithm[] {
                EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                EncryptionAlgorithm.ENCRYPTION_AES128_CCM },
                compressionAlgorithms: TestConfig.SupportedCompressionAlgorithmList.ToArray(),
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server fails the negotiate request with STATUS_INVALID_PARAMETER.");
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_INVALID_PARAMETER, header.Status,
                        "[MS-SMB2] 3.3.5.4 If the negotiate context list does not contain exactly one SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context, " +
                        "then the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.");
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with " +
            "Smb 3.11 dialect and with an invalid HashAlgorithm in SMB2_PREAUTH_INTEGRITY_CAPABILITIES Context.")]
        public void Negotiate_SMB311_InvalidHashAlgorithm()
        {
            if (TestConfig.MaxSmbVersionSupported < DialectRevision.Smb311)
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured server max dialect is lower than 3.11.");

            PreauthIntegrityHashID[] preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.HashAlgorithm_Invalid };

            BaseTestSite.Log.Add(
               LogEntryKind.TestStep,
               "Send Negotiate request with dialect SMB 3.11, SMB2_PREAUTH_INTEGRITY_CAPABILITIES context and " +
               "set HashAlgorithm to an invalid value: 0xFFFF.");
            NegotiateWithNegotiateContexts(
                DialectRevision.Smb311,
                preauthHashAlgs,
                encryptionAlgs: null,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server fails the negotiate request with STATUS_SMB_NO_PREAUTH_INTEGRITY_HASH_OVERLAP.");
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SMB_NO_PREAUTH_INTEGRITY_HASH_OVERLAP, header.Status,
                        "[MS-SMB2] 3.3.5.4 If the SMB2_PREAUTH_INTEGRITY_CAPABILITIES HashAlgorithms array does not contain any hash algorithms " +
                        "that the server supports, then the server MUST fail the negotiate request with STATUS_SMB_NO_PREAUTH_INTEGRITY_HASH_OVERLAP (0xC05D0000).");
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with " +
            "Smb 3.11 dialect and with an invalid Cipher in SMB2_ENCRYPTION_CAPABILITIES Context.")]
        public void Negotiate_SMB311_InvalidCipher()
        {
            if (TestConfig.MaxSmbVersionSupported < DialectRevision.Smb311)
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured server max dialect is lower than 3.11.");

            EncryptionAlgorithm[] encryptionAlgs = new EncryptionAlgorithm[] {
                EncryptionAlgorithm.ENCRYPTION_INVALID };

            BaseTestSite.Log.Add(
               LogEntryKind.TestStep,
               "Send Negotiate request with dialect SMB 3.11, SMB2_ENCRYPTION_CAPABILITIES context and " +
               "set Cipher to an invalid value: 0xFFFF.");
            NegotiateWithNegotiateContexts(
                DialectRevision.Smb311,
                new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 },
                encryptionAlgs,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify that server sets Connection.CipherId to 0 from the response.");
                    BaseTestSite.Assert.AreEqual((EncryptionAlgorithm)0, client.SelectedCipherID,
                        "[MS-SMB2] 3.3.5.4 If the client and server have no common cipher, then the server must set Connection.CipherId to 0.");
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with supported compression algorithms in SMB2_COMPRESSION_CAPABILITIES context.")]
        public void BVT_Negotiate_SMB311_CompressionEnabled()
        {
            CheckCompressionApplicability();

            var compressionAlgorithms = TestConfig.SupportedCompressionAlgorithmList.ToArray();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send NEGOTIATE request with SUT supported compression algorithms.");
            NegotiateWithNegotiateContexts(
                DialectRevision.Smb311,
                new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 },
                compressionAlgorithms: compressionAlgorithms,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status, "SUT MUST return STATUS_SUCCESS if the negotiation finished successfully.");

                    if (TestConfig.IsWindowsPlatform)
                    {
                        bool isExpectedWindowsCompressionContext = client.Smb2Client.CompressionInfo.CompressionIds.Length == 1 && client.Smb2Client.CompressionInfo.CompressionIds[0] == TestConfig.SupportedCompressionAlgorithmList[0];
                        BaseTestSite.Assert.IsTrue(isExpectedWindowsCompressionContext, "Windows 10 v1903 and later and Windows Server v1903 and later only set CompressionAlgorithms to the first common algorithm supported by the client and server.");
                    }
                    else
                    {
                        bool isExpectedNonWindowsCompressionContext = Enumerable.SequenceEqual(client.Smb2Client.CompressionInfo.CompressionIds, TestConfig.SupportedCompressionAlgorithmList);
                        BaseTestSite.Assert.IsTrue(isExpectedNonWindowsCompressionContext, "[MS-SMB2] section 3.3.5.4: Non-Windows implementation MUST set CompressionAlgorithms to the CompressionIds in request if they are all supported by SUT.");
                    }
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [Description("This test case is designed to test whether server can handle NEGOTIATE with unsupported compression algorithms in SMB2_COMPRESSION_CAPABILITIES context.")]
        public void Negotiate_SMB311_Compression_CompressionAlgorithmNotSupported()
        {
            CheckCompressionApplicability();

            var unsupportedCompressionAlgorithms = new CompressionAlgorithm[] { CompressionAlgorithm.Unsupported };

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send NEGOTIATE request with SUT unsupported compression algorithms.");
            NegotiateWithNegotiateContexts(
                DialectRevision.Smb311,
                new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 },
                compressionAlgorithms: unsupportedCompressionAlgorithms,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status, "SUT MUST return STATUS_SUCCESS if the negotiation finished successfully.");

                    bool isExpectedCompressionContext = client.Smb2Client.CompressionInfo.CompressionIds.Length == 1 && client.Smb2Client.CompressionInfo.CompressionIds[0] == CompressionAlgorithm.NONE;

                    BaseTestSite.Assert.IsTrue(
                        isExpectedCompressionContext,
                        "[MS-SMB2] section 3.3.5.4: If the server does not support any of the algorithms provided by the client, " +
                        "the server MUST build an SMB2_COMPRESSION_CAPABILITIES negotiate response context with CompressionAlgorithmCount set to 1 and CompressionAlgorithms set to \"NONE\"."
                        );
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is designed to test whether server ignores SMB2_COMPRESSION_CAPABILITIES context when negotiate with SMB dialect less than 3.1.1.")]
        public void Negotiate_SMB311_Compression_InvalidSmbDialect()
        {
            CheckCompressionApplicability();

            var negotiateDialects = Smb2Utility.GetDialects(DialectRevision.Smb302);

            var compressionAlgorithms = TestConfig.SupportedCompressionAlgorithmList.ToArray();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send NEGOTIATE request with SMB dialects less than 3.1.1.");
            client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                negotiateDialects,
                preauthHashAlgs: null,
                compressionAlgorithms: compressionAlgorithms,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status, "SUT MUST return STATUS_SUCCESS if the negotiation finished successfully.");

                    bool isExpectedCompressionContext = client.Smb2Client.CompressionInfo.CompressionIds.Length == 0;

                    BaseTestSite.Assert.IsTrue(isExpectedCompressionContext, "SUT MUST ignore SMB2_COMPRESSION_CAPABILITIES if negotiated SMB dialect is less than 3.1.1.");

                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is designed to test whether server handle SMB2_COMPRESSION_CAPABILITIES context with invalid DataLength field when negotiate.")]
        public void Negotiate_SMB311_Compression_CompressionContextInvalidDataLength()
        {
            CheckCompressionApplicability();

            var compressionAlgorithms = TestConfig.SupportedCompressionAlgorithmList.ToArray();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send NEGOTIATE request with invalid DataLength field, which is less than the size of SMB2_COMPRESSION_CAPABILITIES structure, in SMB2_COMPRESSION_CAPABILITIES context.");

            Action<Smb2Packet> negotiatePacketModifier = packet =>
            {
                if (packet is Smb2NegotiateRequestPacket)
                {
                    var negotiateRequest = packet as Smb2NegotiateRequestPacket;

                    // Modify DataLength to 4 which must be less than the size of SMB2_COMPRESSION_CAPABILITIES
                    var compressionContext = negotiateRequest.NegotiateContext_COMPRESSION.Value;
                    compressionContext.Header.DataLength = 4;

                    negotiateRequest.NegotiateContext_COMPRESSION = compressionContext;
                }
            };

            client.Smb2Client.PacketSending += negotiatePacketModifier;

            NegotiateWithNegotiateContexts(
                DialectRevision.Smb311,
                new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 },
                compressionAlgorithms: compressionAlgorithms,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_INVALID_PARAMETER,
                        header.Status,
                        "[MS-SMB2] section 3.3.5.4: The server MUST fail the negotiate request with STATUS_INVALID_PARAMETER, " +
                        "if the DataLength of the negotiate context is less than the size of the SMB2_COMPRESSION_CAPABILITIES structure."
                        );
                });

            client.Smb2Client.PacketSending -= negotiatePacketModifier;
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is designed to test whether server handle SMB2_COMPRESSION_CAPABILITIES context with invalid CompressionAlgorithmCount field when negotiate.")]
        public void Negotiate_SMB311_Compression_CompressionAlgorithmEmpty()
        {
            CheckCompressionApplicability();

            var compressionAlgorithms = new CompressionAlgorithm[0];

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send NEGOTIATE request with invalid CompressionAlgorithmCount field, which equals to zero, in SMB2_COMPRESSION_CAPABILITIES.");
            NegotiateWithNegotiateContexts(
                DialectRevision.Smb311,
                new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 },
                compressionAlgorithms: compressionAlgorithms,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_INVALID_PARAMETER, header.Status, "[MS-SMB2] section 3.3.5.4: The server MUST fail the negotiate request with STATUS_INVALID_PARAMETER, if CompressionAlgorithmCount is equal to zero.");
                });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Negotiate)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("This test case is designed to test whether server can ignore SMB2_NETNAME_NEGOTIATE_CONTEXT_ID context when negotiate.")]
        public void Negotiate_SMB311_ContextID_NetName()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send NEGOTIATE request with SMB2_NETNAME_NEGOTIATE_CONTEXT_ID context.");

            DialectRevision clientMaxDialectSupported = DialectRevision.Smb311;
            PreauthIntegrityHashID[] preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
            EncryptionAlgorithm[] encryptionAlgs = new EncryptionAlgorithm[] {
                EncryptionAlgorithm.ENCRYPTION_AES128_GCM,
                EncryptionAlgorithm.ENCRYPTION_AES128_CCM };

            BaseTestSite.Log.Add(
               LogEntryKind.TestStep,
               "Send Negotiate request with dialect SMB 3.11, SMB2_PREAUTH_INTEGRITY_CAPABILITIES context and " +
               "SMB2_ENCRYPTION_CAPABILITIES context and SMB2_NETNAME_NEGOTIATE_CONTEXT_ID context.");

            NegotiateWithNegotiateContexts(
                clientMaxDialectSupported,
                preauthHashAlgs,
                encryptionAlgs,
                addNetNameContextId: true,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                      {
                          BaseTestSite.Assert.AreEqual(
                             Smb2Status.STATUS_SUCCESS,
                             header.Status,
                             "[MS-SMB2] section 2.2.3.1.3: The SMB2_NETNAME_NEGOTIATE_CONTEXT_ID context is specified in an SMB2 NEGOTIATE request to indicate the server name the client connects to. The server MUST ignore this context. "
                             );
                      }
                );
        }
        #endregion

        #region private methods
        private void NegotiateWithSpecificDialect(DialectRevision clientMaxDialectSupported)
        {
            if (clientMaxDialectSupported > TestConfig.MaxSmbVersionClientSupported)
            {
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured MaxSmbVersionClientSupported {0} is lower than {1}.",
                    TestConfig.MaxSmbVersionClientSupported,
                    clientMaxDialectSupported);
            }

            DialectRevision serverMaxDialectSupported = TestConfig.MaxSmbVersionSupported;

            DialectRevision[] negotiateDialects = Smb2Utility.GetDialects(clientMaxDialectSupported);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send Negotiate request with maximum dialect: {0}.", clientMaxDialectSupported);
            client.Negotiate(
                Packet_Header_Flags_Values.NONE,
                negotiateDialects,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    DialectRevision expectedDialect = clientMaxDialectSupported < serverMaxDialectSupported
                        ? clientMaxDialectSupported : serverMaxDialectSupported;

                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Check negotiate response contains expected dialect.");
                    BaseTestSite.Assert.AreEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "{0} should succeed, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                    BaseTestSite.Assert.AreEqual(expectedDialect, response.DialectRevision, "Selected dialect should be {0}", expectedDialect);
                    CheckServerCapabilities(response);
                });
        }

        private void NegotiateWithNegotiateContexts(
            DialectRevision clientMaxDialectSupported,
            PreauthIntegrityHashID[] preauthHashAlgs,
            EncryptionAlgorithm[] encryptionAlgs = null,
            CompressionAlgorithm[] compressionAlgorithms = null,
            bool addNetNameContextId = false,
            ResponseChecker<NEGOTIATE_Response> checker = null)
        {
            // ensure clientMaxDialectSupported higher than 3.11
            if (clientMaxDialectSupported < DialectRevision.Smb311) clientMaxDialectSupported = DialectRevision.Smb311;
            if (clientMaxDialectSupported > TestConfig.MaxSmbVersionClientSupported)
            {
                BaseTestSite.Assume.Inconclusive("Stop to run this test case because the configured MaxSmbVersionClientSupported {0} is lower than {1}.",
                    TestConfig.MaxSmbVersionClientSupported,
                    clientMaxDialectSupported);
            }

            DialectRevision[] negotiateDialects = Smb2Utility.GetDialects(clientMaxDialectSupported);

            status = client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                negotiateDialects,
                preauthHashAlgs: preauthHashAlgs,
                encryptionAlgs: encryptionAlgs,
                compressionAlgorithms: compressionAlgorithms,
                addNetNameContextId: addNetNameContextId,
                checker: checker);
        }

        private void CheckNegotiateResponse(
            Packet_Header header,
            NEGOTIATE_Response response,
            DialectRevision clientMaxDialectSupported,
            EncryptionAlgorithm[] encryptionAlgs,
            bool checkCompression = false)
        {
            DialectRevision expectedDialect = clientMaxDialectSupported < TestConfig.MaxSmbVersionSupported
                        ? clientMaxDialectSupported : TestConfig.MaxSmbVersionSupported;

            BaseTestSite.Assert.AreEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "{0} should succeed, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
            BaseTestSite.Assert.AreEqual(expectedDialect, response.DialectRevision, "Selected dialect should be {0}", expectedDialect);

            if (expectedDialect >= DialectRevision.Smb311)
            {
                BaseTestSite.Assert.AreEqual(
                    PreauthIntegrityHashID.SHA_512,
                    client.SelectedPreauthIntegrityHashID,
                    "[MS-SMB2] 3.3.5.4 The server MUST set Connection.PreauthIntegrityHashId to one of the hash algorithms " +
                    "in the client's SMB2_PREAUTH_INTEGRITY_CAPABILITIES HashAlgorithms array.");

                if (encryptionAlgs != null)
                {
                    BaseTestSite.Assert.IsTrue(
                        TestConfig.SupportedEncryptionAlgorithmList.Contains(client.SelectedCipherID),
                        "[MS-SMB2] 3.3.5.4 The server MUST set Connection.CipherId to one of the ciphers in the client's " +
                        "SMB2_ENCRYPTION_CAPABILITIES Ciphers array in an implementation-specific manner.");
                }
                else
                {
                    BaseTestSite.Assert.AreEqual(
                        EncryptionAlgorithm.ENCRYPTION_NONE,
                        client.SelectedCipherID,
                        "[MS-SMB2] if client doesn't present SMB2_ENCRYPTION_CAPABILITIES context in negotiate request, " +
                        "server should not present this context in negotiate response.");
                }

                if (checkCompression)
                {
                    if (TestConfig.IsWindowsPlatform)
                    {
                        bool isExpectedWindowsCompressionContext = client.Smb2Client.CompressionInfo.CompressionIds.Length == 1 && client.Smb2Client.CompressionInfo.CompressionIds[0] == TestConfig.SupportedCompressionAlgorithmList[0];
                        BaseTestSite.Assert.IsTrue(isExpectedWindowsCompressionContext, "Windows 10 v1903 and later and Windows Server v1903 and later only set CompressionAlgorithms to the first common algorithm supported by the client and server.");
                    }
                    else
                    {
                        bool isExpectedNonWindowsCompressionContext = Enumerable.SequenceEqual(client.Smb2Client.CompressionInfo.CompressionIds, TestConfig.SupportedCompressionAlgorithmList);
                        BaseTestSite.Assert.IsTrue(isExpectedNonWindowsCompressionContext, "[MS-SMB2] section 3.3.5.4: Non-Windows implementation MUST set CompressionAlgorithms to the CompressionIds in request if they are all supported by SUT.");
                    }
                }
            }
            else
            {
                // If server supported dialect version is lower than 3.11, server should ignore the negotiate contexts.
                BaseTestSite.Assert.AreEqual(
                    PreauthIntegrityHashID.HashAlgorithm_NONE,
                    client.SelectedPreauthIntegrityHashID,
                    "[MS-SMB2] The server must ignore the SMB2_PREAUTH_INTEGRITY_CAPABILITIES context if Connection.Dialect is less than 3.11. ");
                BaseTestSite.Assert.AreEqual(
                    EncryptionAlgorithm.ENCRYPTION_NONE,
                    client.SelectedCipherID,
                    "[MS-SMB2] The server must ignore the SMB2_ENCRYPTION_CAPABILITIES context if Connection.Dialect is less than 3.11. ");
            }

            CheckServerCapabilities(response);
        }

        private void CheckServerCapabilities(NEGOTIATE_Response response)
        {
            // Check capability: Leasing
            if (TestConfig.IsLeasingSupported
                && response.DialectRevision != DialectRevision.Smb2002)
            {
                BaseTestSite.Assert.AreEqual<NEGOTIATE_Response_Capabilities_Values>(
                    NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING,
                    response.Capabilities & NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING,
                    "The \"Capabilities\" of Negotiate Response should has flag GLOBAL_CAP_LEASING set.");
            }

            // Check capability: Large MTU
            if (TestConfig.IsMultiCreditSupported
                && response.DialectRevision != DialectRevision.Smb2002)
            {
                BaseTestSite.Assert.AreEqual<NEGOTIATE_Response_Capabilities_Values>(
                    NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LARGE_MTU,
                    response.Capabilities & NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LARGE_MTU,
                    "The \"Capabilities\" of Negotiate Response should has flag GLOBAL_CAP_LARGE_MTU set.");
            }

            // Check capability: MultiChannel
            if (TestConfig.IsMultiChannelCapable
                && response.DialectRevision != DialectRevision.Smb2002
                && response.DialectRevision != DialectRevision.Smb21)
            {
                BaseTestSite.Assert.AreEqual<NEGOTIATE_Response_Capabilities_Values>(
                    NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL,
                    response.Capabilities & NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL,
                    "The \"Capabilities\" of Negotiate Response should has flag GLOBAL_CAP_MULTI_CHANNEL set.");
            }

            // Check capability: Persistent Handle
            if (TestConfig.IsPersistentHandlesSupported
                && response.DialectRevision != DialectRevision.Smb2002
                && response.DialectRevision != DialectRevision.Smb21)
            {
                BaseTestSite.Assert.AreEqual<NEGOTIATE_Response_Capabilities_Values>(
                    NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                    response.Capabilities & NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                    "The \"Capabilities\" of Negotiate Response should has flag GLOBAL_CAP_PERSISTENT_HANDLES set.");
            }

            // Check capability: Directory Leasing
            if (TestConfig.IsDirectoryLeasingSupported
                && response.DialectRevision != DialectRevision.Smb2002
                && response.DialectRevision != DialectRevision.Smb21)
            {
                BaseTestSite.Assert.AreEqual<NEGOTIATE_Response_Capabilities_Values>(
                    NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING,
                    response.Capabilities & NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING,
                    "The \"Capabilities\" of Negotiate Response should has flag GLOBAL_CAP_DIRECTORY_LEASING set.");
            }

            // Check capability: Encryption
            if (TestConfig.IsEncryptionSupported
                && response.DialectRevision == DialectRevision.Smb30
                && response.DialectRevision == DialectRevision.Smb302)
            {
                BaseTestSite.Assert.AreEqual<NEGOTIATE_Response_Capabilities_Values>(
                    NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION,
                    response.Capabilities & NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION,
                    "The \"Capabilities\" of Negotiate Response should has flag GLOBAL_CAP_ENCRYPTION set.");
            }
        }

        private void CheckCompressionApplicability()
        {
            // Check platform
            if (TestConfig.IsWindowsPlatform)
            {
                BaseTestSite.Assume.IsFalse(TestConfig.Platform < Platform.WindowsServerV1903, "Windows 10 v1809 operating system and prior, Windows Server v1809 operating system and prior, and Windows Server 2019 and prior do not support compression.");
            }

            // Check dialect
            BaseTestSite.Assume.IsTrue(TestConfig.MaxSmbVersionSupported >= DialectRevision.Smb311, "The SMB 3.1.1 dialect introduces supporting the compression of messages between client and server.");

            // Check SUT supported compression algorithms
            TestConfig.CheckCompressionAlgorithm();
        }
        #endregion
    }
}
