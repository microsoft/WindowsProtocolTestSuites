// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public partial class Compression : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client;
        private string uncSharePath;
        #endregion

        #region Test Initialize and Cleanup
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
        #endregion

        #region Test Case Initialize and Clean up
        protected override void TestInitialize()
        {
            base.TestInitialize();
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);

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
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "Unexpected exception when disconnect client: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }

        #endregion

        #region Check IsGlobalEncryptDataEnabled

        /// <summary>
        /// Check compression test case applicability for EncryptData SMB server configuration. 
        /// Unencrypted accesses from clients that do not support SMB 3.0 and above are allowed when IsGlobalEncryptDataEnabled is true and IsGlobalRejectUnencryptedAccessEnabled is false.
        /// Or unencrypted accesses from any clients are allowed when IsGlobalEncryptDataEnabled is false.
        /// </summary>
        private void CheckCompressionTestCaseApplicabilityForGlobalEncryptData(string reason = null)
        {
            if (testConfig.IsGlobalEncryptDataEnabled)
            {
                if (testConfig.IsGlobalRejectUnencryptedAccessEnabled)
                {
                    if (string.IsNullOrEmpty(reason))
                    {
                        Site.Assert.Inconclusive("This test case is for compressed packets or chained compressed packets and not applicable to encrypted packets.");
                    }
                    else
                    {
                        Site.Assert.Inconclusive(reason);
                    }
                }
                else
                {
                    BaseTestSite.Assume.IsTrue(TestConfig.MaxSmbVersionClientSupported < DialectRevision.Smb30, "When IsGlobalRejectUnencryptedAccessEnabled is false, it will allow unencrypted accesses from clients that do not support SMB 3.0 and above.");
                }
            }
        }
        #endregion

        #region Test cases
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [Description("This test case is designed to test whether server can decompress WRITE request and compress READ response correctly using LZNT1.")]
        public void BVT_SMB2Compression_LZNT1()
        {
            var compressionAlgorithm = CompressionAlgorithm.LZNT1;
            BasicSMB2Compression(compressionAlgorithm);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [Description("This test case is designed to test whether server can decompress WRITE request and compress READ response correctly using LZ77.")]
        public void BVT_SMB2Compression_LZ77()
        {
            var compressionAlgorithm = CompressionAlgorithm.LZ77;
            BasicSMB2Compression(compressionAlgorithm);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [Description("This test case is designed to test whether server can decompress WRITE request and compress READ response correctly using LZ77 Huffman.")]
        public void BVT_SMB2Compression_LZ77Huffman()
        {
            var compressionAlgorithm = CompressionAlgorithm.LZ77Huffman;
            BasicSMB2Compression(compressionAlgorithm);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [Description("This test case is designed to test whether server can decompress WRITE request and compress READ response correctly using LZNT1 when encryption is enabled.")]
        public void BVT_SMB2Compression_LZNT1_Encrypted()
        {
            var compressionAlgorithm = CompressionAlgorithm.LZNT1;
            BasicSMB2Compression_Encrypted(compressionAlgorithm);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [Description("This test case is designed to test whether server can decompress WRITE request and compress READ response correctly using LZ77 when encryption is enabled.")]
        public void BVT_SMB2Compression_LZ77_Encrypted()
        {
            var compressionAlgorithm = CompressionAlgorithm.LZ77;
            BasicSMB2Compression_Encrypted(compressionAlgorithm);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [Description("This test case is designed to test whether server can decompress WRITE request and compress READ response correctly using LZ77 Huffman when encryption is enabled.")]
        public void BVT_SMB2Compression_LZ77Huffman_Encrypted()
        {
            var compressionAlgorithm = CompressionAlgorithm.LZ77Huffman;
            BasicSMB2Compression_Encrypted(compressionAlgorithm);
        }


        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle compressed WRITE request correctly using supported compression algorithms.")]
        public void SMB2Compression_CompressedWriteRequest()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2Compression_Variant(CompressionTestVariant.CompressibleWrite);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can compress read request correctly if SMB2_READFLAG_REQUEST_COMPRESSED is specified in request and response is compressible.")]
        public void SMB2Compression_CompressibleReadResponse()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2Compression_Variant(CompressionTestVariant.CompressibleRead);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server will not compress read request if SMB2_READFLAG_REQUEST_COMPRESSED is specified in request and response is incompressible.")]
        public void SMB2Compression_IncompressibleReadResponse()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2Compression_Variant(CompressionTestVariant.IncompressibleRead);
        }

        #region Large file tests

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can decompress large file WRITE request and compress READ response correctly using LZNT1.")]
        public void SMB2Compression_LZNT1_LargeFile()
        {
            var compressionAlgorithm = CompressionAlgorithm.LZNT1;
            BasicSMB2Compression(compressionAlgorithm, isLargeFile: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can decompress large file WRITE request and compress READ response correctly using LZ77.")]
        public void SMB2Compression_LZ77_LargeFile()
        {
            var compressionAlgorithm = CompressionAlgorithm.LZ77;
            BasicSMB2Compression(compressionAlgorithm, isLargeFile: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can decompress large file WRITE request and compress READ response correctly using LZ77 Huffman.")]
        public void SMB2Compression_LZ77Huffman_LargeFile()
        {
            var compressionAlgorithm = CompressionAlgorithm.LZ77Huffman;
            BasicSMB2Compression(compressionAlgorithm, isLargeFile: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle compressed large file WRITE request correctly using supported compression algorithms.")]
        public void SMB2Compression_CompressedWriteRequest_LargeFile()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2Compression_Variant(CompressionTestVariant.CompressibleWrite, isLargeFile: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can compress large file read request correctly if SMB2_READFLAG_REQUEST_COMPRESSED is specified in request and response is compressible.")]
        public void SMB2Compression_CompressibleReadResponse_LargeFile()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2Compression_Variant(CompressionTestVariant.CompressibleRead, isLargeFile: true);
        }

        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [Description("This test case is designed to test whether server will disconnect the connection if it received a compressed message with invalid length.")]
        public void SMB2Compression_InvalidCompressedPacketLength()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            Action<Smb2Packet> processedPacketModifier = packet =>
            {
                BaseTestSite.Assert.IsTrue(packet is Smb2CompressedPacket, "The message to be sent should be compressed.");
            };

            Func<byte[], byte[]> onWirePacketModifier = data =>
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send a compressed message with invalid length, which is less than the size of SMB2 COMPRESSION_TRANSFORM_HEADER.");
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Expect SUT to disconnect the SMB2 connection.");

                // Truncate the packet size to 8.
                var result = data.Take(8);
                return result.ToArray();
            };

            Smb2CompressionNegativeTest(null, processedPacketModifier, onWirePacketModifier);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.InvalidIdentifier)]
        [Description("This test case is designed to test whether server will disconnect the connection if the ProtocolId in the decompressed message is invalid.")]
        public void SMB2Compression_InvalidDecompressedProtocolId()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            Action<Smb2Packet> unprocessedPacketModifier = packet =>
            {
                BaseTestSite.Assert.IsTrue(packet is Smb2WriteRequestPacket, "The original message should be SMB2 read request.");

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send a compressed message with the ProtocolId in the decompressed message is invalid, which is of value 0xFFFFFFFF.");
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Expect SUT to disconnect the SMB2 connection.");

                // Modify ProtocolId to 0xFFFFFFFF.
                var readRequest = packet as Smb2WriteRequestPacket;
                readRequest.Header.ProtocolId = 0xFFFFFFFF;
            };

            Action<Smb2Packet> processedPacketModifier = packet =>
            {
                BaseTestSite.Assert.IsTrue(packet is Smb2CompressedPacket, "The message to be sent should be compressed.");
            };

            Smb2CompressionNegativeTest(unprocessedPacketModifier, processedPacketModifier, null);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is designed to test whether server will disconnect the connection if the CompressionAlgorithm in the compressed message is invalid.")]
        public void SMB2Compression_InvalidCompressionAlgorithm()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            Action<Smb2Packet> processedPacketModifier = packet =>
            {
                BaseTestSite.Assert.IsTrue(packet is Smb2CompressedPacket, "The message to be sent should be compressed.");

                var compressedPacket = packet as Smb2CompressedPacket;

                BaseTestSite.Assert.IsTrue(compressedPacket.OriginalPacket is Smb2WriteRequestPacket, "The original message should be SMB2 read request.");

                var readRequest = compressedPacket.OriginalPacket as Smb2ReadRequestPacket;

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send a compressed message with invalid CompressionAlgorithm with value 0x0004, which should be unsupported by SUT.");
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Expect SUT to disconnect the SMB2 connection.");

                // Modify CompressionAlgorithm to Unsupported.
                compressedPacket.Header.CompressionAlgorithm = CompressionAlgorithm.Unsupported;
            };

            Smb2CompressionNegativeTest(null, processedPacketModifier, null);
        }

        #endregion

        private enum CompressionTestVariant
        {
            /// <summary>
            /// Basic scenario with read and write with compression enabled, using example data provided in [MS-XCA].
            /// </summary>
            BasicReadWrite,

            /// <summary>
            /// Write with compressed enabled and verify content written is expected by normal read.
            /// </summary>
            CompressibleWrite,

            /// <summary>
            /// Prepare compressible content with normal write and read with compression enabled, to verify read response is correctly compressed.
            /// </summary>
            CompressibleRead,

            /// <summary>
            /// Prepare incompressible content with normal write and read with compression enabled, to verify read response is not compressed if the packet size will not shrink after compression.
            /// </summary>
            IncompressibleRead,

            /// <summary>
            /// Basic scenario with read and write with chained compression enabled, using data which could be compressed by pattern scanning algorithm V1.
            /// </summary>
            BasicChainedReadWrite,

            /// <summary>
            /// Write with chained compressed enabled and verify content written is expected by normal read. The PatternV1 compressible data appear at front.
            /// </summary>
            ChainedCompressibleWritePatternV1AtFront,

            /// <summary>
            /// Write with chained compressed enabled and verify content written is expected by normal read. The PatternV1 compressible data appear at end.
            /// </summary>
            ChainedCompressibleWritePatternV1AtEnd,

            /// <summary>
            /// Write with chained compressed enabled and verify content written is expected by normal read. The PatternV1 compressible data appear at both front and end.
            /// </summary>
            ChainedCompressibleWritePatternV1AtFrontAndEnd,

            /// <summary>
            /// Prepare compressible content with normal write and read with chained compression enabled, to verify read response is correctly compressed.
            /// </summary>
            ChainedCompressibleRead,

            /// <summary>
            /// Prepare incompressible content with normal write and read with chained compression enabled, to verify read response is not compressed if the packet size will not shrink after compression.
            /// </summary>
            ChainedIncompressibleRead,
        }

        private void BasicSMB2Compression(CompressionAlgorithm compressionAlgorithm, bool isLargeFile = false)
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData($"This test case is handled in {nameof(BasicSMB2Compression_Encrypted)} related cases.");

            CheckCompressionAndEncryptionApplicability(compressionAlgorithm);
            var compressionAlgorithms = new CompressionAlgorithm[] { compressionAlgorithm };
            SMB2CompressionTest(compressionAlgorithms, CompressionTestVariant.BasicReadWrite, isLargeFile: isLargeFile);
        }

        private void BasicSMB2Compression_Encrypted(CompressionAlgorithm compressionAlgorithm)
        {
            CheckCompressionAndEncryptionApplicability(compressionAlgorithm, true);
            var compressionAlgorithms = new CompressionAlgorithm[] { compressionAlgorithm };
            SMB2CompressionTest(compressionAlgorithms, CompressionTestVariant.BasicReadWrite, enableEncryption: true);
        }

        private void SMB2Compression_Variant(CompressionTestVariant variant, bool isLargeFile = false)
        {
            CheckCompressionAndEncryptionApplicability();
            var compressionAlgorithms = TestConfig.SupportedCompressionAlgorithmList.ToArray();
            SMB2CompressionTest(compressionAlgorithms, variant, isLargeFile: isLargeFile);
        }

        private void SMB2CompressionTest(CompressionAlgorithm[] compressionAlgorithms, CompressionTestVariant variant, bool enableEncryption = false, bool enableChainedCompression = false, bool isLargeFile = false)
        {
            uint treeId;
            FILEID fileId;

            if (!enableChainedCompression)
            {
                compressionAlgorithms = Smb2Utility.GetSupportedCompressionAlgorithms(compressionAlgorithms);
            }

            CreateTestFile(compressionAlgorithms, enableEncryption, out treeId, out fileId, enableChainedCompression);

            var instances = CompressionTestRunner.Generate(compressionAlgorithms, variant);

            foreach (var instance in instances)
            {
                instance.Run(client, variant, treeId, fileId, isLargeFile);
            }

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        private void Smb2CompressionNegativeTest(Action<Smb2Packet> unprocessedPacketModifier, Action<Smb2Packet> processedPacketModifier, Func<byte[], byte[]> onWirePacketModifier, bool needChainedCompression = false, bool needPatternV1 = false)
        {
            CompressionAlgorithm? compressionAlgorithm = null;

            if (needPatternV1)
            {
                compressionAlgorithm = CompressionAlgorithm.Pattern_V1;
            }

            CheckCompressionAndEncryptionApplicability(compressionAlgorithm: compressionAlgorithm, needChainedCompression: needChainedCompression);
            var compressionAlgorithms = TestConfig.SupportedCompressionAlgorithmList.ToArray();
            CreateTestFile(compressionAlgorithms, false, out uint treeId, out FILEID fileId, needChainedCompression);


            client.Smb2Client.PacketSending += unprocessedPacketModifier;
            client.Smb2Client.ProcessedPacketModifier += processedPacketModifier;
            client.Smb2Client.OnWirePacketModifier += onWirePacketModifier;

            // Specify no preference, first compression algorithm is to be used.
            client.Smb2Client.CompressionInfo.PreferredCompressionAlgorithm = CompressionAlgorithm.NONE;

            // Construct compressible data by repeat 0xE0 1024 times.
            var commonCompressibleData = Enumerable.Repeat<byte>(0xE0, 1024).ToArray();

            // Use common compressible test data
            var data = commonCompressibleData;

            if (needChainedCompression)
            {
                client.Smb2Client.CompressionInfo.CompressBufferOnly = true;
            }

            try
            {
                client.Write(treeId, fileId, data, compressWrite: true);
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Exception is thrown by SMB2 client: {0}", ex);
            }

            BaseTestSite.Assert.IsTrue(client.Smb2Client.IsServerDisconnected, "[MS-SMB2] section 3.3.5.2.13: The server MUST disconnect the connection.");

            client.Smb2Client.PacketSending -= unprocessedPacketModifier;
            client.Smb2Client.ProcessedPacketModifier -= processedPacketModifier;
            client.Smb2Client.OnWirePacketModifier -= onWirePacketModifier;
        }

        private void CreateTestFile(CompressionAlgorithm[] compressionAlgorithms, bool enableEncryption, out uint treeId, out FILEID fileId, bool enableChainedCompression = false)
        {
            var capabilities = Capabilities_Values.NONE;
            if (enableEncryption)
            {
                capabilities |= Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            }
            EncryptionAlgorithm[] encryptionAlgorithms = null;
            if (enableEncryption)
            {
                encryptionAlgorithms = new EncryptionAlgorithm[] { EncryptionAlgorithm.ENCRYPTION_AES128_CCM, EncryptionAlgorithm.ENCRYPTION_AES128_GCM };
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Starts a client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT.");
            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            client.NegotiateWithContexts(
                Packet_Header_Flags_Values.NONE,
                TestConfig.RequestDialects,
                preauthHashAlgs: new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 },
                encryptionAlgs: encryptionAlgorithms,
                compressionAlgorithms: compressionAlgorithms,
                compressionFlags: enableChainedCompression ? SMB2_COMPRESSION_CAPABILITIES_Flags.SMB2_COMPRESSION_CAPABILITIES_FLAG_CHAINED : SMB2_COMPRESSION_CAPABILITIES_Flags.SMB2_COMPRESSION_CAPABILITIES_FLAG_NONE,
                capabilityValue: capabilities,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status, "SUT MUST return STATUS_SUCCESS if the negotiation finished successfully.");

                    if (TestConfig.IsWindowsPlatform)
                    {
                        CheckCompressionAlgorithmsForWindowsImplementation(compressionAlgorithms);
                    }
                    else
                    {
                        bool isExpectedNonWindowsCompressionContext = Enumerable.SequenceEqual(client.Smb2Client.CompressionInfo.CompressionIds, compressionAlgorithms);
                        {
                            BaseTestSite.Assert.IsTrue(isExpectedNonWindowsCompressionContext, "[MS-SMB2] section 3.3.5.4: Non-Windows implementation MUST set CompressionAlgorithms to the CompressionIds in request if they are all supported by SUT.");
                        }
                    }
                });


            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            if (enableEncryption)
            {
                client.EnableSessionSigningAndEncryption(enableSigning: false, enableEncryption: true);
            }
            client.TreeConnect(uncSharePath, out treeId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends CREATE request with desired access set to GENERIC_READ and GENERIC_WRITE to create a file.");
            Smb2CreateContextResponse[] serverCreateContexts;

            string fileName = GetTestFileName(uncSharePath);
            client.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts,
                accessMask: AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE
                );
        }

        private void CheckCompressionAlgorithmsForWindowsImplementation(CompressionAlgorithm[] compressionAlgorithms)
        {
            if (TestConfig.Platform <= Platform.WindowsServerV1909)
            {
                bool isExpectedWindowsCompressionContext = client.Smb2Client.CompressionInfo.CompressionIds.Length == 1 && client.Smb2Client.CompressionInfo.CompressionIds[0] == compressionAlgorithms[0];
                BaseTestSite.Assert.IsTrue(isExpectedWindowsCompressionContext, "Windows 10 v1903, Windows 10 v1909, Windows Server v1903, and Windows Server v1909 only set CompressionAlgorithms to the first common algorithm supported by the client and server.");
            }
            else
            {
                var firstSupportedCompressionAlgorithm = Smb2Utility.GetSupportedCompressionAlgorithms(compressionAlgorithms.ToArray()).Take(1);

                var firstSupportedPatternScanningAlgorithm = Smb2Utility.GetSupportedPatternScanningAlgorithms(compressionAlgorithms.ToArray()).Take(1);

                var expectedCompressionAlgorithms = firstSupportedCompressionAlgorithm.Concat(firstSupportedPatternScanningAlgorithm);

                bool isExpectedWindowsCompressionContext = Enumerable.SequenceEqual(client.Smb2Client.CompressionInfo.CompressionIds.OrderBy(compressionAlgorithm => compressionAlgorithm), expectedCompressionAlgorithms.OrderBy(compressionAlgorithm => compressionAlgorithm));

                BaseTestSite.Assert.IsTrue(isExpectedWindowsCompressionContext, "Windows 10 v2004 and Windows Server v2004 select a common pattern scanning algorithm and the first common compression algorithm, specified in section 2.2.3.1.3, supported by the client and server.");
            }
        }

        private void CheckCompressionAndEncryptionApplicability(CompressionAlgorithm? compressionAlgorithm = null, bool needEncryption = false, bool needChainedCompression = false, bool needCompressionAlgorithms = false)
        {
            // Check platform
            if (TestConfig.IsWindowsPlatform)
            {
                BaseTestSite.Assume.IsFalse(TestConfig.Platform < Platform.WindowsServerV1903, "Windows 10 v1809 operating system and prior, Windows Server v1809 operating system and prior, and Windows Server 2019 and prior do not support compression.");
            }

            // Check dialect
            BaseTestSite.Assume.IsTrue(TestConfig.MaxSmbVersionSupported >= DialectRevision.Smb311, "The SMB 3.1.1 dialect introduces supporting the compression of messages between client and server.");

            // Check SUT supported compression algorithms
            TestConfig.CheckCompressionAlgorithm(compressionAlgorithm);

            // Check whether SUT supports encryption
            if (needEncryption)
            {
                TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION);
            }

            if (needChainedCompression)
            {
                BaseTestSite.Assume.IsTrue(TestConfig.IsChainedCompressionSupported, "In order to run this test case, SUT MUST support chained compression feature.");
            }

            if (needCompressionAlgorithms)
            {
                var supportedCompressionAlgorithms = Smb2Utility.GetSupportedCompressionAlgorithms(TestConfig.SupportedCompressionAlgorithmList.ToArray());

                BaseTestSite.Assume.IsTrue(supportedCompressionAlgorithms.Length > 0, "In order to run this test case, SUT MUST support at least one compression algorithm.");
            }
        }

        private partial class CompressionTestRunner
        {
            private static Dictionary<CompressionAlgorithm, byte[]> exampleTestData;
            private static byte[] commonCompressibleData;
            private static byte[] commonIncompressibleData;

            static CompressionTestRunner()
            {
                // Example test data for LZ77 and LZ77 Huffman is from [MS-XCA], "abc" repeated 100 times(300 bytes).
                var lz77AndLZ77HuffmanExampleTestData = Encoding.ASCII.GetBytes(String.Join("", Enumerable.Repeat("abc", 100).ToArray()));

                // Example test data for LZNT1 is from [MS-XCA].
                var lznt1ExampleTestData = Encoding.ASCII.GetBytes("F# F# G A A G F# E D D E F# F# E E F# F# G A A G F# E D D E F# E D D E E F# D E F# G F# D E F# G F# E D E A F# F# G A A G F# E D D E F# E D D\0");

                // Example test data for Pattern_V1 is an array containing 0xAA repeated by 256 times (256 bytes).
                var patternV1ExampleTestData = Enumerable.Repeat<byte>(0xAA, 256).ToArray();

                exampleTestData = new Dictionary<CompressionAlgorithm, byte[]>
                {
                    [CompressionAlgorithm.LZ77] = lz77AndLZ77HuffmanExampleTestData,
                    [CompressionAlgorithm.LZ77Huffman] = lz77AndLZ77HuffmanExampleTestData,
                    [CompressionAlgorithm.LZNT1] = lznt1ExampleTestData,
                    [CompressionAlgorithm.Pattern_V1] = patternV1ExampleTestData,
                };

                // Construct compressible data by repeat {0xE0, 0xE1} 1024 times.
                commonCompressibleData = Enumerable.Repeat<byte[]>(new byte[] { 0xE0, 0xE1 }, 1024).SelectMany(b => b).ToArray();

                // Construct incompressible data with only 1 0xE0.
                commonIncompressibleData = new byte[] { 0xE0 };
            }

            private bool compressWriteRequest;
            private bool compressWriteRequestBufferOnly;
            private bool compressReadRequest;
            private bool readResponseShouldBeCompressed;
            private bool readResponseShouldBeChained;
            private CompressionAlgorithm compressionAlgorithmForTest;
            private byte[] testData;

            /// <summary>
            /// Generate a series of compression test runner with given test variant.
            /// </summary>
            /// <param name="variant">Compression test variant to use.</param>
            /// <returns></returns>
            public static CompressionTestRunner[] Generate(CompressionAlgorithm[] compressionAlgorithms, CompressionTestVariant variant)
            {
                var callDict = new Dictionary<CompressionTestVariant, Func<CompressionAlgorithm[], CompressionTestRunner[]>>
                {
                    [CompressionTestVariant.BasicReadWrite] = GenerateBasicReadWrite,
                    [CompressionTestVariant.CompressibleWrite] = GenerateCompressibleWrite,
                    [CompressionTestVariant.CompressibleRead] = GenerateCompressibleRead,
                    [CompressionTestVariant.IncompressibleRead] = GenerateIncompressibleRead,
                    [CompressionTestVariant.BasicChainedReadWrite] = GenerateBasicChainedReadWrite,
                    [CompressionTestVariant.ChainedCompressibleWritePatternV1AtFront] = GenerateChainedCompressibleWritePatternV1AtFront,
                    [CompressionTestVariant.ChainedCompressibleWritePatternV1AtEnd] = GenerateChainedCompressibleWritePatternV1AtEnd,
                    [CompressionTestVariant.ChainedCompressibleWritePatternV1AtFrontAndEnd] = GenerateChainedCompressibleWritePatternV1AtFrontAndEnd,
                    [CompressionTestVariant.ChainedCompressibleRead] = GenerateChainedCompressibleRead,
                    [CompressionTestVariant.ChainedIncompressibleRead] = GenerateChainedIncompressibleRead,
                };

                var result = callDict[variant](compressionAlgorithms);

                return result;
            }

            private static CompressionTestRunner[] GenerateBasicReadWrite(CompressionAlgorithm[] compressionAlgorithms)
            {
                // Use specific test data for all supported compression algorithms
                var result = compressionAlgorithms.Select(compressionAlgorithm =>
                {
                    var instance = new CompressionTestRunner();

                    instance.compressionAlgorithmForTest = compressionAlgorithm;
                    instance.compressWriteRequest = true;
                    instance.compressWriteRequestBufferOnly = false;
                    instance.compressReadRequest = true;
                    instance.readResponseShouldBeCompressed = true;
                    instance.readResponseShouldBeChained = false;
                    // Use example test data.
                    instance.testData = exampleTestData[compressionAlgorithm];

                    return instance;
                }).ToArray();

                return result;
            }

            private static CompressionTestRunner[] GenerateCompressibleWrite(CompressionAlgorithm[] compressionAlgorithms)
            {
                // Use common test data for all supported compression algorithms
                var result = compressionAlgorithms.Select(compressionAlgorithm =>
                {
                    var instance = new CompressionTestRunner();

                    instance.compressionAlgorithmForTest = compressionAlgorithm;
                    instance.compressWriteRequest = true;
                    instance.compressWriteRequestBufferOnly = false;
                    instance.compressReadRequest = false;
                    instance.readResponseShouldBeCompressed = false;
                    instance.readResponseShouldBeChained = false;
                    // Use example test data.
                    instance.testData = commonCompressibleData;

                    return instance;
                }).ToArray();

                return result;
            }

            private static CompressionTestRunner[] GenerateCompressibleRead(CompressionAlgorithm[] compressionAlgorithms)
            {
                // Use common test data
                var instance = new CompressionTestRunner();
                instance.compressionAlgorithmForTest = CompressionAlgorithm.NONE;
                instance.compressWriteRequest = false;
                instance.compressWriteRequestBufferOnly = false;
                instance.compressReadRequest = true;
                instance.readResponseShouldBeCompressed = true;
                instance.readResponseShouldBeChained = false;
                instance.testData = commonCompressibleData;

                var result = new CompressionTestRunner[]
                {
                    instance,
                };

                return result;
            }

            private static CompressionTestRunner[] GenerateIncompressibleRead(CompressionAlgorithm[] compressionAlgorithms)
            {
                // Use common test data
                var instance = new CompressionTestRunner();
                instance.compressionAlgorithmForTest = CompressionAlgorithm.NONE;
                instance.compressWriteRequest = false;
                instance.compressWriteRequestBufferOnly = false;
                instance.compressReadRequest = true;
                instance.readResponseShouldBeCompressed = false;
                instance.readResponseShouldBeChained = false;
                instance.testData = commonIncompressibleData;

                var result = new CompressionTestRunner[]
                {
                    instance,
                };

                return result;
            }

            /// <summary>
            /// Run compression test against given input parameters.
            ///     1. Write testData to test file given by treeId and fileId, and compress message based on compressWriteRequest using compressionAlgorithmForTest.
            ///     2. Read out the data just written, and request compressing READ response message based on compressReadRequest.
            ///     3. Check whether the READ response based on readResponseShouldBeCompressed.
            ///     4. Check whether data read out is equal to test data.
            /// </summary>
            /// <param name="client">SMB2 functional client to use.</param>
            /// <param name="variant">Compression test variant.</param>
            /// <param name="treeId">TreeId to use.</param>
            /// <param name="fileId">FileId to use.</param>
            /// <param name="isLargeFile">Whether is large file.</param>
            public void Run(Smb2FunctionalClient client, CompressionTestVariant variant, uint treeId, FILEID fileId, bool isLargeFile = false)
            {
                if (compressionAlgorithmForTest != CompressionAlgorithm.NONE)
                {
                    if (!client.Smb2Client.CompressionInfo.CompressionIds.Any(compressionAlgorithmSupported => compressionAlgorithmSupported == compressionAlgorithmForTest))
                    {
                        // The specified compression algorithm is not supported by SUT.
                        return;
                    }
                }

                BaseTestSite.Log.Add(
                    LogEntryKind.TestStep,
                    "Test will trigger WRITE request with CompressWrite: {0} and preferred compression algorithm: {1}.",
                    compressWriteRequest,
                    compressionAlgorithmForTest
                    );

                // Specify the compression algorithm for write request.
                client.Smb2Client.CompressionInfo.PreferredCompressionAlgorithm = compressionAlgorithmForTest;

                if (compressWriteRequestBufferOnly)
                {
                    client.Smb2Client.CompressionInfo.CompressBufferOnly = true;
                }

                if (isLargeFile)
                {
                    // Write several times for existing testData for large data write and read tests
                    // Base testData:
                    // Length of LZ77 and LZ77 Huffman testData is 300 bytes.
                    // Length of LZNT1 testData is 142 bytes.
                    // Length of Pattern_V1 testData is 256 bytes.
                    // Length of Compressible data testData is 2048 bytes.
                    // We will change testData to 1 MB, and write 100 times to generate a 100 MB file
                    int writeRequestCount = 100;
                    ulong offset = 0;

                    // According to real experience with copying large file to shared folder, the client will request 1 MB packet per request.
                    // Change testData to 512 bytes * 2048 = 1 megabytes, so we can test large file
                    if (compressionAlgorithmForTest == CompressionAlgorithm.LZ77Huffman)
                    {
                        // LZ77Huffman match length needs to be less than 65538, so we use 512*128=65536.
                        var test512Bytes = Enumerable.Repeat(testData, 2).SelectMany(a => a).Take(512).ToArray();
                        testData = Enumerable.Repeat(test512Bytes, 128).SelectMany(b => b).ToArray();
                    }
                    else if (variant == CompressionTestVariant.ChainedCompressibleWritePatternV1AtFront)
                    {
                        // For ChainedCompressibleWritePatternV1AtFront, we need to prepend some data which can be compressed with PatternV1.
                        var test256Bytes = commonCompressibleData.Take(256).ToArray();
                        var newCommonCompressibleData = Enumerable.Repeat(test256Bytes, 4 * 1024 - 1).SelectMany(b => b).ToArray();
                        testData = GenerateByteArray(exampleTestData[CompressionAlgorithm.Pattern_V1], newCommonCompressibleData);
                    }
                    else if (variant == CompressionTestVariant.ChainedCompressibleRead || variant == CompressionTestVariant.ChainedCompressibleWritePatternV1AtEnd)
                    {
                        // For ChainedCompressibleRead or ChainedCompressibleWritePatternV1AtEnd, we need to append some data which can be compressed with PatternV1.
                        var test256Bytes = commonCompressibleData.Take(256).ToArray();
                        var newCommonCompressibleData = Enumerable.Repeat(test256Bytes, 4 * 1024 - 1).SelectMany(b => b).ToArray();
                        testData = GenerateByteArray(newCommonCompressibleData, exampleTestData[CompressionAlgorithm.Pattern_V1]);
                    }
                    else if (variant == CompressionTestVariant.ChainedCompressibleWritePatternV1AtFrontAndEnd)
                    {
                        // For ChainedCompressibleWritePatternV1AtFrontAndEnd, we need to add start and end with PatternV1.
                        var test256Bytes = commonCompressibleData.Take(256).ToArray();
                        var newCommonCompressibleData = Enumerable.Repeat(test256Bytes, 4 * 1024 - 2).SelectMany(b => b).ToArray();
                        testData = GenerateByteArray(exampleTestData[CompressionAlgorithm.Pattern_V1], newCommonCompressibleData, exampleTestData[CompressionAlgorithm.Pattern_V1]);
                    }
                    else
                    {
                        // For other cases, testData with a length of 1 MB will be used.
                        var test512Bytes = Enumerable.Repeat(testData, 4).SelectMany(a => a).Take(512).ToArray();
                        testData = Enumerable.Repeat(test512Bytes, 2048).SelectMany(b => b).ToArray();
                    }
                    int requestBytes = testData.Length;

                    for (int time = 0; time < writeRequestCount; time++)
                    {
                        client.Write(treeId, fileId, testData, offset, compressWrite: compressWriteRequest);
                        offset += (uint)requestBytes;
                    }
                }
                else
                {
                    client.Write(treeId, fileId, testData, compressWrite: compressWriteRequest);
                }

                if (compressWriteRequestBufferOnly)
                {
                    client.Smb2Client.CompressionInfo.CompressBufferOnly = false;
                }

                byte[] readOutData = null;

                bool readResponseIsCompressed = false;
                bool readResponseIsChained = false;
                Smb2ReadResponsePacket readResponsePacket = null;
                Smb2CompressedPacket compressedPacket = null;

                Action<Smb2Packet> Smb2Client_PacketReceived = (Smb2Packet obj) =>
                {
                    if (obj is Smb2ReadResponsePacket)
                    {
                        readResponsePacket = obj as Smb2ReadResponsePacket;
                        readResponseIsCompressed = readResponsePacket.Compressed;

                        compressedPacket = readResponsePacket.CompressedPacket;

                        if (compressedPacket is Smb2ChainedCompressedPacket)
                        {
                            readResponseIsChained = true;
                        }
                    }
                };

                BaseTestSite.Log.Add(
                    LogEntryKind.TestStep,
                    "Test will trigger READ request with CompressRead: {0} and check whether READ response is compressed: {1}.",
                    compressReadRequest,
                    readResponseShouldBeCompressed
                    );

                client.Smb2Client.PacketReceived += Smb2Client_PacketReceived;

                if (isLargeFile)
                {
                    // Read several times for exist testData for large data write and read tests
                    int requestBytes = testData.Length;
                    int readRequestCount = 100;
                    ulong offset = 0;

                    for (int time = 0; time < readRequestCount; time++)
                    {
                        client.Read(treeId, fileId, (uint)offset, (uint)requestBytes, out readOutData, compressRead: compressReadRequest);
                        BaseTestSite.Assert.IsTrue(Enumerable.SequenceEqual(testData, readOutData), $"Request times:{time + 1}, packet offset: {offset}, byteSize:{requestBytes}, the read out content MUST be the same with that is written.");
                        offset += (uint)requestBytes;
                    }
                }
                else
                {
                    client.Read(treeId, fileId, 0, (uint)testData.Length, out readOutData, compressRead: compressReadRequest);
                    BaseTestSite.Assert.IsTrue(Enumerable.SequenceEqual(testData, readOutData), "The read out content MUST be the same with that is written.");
                }

                client.Smb2Client.PacketReceived -= Smb2Client_PacketReceived;

                if (compressReadRequest)
                {
                    if (readResponseShouldBeCompressed)
                    {
                        BaseTestSite.Assert.IsTrue(readResponseIsCompressed && compressedPacket != null, "[MS-SMB2] section 3.3.5.12: When SMB2_READFLAG_REQUEST_COMPRESSED is specified in read request, the server MUST compress the message if compression will shrink the message size.");

                        BaseTestSite.Log.Add(LogEntryKind.Debug, "Read response is compressed using {0}.", compressedPacket.Header.CompressionAlgorithm);

                        if (readResponseShouldBeChained)
                        {
                            BaseTestSite.Assert.IsTrue(readResponseIsChained, "The read response should be chained.");
                        }
                        else
                        {
                            BaseTestSite.Assert.IsFalse(readResponseIsChained, "The read response should not be chained.");
                        }
                    }
                    else
                    {
                        BaseTestSite.Assert.IsTrue(!readResponseIsCompressed && compressedPacket == null, "[MS-SMB2] section 3.3.5.12: When SMB2_READFLAG_REQUEST_COMPRESSED is specified in read request, the server MUST NOT compress the message if compression will not shrink the message size.");
                    }
                }
                else
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "SMB2_READFLAG_REQUEST_COMPRESSED is not specified in read request, and read response is compressed: {0}.", readResponseIsCompressed);

                    if (readResponseIsCompressed)
                    {
                        BaseTestSite.Assert.IsTrue(compressedPacket != null, "Compressed packet is received.");
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "Read response is compressed using {0}.", compressedPacket.Header.CompressionAlgorithm);
                    }
                }
            }
        }
    }
}
