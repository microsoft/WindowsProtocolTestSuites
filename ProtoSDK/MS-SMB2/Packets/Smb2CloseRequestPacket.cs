// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 CLOSE Request packet is used by the client to close an instance of a file 
    /// that was opened previously with a successful SMB2 CREATE Request.
    /// </summary>
    public class Smb2CloseRequestPacket : Smb2StandardPacket<CLOSE_Request>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2CloseRequestPacket()
        {
            // The fixed length fields are 24 bytes in total
            PayLoad.StructureSize = 24;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C CLOSE");
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            return sb.ToString();
        }
    }
}
