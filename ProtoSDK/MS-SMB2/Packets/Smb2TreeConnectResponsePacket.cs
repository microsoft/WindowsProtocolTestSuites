// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 TREE_CONNECT Response packet is sent by the server when an SMB2 TREE_CONNECT request is processed successfully by the server
    /// </summary>
    public class Smb2TreeConnectResponsePacket : Smb2StandardPacket<TREE_CONNECT_Response>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R TREE CONNECT");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }
            else
            {
                sb.Append(", TreeId=" + string.Format("0x{0:x}", Header.TreeId));
            }
            return sb.ToString();
        }
    }
}
