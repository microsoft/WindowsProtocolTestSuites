// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    /// <summary>
    /// The transport type for RPC call.
    /// </summary>
    public enum TransportType
    {
        /// <summary>
        /// the TCP/IP.
        /// </summary>
        TcpIp = 0,

        /// <summary>
        /// Named pipe.
        /// </summary>
        NamedPipe = 1,
    }
}
