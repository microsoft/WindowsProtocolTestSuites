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
    public static partial class Smb2Compression
    {
        private static Dictionary<CompressionAlgorithm, XcaCompressor> compressorInstances;
        private static Dictionary<CompressionAlgorithm, XcaDecompressor> decompressorInstances;

        private static int PatternPayloadLength;

        private static int FieldSizeOriginalPayloadSize;

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

            var pattern = new SMB2_COMPRESSION_PATTERN_PAYLOAD_V1();

            PatternPayloadLength = TypeMarshal.GetBlockMemorySize(pattern);

            var payloadHeader = new SMB2_COMPRESSION_PAYLOAD_HEADER();

            FieldSizeOriginalPayloadSize = TypeMarshal.GetBlockMemorySize(payloadHeader.OriginalPayloadSize);
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
                throw new InvalidOperationException("Invalid decompressor algorithm!");
            }
            return decompressorInstances[compressionAlgorithm];
        }

        /// <summary>
        /// Compress SMB2 packet.
        /// </summary>
        /// <param name="packet">The SMB2 packet.</param>
        /// <param name="compressionInfo">Compression info.</param>
        /// <param name="role">SMB2 role.</param>
        /// <returns>The compressed packet, or original packet if compression is not applicable.</returns>
        public static Smb2Packet Compress(Smb2CompressiblePacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role)
        {
            Smb2Packet compressed;

            if (compressionInfo.SupportChainedCompression)
            {
                compressed = CompressForChained(packet, compressionInfo, role);
            }
            else
            {
                compressed = CompressForNonChained(packet, compressionInfo, role);
            }

            if (compressed == packet)
            {
                // Compression is not applicable.
                return packet;
            }

            var originalBytes = packet.ToBytes();

            var compressedBytes = compressed.ToBytes();

            // Check whether compression shrinks the on-wire packet size
            if (compressedBytes.Length < originalBytes.Length)
            {
                return compressed;
            }
            else
            {
                return packet;
            }
        }

        private static Smb2Packet CompressForNonChained(Smb2CompressiblePacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role)
        {
            var compressionAlgorithm = GetCompressionAlgorithm(packet, compressionInfo, role);

            if (compressionAlgorithm == CompressionAlgorithm.NONE)
            {
                return packet;
            }

            var packetBytes = packet.ToBytes();

            var compressor = GetCompressor(compressionAlgorithm);

            uint offset = 0;

            if (compressionInfo.CompressBufferOnly)
            {
                offset = (packet as IPacketBuffer).BufferOffset;
            }

            var compressedPacket = new Smb2NonChainedCompressedPacket();
            compressedPacket.Header.ProtocolId = Smb2Consts.ProtocolIdInCompressionTransformHeader;
            compressedPacket.Header.OriginalCompressedSegmentSize = (uint)packetBytes.Length;
            compressedPacket.Header.CompressionAlgorithm = compressionAlgorithm;
            compressedPacket.Header.Flags = Compression_Transform_Header_Flags.SMB2_COMPRESSION_FLAG_NONE;
            compressedPacket.Header.Offset = offset;
            compressedPacket.UncompressedData = packetBytes.Take((int)offset).ToArray();
            compressedPacket.CompressedData = compressor.Compress(packetBytes.Skip((int)offset).ToArray());

            compressedPacket.OriginalPacket = packet;

            return compressedPacket;
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
            bool isChained = packet.Header.Flags.HasFlag(Compression_Transform_Header_Flags.SMB2_COMPRESSION_FLAG_CHAINED);

            byte[] decompressedData;

            if (isChained)
            {
                decompressedData = DecompressForChained(packet, compressionInfo, role);
            }
            else
            {
                decompressedData = DecompressForNonChained(packet, compressionInfo, role);
            }

            if (decompressedData.Length != packet.Header.OriginalCompressedSegmentSize)
            {
                throw new InvalidOperationException($"The length of decompressed data (0x{decompressedData.Length:X08}) is inconsistent with compression header (0x{packet.Header.OriginalCompressedSegmentSize:X08}).");
            }

            return decompressedData;
        }

        private static byte[] DecompressForNonChained(Smb2CompressedPacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role)
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

            var p = packet as Smb2NonChainedCompressedPacket;

            var decompressedBytes = decompressor.Decompress(p.CompressedData);

            var originalPacketBytes = p.UncompressedData.Concat(decompressedBytes).ToArray();

            return originalPacketBytes;
        }

        private static bool IsCompressionNeeded(Smb2CompressiblePacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role)
        {
            bool supportCompression = compressionInfo.CompressionIds.Any(compressionAlgorithm => compressionAlgorithm != CompressionAlgorithm.NONE);
            if (!supportCompression)
            {
                return false;
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

            if (needCompression && compressionInfo.CompressBufferOnly)
            {
                // Not compress packet if it does not contain buffer.
                if (!(packet is IPacketBuffer) || (packet as IPacketBuffer).BufferLength == 0)
                {
                    needCompression = false;
                }
            }

            return needCompression;
        }

        private static CompressionAlgorithm GetCompressionAlgorithm(Smb2CompressiblePacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role)
        {
            bool needCompression = IsCompressionNeeded(packet, compressionInfo, role);

            if (!needCompression)
            {
                return CompressionAlgorithm.NONE;
            }

            var result = GetPreferredCompressionAlgorithm(compressionInfo);

            return result;
        }

        private static CompressionAlgorithm GetPreferredCompressionAlgorithm(Smb2CompressionInfo compressionInfo)
        {
            if (compressionInfo.PreferredCompressionAlgorithm == CompressionAlgorithm.NONE)
            {
                var commonSupportedCompressionAlgorithms = Smb2Utility.GetSupportedCompressionAlgorithms(compressionInfo.CompressionIds);

                if (commonSupportedCompressionAlgorithms.Length > 0)
                {
                    return commonSupportedCompressionAlgorithms.First();
                }
                else
                {
                    return CompressionAlgorithm.NONE;
                }
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
