// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers.Binary;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// A 12-bit unsigned integer that specifies a bitmap of flags indicating the presence of optional payloads that follow the header.
    /// </summary>
    [Flags]
    public enum Rdpeudp2PacketHeaderFlags : ushort
    {
        /// <summary>
        /// ACK payload (section 2.2.1.2.1) is present. This flag MUST NOT be set if the ACKVEC flag is set.
        /// </summary>
        ACK = 0x001,

        /// <summary>
        /// DataHeader payload (section 2.2.1.2.5) and DataBody payload (section 2.2.1.2.7) are present.
        /// </summary>
        DATA = 0x004,

        /// <summary>
        /// ACK Vector payload (section 2.2.1.2.6) is present. This flag MUST NOT be set if the ACK flag is set.
        /// </summary>
        ACKVEC = 0x008,

        /// <summary>
        /// AckOfAcks payload (section 2.2.1.2.4) is present.
        /// </summary>
        AOA = 0x010,

        /// <summary>
        /// OverheadSize payload (section 2.2.1.2.2) is present.
        /// </summary>
        OVERHEADSIZE = 0x040,

        /// <summary>
        /// DelayAckInfo payload (section 2.2.1.2.3) is present.
        /// </summary>
        DELAYACKINFO = 0x100
    }

    /// <summary>
    /// The Header field is mandatory, and it specifies the presence of the various optional payloads that follow.
    /// </summary>
    public struct Rdpeudp2PacketHeader : IRdpeudp2PayloadBase
    {
        /// <summary>
        /// A 12-bit unsigned integer that specifies a bitmap of flags indicating the presence of optional payloads that follow the header.
        /// </summary>
        public Rdpeudp2PacketHeaderFlags Flags;

        /// <summary>
        /// A 4-bit unsigned integer that specifies the logarithm base 2 of the maximum buffer size in multiples of the MTU.
        /// </summary>
        public byte LogWindowSize;

        public Rdpeudp2PacketHeader(ReadOnlySpan<byte> data, out int consumedLength)
        {
            var headerBytes = data[0..2];

            Span<byte> flagsBytes = stackalloc byte[2] { headerBytes[0], (byte)(0x0F & headerBytes[1]) };
            Flags = (Rdpeudp2PacketHeaderFlags)BinaryPrimitives.ReadUInt16LittleEndian(flagsBytes);
            LogWindowSize = (byte)((0xF0 & headerBytes[1]) >> 4);

            consumedLength = 2;
        }

        public byte[] ToBytes()
        {
            var headerBytes = new byte[2];

            BinaryPrimitives.WriteUInt16LittleEndian(headerBytes, (ushort)Flags);
            headerBytes[1] |= (byte)(LogWindowSize << 4);

            return headerBytes;
        }
    }
}
