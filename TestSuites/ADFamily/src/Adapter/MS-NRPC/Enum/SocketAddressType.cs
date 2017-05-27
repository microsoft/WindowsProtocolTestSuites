// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// Socket address.
    /// </summary>
    public enum SocketAddressType
    {
        /// <summary>
        /// An IPv4 socket address.
        /// </summary>
        Ipv4SocketAddress,

        /// <summary>
        /// An IPv6 socket address.
        /// </summary>
        Ipv6SocketAddress,

        /// <summary>
        /// An invalid socket address which does not map to any site.
        /// </summary>
        InvalidSocketAddress,

        /// <summary>
        /// An invalid format address which is neither ipv4 nor ipv6.
        /// </summary>
        InvalidFormatSocketAddress,
    }
}
