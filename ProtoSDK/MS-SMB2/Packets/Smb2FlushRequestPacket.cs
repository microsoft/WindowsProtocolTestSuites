// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 FLUSH Request packet is sent by a client to request that a server flush all cached 
    /// file information for a specified open of a file to the persistent store that backs the file
    /// </summary>
    public class Smb2FlushRequestPacket : Smb2StandardPacket<FLUSH_Request>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2FlushRequestPacket()
        {
            // The fixed length fields are 24 bytes in total
            PayLoad.StructureSize = 24;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C FLUSH");
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            return sb.ToString();
        }
    }
}
