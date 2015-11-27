// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 WRITE Request packet is sent by the client to write data to the file or named pipe on the server
    /// </summary>
    public class Smb2WriteRequestPacket : Smb2StandardPacket<WRITE_Request>, IPacketBuffer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2WriteRequestPacket()
        {
            // The fixed length fields are 48 bytes in total, plus 1 representing the variable length buffer
            PayLoad.StructureSize = 49;
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
            get { return PayLoad.Length; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C WRITE");
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            sb.Append(", Write " + PayLoad.Length + " bytes from offset " + PayLoad.Offset);
            return sb.ToString();
        }
    }
}
