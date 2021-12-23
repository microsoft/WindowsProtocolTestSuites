// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers.Binary;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// The DataHeader payload is the header portion of the data payload.
    /// </summary>
    public struct DataHeaderPayload : IRdpeudp2PayloadBase
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies lower 16 bits of the sequence number representing this data segment.
        /// </summary>
        public ushort DataSeqNum;

        public DataHeaderPayload(ReadOnlySpan<byte> data, out int consumedLength)
        {
            DataSeqNum = BinaryPrimitives.ReadUInt16LittleEndian(data);

            consumedLength = 2;
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[2];

            BinaryPrimitives.WriteUInt16LittleEndian(bytes, DataSeqNum);

            return bytes;
        }
    }
}
