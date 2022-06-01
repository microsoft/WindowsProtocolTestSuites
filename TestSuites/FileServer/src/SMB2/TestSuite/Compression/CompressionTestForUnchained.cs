// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public partial class Compression
    {
        #region Test cases

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [Description("This test case is designed to test whether server can compress WRITE request and READ response using unchained compression.")]
        public void BVT_SMB2Compression_Unchained()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2UnchainedCompression_Variant(false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.Compression)]
        [Description("This test case is designed to test whether server can compress WRITE request and READ response using unchained compression for large file.")]
        public void BVT_SMB2Compression_Unchained_LargeFile()
        {
            CheckCompressionTestCaseApplicabilityForGlobalEncryptData();

            SMB2UnchainedCompression_Variant(true);
        }

        #endregion

        #region Test case utility

        private void SMB2UnchainedCompression_Variant(bool isLargeFile = false)
        {
            CheckCompressionAndEncryptionApplicability(needCompressionAlgorithms: true);

            var compressionAlgorithms = TestConfig.SupportedCompressionAlgorithmList.ToArray();
            CreateTestFile(compressionAlgorithms, false, out uint treeId, out FILEID fileId, false);

            Action<Smb2Packet> processedPacketModifier = packet =>
            {
                BaseTestSite.Assert.IsTrue(packet is Smb2NonChainedCompressedPacket, "The message to be sent MUST NOT be chained compressed.");

                var nonChainedCompressedPacket = packet as Smb2NonChainedCompressedPacket;

                BaseTestSite.Assert.AreEqual<uint>(nonChainedCompressedPacket.Header.Offset, (uint)nonChainedCompressedPacket.UncompressedData.Length, "The Offset must be the length of the UncompressedData");
            };

            client.Smb2Client.ProcessedPacketModifier += processedPacketModifier;

            // Specify no preference, first compression algorithm is to be used.
            client.Smb2Client.CompressionInfo.PreferredCompressionAlgorithm = CompressionAlgorithm.NONE;

            // Construct compressible data by repeat 0xE0 1024 times.
            var commonCompressibleData = Enumerable.Repeat<byte>(0xE0, 1024).ToArray();

            // Use common compressible test data
            var data = commonCompressibleData;

            try
            {
                if (isLargeFile)
                {
                    var test512Bytes = Enumerable.Repeat(commonCompressibleData, 4).SelectMany(a => a).Take(512).ToArray();
                    var testData = Enumerable.Repeat(test512Bytes, 2048).SelectMany(b => b).ToArray();
                    int requestBytes = testData.Length;

                    int writeRequestCount = 100;
                    ulong offset = 0;
                    for (int time = 0; time < writeRequestCount; time++)
                    {
                        client.Write(treeId, fileId, testData, offset, compressWrite: true);
                        offset += (uint)requestBytes;
                    }
                }
                else
                {
                    client.Write(treeId, fileId, data, compressWrite: true);
                }
            }
            catch (Exception ex)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Exception is thrown by SMB2 client: {0}", ex);
            }

            client.Smb2Client.ProcessedPacketModifier -= processedPacketModifier;
        }

        #endregion
    }
}
