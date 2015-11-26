// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 IOCTL Request packet is sent by a client to issue an implementation-specific file 
    /// system control or device control (FSCTL/IOCTL) command across the network.
    /// </summary>
    public class Smb2IOCtlRequestPacket : Smb2StandardPacket<IOCTL_Request>, IPacketBuffer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2IOCtlRequestPacket()
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
            get { return PayLoad.InputCount + PayLoad.OutputCount; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C IOCTL");
            sb.Append(", CtlCode=" + (CtlCode_Values)PayLoad.CtlCode);
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            return sb.ToString();
        }
    }
}
