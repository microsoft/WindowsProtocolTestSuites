// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 FLUSH Response packet is sent by the server to confirm 
    /// that an SMB2 FLUSH Request (section 2.2.17) was successfully processed
    /// </summary>
    public class Smb2FlushResponsePacket : Smb2StandardPacket<FLUSH_Response>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R FLUSH");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }

            return sb.ToString();
        }
    }
}
