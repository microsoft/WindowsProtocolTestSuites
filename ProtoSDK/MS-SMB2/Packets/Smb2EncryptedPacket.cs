// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2.Packets
{
    /// <summary>
    /// SMB2 encrypted packet.
    /// </summary>
    public class Smb2EncryptedPacket : Smb2Packet
    {
        /// <summary>
        /// Header.
        /// </summary>
        public Transform_Header Header;

        /// <summary>
        /// Compressed data part.
        /// </summary>
        public byte[] EncryptdData;

        /// <summary>
        /// Marshaling this packet to bytes.
        /// </summary>
        /// <returns>The bytes of this packet </returns>
        public override byte[] ToBytes()
        {
            var result = new List<byte>();

            result.AddRange(Smb2Utility.MarshalStructure(Header));

            result.AddRange(EncryptdData);

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
                throw new InvalidOperationException("[MS-SMB2] section 3.3.5.2.1.1 If the size of the message received from the client is not greater than" +
                    "the size of the SMB2 TRANSFORM_HEADER, the server MUST disconnect the connection.");
            }

            Header = Smb2Utility.UnmarshalStructure<Transform_Header>(data.Take(minimumLength).ToArray());

            EncryptdData = data.Skip(minimumLength).ToArray();

            consumedLen = data.Length;
            expectedLen = 0;
        }
    }
}
