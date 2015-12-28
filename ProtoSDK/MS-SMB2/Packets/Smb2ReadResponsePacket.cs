// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 READ Response packet is sent in response to an SMB2 READ Request (section 2.2.19) packet
    /// </summary>
    public class Smb2ReadResponsePacket : Smb2StandardPacket<READ_Response>, IPacketBuffer
    {
        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// </summary>
        public ushort BufferOffset
        {
            // 64 bytes of packet header and 16 bytes of fixed length fields
            get { return 64 + 16; }
        }

        public uint BufferLength
        {
            get { return PayLoad.DataLength; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R READ");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }
            else
            {
                sb.Append(", Read " + PayLoad.DataLength + " bytes");
            }
            return sb.ToString();
        }
    }
}
