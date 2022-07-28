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

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public partial class Compression
    {
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [Description("This test case is designed to test whether server can decompress WRITE request and compress READ response correctly using chained compression and PatternV1.")]
        public void BVT_SMB2Compression_Chained_PatternV1()
        {
            // When both IsGlobalEncryptDataEnabled and IsGlobalRejectUnencryptedAccessEnabled are true, the case become encryption scenario and it is covered in test case BVT_SMB2Compression_Chained_PatternV1_Encrypted.
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData($"When IsGlobalEncryptDataEnabled is set to true, the scenario with compression is covered in test case {nameof(BVT_SMB2Compression_Chained_PatternV1_Encrypted)}.");

            SMB2ChainedCompression_Variant(CompressionTestVariant.BasicChainedReadWrite, needPatternV1: true, needCompressionAlgorithm: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [Description("This test case is designed to test whether server can decompress WRITE request and compress READ response correctly using chained compression and PatternV1 when encryption is enabled.")]
        public void BVT_SMB2Compression_Chained_PatternV1_Encrypted()
        {
            SMB2ChainedCompression_Variant(CompressionTestVariant.BasicChainedReadWrite, needPatternV1: true, needCompressionAlgorithm: true, needEncryption: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle chained and compressed WRITE request correctly using supported compression algorithms. The PatternV1 compressible data appear at front.")]
        public void SMB2Compression_Chained_PatternV1_CompressedWriteRequest_PatternV1AtFront()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2ChainedCompression_Variant(CompressionTestVariant.ChainedCompressibleWritePatternV1AtFront, needPatternV1: true, needCompressionAlgorithm: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle chained and compressed WRITE request correctly using supported compression algorithms. The PatternV1 compressible data appear at end.")]
        public void SMB2Compression_Chained_PatternV1_CompressedWriteRequest_PatternV1AtEnd()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2ChainedCompression_Variant(CompressionTestVariant.ChainedCompressibleWritePatternV1AtEnd, needPatternV1: true, needCompressionAlgorithm: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle chained and compressed WRITE request correctly using supported compression algorithms. The PatternV1 compressible data appear at both front and end.")]
        public void SMB2Compression_Chained_PatternV1_CompressedWriteRequest_PatternV1AtFrontAndEnd()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2ChainedCompression_Variant(CompressionTestVariant.ChainedCompressibleWritePatternV1AtFrontAndEnd, needPatternV1: true, needCompressionAlgorithm: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can chain and compress read request correctly if SMB2_READFLAG_REQUEST_COMPRESSED is specified in request and response is compressible.")]
        public void SMB2Compression_Chained_PatternV1_CompressibleReadResponse()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2ChainedCompression_Variant(CompressionTestVariant.ChainedCompressibleRead, needPatternV1: true, needCompressionAlgorithm: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server will not chain or compress read request if SMB2_READFLAG_REQUEST_COMPRESSED is specified in request and response is incompressible.")]
        public void SMB2Compression_Chained_PatternV1_IncompressibleReadResponse()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2ChainedCompression_Variant(CompressionTestVariant.ChainedIncompressibleRead, needPatternV1: true, needCompressionAlgorithm: true);
        }

        #region Large file tests

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can decompress large file WRITE request and compress READ response correctly using chained compression and PatternV1.")]
        public void SMB2Compression_Chained_PatternV1_LargeFile()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2ChainedCompression_Variant(CompressionTestVariant.BasicChainedReadWrite, needPatternV1: true, needCompressionAlgorithm: true, isLargeFile: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle chained and compressed large file WRITE request correctly using supported compression algorithms. The PatternV1 compressible data appear at front.")]
        public void SMB2Compression_Chained_PatternV1_CompressedWriteRequest_PatternV1AtFront_LargeFile()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2ChainedCompression_Variant(CompressionTestVariant.ChainedCompressibleWritePatternV1AtFront, needPatternV1: true, needCompressionAlgorithm: true, isLargeFile: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle chained and compressed large file WRITE request correctly using supported compression algorithms. The PatternV1 compressible data appear at end.")]
        public void SMB2Compression_Chained_PatternV1_CompressedWriteRequest_PatternV1AtEnd_LargeFile()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2ChainedCompression_Variant(CompressionTestVariant.ChainedCompressibleWritePatternV1AtEnd, needPatternV1: true, needCompressionAlgorithm: true, isLargeFile: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can handle chained and compressed large file WRITE request correctly using supported compression algorithms. The PatternV1 compressible data appear at both front and end.")]
        public void SMB2Compression_Chained_PatternV1_CompressedWriteRequest_PatternV1AtFrontAndEnd_LargeFile()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2ChainedCompression_Variant(CompressionTestVariant.ChainedCompressibleWritePatternV1AtFrontAndEnd, needPatternV1: true, needCompressionAlgorithm: true, isLargeFile: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.Positive)]
        [Description("This test case is designed to test whether server can chain and compress large file read request correctly if SMB2_READFLAG_REQUEST_COMPRESSED is specified in request and response is compressible.")]
        public void SMB2Compression_Chained_PatternV1_CompressibleReadResponse_LargeFile()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2ChainedCompression_Variant(CompressionTestVariant.ChainedCompressibleRead, needPatternV1: true, needCompressionAlgorithm: true, isLargeFile: true);
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This test case is designed to test whether server will disconnect the connection if it received a compressed message with invalid CompressionAlgorithm in compression payload header.")]
        public void SMB2Compression_Chained_InvalidCompressionAlgorithmInCompressionPayloadHeader()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            Action<Smb2Packet> processedPacketModifier = packet =>
            {
                BaseTestSite.Assert.IsTrue(packet is Smb2ChainedCompressedPacket, "The message to be sent should be chained compressed.");

                var chainedCompressedPacket = packet as Smb2ChainedCompressedPacket;

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send a compressed message with invalid CompressionAlgorithm in compression payload header.");

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Expect SUT to disconnect the SMB2 connection.");

                chainedCompressedPacket.Payloads = chainedCompressedPacket.Payloads.Select(payload =>
                {
                    if (payload.Item1.CompressionAlgorithm != CompressionAlgorithm.NONE)
                    {
                        var header = payload.Item1;

                        header.CompressionAlgorithm = CompressionAlgorithm.Unsupported;

                        return new Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>(header, payload.Item2);
                    }
                    else
                    {
                        return payload;
                    }
                }).ToArray();
            };

            Smb2ChainedCompressionNegativeTest(null, processedPacketModifier, null);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [Description("This test case is designed to test whether server will disconnect the connection if it received a compressed message with invalid Length in compression payload header.")]
        public void SMB2Compression_Chained_InvalidLengthInCompressionPayloadHeader()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            Action<Smb2Packet> processedPacketModifier = packet =>
            {
                BaseTestSite.Assert.IsTrue(packet is Smb2ChainedCompressedPacket, "The message to be sent should be chained compressed.");

                var chainedCompressedPacket = packet as Smb2ChainedCompressedPacket;

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send a compressed message with invalid Length in compression payload header.");

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Expect SUT to disconnect the SMB2 connection.");

                chainedCompressedPacket.Payloads = chainedCompressedPacket.Payloads.Select(payload =>
                {
                    if (payload.Item1.CompressionAlgorithm == CompressionAlgorithm.NONE)
                    {
                        var header = payload.Item1;

                        header.Length = chainedCompressedPacket.Header.OriginalCompressedSegmentSize + 1;

                        return new Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>(header, payload.Item2);
                    }
                    else
                    {
                        return payload;
                    }
                }).ToArray();
            };

            Smb2ChainedCompressionNegativeTest(null, processedPacketModifier, null);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [Description("This test case is designed to test whether server will disconnect the connection if it received a compressed message with invalid Repetitions in compression pattern payload V1.")]
        public void SMB2Compression_Chained_PatternV1_InvalidCompressionPatternPayloadV1Repetitions()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            Action<Smb2Packet> processedPacketModifier = packet =>
            {
                BaseTestSite.Assert.IsTrue(packet is Smb2ChainedCompressedPacket, "The message to be sent should be chained compressed.");

                var chainedCompressedPacket = packet as Smb2ChainedCompressedPacket;

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send a compressed message with invalid  Repetitions in compression pattern payload V1.");

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Expect SUT to disconnect the SMB2 connection.");

                chainedCompressedPacket.Payloads = chainedCompressedPacket.Payloads.Select(payload =>
                {
                    if (payload.Item1.CompressionAlgorithm == CompressionAlgorithm.Pattern_V1)
                    {
                        var head = payload.Item1;

                        var pattern = (SMB2_COMPRESSION_PATTERN_PAYLOAD_V1)payload.Item2;

                        pattern.Repetitions = chainedCompressedPacket.Header.OriginalCompressedSegmentSize + 1;

                        return new Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>(payload.Item1, pattern);
                    }
                    else
                    {
                        return payload;
                    }
                }).ToArray();
            };

            Smb2ChainedCompressionNegativeTest(null, processedPacketModifier, null, needPatternV1: true);
        }

        private void SMB2ChainedCompression_Variant(CompressionTestVariant variant, bool needPatternV1 = false, bool needCompressionAlgorithm = false, bool needEncryption = false, bool isLargeFile = false)
        {
            CompressionAlgorithm? compressionAlgorithmToCheck = null;

            if (needPatternV1)
            {
                compressionAlgorithmToCheck = CompressionAlgorithm.Pattern_V1;
            }

            CheckCompressionAndEncryptionApplicability(compressionAlgorithmToCheck, needEncryption, true, needCompressionAlgorithm);

            var compressionAlgorithms = new List<CompressionAlgorithm>();

            if (needPatternV1)
            {
                compressionAlgorithms.Add(CompressionAlgorithm.Pattern_V1);
            }

            if (needCompressionAlgorithm)
            {
                compressionAlgorithms.AddRange(Smb2Utility.GetSupportedCompressionAlgorithms(TestConfig.SupportedCompressionAlgorithmList.ToArray()));
            }

            SMB2CompressionTest(compressionAlgorithms.ToArray(), variant, needEncryption, enableChainedCompression: true, isLargeFile: isLargeFile);
        }

        private void Smb2ChainedCompressionNegativeTest(Action<Smb2Packet> unprocessedPacketModifier, Action<Smb2Packet> processedPacketModifier, Func<byte[], byte[]> onWirePacketModifier, bool needPatternV1 = false)
        {
            Smb2CompressionNegativeTest(unprocessedPacketModifier, processedPacketModifier, onWirePacketModifier, needChainedCompression: true, needPatternV1: needPatternV1);
        }

        private partial class CompressionTestRunner
        {
            private static CompressionTestRunner[] GenerateBasicChainedReadWrite(CompressionAlgorithm[] compressionAlgorithms)
            {
                var instance = new CompressionTestRunner();
                instance.compressionAlgorithmForTest = CompressionAlgorithm.NONE;
                instance.compressWriteRequest = true;
                instance.compressWriteRequestBufferOnly = false;
                instance.compressReadRequest = true;
                instance.readResponseShouldBeCompressed = true;
                instance.readResponseShouldBeChained = true;
                instance.testData = exampleTestData[CompressionAlgorithm.Pattern_V1];

                var result = new CompressionTestRunner[]
                {
                    instance,
                };

                return result;
            }

            private static CompressionTestRunner[] GenerateChainedCompressibleWritePatternV1AtFront(CompressionAlgorithm[] compressionAlgorithms)
            {
                // PatternV1 + XCA compression.
                var testDataPatternV1AtFront = GenerateByteArray(exampleTestData[CompressionAlgorithm.Pattern_V1], commonCompressibleData);

                return GenerateChainedCompressibleWrite(testDataPatternV1AtFront);
            }

            private static CompressionTestRunner[] GenerateChainedCompressibleWritePatternV1AtEnd(CompressionAlgorithm[] compressionAlgorithms)
            {
                // XCA compression + PatternV1.
                var testDataPatternV1AtEnd = GenerateByteArray(commonCompressibleData, exampleTestData[CompressionAlgorithm.Pattern_V1]);

                return GenerateChainedCompressibleWrite(testDataPatternV1AtEnd);
            }

            private static CompressionTestRunner[] GenerateChainedCompressibleWritePatternV1AtFrontAndEnd(CompressionAlgorithm[] compressionAlgorithms)
            {
                // PatternV1 + XCA compression + PatternV1.
                var testDataPatternV1AtFrontAndEnd = GenerateByteArray(exampleTestData[CompressionAlgorithm.Pattern_V1], commonCompressibleData, exampleTestData[CompressionAlgorithm.Pattern_V1]);

                return GenerateChainedCompressibleWrite(testDataPatternV1AtFrontAndEnd);
            }

            private static CompressionTestRunner[] GenerateChainedCompressibleWrite(byte[] testData)
            {
                var instance = new CompressionTestRunner();
                instance.compressionAlgorithmForTest = CompressionAlgorithm.NONE;
                instance.compressWriteRequest = true;
                instance.compressWriteRequestBufferOnly = true;
                instance.compressReadRequest = false;
                instance.readResponseShouldBeCompressed = false;
                instance.readResponseShouldBeChained = false;
                instance.testData = testData;

                return new CompressionTestRunner[] { instance, };
            }

            private static CompressionTestRunner[] GenerateChainedCompressibleRead(CompressionAlgorithm[] compressionAlgorithms)
            {
                var testDataPatternV1AtEnd = GenerateByteArray(commonCompressibleData, exampleTestData[CompressionAlgorithm.Pattern_V1]);

                var testDataArray = new byte[][]
                {
                    testDataPatternV1AtEnd,
                };

                var result = testDataArray.Select(testData =>
                {
                    var instance = new CompressionTestRunner();
                    instance.compressionAlgorithmForTest = CompressionAlgorithm.NONE;
                    instance.compressWriteRequest = false;
                    instance.compressWriteRequestBufferOnly = false;
                    instance.compressReadRequest = true;
                    instance.readResponseShouldBeCompressed = true;
                    instance.readResponseShouldBeChained = true;
                    instance.testData = testData;

                    return instance;
                }).ToArray();

                return result;
            }

            private static CompressionTestRunner[] GenerateChainedIncompressibleRead(CompressionAlgorithm[] compressionAlgorithms)
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

            private static byte[] GenerateByteArray(params byte[][] arrays)
            {
                var result = arrays.SelectMany(array => array);

                return result.ToArray();
            }
        }
    }
}