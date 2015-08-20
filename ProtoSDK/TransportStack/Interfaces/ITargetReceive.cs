// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for target transport device.<para/>
    /// provides interfaces that need to specify a target to transport<para/>
    /// such as TcpServer, NetbiosServer and UdpServer.
    /// </summary>
    internal interface ITargetReceive : ITransport
    {
        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be a TcpServer or UdpServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that indicates the remote endpoint received data from.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received data at.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and UdpServer.
        /// </exception>
        byte[] ExpectBytes(TimeSpan timeout, int maxCount, out object remoteEndPoint, out object localEndPoint);


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be a TcpServer or UdpServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the remote endpoint expected packet.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the local endpoint received packet at.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and UdpServer.
        /// </exception>
        StackPacket ExpectPacket(TimeSpan timeout, out object remoteEndPoint, out object localEndPoint);
    }
}
