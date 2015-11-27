// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// A structure contains information about session
    /// </summary>
    public class Smb2ClientSession
    {
        /// <summary>
        /// An 8-byte identifier returned by the server to identify this session on this SMB2 transport connection.
        /// </summary>
        public ulong SessionId
        {
            get;
            set;
        }

        /// <summary>
        /// A table of tree connects, as specified in section 3.2.1.6.
        /// The table MUST allow lookup by both TreeConnect.TreeConnectId and by share name
        /// </summary>
        internal Dictionary<uint, Smb2ClientTreeConnect> TreeConnectTable
        {
            get;
            set;
        }

        /// <summary>
        /// The first 16 bytes of the cryptographic key for this authenticated context.
        /// If the cryptographic key is less than 16 bytes, it is right-padded with zero bytes.
        /// </summary>
        public byte[] SessionKey
        {
            get;
            set;
        }

        /// <summary>
        /// A Boolean that indicates whether this session MUST sign communication if signing is enabled on this connection.
        /// </summary>
        internal bool ShouldSign
        {
            get;
            set;
        }

        /// <summary>
        /// A reference to the connection on which this session was established.
        /// </summary>
        internal Smb2ClientConnection Connection
        {
            get;
            set;
        }

        /// <summary>
        /// An OS-specific security context of the user who initiated the session.
        /// </summary>
        internal ICredential UserCredentials
        {
            get;
            set;
        }
    }
}
