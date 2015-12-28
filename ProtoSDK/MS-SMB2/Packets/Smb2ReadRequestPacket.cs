// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 READ Request packet is sent by the client to request a read operation 
    /// on the file that is specified by the FileId
    /// </summary>
    public class Smb2ReadRequestPacket : Smb2StandardPacket<READ_Request>, IPacketBuffer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2ReadRequestPacket()
        {
            // The fixed length fields are 48 bytes in total, plus 1 representing the variable length buffer
            PayLoad.StructureSize = 49;
            // The client MUST set one byte of this field to 0, and the server MUST ignore it on receipt.
            Buffer = new byte[1];
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
            get { return PayLoad.ReadChannelInfoOffset == 0 ? (uint)1 : PayLoad.ReadChannelInfoLength; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C READ");
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            sb.Append(", Read " + PayLoad.Length + " bytes from offset " + PayLoad.Offset);
            return sb.ToString();
        }
    }
}
