// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// TransportConfig stores the configurable parameters used by transport over TCP/UDP.
    /// </summary>
    public class SocketTransportConfig : TransportConfig
    {
        #region Fields

        /// <summary>
        /// the max connections supported by the transport.
        /// </summary>
        private int maxConnections;

        /// <summary>
        /// the local IPAddress.
        /// </summary>
        private IPAddress localIpAddress;

        /// <summary>
        /// the local IP port.
        /// </summary>
        private int localIpPort;

        /// <summary>
        /// the remote IPAddress.
        /// </summary>
        private IPAddress remoteIpAddress;

        /// <summary>
        /// the remote IP port.
        /// </summary>
        private int remoteIpPort;

        /// <summary>
        /// Timeout to wait before fail the connection.
        /// </summary>
        private TimeSpan timeout;

        #endregion

        #region Properties

        /// <summary>
        /// get/set an int value that specifies the max connections supported by the transport.<para/>
        /// only for TcpServerTransport.
        /// </summary>
        public int MaxConnections
        {
            get
            {
                return this.maxConnections;
            }
            set
            {
                this.maxConnections = value;
            }
        }


        /// <summary>
        /// get/set an IPAddress object that specifies the local IPAddress.<para/>
        /// only for TcpServerTransport and UdpTransport.
        /// </summary>
        public IPAddress LocalIpAddress
        {
            get
            {
                return this.localIpAddress;
            }
            set
            {
                this.localIpAddress = value;
            }
        }


        /// <summary>
        /// get/set an int value that indicates the local IP port.<para/>
        /// only for TcpServerTransport and UdpTransport.
        /// </summary>
        public int LocalIpPort
        {
            get
            {
                return this.localIpPort;
            }
            set
            {
                this.localIpPort = value;
            }
        }


        /// <summary>
        /// get/set an IPAddress object that specifies the remote IPAddress.<para/>
        /// only for TcpClientTransport and UdpTransport.
        /// </summary>
        public IPAddress RemoteIpAddress
        {
            get
            {
                return this.remoteIpAddress;
            }
            set
            {
                this.remoteIpAddress = value;
            }
        }


        /// <summary>
        /// get/set an int value that indicates the remote IP port.<para/>
        /// only for TcpClientTransport and UdpTransport.
        /// </summary>
        public int RemoteIpPort
        {
            get
            {
                return this.remoteIpPort;
            }
            set
            {
                this.remoteIpPort = value;
            }
        }

        /// <summary>
        /// get/set the connection timeout, only for TcpClientTransport.
        /// </summary>
        public TimeSpan Timeout
        {
            get
            {
                return this.timeout;
            }
            set
            {
                this.timeout = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        public SocketTransportConfig()
            : base()
        {
        }

        #endregion
    }
}
