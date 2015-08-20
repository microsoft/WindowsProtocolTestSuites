// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for source transport device.<para/>
    /// provides methods to send to target that is stored and no need to specify.<para/>
    /// such as TcpClient, Stream and NetbiosClient.
    /// </summary>
    internal interface ISourceSend : ITransport
    {
        /// <summary>
        /// Send a packet over the transport.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
        /// </summary>
        /// <param name="packet">
        /// a StackPacket object that contains the packet to send to target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when packet is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, Stream and NetbiosClient.
        /// </exception>
        void SendPacket(StackPacket packet);


        /// <summary>
        /// Send an arbitrary message over the transport.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
        /// </summary>
        /// <param name="message">
        /// a bytes array that contains the data to send to target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when message is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, Stream and NetbiosClient.
        /// </exception>
        void SendBytes(byte[] message);
    }
}
