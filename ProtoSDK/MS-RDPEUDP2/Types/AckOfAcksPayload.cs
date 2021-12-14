// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers.Binary;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// The AckOfAcks payload carries information about the lowest unacknowledged packets on the Sender side.
    /// </summary>
    public struct AckOfAcksPayload : IRdpeudp2PayloadBase
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies the lower 16 bits of the sequence number the Sender is waiting to receive acknowledgment of.
        /// </summary>
        public ushort AckOfAcksSeqNum;

        public AckOfAcksPayload(ReadOnlySpan<byte> data, out int consumedLength)
        {
            AckOfAcksSeqNum = BinaryPrimitives.ReadUInt16LittleEndian(data);

            consumedLength = 2;
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[2];

            BinaryPrimitives.WriteUInt16LittleEndian(bytes, AckOfAcksSeqNum);

            return bytes;
        }

        public override string ToString()
        {
            return $"To update the receiver window lower bound to {AckOfAcksSeqNum}";
        }
    }
}
