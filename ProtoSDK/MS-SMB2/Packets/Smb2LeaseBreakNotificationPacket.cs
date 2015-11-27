// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 Lease Break Notification packet is sent by the server when the underlying object 
    /// store indicates that a lease is being broken, representing a change in the lease state.
    /// </summary>
    public class Smb2LeaseBreakNotificationPacket : Smb2StandardPacket<LEASE_BREAK_Notification_Packet>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R Lease Break NOTIFICATION");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }
            else
            {
                sb.Append(", LeaseState was changed from " + PayLoad.CurrentLeaseState + " to " + PayLoad.NewLeaseState);
            }
            return sb.ToString();
        }
    }
}
