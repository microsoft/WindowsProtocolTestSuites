// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 SESSION_SETUP Request packet is sent by the client to request a new authenticated 
    /// session within a new or existing SMB 2 Protocol transport connection to the server
    /// </summary>
    public class Smb2SessionSetupRequestPacket : Smb2StandardPacket<SESSION_SETUP_Request>, IPacketBuffer
    {
        public Smb2SessionSetupRequestPacket()
        {
            // The fixed length fields are 24 bytes in total, plus 1 representing the variable length buffer
            PayLoad.StructureSize = 25;
        }

        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// 64 bytes of packet header and 24 bytes of fixed length fields. 
        /// </summary>
        public ushort BufferOffset
        {
            get { return 64 + 24; }
        }

        public uint BufferLength
        {
            get { return PayLoad.SecurityBufferLength; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C SESSION_SETUP");

            if ((PayLoad.Flags & SESSION_SETUP_Request_Flags.SESSION_FLAG_BINDING) == SESSION_SETUP_Request_Flags.SESSION_FLAG_BINDING)
                sb.Append(", Alternative Channel");

            sb.Append(", SecurityMode=" + PayLoad.SecurityMode);

            sb.Append(", Capabilities=" + PayLoad.Capabilities);

            return sb.ToString();
        }
    }
}
