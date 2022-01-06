// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// The Acknowledgment payload carries acknowledgment information for one or more packets received by the Receiver.
    /// </summary>
    public struct AcknowledgementPayload : IRdpeudp2PayloadBase
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies the lower 16 bits of the sequence number for the data packet that is acknowledged.
        /// </summary>
        public ushort SeqNum;

        /// <summary>
        /// A 24-bit unsigned integer that specifies the lower 24 bits of the time stamp as of when the packet that is being received.
        /// </summary>
        public uint receivedTS;

        /// <summary>
        /// An 8-bit unsigned integer that specifies the time interval in milliseconds between when the packet being acknowledged was received and when the ACK was sent for that packet.
        /// </summary>
        public byte sendAckTimeGap;

        /// <summary>
        /// A 4-bit unsigned integer that specifies the number of delayed ACKs.
        /// </summary>
        public byte numDelayedAcks;

        /// <summary>
        /// A 4-bit unsigned integer that specifies the scale applied to the time differences for all the delayed ACKs carried in this packet.
        /// </summary>
        public byte delayAckTimeScale;

        /// <summary>
        /// delayAckTimeAdditions is an array of bytes with numDelayedAcks as the array size, and each byte represents the time difference between 2 adjacent acknowledgments in units of (1<<delayedAckTimeScale) microseconds.
        /// The array is arranged in reverse order for the packets to be acknowledged.
        /// </summary>
        public byte[] delayAckTimeAdditions;

        public AcknowledgementPayload(ReadOnlySpan<byte> data, out int consumedLength)
        {
            SeqNum = BinaryPrimitives.ReadUInt16LittleEndian(data);
            receivedTS = BinaryPrimitives.ReadUInt32LittleEndian(data[2..]) & 0x00FFFFFF;
            sendAckTimeGap = data[5];

            var buffer = data[6];
            numDelayedAcks = (byte)(0x0F & buffer);
            delayAckTimeScale = (byte)((0xF0 & buffer) >> 4);

            if (numDelayedAcks > 0)
            {
                delayAckTimeAdditions = data[7..(7 + numDelayedAcks)].ToArray();
            }
            else
            {
                delayAckTimeAdditions = new byte[0];
            }

            consumedLength = 2 + 3 + 1 + 1 + delayAckTimeAdditions.Length;
        }

        public byte[] ToBytes()
        {
            using var writer = new MemoryStream();

            Span<byte> buffer1 = stackalloc byte[2];
            BinaryPrimitives.WriteUInt16LittleEndian(buffer1, SeqNum);
            writer.Write(buffer1);

            Span<byte> buffer2 = stackalloc byte[4];
            BinaryPrimitives.WriteUInt32LittleEndian(buffer2, receivedTS);
            writer.Write(buffer2[..^1]);

            writer.WriteByte(sendAckTimeGap);

            byte buffer3 = 0;
            buffer3 |= numDelayedAcks;
            buffer3 |= (byte)(delayAckTimeScale << 4);
            writer.WriteByte(buffer3);

            if (numDelayedAcks > 0)
            {
                writer.Write(delayAckTimeAdditions);
            }

            return writer.ToArray();
        }

        /// <summary>
        /// Get a sequence of acknowledged SeqNums.
        /// </summary>
        /// <returns>The sequence of acknowledged SeqNums.</returns>
        public List<uint> GetAckedSeq()
        {
            var baseSeqNum = SeqNum;

            var ackedSeq = new List<uint>();
            ackedSeq.Add(baseSeqNum);

            if (numDelayedAcks == 0)
            {
                return ackedSeq;
            }

            for (var i = 0; i < numDelayedAcks; i++)
            {
                ackedSeq.Add(--baseSeqNum);
            }

            return ackedSeq;
        }

        public override string ToString()
        {
            if (numDelayedAcks > 0)
            {
                return $"To acknowledge SeqNum {SeqNum} and previous {numDelayedAcks} SeqNums";
            }
            else
            {
                return $"To acknowledge SeqNum {SeqNum}";
            }
        }
    }
}
