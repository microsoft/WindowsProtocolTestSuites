// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The Oplock Break Acknowledgment packet is sent by the client 
    /// in response to an SMB2 Oplock Break Notification packet sent by the server
    /// </summary>
    public class Smb2OpLockBreakAckPacket : Smb2StandardPacket<OPLOCK_BREAK_Acknowledgment>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2OpLockBreakAckPacket()
        {
            // The fixed length fields are 24 bytes in total
            PayLoad.StructureSize = 24;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C OPLOCK BREAK Acknowledgment");
            sb.Append(", FileId=" + string.Format("0x{0:x}.", PayLoad.FileId.Volatile));
            sb.Append(", OplockLevel=" + (OPLOCK_BREAK_Acknowledgment_OplockLevel_Values)PayLoad.OplockLevel);
            return sb.ToString();
        }
    }
}
