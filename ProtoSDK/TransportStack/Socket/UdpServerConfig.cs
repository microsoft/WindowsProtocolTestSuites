// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// TransportConfig stores the configurable parameters used by transport over TCP/UDP.
    /// </summary>
    public class UdpServerConfig : SocketTransportConfig
    {
        #region Constructor

        /// <summary>
        /// construct a SocketTransportConfig object that contains the config for Udp transport.
        /// </summary>
        public UdpServerConfig()
            : base()
        {
            this.Type = StackTransportType.Udp;
            this.Role = Role.Server;
        }


        /// <summary>
        /// constructor a SocketTransportConfig object that contains the config for Udp transport.
        /// </summary>
        /// <param name="localEndPoint">
        /// an IPEndPoint object that represents the local port to which you bind the UDP connection.
        /// </param>
        public UdpServerConfig(IPEndPoint localEndPoint)
            : this()
        {
            this.LocalIpAddress = localEndPoint.Address;
            this.LocalIpPort = localEndPoint.Port;
        }

        #endregion
    }
}
