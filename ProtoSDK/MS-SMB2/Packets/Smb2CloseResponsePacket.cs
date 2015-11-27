// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 CLOSE Response packet is sent by the server to indicate that an 
    /// SMB2 CLOSE Request was processed successfully
    /// </summary>
    public class Smb2CloseResponsePacket : Smb2StandardPacket<CLOSE_Response>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R CLOSE");
            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }

            return sb.ToString();
        }
    }
}
