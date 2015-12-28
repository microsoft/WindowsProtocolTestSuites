// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 CHANGE_NOTIFY Request packet is sent by the client to request change notifications on a directory.
    /// </summary>
    public class Smb2ChangeNotifyRequestPacket : Smb2StandardPacket<CHANGE_NOTIFY_Request>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2ChangeNotifyRequestPacket()
        {
            // The fixed length fields are 32 bytes in total
            PayLoad.StructureSize = 32;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("C CHANGE NOTIFY");
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            return sb.ToString();
        }
    }
}
