// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    public class QuicClientConfig : SocketTransportConfig
    {
        public QuicClientConfig()
            : base()
        {
            this.Role = Role.Client;

            this.Type = StackTransportType.Quic;
        }

        public QuicClientConfig(IPEndPoint serverIPEndPoint)
            : base()
        {
            this.RemoteIpAddress = serverIPEndPoint.Address;

            this.RemoteIpPort = serverIPEndPoint.Port;
        }
    }
}
