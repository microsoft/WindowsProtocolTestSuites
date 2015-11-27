// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 CANCEL Request packet is sent by the client to cancel a previously sent 
    /// message on the same SMB2 transport connection.
    /// </summary>
    public class Smb2CancelRequestPacket : Smb2StandardPacket<CANCEL_Request>
    {
        public Smb2CancelRequestPacket()
        {
            // The fixed length fields are 4 bytes in total
            PayLoad.StructureSize = 4;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C CANCEL");

            sb.Append(", MessageId=" + Header.MessageId);
            sb.Append(", SessionId=" + Header.SessionId);
            return sb.ToString();
        }
    }
}
