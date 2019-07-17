// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2.Common
{
    /// <summary>
    /// SMB2 Compression Utility.
    /// </summary>
    public static class Smb2Compression
    {
        private static Dictionary<CompressionAlgorithm, XcaCompressor> compressorInstances;
        private static Dictionary<CompressionAlgorithm, XcaDecompressor> decompressorInstances;


        static Smb2Compression()
        {
            compressorInstances = new Dictionary<CompressionAlgorithm, XcaCompressor>
            {
                [CompressionAlgorithm.LZNT1] = new LZNT1Compressor(),
                [CompressionAlgorithm.LZ77] = new PlainLZ77Compressor(),
                [CompressionAlgorithm.LZ77Huffman] = new LZ77HuffmanCompressor()
            };
            decompressorInstances = new Dictionary<CompressionAlgorithm, XcaDecompressor>
            {
                [CompressionAlgorithm.LZNT1] = new LZNT1Decompressor(),
                [CompressionAlgorithm.LZ77] = new PlainLZ77Decompressor(),
                [CompressionAlgorithm.LZ77Huffman] = new LZ77HuffmanDecompressor()
            };
        }

        /// <summary>
        /// Get compressor instance.
        /// </summary>
        /// <param name="compressionAlgorithm">The compression algorithm to use.</param>
        /// <returns>The compressor.</returns>
        public static XcaCompressor GetCompressor(CompressionAlgorithm compressionAlgorithm)
        {
            if (!compressorInstances.ContainsKey(compressionAlgorithm))
            {
                throw new InvalidOperationException("Invalid compressor algorithm!");
            }
            return compressorInstances[compressionAlgorithm];
        }

        /// <summary>
        /// Get decompressor instance.
        /// </summary>
        /// <param name="compressionAlgorithm">The compression algorithm to use.</param>
        /// <returns>The decompressor.</returns>
        public static XcaDecompressor GetDecompressor(CompressionAlgorithm compressionAlgorithm)
        {
            if (!compressorInstances.ContainsKey(compressionAlgorithm))
            {
                throw new InvalidOperationException();
            }
            return decompressorInstances[compressionAlgorithm];
        }

        /// <summary>
        /// Compress SMB2 packet.
        /// </summary>
        /// <param name="packet">The SMB2 packet.</param>
        /// <param name="compressionInfo">Compression info.</param>
        /// <param name="role">SMB2 role.</param>
        /// <param name="offset">The offset where compression start, default zero.</param>
        /// <returns></returns>
        public static Smb2Packet Compress(Smb2CompressiblePacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role, uint offset = 0)
        {
            var compressionAlgorithm = GetCompressionAlgorithm(packet, compressionInfo, role);

            if (compressionAlgorithm == CompressionAlgorithm.NONE)
            {
                return packet;
            }

            var packetBytes = packet.ToBytes();

            var compressor = GetCompressor(compressionAlgorithm);

            var compressedPacket = new Smb2CompressedPacket();
            compressedPacket.Header.ProtocolId = Smb2Consts.ProtocolIdInCompressionTransformHeader;
            compressedPacket.Header.OriginalCompressedSegmentSize = (uint)packetBytes.Length;
            compressedPacket.Header.CompressionAlgorithm = compressionAlgorithm;
            compressedPacket.Header.Reserved = 0;
            compressedPacket.Header.Offset = offset;
            compressedPacket.UncompressedData = packetBytes.Take((int)offset).ToArray();
            compressedPacket.CompressedData = compressor.Compress(packetBytes.Skip((int)offset).ToArray());

            var compressedPackectBytes = compressedPacket.ToBytes();

            // Check whether compression shrinks the on-wire packet size
            if (compressedPackectBytes.Length < packetBytes.Length)
            {
                compressedPacket.OriginalPacket = packet;
                return compressedPacket;
            }
            else
            {
                return packet;
            }
        }

        /// <summary>
        /// Decompress the Smb2CompressedPacket.
        /// </summary>
        /// <param name="packet">The compressed packet.</param>
        /// <param name="compressionInfo">Compression info.</param>
        /// <param name="role">SMB2 role.</param>
        /// <returns>Byte array containing the decompressed packet.</returns>
        public static byte[] Decompress(Smb2CompressedPacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role)
        {
            if (packet.Header.CompressionAlgorithm == CompressionAlgorithm.NONE)
            {
                throw new InvalidOperationException("Invalid CompressionAlgorithm in header!");
            }

            if (!compressionInfo.CompressionIds.Any(compressionAlgorithm => compressionAlgorithm == packet.Header.CompressionAlgorithm))
            {
                throw new InvalidOperationException("The CompressionAlgorithm is not supported!");
            }

            var decompressor = GetDecompressor(packet.Header.CompressionAlgorithm);

            var decompressedBytes = decompressor.Decompress(packet.CompressedData);

            var originalPacketBytes = packet.UncompressedData.Concat(decompressedBytes).ToArray();

            return originalPacketBytes;
        }

        private static CompressionAlgorithm GetCompressionAlgorithm(Smb2CompressiblePacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role)
        {
            bool supportCompression = compressionInfo.CompressionIds.Any(compressionAlgorithm => compressionAlgorithm != CompressionAlgorithm.NONE);
            if (!supportCompression)
            {
                return CompressionAlgorithm.NONE;
            }

            bool needCompression = false;
            switch (role)
            {
                case Smb2Role.Client:
                    {
                        // Client will compress outgoing packets when:
                        // 1. EligibleForCompression is set for write request. (when user hopes write request to be compressed)
                        // 2. CompressAllPackets is set. (when user hopes all request to be compressed)
                        if (compressionInfo.CompressAllPackets)
                        {
                            needCompression = true;
                        }
                        else if (packet is Smb2WriteRequestPacket)
                        {
                            needCompression = packet.EligibleForCompression;
                        }
                    }
                    break;
                case Smb2Role.Server:
                    {
                        // Server will compress outgoing packets when:
                        // 1. CompressAllPackets is set and EligibleForCompression. (when server hopes all responses to be compressed, and request is compressed)
                        // 2. EligibleForCompression is set for read response.  (when compress read is specified in read request)
                        if (compressionInfo.CompressAllPackets || packet is Smb2ReadResponsePacket)
                        {
                            needCompression = packet.EligibleForCompression;
                        }
                    }
                    break;
                default:
                    {
                        throw new InvalidOperationException("Unknown SMB2 role!");
                    }
            }

            if (!needCompression)
            {
                return CompressionAlgorithm.NONE;
            }

            if (compressionInfo.PreferredCompressionAlgorithm == CompressionAlgorithm.NONE)
            {
                return compressionInfo.CompressionIds.First(compressionAlgorithm => compressionAlgorithm != CompressionAlgorithm.NONE);
            }
            else
            {
                if (!compressionInfo.CompressionIds.Contains(compressionInfo.PreferredCompressionAlgorithm))
                {
                    throw new InvalidOperationException("Specified preferred compression algorithm is not supported by SUT!");
                }
                return compressionInfo.PreferredCompressionAlgorithm;
            }
        }
    }
}
