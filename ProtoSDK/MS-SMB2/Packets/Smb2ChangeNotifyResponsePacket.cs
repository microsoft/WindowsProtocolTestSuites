// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 CHANGE_NOTIFY Response packet is sent by the server to transmit the results 
    /// of a client's SMB2 CHANGE_NOTIFY Request (section 2.2.35).
    /// </summary>
    public class Smb2ChangeNotifyResponsePacket : Smb2StandardPacket<CHANGE_NOTIFY_Response>, IPacketBuffer
    {
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
            get { return PayLoad.OutputBufferLength; }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R CHANGE NOTIFY");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }

            return sb.ToString();
        }
    }
}
