// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for startable transport device.<para/>
    /// provides interfaces that can listen, start, expect connect, disconnect and stop.<para/>
    /// such as TcpServer and NetbiosServer.
    /// </summary>
    internal interface IAcceptable : ITransport
    {
        /// <summary>
        /// expect connection from client.<para/>
        /// the underlayer transport must be TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout.
        /// </param>
        /// <returns>
        /// an object that contains the connection.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and NetbiosServer.
        /// </exception>
        object ExpectConnect(TimeSpan timeout);


        /// <summary>
        /// expect the server to disconnect<para/>
        /// the underlayer transport must be TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <param name="remoteEndPoint">
        /// an object that specifies which endpoint is expected to be disconnected.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and NetbiosServer.
        /// </exception>
        void ExpectDisconnect(TimeSpan timeout, object remoteEndPoint);


        /// <summary>
        /// disconnect from a special remote host.<para/>
        /// the underlayer transport must be TcpServer or NetbiosServer.
        /// </summary>
        /// <param name="remoteEndPoint">
        /// an object that specifies the endpoint to be disconnected.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer and NetbiosServer.
        /// </exception>
        void Disconnect(object remoteEndPoint);
    }
}
