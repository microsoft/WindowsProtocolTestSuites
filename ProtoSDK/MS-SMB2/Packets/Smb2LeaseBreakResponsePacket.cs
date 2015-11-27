// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 Lease Break Response packet is sent by the server in response to a Lease Break Acknowledgment from the client
    /// </summary>
    public class Smb2LeaseBreakResponsePacket : Smb2StandardPacket<LEASE_BREAK_Response>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R LEASE BREAK");
            sb.Append(", LeaseState=" + PayLoad.LeaseState);
            return sb.ToString();
        }
    }
}
