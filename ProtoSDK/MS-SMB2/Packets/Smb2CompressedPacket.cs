// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// SMB2 compressed packet.
    /// </summary>
    public class Smb2CompressedPacket : Smb2Packet
    {
        /// <summary>
        /// Header.
        /// </summary>
        public Compression_Transform_Header Header;

        /// <summary>
        /// Uncompressed data part.
        /// </summary>
        public byte[] UncompressedData;

        /// <summary>
        /// Compressed data part.
        /// </summary>
        public byte[] CompressedData;

        /// <summary>
        /// The original packet before compression.
        /// </summary>
        public Smb2CompressiblePacket OriginalPacket { get; internal set; }

        /// <summary>
        /// Marshaling this packet to bytes.
        /// </summary>
        /// <returns>The bytes of this packet </returns>
        public override byte[] ToBytes()
        {
            var result = new List<byte>();

            result.AddRange(TypeMarshal.ToBytes(Header));

            result.AddRange(UncompressedData);

            result.AddRange(CompressedData);

            return result.ToArray();
        }

        /// <summary>
        /// Build a Smb2CompressedPacket from a byte array.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <param name="consumedLen">The consumed data length.</param>
        /// <param name="expectedLen">The expected data length.</param>
        internal override void FromBytes(byte[] data, out int consumedLen, out int expectedLen)
        {
            int minimumLength = Marshal.SizeOf(Header);
            if (data.Length < minimumLength)
            {
                throw new InvalidOleVariantTypeException("Not enough data for Compression_Transform_Header!");
            }
            int offset = 0;
            Header = TypeMarshal.ToStruct<Compression_Transform_Header>(data, ref offset);

            if (offset + Header.Offset > data.Length)
            {
                throw new InvalidOleVariantTypeException("Not enough data for Smb2CompressedPacket!");
            }
            UncompressedData = new byte[Header.Offset];
            Array.Copy(data, offset, UncompressedData, 0, Header.Offset);

            long compressedDataLength = data.Length - offset - Header.Offset;
            CompressedData = new byte[compressedDataLength];
            Array.Copy(data, offset + Header.Offset, CompressedData, 0, compressedDataLength);

            consumedLen = data.Length;
            expectedLen = 0;
        }
    }
}
