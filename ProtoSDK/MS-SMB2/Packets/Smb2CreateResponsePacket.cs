// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 CREATE Response packet is sent by the server to notify the 
    /// client of the status of its SMB2 CREATE Request.
    /// </summary>
    public class Smb2CreateResponsePacket : Smb2StandardPacket<CREATE_Response>, IPacketBuffer
    {
        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// </summary>
        public ushort BufferOffset
        {
            // 64 bytes of packet header and 88 bytes of fixed length fields
            get { return 64 + 88; }
        }

        public uint BufferLength
        {
            get { return PayLoad.CreateContextsLength; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R CREATE");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }
            else
            {
                sb.Append(", FileId=");
                sb.Append(PayLoad.FileId.ToString());
            }
            return sb.ToString();
        }
    }
}
