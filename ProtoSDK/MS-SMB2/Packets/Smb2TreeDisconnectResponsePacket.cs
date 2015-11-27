// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 TREE_DISCONNECT Response packet is sent by the server to confirm 
    /// that an SMB2 TREE_DISCONNECT Request (section 2.2.11) was successfully processed
    /// </summary>
    public class Smb2TreeDisconnectResponsePacket : Smb2StandardPacket<TREE_DISCONNECT_Response>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R TREE DISCONNECT");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }

            return sb.ToString();
        }
    }
}
