// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.Transport
{
    using System;
    using System.Net;

    /// <summary>
    /// Receive message
    /// </summary>
    /// <param name="remoteEndpoint">Remote IP address</param>
    /// <param name="data">Received message</param>
    public delegate void MessageArrived(IPEndPoint remoteEndpoint, byte[] data);

    /// <summary>
    /// This interface is used for transporting message.
    /// </summary>
    public interface ITransport : IDisposable
    {
        /// <summary>
        /// Receive message
        /// </summary>
        event MessageArrived MessageArrived;

        /// <summary>
        /// Send message to destination
        /// </summary>
        /// <param name="bytes">Message data</param>
        /// <param name="address">Destination address</param>
        /// <param name="port">Destination port</param>
        void SendBytes(byte[] bytes, string address, int port);

        /// <summary>
        /// Start listener 
        /// </summary>
        void StartListening();

        /// <summary>
        /// Stop listener
        /// </summary>
        void StopListening();
    }
}
