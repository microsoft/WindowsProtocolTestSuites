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
    internal interface ISourceReceive : ITransport
    {
        /// <summary>
        /// to receive bytes from connection.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="maxCount">
        /// an int value that specifies the maximum count of expect bytes.
        /// </param>
        /// <returns>
        /// a bytes array that contains the received bytes.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, Stream and NetbiosClient.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// thrown when maxCount is negative.
        /// </exception>
        byte[] ExpectBytes(TimeSpan timeout, int maxCount);


        /// <summary>
        /// expect packet from transport.<para/>
        /// the underlayer transport must be TcpClient, Stream or NetbiosClient.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that indicates the timeout to expect event.
        /// </param>
        /// <returns>
        /// a StackPacket object that specifies the received packet.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, Stream and NetbiosClient.
        /// </exception>
        StackPacket ExpectPacket(TimeSpan timeout);
    }
}
