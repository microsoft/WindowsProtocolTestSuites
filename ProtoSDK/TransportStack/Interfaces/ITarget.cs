// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for target transport device.<para/>
    /// provides interfaces that need to specify a target to transport<para/>
    /// such as TcpServer, NetbiosServer and UdpClient.
    /// </summary>
    internal interface ITarget : ITransport
    {
        /// <summary>
        /// Send a packet to a special remote host.<para/>
        /// the transport must be a TcpServer or UdpClient.
        /// </summary>
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
        /// thrown when the underlayer transport is not TcpServer and UdpClient.
        /// </exception>
        void SendPacket(object remoteEndPoint, StackPacket packet);


        /// <summary>
        /// Send arbitrary message to a special remote host.<para/>
        /// the transport must be a TcpServer or UdpClient.
        /// </summary>
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
        /// thrown when the underlayer transport is not TcpServer and UdpClient.
        /// </exception>
        void SendBytes(object remoteEndPoint, byte[] message);


        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be a TcpServer or UdpClient.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that indicates the connection received data from.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and UdpClient.
        /// </exception>
        byte[] ExpectBytes(TimeSpan timeout, int maxCount, out object remoteEndPoint);


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a TcpServer or UdpClient.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the connection expected packet.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and UdpClient.
        /// </exception>
        StackPacket ExpectPacket(TimeSpan timeout, out object remoteEndPoint);
    }
}
