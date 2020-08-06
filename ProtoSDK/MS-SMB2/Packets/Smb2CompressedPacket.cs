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
    public abstract class Smb2CompressedPacket : Smb2Packet
    {
        /// <summary>
        /// Header.
        /// </summary>
        public Compression_Transform_Header Header;

        /// <summary>
        /// The original packet before compression.
        /// </summary>
        public Smb2CompressiblePacket OriginalPacket { get; internal set; }

        public static bool IsChained(byte[] data)
        {
            var header = TypeMarshal.ToStruct<Compression_Transform_Header>(data);

            bool result = header.Flags.HasFlag(Compression_Transform_Header_Flags.SMB2_COMPRESSION_FLAG_CHAINED);

            return result;
        }

        public static Smb2CompressedPacket Create(byte[] data)
        {
            bool isChained = IsChained(data);

            Smb2CompressedPacket result;

            int consumedLength;

            int expectedLength;

            if (isChained)
            {
                var compressedPacket = new Smb2ChainedCompressedPacket();

                compressedPacket.FromBytes(data, out consumedLength, out expectedLength);

                result = compressedPacket;
            }
            else
            {
                var compressedPacket = new Smb2NonChainedCompressedPacket();

                compressedPacket.FromBytes(data, out consumedLength, out expectedLength);

                result = compressedPacket;
            }

            return result;
        }
    }
}
