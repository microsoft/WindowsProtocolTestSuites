// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// The Acknowledgment Vector payload contains the information of a vector of acknowledgments for all packets within the current Receiver Window (section 3.1.1.2.2).
    /// </summary>
    public struct AcknowledgementVectorPayload : IRdpeudp2PayloadBase
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies the lower 16 bits of sequence number for the base of this acknowledgment array.
        /// </summary>
        public ushort BaseSeqNum;

        /// <summary>
        /// A 7-bit unsigned integer that specifies the number of entries in the codedAckVector field.
        /// </summary>
        public byte codedAckVecSize;

        /// <summary>
        /// A 1-bit flag that indicates if the TimeStamp is present.
        /// </summary>
        public bool TimeStampPresent;

        /// <summary>
        /// A 24-bit unsigned integer that specifies the lower 24 bits of the timestamp for the highest sequence number received that has not been acknowledged yet.
        /// </summary>
        public uint? TimeStamp;

        /// <summary>
        /// An 8-bit unsigned integer that specifies the time interval (in milliseconds) between the sent time of the current Acknowledgement Vector packet and the arrival time of the latest data packet that has been received.
        /// </summary>
        public byte? SendAckTimeGapInMs;

        /// <summary>
        /// An array of bytes, of size specified by codedAckVecSize, which contains a list describing the acknowledgement state of each packet in the range starting at BaseSeqNum.
        /// </summary>
        public byte[] codedAckVector;

        public AcknowledgementVectorPayload(ReadOnlySpan<byte> data, out int consumedLength)
        {
            BaseSeqNum = BinaryPrimitives.ReadUInt16LittleEndian(data);

            var buffer = data[2];
            codedAckVecSize = (byte)(0b01111111 & buffer);
            TimeStampPresent = (byte)((0b10000000 & buffer) >> 7) == 1 ? true : false;

            if (TimeStampPresent)
            {
                TimeStamp = BinaryPrimitives.ReadUInt32LittleEndian(data[3..]) & 0x00FFFFFF;
                SendAckTimeGapInMs = data[6];
            }
            else
            {
                TimeStamp = null;
                SendAckTimeGapInMs = null;
            }

            if (codedAckVecSize > 0)
            {
                codedAckVector = data[7..(7 + codedAckVecSize)].ToArray();
            }
            else
            {
                codedAckVector = new byte[0];
            }

            consumedLength = 2 + 1 + (TimeStampPresent ? 3 + 1 : 0) + codedAckVector.Length;
        }

        public byte[] ToBytes()
        {
            using var writer = new MemoryStream();

            Span<byte> buffer1 = stackalloc byte[2];
            BinaryPrimitives.WriteUInt16LittleEndian(buffer1, BaseSeqNum);
            writer.Write(buffer1);

            byte buffer2 = 0;
            buffer2 |= codedAckVecSize;
            buffer2 |= (byte)((TimeStampPresent ? 1 : 0) << 7);
            writer.WriteByte(buffer2);

            if (TimeStampPresent)
            {
                Span<byte> buffer3 = stackalloc byte[4];
                BinaryPrimitives.WriteUInt32LittleEndian(buffer3, TimeStamp.Value);
                writer.Write(buffer3[..^1]);

                writer.WriteByte(SendAckTimeGapInMs.Value);
            }

            if (codedAckVecSize > 0)
            {
                writer.Write(codedAckVector);
            }

            return writer.ToArray();
        }

        /// <summary>
        /// Get a sequence of acknowledged SeqNums.
        /// </summary>
        /// <returns>The sequence of acknowledged SeqNums.</returns>
        public List<uint> GetAckedSeq()
        {
            var codedVecs = codedAckVector;
            var baseSeqNum = BaseSeqNum;

            var ackedSeq = new List<uint>();

            for (var index = 0; index < codedAckVecSize; index++)
            {
                var codedVec = codedVecs[index];
                if (((codedVec & 0b10000000) >> 7) == 0)
                {
                    // State map mode
                    for (var j = 0; j < 7; j++)
                    {
                        if ((codedVec & (1 << j)) == (1 << j))
                        {
                            ackedSeq.Add(baseSeqNum++);
                        }
                        else
                        {
                            baseSeqNum++;
                        }
                    }
                }
                else
                {
                    // Run-Length mode
                    var state = (codedVec & 0b01000000) >> 6;
                    var legnth = codedVec & 0b00111111;
                    if (state == 1)
                    {
                        for (var j = 0; j < legnth; j++)
                        {
                            ackedSeq.Add(baseSeqNum++);
                        }
                    }
                    else
                    {
                        baseSeqNum += (ushort)(legnth - 1);
                    }
                }
            }

            return ackedSeq;
        }

        public override string ToString()
        {
            var ackedSeq = GetAckedSeq();
            var ackedSeqNumsStr = string.Join(", ", ackedSeq);

            return $"BaseSeqNum: {BaseSeqNum}, Received packets with the following SeqNums: {ackedSeqNumsStr}";
        }
    }
}
