// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers.Binary;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// The DelayAckInfo payload carries information about the delay acknowledgment parameters allowed by the Sender.
    /// </summary>
    public struct DelayAckInfoPayload : IRdpeudp2PayloadBase
    {
        /// <summary>
        /// An 8-bit unsigned integer that specifies the maximum number of delayed acknowledgments that can be packed in a single ACK payload (section 2.2.1.2.1).
        /// </summary>
        public byte MaxDelayedAcks;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the timeout in milliseconds before which acknowledgments need to be sent for all packets.
        /// </summary>
        public ushort DelayAckTimeoutInMs;

        public DelayAckInfoPayload(ReadOnlySpan<byte> data, out int consumedLength)
        {
            MaxDelayedAcks = data[0];
            DelayAckTimeoutInMs = BinaryPrimitives.ReadUInt16LittleEndian(data[1..]);

            consumedLength = 3;
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[3];

            bytes[0] = MaxDelayedAcks;
            BinaryPrimitives.WriteUInt16LittleEndian(bytes[1..], DelayAckTimeoutInMs);

            return bytes;
        }
    }
}
