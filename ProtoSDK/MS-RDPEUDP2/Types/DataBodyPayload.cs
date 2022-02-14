// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers.Binary;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// The DataBody payload contains the Remote Desktop Protocol data that is being sent over the UDP transport.
    /// </summary>
    public struct DataBodyPayload : IRdpeudp2PayloadBase
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies the lower 16 bits of channel sequence number.
        /// </summary>
        public ushort ChannelSeqNum;

        /// <summary>
        /// A variable size array of bytes sent by higher layers of the Remote Desktop Protocol stack.
        /// </summary>
        public byte[] Data;

        public DataBodyPayload(ReadOnlySpan<byte> data, out int consumedLength)
        {
            ChannelSeqNum = BinaryPrimitives.ReadUInt16LittleEndian(data);
            Data = data[2..].ToArray();

            consumedLength = 2 + Data.Length;
        }

        public byte[] ToBytes()
        {
            using var writer = new MemoryStream();

            Span<byte> buffer = stackalloc byte[2];
            BinaryPrimitives.WriteUInt16LittleEndian(buffer, ChannelSeqNum);
            writer.Write(buffer);

            writer.Write(Data, 0, Data.Length);

            return writer.ToArray();
        }
    }
}
