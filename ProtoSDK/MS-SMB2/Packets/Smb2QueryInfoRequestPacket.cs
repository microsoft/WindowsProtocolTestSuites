// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 QUERY_INFO Request (section 2.2.37) packet is sent by 
    /// a client to request information on a file, named pipe, or underlying volume
    /// </summary>
    public class Smb2QueryInfoRequestPacket : Smb2StandardPacket<QUERY_INFO_Request>, IPacketBuffer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2QueryInfoRequestPacket()
        {
            // The fixed length fields are 40 bytes in total, plus 1 representing the variable length buffer
            PayLoad.StructureSize = 41;
        }

        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// </summary>
        public ushort BufferOffset
        {
            // 64 bytes of packet header and 40 bytes of fixed length fields
            get { return 64 + 40; }
        }

        public uint BufferLength
        {
            get { return PayLoad.InputBufferLength; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C QUERY INFO");
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            sb.Append(", InfoType=" + (InfoType_Values)PayLoad.InfoType);
            return sb.ToString();
        }
    }
}
