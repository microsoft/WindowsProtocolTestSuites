// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 Lease Break Acknowledgment packet is sent by the client in response 
    /// to an SMB2 Lease Break Notification packet sent by the server
    /// </summary>
    public class Smb2LeaseBreakAckPacket : Smb2StandardPacket<LEASE_BREAK_Acknowledgment>
    {
        public Smb2LeaseBreakAckPacket()
        {
            // The fixed length fields are 36 bytes in total
            PayLoad.StructureSize = 36;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C LEASE BREAK Acknowledgment");
            sb.Append(", LeaseState=" + PayLoad.LeaseState);
            return sb.ToString();
        }
    }
}
