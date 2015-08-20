// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for special target transport device.<para/>
    /// provides interfaces that bind to the special target<para/>
    /// the transport must be UdpServer
    /// </summary>
    internal interface ILocalTarget : ITransport
    {
        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the local listening endpoint to received bytes.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.<para/>
        /// return the whole received bytes, even though the maxCount is smaller that the bytes length.
        /// </param>
        /// <param name="remoteEndPoint">
        /// return an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the remote listening endpoint to received bytes.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not UdpServer
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        byte[] ExpectBytes(TimeSpan timeout, int maxCount, object localEndPoint, out object remoteEndPoint);


        /// <summary>
        /// expect packet from transport.<para/>
        /// the transport must be UdpServer
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <param name="localEndPoint">
        /// an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the local listening endpoint to received bytes.
        /// </param>
        /// <param name="remoteEndPoint">
        /// return an object that indicates the connection to received data from.<para/>
        /// it's an IPEndPoint that specifies the remote listening endpoint to received bytes.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not UdpServer
        /// </exception>
        StackPacket ExpectPacket(TimeSpan timeout, object localEndPoint, out object remoteEndPoint);
    }
}
