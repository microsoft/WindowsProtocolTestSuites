// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for connectable transport device.<para/>
    /// provides interfaces that can connect and disconnect<para/>
    /// such as TcpClient and NetbiosClient
    /// </summary>
    internal interface IConnectable : ITransport
    {
        /// <summary>
        /// connect to remote endpoint.<para/>
        /// the underlayer transport must be TcpClient, UdpClient or NetbiosClient.
        /// </summary>
        /// <returns>
        /// the remote endpoint of the connection.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient, UdpClient and NetbiosClient
        /// </exception>
        object Connect();
    }
}
