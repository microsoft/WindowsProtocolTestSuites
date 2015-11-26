// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 IOCTL Response packet is sent by the server to transmit the results of a client SMB2 IOCTL Request
    /// </summary>
    public class Smb2IOCtlResponsePacket : Smb2StandardPacket<IOCTL_Response>, IPacketBuffer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2IOCtlResponsePacket()
        {
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
            get { return (PayLoad.InputOffset * PayLoad.OutputOffset == 0) ? (PayLoad.InputCount + PayLoad.OutputCount) : (PayLoad.OutputOffset - PayLoad.InputOffset + PayLoad.OutputCount); }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R IOCTL");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }
            else
            {
                sb.Append(", CtlCode=" + (CtlCode_Values)PayLoad.CtlCode);
            }
            return sb.ToString();
        }
    }
}
