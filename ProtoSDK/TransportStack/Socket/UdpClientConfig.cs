// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// TransportConfig stores the configurable parameters used by transport over TCP/UDP.
    /// </summary>
    public class UdpClientConfig : SocketTransportConfig
    {
        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        protected UdpClientConfig()
            : base()
        {
            this.Type = StackTransportType.Udp;
            this.Role = Role.Client;
        }


        /// <summary>
        /// construct a SocketTransportConfig object that contains the config for Udp transport.
        /// </summary>
        /// <param name="serverIPEndPoint">
        /// an IPEndPoint object that indicates the server to connect to.
        /// </param>
        public UdpClientConfig(IPEndPoint serverIPEndPoint)
            : this()
        {
            this.RemoteIpAddress = serverIPEndPoint.Address;
            this.RemoteIpPort = serverIPEndPoint.Port;
        }


        /// <summary>
        /// constructor a SocketTransportConfig object that contains the config for Udp transport.
        /// </summary>
        /// <param name="localPort">
        /// an int value that represents the local port to which you bind the UDP connection.
        /// </param>
        /// <param name="serverIPEndPoint">
        /// an IPEndPoint object that indicates the server to connect to.
        /// </param>
        public UdpClientConfig(int localPort, IPEndPoint serverIPEndPoint)
            : this(serverIPEndPoint)
        {
            this.LocalIpPort = localPort;
        }

        #endregion
    }
}
