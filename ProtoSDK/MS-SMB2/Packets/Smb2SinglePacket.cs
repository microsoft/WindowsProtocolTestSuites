// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Every SinglePacket must inherit from this packet, it contains an smb2 header, but no payload.
    /// </summary>
    public abstract class Smb2SinglePacket : Smb2CompressiblePacket
    {
        /// <summary>
        /// The header of the packet
        /// </summary>
        public Packet_Header Header;

        public ulong AsyncId
        {
            get
            {
                return (Header.TreeId << 32) + Header.ProcessId;
            }
            set
            {
                Header.ProcessId = (uint)(value & 0xFFFFFFFF);
                Header.TreeId = (uint)((value >> 32) & 0xFFFFFFFF);
            }
        }

        /// <summary>
        /// Indicate if the packet is in compoundPacket
        /// </summary>
        internal bool IsInCompoundPacket
        {
            get;
            set;
        }

        /// <summary>
        /// Indicate if the packet is the last packet in a compound packet
        /// </summary>
        internal bool IsLast
        {
            get;
            set;
        }


        /// <summary>
        /// The compound packet who contains this packet
        /// </summary>
        /// <returns>The compound packet who contains this packet</returns>
        internal Smb2CompoundPacket OuterCompoundPacket
        {
            get;
            set;
        }

        /// <summary>
        /// If the packet is the last packet in a compound packet
        /// there may be some padding at the end of the message.
        /// </summary>
        public byte[] Padding
        {
            get;
            set;
        }

        public Smb2ErrorResponsePacket Error
        {
            get;
            set;
        }

        public Smb2SinglePacket()
        {
            // SMB2 signature: 0xFE S M B
            Header.ProtocolId = 0x424d53fe;
            Header.StructureSize = 64;

            Header.ProcessId = 0xFEFF;
            Header.Signature = new byte[Smb2Consts.SignatureSize];

            Padding = new byte[0];
        }

        /// <summary>
        /// Get the index in compoundPacket
        /// </summary>
        /// <returns></returns>
        internal int GetPacketIndexInCompoundPacket()
        {
            if (OuterCompoundPacket == null)
            {
                return 0;
            }

            for (int i = 0; i < OuterCompoundPacket.Packets.Count; i++)
            {
                if (OuterCompoundPacket.Packets[i] == this)
                {
                    return i;
                }
            }

            throw new InvalidOperationException(
                "The OuterCompoundPacket is not the packet which contains this packet");
        }
    }
}
