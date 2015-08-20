// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for target transport device.<para/>
    /// provides interfaces that need to specify a target to transport<para/>
    /// the transport must be UdpServer
    /// </summary>
    internal interface ITargetSend : ITransport
    {
        /// <summary>
        /// Send a packet to a special remote host.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the local endpoint to send bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the target endpoint to which send packet.
        /// </param>
        /// <param name="packet">
        /// a StackPacket object that contains the packet to send to target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when packet is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not UdpServer.
        /// </exception>
        void SendPacket(object localEndPoint, object remoteEndPoint, StackPacket packet);


        /// <summary>
        /// Send arbitrary message to a special remote host.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the local endpoint to send bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the target endpoint to which send bytes.
        /// </param>
        /// <param name="message">
        /// a bytes array that contains the bytes to send to target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when message is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not UdpServer.
        /// </exception>
        void SendBytes(object localEndPoint, object remoteEndPoint, byte[] message);
    }
}
