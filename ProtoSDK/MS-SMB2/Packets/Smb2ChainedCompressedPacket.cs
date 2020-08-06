// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// SMB2 chained compression packet.
    /// </summary>
    public class Smb2ChainedCompressedPacket : Smb2CompressedPacket
    {
        /// <summary>
        /// An array containing all chained payload headers and their payloads.
        /// </summary>
        public Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>[] Payloads { get; set; }

        private static int FirstPayloadHeaderOffset;

        private static int FieldSizeOriginalPayloadSize;

        static Smb2ChainedCompressedPacket()
        {
            var header = new Compression_Transform_Header();

            FirstPayloadHeaderOffset = TypeMarshal.GetBlockMemorySize(header.ProtocolId) + TypeMarshal.GetBlockMemorySize(header.OriginalCompressedSegmentSize);

            var payloadHeader = new SMB2_COMPRESSION_PAYLOAD_HEADER();

            FieldSizeOriginalPayloadSize = TypeMarshal.GetBlockMemorySize(payloadHeader.OriginalPayloadSize);
        }

        /// <summary>
        /// Marshaling this packet to bytes.
        /// </summary>
        /// <returns>The bytes of this packet </returns>
        public override byte[] ToBytes()
        {
            if (Payloads.Length == 0)
            {
                throw new InvalidOperationException("There should be at least one payload!");
            }

            var result = new List<byte>();

            var headerBytes = TypeMarshal.ToBytes(Header).Take(FirstPayloadHeaderOffset);

            result.AddRange(headerBytes);

            foreach (var payload in Payloads)
            {
                var payloadHeader = payload.Item1;

                byte[] payloadData;

                switch (payloadHeader.CompressionAlgorithm)
                {
                    case CompressionAlgorithm.NONE:
                    case CompressionAlgorithm.LZNT1:
                    case CompressionAlgorithm.LZ77:
                    case CompressionAlgorithm.LZ77Huffman:
                        {
                            payloadData = payload.Item2 as byte[];
                        }
                        break;

                    case CompressionAlgorithm.Pattern_V1:
                        {
                            var pattern = (SMB2_COMPRESSION_PATTERN_PAYLOAD_V1)payload.Item2;

                            payloadData = TypeMarshal.ToBytes(pattern);
                        }
                        break;

                    default:
                        {
                            // For negative tests.
                            if (payload.Item2 is byte[])
                            {
                                payloadData = payload.Item2 as byte[];
                            }
                            else if (payload.Item2 is SMB2_COMPRESSION_PATTERN_PAYLOAD_V1)
                            {
                                var pattern = (SMB2_COMPRESSION_PATTERN_PAYLOAD_V1)payload.Item2;

                                payloadData = TypeMarshal.ToBytes(pattern);
                            }
                            else
                            {
                                throw new InvalidOperationException("Unexpected payload data format!");
                            }
                        }
                        break;
                }

                result.AddRange(TypeMarshal.ToBytes(payloadHeader));

                result.AddRange(payloadData);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Build an Smb2ChainedCompressedPacket from a byte array.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <param name="consumedLen">The consumed data length.</param>
        /// <param name="expectedLen">The expected data length.</param>
        internal override void FromBytes(byte[] data, out int consumedLen, out int expectedLen)
        {
            Header = TypeMarshal.ToStruct<Compression_Transform_Header>(data);

            if (!Header.Flags.HasFlag(Compression_Transform_Header_Flags.SMB2_COMPRESSION_FLAG_CHAINED))
            {
                throw new InvalidOperationException("Smb2ChainedCompressedPacket should have SMB2_COMPRESSION_FLAG_CHAINED set!");
            }

            int offset = FirstPayloadHeaderOffset;

            var payloads = new List<Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>>();

            while (offset < data.Length)
            {
                var payloadHeader = TypeMarshal.ToStruct<SMB2_COMPRESSION_PAYLOAD_HEADER>(data, ref offset);

                object payloadData;

                switch (payloadHeader.CompressionAlgorithm)
                {
                    case CompressionAlgorithm.NONE:
                    case CompressionAlgorithm.LZNT1:
                    case CompressionAlgorithm.LZ77:
                    case CompressionAlgorithm.LZ77Huffman:
                        {
                            int payloadDataLength;

                            if (payloadHeader.CompressionAlgorithm == CompressionAlgorithm.NONE)
                            {
                                payloadDataLength = (int)payloadHeader.Length;
                            }
                            else
                            {
                                payloadDataLength = (int)(payloadHeader.Length - FieldSizeOriginalPayloadSize);
                            }

                            if (payloadDataLength <= 0 || payloadDataLength >= data.Length - offset)
                            {
                                throw new InvalidOperationException("The value of field Length is invalid.");
                            }

                            var payloadDataBytes = new byte[payloadDataLength];

                            Array.Copy(data, offset, payloadDataBytes, 0, payloadDataLength);

                            offset += payloadDataLength;

                            payloadData = payloadDataBytes;
                        }
                        break;

                    case CompressionAlgorithm.Pattern_V1:
                        {
                            int oldOffset = offset;

                            var pattern = TypeMarshal.ToStruct<SMB2_COMPRESSION_PATTERN_PAYLOAD_V1>(data, ref offset);

                            int patternLength = offset - oldOffset;

                            if (payloadHeader.Length != patternLength)
                            {
                                throw new InvalidOperationException("The Length is inconsistent with size of SMB2_COMPRESSION_PATTERN_PAYLOAD_V1!");
                            }

                            payloadData = pattern;
                        }
                        break;

                    default:
                        {
                            throw new InvalidOperationException("Unexpected compression algorithm!");
                        }
                        break;
                }

                payloads.Add(new Tuple<SMB2_COMPRESSION_PAYLOAD_HEADER, object>(payloadHeader, payloadData));
            }

            Payloads = payloads.ToArray();

            consumedLen = data.Length;

            expectedLen = data.Length;
        }
    }
}
