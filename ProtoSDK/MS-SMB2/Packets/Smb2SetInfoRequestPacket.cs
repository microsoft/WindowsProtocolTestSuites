// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 SET_INFO Request packet is sent by a client to set information on a file or underlying object store
    /// </summary>
    public class Smb2SetInfoRequestPacket : Smb2StandardPacket<SET_INFO_Request>, IPacketBuffer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2SetInfoRequestPacket()
        {
            // The fixed length fields are 32 bytes in total, plus 1 representing the variable length buffer
            PayLoad.StructureSize = 33;
        }

        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// </summary>
        public ushort BufferOffset
        {
            // 64 bytes of packet header and 32 bytes of fixed length fields
            get { return 64 + 32; }
        }

        public uint BufferLength
        {
            get { return PayLoad.BufferLength; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C SET INFO");
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            sb.Append(", InfoType=" + (SET_INFO_Request_InfoType_Values)PayLoad.InfoType);
            return sb.ToString();
        }
    }
}
