// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The Oplock Break Response packet is sent by the server in response to
    /// an Oplock Break Acknowledgment from the client
    /// </summary>
    public class Smb2OpLockBreakResponsePacket : Smb2StandardPacket<OPLOCK_BREAK_Response>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2OpLockBreakResponsePacket()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R OPLOCK BREAK");
            sb.Append(", FileId=");
            sb.Append(PayLoad.FileId.ToString());
            sb.Append(", OplockLevel=" + PayLoad.OplockLevel);
            return sb.ToString();
        }
    }
}
