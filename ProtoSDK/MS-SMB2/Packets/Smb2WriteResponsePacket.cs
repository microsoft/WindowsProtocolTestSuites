// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 WRITE Response packet is sent in response to an SMB2 WRITE Request (section 2.2.21) packet
    /// </summary>
    public class Smb2WriteResponsePacket : Smb2StandardPacket<WRITE_Response>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R WRITE");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }
            else
            {
                sb.Append(", Written " + PayLoad.Count + " bytes");
            }
            return sb.ToString();
        }
    }
}
