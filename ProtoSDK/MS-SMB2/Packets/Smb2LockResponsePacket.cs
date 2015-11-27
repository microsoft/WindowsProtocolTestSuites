// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 LOCK Response packet is sent by a server in response to an SMB2 LOCK Request (section 2.2.26) packet
    /// </summary>
    public class Smb2LockResponsePacket : Smb2StandardPacket<LOCK_Response>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R LOCK");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }

            return sb.ToString();
        }
    }
}
