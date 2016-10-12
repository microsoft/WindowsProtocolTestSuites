// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    /// <summary>
    /// All Peer Content Caching and Retrieval: Hosted Cache Protocol request messages 
    /// use a common connection information structure, which describes the information 
    /// needed by the hosted cache to use the Peer Content Caching and Retrieval: Retrieval 
    /// Protocol [MS-PCCRR] as a client-role peer, to download needed blocks from the 
    /// client as a server-role peer.
    /// </summary>
    public struct CONNECTION_INFORMATION
    {
        /// <summary>
        /// Port (2 bytes): A 16-bit unsigned integer that MUST be set by the client to the port 
        /// on which it is listening as a server-role peer, for use with the retrieval protocol.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public ushort Port;

        /// <summary>
        /// Padding (6 bytes): The value of this field is indeterminate and MUST be ignored on processing
        /// </summary>
        [StaticSize(6)]
        public byte[] Padding;
    }
}
