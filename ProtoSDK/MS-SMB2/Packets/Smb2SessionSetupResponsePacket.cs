// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 SESSION_SETUP Response packet is sent by the server in response to an SMB2 SESSION_SETUP Request packet
    /// </summary>
    public class Smb2SessionSetupResponsePacket : Smb2StandardPacket<SESSION_SETUP_Response>, IPacketBuffer
    {
        public byte[] MessageBytes;

        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// </summary>
        public ushort BufferOffset
        {
            // 64 bytes of packet header and 8 bytes of fixed length fields
            get { return 64 + 8; }
        }

        public uint BufferLength
        {
            get { return PayLoad.SecurityBufferLength; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R SESSION_SETUP");

            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }
            else
            {
                sb.Append(", SessionFlags=" + PayLoad.SessionFlags);
                sb.Append(", SessionId=" + Header.SessionId);
            }
            return sb.ToString();
        }
    }
}
