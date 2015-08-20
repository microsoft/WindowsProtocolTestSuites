// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for connectable transport device.<para/>
    /// provides interfaces that can connect and disconnect<para/>
    /// such as TcpClient, NetbiosClient, TcpServer and NetbiosServer.
    /// </summary>
    internal interface IDisconnectable : ITransport
    {
        /// <summary>
        /// disconnect from remote host.<para/>
        /// the underlayer transport must be TcpClient, NetbiosClient, TcpServer or NetbiosServer.<para/>
        /// client side will disconnect the connection to server.<para/>
        /// server side will disconnect all client connection.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, NetbiosClient, TcpServer and NetbiosServer.
        /// </exception>
        void Disconnect();


        /// <summary>
        /// expect the server to disconnect<para/>
        /// the underlayer transport must be TcpClient, NetbiosClient, TcpServer or NetbiosServer.<para/>
        /// client side will expect the disconnection from server.<para/>
        /// server side will expect the disconnection from any client.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout for this operation.
        /// </param>
        /// <returns>
        /// return an object that is disconnected. client return null.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, NetbiosClient, TcpServer and NetbiosServer.
        /// </exception>
        object ExpectDisconnect(TimeSpan timeout);
    }
}
