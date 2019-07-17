// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Common class compressible SMB2 packet.
    /// </summary>
    public abstract class Smb2CompressiblePacket : Smb2Packet
    {
        public Smb2CompressiblePacket()
        {
            EligibleForCompression = false;
            Compressed = false;
            CompressedPacket = null;
        }

        /// <summary>
        /// Indicating whether this packet is eligible for compression at sending.
        /// </summary>
        public bool EligibleForCompression { get; set; }

        /// <summary>
        /// Indicating whether this packet is compressed at receiving.
        /// </summary>
        public bool Compressed { get; set; }

        /// <summary>
        /// Containing the received compressed packet at receiving.
        /// </summary>
        public Smb2CompressedPacket CompressedPacket { get; set; }
    }
}
