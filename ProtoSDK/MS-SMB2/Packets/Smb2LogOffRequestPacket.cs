// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 LOGOFF Request packet
    /// </summary>
    public class Smb2LogOffRequestPacket : Smb2StandardPacket<LOGOFF_Request>
    {
        public Smb2LogOffRequestPacket()
        {
            // The fixed length fields are 4 bytes in total
            PayLoad.StructureSize = 4;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("C LOGOFF");
            sb.Append(", SessionId=" + Header.SessionId);
            return sb.ToString();
        }
    }
}
