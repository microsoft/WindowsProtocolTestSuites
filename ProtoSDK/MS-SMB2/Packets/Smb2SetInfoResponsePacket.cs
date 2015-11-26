// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 SET_INFO Response packet is sent by the server in response to an SMB2 SET_INFO Request
    /// (section 2.2.39) to notify the client that its request has been successfully processed
    /// </summary>
    public class Smb2SetInfoResponsePacket : Smb2StandardPacket<SET_INFO_Response>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R SET INFO");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }

            return sb.ToString();
        }
    }
}
