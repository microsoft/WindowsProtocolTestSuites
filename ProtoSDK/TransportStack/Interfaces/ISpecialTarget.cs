// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for special target transport device.<para/>
    /// provides interfaces that bind to the special target<para/>
    /// such as TcpServer and NetbiosServer can Receive data from specified target.
    /// </summary>
    internal interface ISpecialTarget : ITarget
    {
        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that indicates the connection to received data from.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and NetbiosServer.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        byte[] ExpectBytes(TimeSpan timeout, int maxCount, object remoteEndPoint);


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies the connection to expect packet.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and NetbiosServer.
        /// </exception>
        StackPacket ExpectPacket(TimeSpan timeout, object remoteEndPoint);
    }
}
