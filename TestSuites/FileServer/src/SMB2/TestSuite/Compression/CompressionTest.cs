// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class Compression : SMB2TestBase
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
            SMB2Compression_Variant(CompressionTestVariant.CompressibleWrite);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can compress read request correctly if SMB2_READFLAG_REQUEST_COMPRESSED is specified in request and response is compressible.")]
        public void SMB2Compression_CompressibleReadResponse()
        {
            SMB2Compression_Variant(CompressionTestVariant.CompressibleRead);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server will not compress read request if SMB2_READFLAG_REQUEST_COMPRESSED is specified in request and response is incompressible.")]
        public void SMB2Compression_IncompressibleReadResponse()
        {
            SMB2Compression_Variant(CompressionTestVariant.IncompressibleRead);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [Description("This test case is designed to test whether server will disconnect the connection if it received a compressed message with invalid length.")]
        public void SMB2Compression_InvalidCompressedPacketLength()
        {
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
        }

        private void BasicSMB2Compression(CompressionAlgorithm compressionAlgorithm)
        {
            CheckCompressionAndEncryptionApplicability(compressionAlgorithm);
            var compressionAlgorithms = new CompressionAlgorithm[] { compressionAlgorithm };
            SMB2CompressionTest(compressionAlgorithms, false, CompressionTestVariant.BasicReadWrite);
        }

        private void BasicSMB2Compression_Encrypted(CompressionAlgorithm compressionAlgorithm)
        {
            CheckCompressionAndEncryptionApplicability(compressionAlgorithm, true);
            var compressionAlgorithms = new CompressionAlgorithm[] { compressionAlgorithm };
            SMB2CompressionTest(compressionAlgorithms, true, CompressionTestVariant.BasicReadWrite);
        }

        private void SMB2Compression_Variant(CompressionTestVariant variant)
        {
            CheckCompressionAndEncryptionApplicability();
            var compressionAlgorithms = TestConfig.SupportedCompressionAlgorithmList.ToArray();
            SMB2CompressionTest(compressionAlgorithms, false, variant);
        }

        private void SMB2CompressionTest(CompressionAlgorithm[] compressionAlgorithms, bool enableEncryption, CompressionTestVariant variant)
        {
            uint treeId;
            FILEID fileId;
            CreateTestFile(compressionAlgorithms, enableEncryption, out treeId, out fileId);

            var instances = CompressionTestRunner.Generate(variant);

            foreach (var instance in instances)
            {
                instance.Run(client, treeId, fileId);
            }

            client.Close(treeId, fileId);
            client.TreeDisconnect(treeId);
            client.LogOff();
        }

        private class CompressionTestRunner
        {
            private static CompressionAlgorithm[] compressionAlgorithms;
            private static Dictionary<CompressionAlgorithm, byte[]> exampleTestData;
            private static byte[] commonCompressibleData;
            private static byte[] commonIncompressibleData;

            static CompressionTestRunner()
            {
                compressionAlgorithms = new CompressionAlgorithm[] { CompressionAlgorithm.LZ77, CompressionAlgorithm.LZ77Huffman, CompressionAlgorithm.LZNT1 };
                // Example test data for LZ77 and LZ77 Huffman is from [MS-XCA], "abc" repeated 100 times(300 bytes).
                var lznt1ExampleTestData = Encoding.ASCII.GetBytes("F# F# G A A G F# E D D E F# F# E E F# F# G A A G F# E D D E F# E D D E E F# D E F# G F# D E F# G F# E D E A F# F# G A A G F# E D D E F# E D D\0");
                // Example test data for LZNT1 is from [MS-XCA].
                var lz77AndLZ77HuffmanExampleTestData = Encoding.ASCII.GetBytes(String.Join("", Enumerable.Repeat("abc", 100).ToArray()));
                exampleTestData = new Dictionary<CompressionAlgorithm, byte[]>
                {
                    [CompressionAlgorithm.LZ77] = lz77AndLZ77HuffmanExampleTestData,
                    [CompressionAlgorithm.LZ77Huffman] = lz77AndLZ77HuffmanExampleTestData,
                    [CompressionAlgorithm.LZNT1] = lznt1ExampleTestData,
                };

                // Construct compressible data by repeat 0xE0 1024 times.
                commonCompressibleData = Enumerable.Repeat<byte>(0xE0, 1024).ToArray();

                // Construct incompressible data with only 1 0xE0.
                commonIncompressibleData = new byte[] { 0xE0 };
            }

            private bool compressWriteRequest;
            private bool compressReadRequest;
            private bool readResponseShouldBeCompressed;
            private CompressionAlgorithm compressionAlgorithmForTest;
            private byte[] testData;

            /// <summary>
            /// Generate a series of compression test runner with given test variant.
            /// </summary>
            /// <param name="variant">Compression test variant to use.</param>
            /// <returns></returns>
            public static CompressionTestRunner[] Generate(CompressionTestVariant variant)
            {
                CompressionTestRunner[] result = null;
                switch (variant)
                {
                    case CompressionTestVariant.BasicReadWrite:
                        {
                            // Use specific test data for all supported compression algorithms
                            result = compressionAlgorithms.Select(compressionAlgorithm =>
                            {
                                var instance = new CompressionTestRunner();

                                instance.compressionAlgorithmForTest = compressionAlgorithm;
                                instance.compressWriteRequest = true;
                                instance.compressReadRequest = true;
                                instance.readResponseShouldBeCompressed = true;
                                // Use example test data.
                                instance.testData = exampleTestData[compressionAlgorithm];

                                return instance;
                            }).ToArray();
                        }
                        break;
                    case CompressionTestVariant.CompressibleWrite:
                        {
                            // Use common test data for all supported compression algorithms
                            result = compressionAlgorithms.Select(compressionAlgorithm =>
                            {
                                var instance = new CompressionTestRunner();

                                instance.compressionAlgorithmForTest = compressionAlgorithm;
                                instance.compressWriteRequest = true;
                                instance.compressReadRequest = false;
                                instance.readResponseShouldBeCompressed = false;
                                // Use example test data.
                                instance.testData = commonCompressibleData;

                                return instance;
                            }).ToArray();
                        }
                        break;
                    case CompressionTestVariant.CompressibleRead:
                        {
                            // Use common test data
                            var instance = new CompressionTestRunner();
                            instance.compressionAlgorithmForTest = CompressionAlgorithm.NONE;
                            instance.compressWriteRequest = false;
                            instance.compressReadRequest = true;
                            instance.readResponseShouldBeCompressed = true;
                            instance.testData = commonCompressibleData;

                            result = new CompressionTestRunner[] { instance };
                        }
                        break;
                    case CompressionTestVariant.IncompressibleRead:
                        {
                            // Use common test data
                            var instance = new CompressionTestRunner();
                            instance.compressionAlgorithmForTest = CompressionAlgorithm.NONE;
                            instance.compressWriteRequest = false;
                            instance.compressReadRequest = true;
                            instance.readResponseShouldBeCompressed = false;
                            instance.testData = commonIncompressibleData;

                            result = new CompressionTestRunner[] { instance };
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Unknown test variant!");
                }
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
            /// <param name="treeId">TreeId to use.</param>
            /// <param name="fileId">FileId to use.</param>
            public void Run(Smb2FunctionalClient client, uint treeId, FILEID fileId)
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

                client.Write(treeId, fileId, testData, compressWrite: compressWriteRequest);

                byte[] readOutData = null;

                bool readResponseIsCompressed = false;
                Smb2ReadResponsePacket readResponsePacket = null;
                Smb2CompressedPacket compressedPacket = null;

                Action<Smb2Packet> Smb2Client_PacketReceived = (Smb2Packet obj) =>
                {
                    if (obj is Smb2ReadResponsePacket)
                    {
                        readResponsePacket = obj as Smb2ReadResponsePacket;
                        readResponseIsCompressed = readResponsePacket.Compressed;

                        compressedPacket = readResponsePacket.CompressedPacket;
                    }
                };

                BaseTestSite.Log.Add(
                    LogEntryKind.TestStep,
                    "Test will trigger READ request with CompressRead: {0} and check whether READ response is compressed: {1}.",
                    compressReadRequest,
                    readResponseShouldBeCompressed
                    );

                client.Smb2Client.PacketReceived += Smb2Client_PacketReceived;
                client.Read(treeId, fileId, 0, (uint)testData.Length, out readOutData, compressRead: compressReadRequest);
                client.Smb2Client.PacketReceived -= Smb2Client_PacketReceived;

                if (compressReadRequest)
                {
                    if (readResponseShouldBeCompressed)
                    {
                        BaseTestSite.Assert.IsTrue(readResponseIsCompressed && compressedPacket != null, "[MS-SMB2] section 3.3.5.12: When SMB2_READFLAG_REQUEST_COMPRESSED is specified in read request, the server MUST compress the message if compression will shrink the message size.");

                        BaseTestSite.Log.Add(LogEntryKind.Debug, "Read response is compressed using {0}.", compressedPacket.Header.CompressionAlgorithm);
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

                BaseTestSite.Assert.IsTrue(Enumerable.SequenceEqual(testData, readOutData), "The read out content MUST be the same with that is written.");
            }
        }

        private void Smb2CompressionNegativeTest(Action<Smb2Packet> unprocessedPacketModifier, Action<Smb2Packet> processedPacketModifier, Func<byte[], byte[]> onWirePacketModifier)
        {
            CheckCompressionAndEncryptionApplicability();
            var compressionAlgorithms = TestConfig.SupportedCompressionAlgorithmList.ToArray();
            CreateTestFile(compressionAlgorithms, false, out uint treeId, out FILEID fileId);


            client.Smb2Client.PacketSending += unprocessedPacketModifier;
            client.Smb2Client.ProcessedPacketModifier += processedPacketModifier;
            client.Smb2Client.OnWirePacketModifier += onWirePacketModifier;

            // Specify no preference, first compression algorithm is to be used.
            client.Smb2Client.CompressionInfo.PreferredCompressionAlgorithm = CompressionAlgorithm.NONE;

            // Construct compressible data by repeat 0xE0 1024 times.
            var commonCompressibleData = Enumerable.Repeat<byte>(0xE0, 1024).ToArray();

            // Use common compressible test data
            var data = commonCompressibleData;

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

        private void CreateTestFile(CompressionAlgorithm[] compressionAlgorithms, bool enableEncryption, out uint treeId, out FILEID fileId)
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
                capabilityValue: capabilities,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, header.Status, "SUT MUST return STATUS_SUCCESS if the negotiation finished successfully.");

                    if (TestConfig.IsWindowsPlatform)
                    {
                        bool isExpectedWindowsCompressionContext = client.Smb2Client.CompressionInfo.CompressionIds.Length == 1 && client.Smb2Client.CompressionInfo.CompressionIds[0] == compressionAlgorithms[0];

                        BaseTestSite.Assert.IsTrue(isExpectedWindowsCompressionContext, "Windows 10 v1903 and later and Windows Server v1903 and later only set CompressionAlgorithms to the first common algorithm supported by the client and server.");
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

        private void CheckCompressionAndEncryptionApplicability(CompressionAlgorithm? compressionAlgorithm = null, bool needEncryption = false)
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
        }
    }
}
