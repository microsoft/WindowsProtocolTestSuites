// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// The PacketPrefixByte is a byte that MUST be sent with each RDP-UDP2 packet.
    /// </summary>
    public struct PacketPrefixByte : IRdpeudp2PayloadBase
    {
        /// <summary>
        /// A 1-bit integer that is reserved and MUST be set to 0.
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// A 4-bit unsigned integer that indicates the type of the packet.
        /// The value for this field MUST be set to either 0 or 8.
        /// </summary>
        public byte Packet_Type_Index;

        /// <summary>
        /// A 3-bit unsigned integer that specifies the length, in bytes.
        /// </summary>
        public byte Short_Packet_Length;

        public PacketPrefixByte(ReadOnlySpan<byte> data, out int consumedLength)
        {
            var prefixByteOnWire = data[0];

            Reserved = (byte)(0b00000001 & prefixByteOnWire);
            Packet_Type_Index = (byte)((0b00011110 & prefixByteOnWire) >> 1);
            Short_Packet_Length = (byte)((0b11100000 & prefixByteOnWire) >> 5);

            consumedLength = 1;
        }

        public byte[] ToBytes()
        {
            byte prefixByteOnWire = 0;

            prefixByteOnWire |= Reserved;
            prefixByteOnWire |= (byte)(Packet_Type_Index << 1);
            prefixByteOnWire |= (byte)(Short_Packet_Length << 5);

            return new byte[] { prefixByteOnWire };
        }
    }
}
