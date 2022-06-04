// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    public class QuicServerConfig : TransportConfig
    {
        #region Fields

        private IPAddress localIpAddress;

        private int localIpPort;

        private IPAddress remoteIpAddress;

        private int remoteIpPort;

        private TimeSpan timeout;

        private X509Certificate2 serverCertificate;

        #endregion

        #region Properties

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

        public X509Certificate2 ServerCertificate
        {
            get 
            { 
                return serverCertificate; 
            }
            set 
            { 
                serverCertificate = value; 
            }
        }

        #endregion

        #region Constructors

        public QuicServerConfig()
            : base()
        {
            this.Role = Role.Server;

            this.Type = StackTransportType.Quic;
        }

        public QuicServerConfig(IPEndPoint localIPEndPoint)
            : base()
        {
            this.LocalIpAddress = localIPEndPoint.Address;

            this.LocalIpPort = localIPEndPoint.Port;
        }

        public QuicServerConfig(IPEndPoint localIPEndPoint, X509Certificate2 serverCertificate)
            : this(localIPEndPoint)
        {
            this.serverCertificate = serverCertificate;
        }

        #endregion
    }
}
