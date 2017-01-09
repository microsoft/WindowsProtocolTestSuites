// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 CREATE Request packet is sent by a client to request either creation of or access to a file.
    /// </summary>
    public class Smb2CreateRequestPacket : Smb2StandardPacket<CREATE_Request>, IPacketBuffer
    {
        public Smb2CreateRequestPacket()
        {
            // The fixed length fields are 56 bytes in total, plus 1 representing the variable length buffer
            PayLoad.StructureSize = 57;
        }

        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// </summary>
        public ushort BufferOffset
        {
            // 64 bytes of packet header and 56 bytes of fixed length fields
            get { return 64 + 56; }
        }

        public uint BufferLength
        {
            get 
            {
                // In the request, the Buffer field MUST be at least one byte in length.
                if (PayLoad.CreateContextsOffset == 0 && PayLoad.NameOffset == 0)
                {
                    return 1;
                }
                else if (PayLoad.CreateContextsOffset == 0)
                {
                    return PayLoad.NameLength;
                }
                else
                {
                    return PayLoad.CreateContextsOffset - PayLoad.NameOffset + PayLoad.CreateContextsLength;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C CREATE");
            byte[] debug = this.Buffer;
            sb.Append(", File=" + Encoding.Unicode.GetString(this.Buffer.Take(PayLoad.NameLength).ToArray()));
            sb.Append(", DesiredAccess=" + PayLoad.DesiredAccess);
            string shareAccess = "";
            if (PayLoad.ShareAccess == ShareAccess_Values.NONE)
            {
                shareAccess = "None";
            }
            else
            {
                if ((PayLoad.ShareAccess & ShareAccess_Values.FILE_SHARE_READ) == ShareAccess_Values.FILE_SHARE_READ)
                {
                    shareAccess += "R";
                }
                if ((PayLoad.ShareAccess & ShareAccess_Values.FILE_SHARE_WRITE) == ShareAccess_Values.FILE_SHARE_WRITE)
                {
                    shareAccess += "W";
                }
                if ((PayLoad.ShareAccess & ShareAccess_Values.FILE_SHARE_DELETE) == ShareAccess_Values.FILE_SHARE_DELETE)
                {
                    shareAccess += "D";
                }
            }

            sb.Append(", ShareAccess=(" + shareAccess + ")");
            return sb.ToString();
        }
    }
}
