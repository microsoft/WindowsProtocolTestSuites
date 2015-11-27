// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 LOCK Request packet is sent by the client to either lock or unlock portions of a file
    /// </summary>
    public class Smb2LockRequestPacket : Smb2StandardPacket<LOCK_Request>, IPacketBuffer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2LockRequestPacket()
        {
            // The fixed length fields are 48 bytes in total
            PayLoad.StructureSize = 48;
        }

        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// </summary>
        public ushort BufferOffset
        {
            // 64 bytes of packet header and 48 bytes of fixed length fields
            get { return 64 + 48; }
        }

        public uint BufferLength
        {
            get
            {
                return (uint)24 * PayLoad.LockCount;
            }
        }

        public LOCK_ELEMENT[] Locks
        {
            get
            {
                return Smb2Utility.UnmarshalStructArray<LOCK_ELEMENT>(Buffer, PayLoad.LockCount);
            }
            set
            {
                Buffer = Smb2Utility.MarshalStructArray(value);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C LOCK");
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            return sb.ToString();
        }
    }
}
