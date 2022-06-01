// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    /// <summary>
    /// Specifies the transport type used by Ws-Discovery.
    /// </summary>
    public enum TransportType
    {
        /// <summary>
        /// Represents no transport type
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents soap messages using the udp transport protocol
        /// </summary>
        SoapOverUdp = 1,
    }
}

