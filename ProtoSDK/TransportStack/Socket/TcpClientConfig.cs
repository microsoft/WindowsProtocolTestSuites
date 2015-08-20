// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// TransportConfig stores the configurable parameters used by transport over TCP/UDP.
    /// </summary>
    public class TcpClientConfig : SocketTransportConfig
    {
        #region Constructor

        /// <summary>
        /// construct a SocketTransportConfig object that contains the config for Tcp client.
        /// </summary>
        /// <param name="serverIPEndPoint">
        /// an IPEndPoint object that indicates the server to connect to.
        /// </param>
        public TcpClientConfig(IPEndPoint serverIPEndPoint)
            : base()
        {
            this.Role = Role.Client;
            this.Type = StackTransportType.Tcp;

            this.RemoteIpAddress = serverIPEndPoint.Address;
            this.RemoteIpPort = serverIPEndPoint.Port;
        }

        #endregion
    }
}
