// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the type of transport.
    /// </summary>
    public enum StackTransportType
    {
        /// <summary>
        /// none
        /// </summary>
        None = 0x00,

        /// <summary>
        /// TCP transport
        /// </summary>
        Tcp = 0x01,

        /// <summary>
        /// UDP transport
        /// </summary>
        Udp = 0x02,

        /// <summary>
        /// Stream transport
        /// </summary>
        Stream = 0x03,

        /// <summary>
        /// Netbios transport
        /// </summary>
        Netbios = 0x04,
    }
}
