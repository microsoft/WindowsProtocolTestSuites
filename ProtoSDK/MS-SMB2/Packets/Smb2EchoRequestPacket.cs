// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 ECHO Request packet is sent by a client to determine whether a server is processing requests.
    /// </summary>
    public class Smb2EchoRequestPacket : Smb2StandardPacket<ECHO_Request>
    {
        public Smb2EchoRequestPacket()
        {
            // The fixed length fields are 4 bytes in total
            PayLoad.StructureSize = 4;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C ECHO");

            return sb.ToString();
        }
    }
}
