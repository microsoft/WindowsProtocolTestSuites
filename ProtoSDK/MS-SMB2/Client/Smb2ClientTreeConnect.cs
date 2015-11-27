// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// A structure contains information about treeConnect
    /// </summary>
    public class Smb2ClientTreeConnect
    {
        /// <summary>
        /// The name of the share which the treeconnect established
        /// </summary>
        public string ShareName
        {
            get;
            set;
        }

        /// <summary>
        /// A 4-byte identifier returned by the server to identify this tree connect.
        /// </summary>
        public uint TreeConnectId
        {
            get;
            set;
        }

        /// <summary>
        /// A reference to the session on which this tree connect was established.
        /// </summary>
        public Smb2ClientSession Session
        {
            get;
            set;
        }

        /// <summary>
        /// A Boolean that, if set, indicates that the tree connect was established to a DFS share.
        /// </summary>
        public bool IsDfsShare
        {
            get;
            set;
        }
    }
}
