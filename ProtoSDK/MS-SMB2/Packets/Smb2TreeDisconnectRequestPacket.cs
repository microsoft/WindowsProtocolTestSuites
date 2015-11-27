// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 TREE_DISCONNECT Request packet is sent by the client to request that the tree connect that is specified 
    /// in the TreeId within the SMB2 header be disconnected
    /// </summary>
    public class Smb2TreeDisconnectRequestPacket : Smb2StandardPacket<TREE_DISCONNECT_Request>
    {
        public Smb2TreeDisconnectRequestPacket()
        {
            // The fixed length fields are 4 bytes in total
            PayLoad.StructureSize = 4;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("C TREE DISCONNECT");
            sb.Append(", TreeId=" + string.Format("0x{0:x}", Header.TreeId));
            return sb.ToString();
        }
    }
}
