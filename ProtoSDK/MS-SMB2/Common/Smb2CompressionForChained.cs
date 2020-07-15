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
        private static Smb2Packet CompressForChained(Smb2CompressiblePacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role)
        {
            bool needCompression = IsCompressionNeeded(packet, compressionInfo, role);

            if (!needCompression)
            {
                return packet;
            }

            bool isPatternV1Supported = compressionInfo.CompressionIds.Contains(CompressionAlgorithm.Pattern_V1);

            Smb2Packet compressed;

            if (isPatternV1Supported)
            {
                compressed = CompressWithPatternV1(packet, compressionInfo, role);
            }
            else
            {
                // Regress to non-chained since pattern scanning algorithm is not supported.
                compressed = CompressForNonChained(packet, compressionInfo, role);
            }

            return compressed;
        }

        private static Smb2Packet CompressWithPatternV1(Smb2CompressiblePacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role)
        {
            var data = packet.ToBytes();

            var dataToCompress = data;

            if (compressionInfo.CompressBufferOnly)
            {
                dataToCompress = data.Skip((packet as IPacketBuffer).BufferOffset).ToArray();
            }

            SMB2_COMPRESSION_PATTERN_PAYLOAD_V1? forwardDataPattern;

            SMB2_COMPRESSION_PATTERN_PAYLOAD_V1? backwardDataPattern;

            ScanForPatternV1(dataToCompress, out forwardDataPattern, out backwardDataPattern);

            if (forwardDataPattern == null && backwardDataPattern == null)
            {
                // Regress to non-chained since no pattern is found at front or end.
                return CompressForNonChained(packet, compressionInfo, role);
            }

            var result = new Smb2ChainedCompressedPacket();

            result.OriginalPacket = packet;

            result.Header = new Compression_Transform_Header();

            result.Header.ProtocolId = Smb2Consts.ProtocolIdInCompressionTransformHeader;

            result.Header.OriginalCompressedSegmentSize = (UInt32)data.Length;

            result.Header.Flags = Compression_Transform_Header_Flags.SMB2_COMPRESSION_FLAG_CHAINED;

            var payloads = new List<Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>>();

            bool isFirst = true;

            if (compressionInfo.CompressBufferOnly)
            {
                var header = data.Take((packet as IPacketBuffer).BufferOffset).ToArray();

                var payloadHeader = SMB2_COMPRESSION_PAYLOAD_HEADER.Create(CompressionAlgorithm.NONE, (UInt32)header.Length, 0, ref isFirst);

                payloads.Add(new Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>(payloadHeader, header));
            }

            if (forwardDataPattern != null)
            {
                var payloadHeader = SMB2_COMPRESSION_PAYLOAD_HEADER.Create(CompressionAlgorithm.Pattern_V1, (UInt32)PatternPayloadLength, 0, ref isFirst);

                payloads.Add(new Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>(payloadHeader, forwardDataPattern));
            }

            int forwardDataPatternLength = (int)(forwardDataPattern?.Repetitions ?? 0);

            int backwardDataPatternLength = (int)(backwardDataPattern?.Repetitions ?? 0);

            var innerData = dataToCompress.Skip(forwardDataPatternLength).Take(dataToCompress.Length - forwardDataPatternLength - backwardDataPatternLength).ToArray();

            if (innerData.Length > 0)
            {
                var innerPayload = ChainedCompressWithCompressionAlgorithm(innerData, compressionInfo, ref isFirst);

                payloads.Add(innerPayload);
            }

            if (backwardDataPattern != null)
            {
                var payloadHeader = SMB2_COMPRESSION_PAYLOAD_HEADER.Create(CompressionAlgorithm.Pattern_V1, (UInt32)PatternPayloadLength, 0, ref isFirst);

                payloads.Add(new Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>(payloadHeader, backwardDataPattern));
            }

            result.Payloads = payloads.ToArray();

            return result;
        }

        private static Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object> ChainedCompressWithCompressionAlgorithm(byte[] data, Smb2CompressionInfo compressionInfo, ref bool isFirst)
        {
            var compressionAlgorithm = GetPreferredCompressionAlgorithm(compressionInfo);

            SMB2_COMPRESSION_PAYLOAD_HEADER payloadHeader;

            bool isFirstCopy;

            isFirstCopy = isFirst;

            var compressedPayloadHeader = SMB2_COMPRESSION_PAYLOAD_HEADER.Create(compressionAlgorithm, 0, (UInt32)data.Length, ref isFirstCopy);

            isFirstCopy = isFirst;

            var uncompressedPayloadHeader = SMB2_COMPRESSION_PAYLOAD_HEADER.Create(CompressionAlgorithm.NONE, (UInt32)data.Length, 0, ref isFirstCopy);

            isFirst = isFirstCopy;

            byte[] payloadData;

            if (compressionAlgorithm != CompressionAlgorithm.NONE)
            {
                var compressor = GetCompressor(compressionAlgorithm);

                var compressedData = compressor.Compress(data);

                int compressedDataLength = TypeMarshal.ToBytes(compressedPayloadHeader).Length + compressedData.Length;

                int uncompressedDataLength = TypeMarshal.ToBytes(uncompressedPayloadHeader).Length + data.Length;

                if (compressedDataLength < uncompressedDataLength)
                {
                    compressedPayloadHeader.Length = (UInt32)(compressedData.Length + FieldSizeOriginalPayloadSize);

                    payloadHeader = compressedPayloadHeader;

                    payloadData = compressedData;
                }
                else
                {
                    payloadHeader = uncompressedPayloadHeader;

                    payloadData = data;
                }
            }
            else
            {
                payloadHeader = uncompressedPayloadHeader;

                payloadData = data;
            }

            return new Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>(payloadHeader, payloadData);
        }

        private static void ScanForPatternV1(byte[] data, out SMB2_COMPRESSION_PATTERN_PAYLOAD_V1? forwardDataPattern, out SMB2_COMPRESSION_PATTERN_PAYLOAD_V1? backwardDataPattern)
        {
            forwardDataPattern = null;

            backwardDataPattern = null;

            int endPosition;

            if (data.Length < 64)
            {
                return;
            }

            byte front = data[0];

            int frontRepetition = 1;

            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] == front)
                {
                    frontRepetition++;
                }
                else
                {
                    break;
                }
            }

            if (frontRepetition >= 64)
            {
                forwardDataPattern = new SMB2_COMPRESSION_PATTERN_PAYLOAD_V1()
                {
                    Pattern = front,
                    Reserved1 = 0,
                    Reserved2 = 0,
                    Repetitions = (UInt32)frontRepetition,
                };
            }

            endPosition = frontRepetition;

            byte end = data[data.Length - 1];

            int endRepetition = 1;

            for (int i = data.Length - 2; i >= endPosition; i--)
            {
                if (data[i] == end)
                {
                    endRepetition++;
                }
                else
                {
                    break;
                }
            }

            if (endRepetition >= 64)
            {
                backwardDataPattern = new SMB2_COMPRESSION_PATTERN_PAYLOAD_V1()
                {
                    Pattern = end,
                    Reserved1 = 0,
                    Reserved2 = 0,
                    Repetitions = (UInt32)endRepetition,
                };
            }
        }

        private static byte[] DecompressForChained(Smb2CompressedPacket packet, Smb2CompressionInfo compressionInfo, Smb2Role role)
        {
            var p = packet as Smb2ChainedCompressedPacket;

            var result = p.Payloads.SelectMany(tuple =>
            {
                var payloadHeader = tuple.Item1;

                switch (payloadHeader.CompressionAlgorithm)
                {
                    case CompressionAlgorithm.NONE:
                        {
                            return tuple.Item2 as byte[];
                        }
                        break;

                    case CompressionAlgorithm.LZNT1:
                    case CompressionAlgorithm.LZ77:
                    case CompressionAlgorithm.LZ77Huffman:
                        {
                            var decompressor = GetDecompressor(payloadHeader.CompressionAlgorithm);

                            var decompressedData = decompressor.Decompress(tuple.Item2 as byte[]);

                            if (decompressedData.Length != payloadHeader.OriginalPayloadSize)
                            {
                                throw new InvalidOperationException("The length decompressed chained payload is inconsistent with OriginalPayloadSize of payload header.");
                            }

                            return decompressedData;
                        }
                        break;

                    case CompressionAlgorithm.Pattern_V1:
                        {
                            var pattern = (SMB2_COMPRESSION_PATTERN_PAYLOAD_V1)tuple.Item2;

                            return Enumerable.Repeat(pattern.Pattern, (int)pattern.Repetitions).ToArray();
                        }
                        break;

                    default:
                        {
                            throw new InvalidOperationException("Unexpected compression algorithm!");
                        }
                        break;
                }
            });

            return result.ToArray();
        }
    }
}
