// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 QUERY_DIRECTORY Request packet is sent by the client to obtain a directory enumeration on a directory open
    /// </summary>
    public class Smb2QueryDirectoryRequestPacket : Smb2StandardPacket<QUERY_DIRECTORY_Request>, IPacketBuffer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2QueryDirectoryRequestPacket()
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
            get { return PayLoad.FileNameLength; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C QUERY DIRECTORY");
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            sb.Append(", File=" + Encoding.Unicode.GetString(this.Buffer.Take(PayLoad.FileNameLength).ToArray()));
            return sb.ToString();
        }
    }
}
