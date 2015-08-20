// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the role of transport
    /// </summary>
    public enum Role : int
    {
        /// <summary>
        /// none
        /// </summary>
        None = 0x00,

        /// <summary>
        /// client role
        /// </summary>
        Client = 0x01,

        /// <summary>
        /// server role
        /// </summary>
        Server = 0x02,

        /// <summary>
        /// p2p role
        /// </summary>
        P2P = 0x03,
    }
}
