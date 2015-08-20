// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// TransportConfig stores the configurable parameters used by transport over TCP/UDP.
    /// </summary>
    public class TcpServerConfig : SocketTransportConfig
    {
        #region Constructor

        /// <summary>
        /// construct a SocketTransportConfig object that contains the config for Tcp server.
        /// </summary>
        public TcpServerConfig()
            : base()
        {
            this.Role = Role.Server;
            this.Type = StackTransportType.Tcp;
        }


        /// <summary>
        /// construct a SocketTransportConfig object that contains the config for Tcp server.
        /// </summary>
        /// <param name="localIPEndPoint">
        /// an IPEndPoint object that indicates the endpoint of server to bind and listen.
        /// </param>
        public TcpServerConfig(IPEndPoint localIPEndPoint)
            : this()
        {
            this.LocalIpAddress = localIPEndPoint.Address;
            this.LocalIpPort = localIPEndPoint.Port;
        }

        #endregion
    }
}
