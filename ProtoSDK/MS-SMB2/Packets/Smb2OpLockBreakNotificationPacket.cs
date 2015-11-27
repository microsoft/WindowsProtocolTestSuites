// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 Oplock Break Notification packet is sent by the server 
    /// when the underlying object store indicates that an opportunistic lock (oplock) is being broken,
    /// representing a change in the oplock level
    /// </summary>
    public class Smb2OpLockBreakNotificationPacket : Smb2StandardPacket<OPLOCK_BREAK_Notification_Packet>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2OpLockBreakNotificationPacket()
        {

        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("N Oplock Break Notification");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }
            else
            {
                sb.Append(", FileId=");
                sb.Append(PayLoad.FileId.ToString());
                sb.Append(", OplockLevel=" + PayLoad.OplockLevel);
            }
            return sb.ToString();
        }
    }
}
