// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for startable transport device.<para/>
    /// provides interfaces that can listen, start, expect connect, disconnect and stop.<para/>
    /// such as TcpServer, UdpServer and NetbiosServer.
    /// </summary>
    internal interface IStartable : ITransport
    {
        /// <summary>
        /// to start the transport. <para/>
        /// the underlayer transport must be TcpServer, UdpServer or NetbiosServer.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, UdpServer and NetbiosServer.
        /// </exception>
        void Start();


        /// <summary>
        /// start at the specified endpoint<para/>
        /// the underlayer transport must be TcpServer, UdpServer or NetbiosServer.
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the listener.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, UdpServer and NetbiosServer.
        /// </exception>
        void Start(object localEndPoint);


        /// <summary>
        /// stop all listener of server.<para/>
        /// the underlayer transport must be TcpServer, UdpServer or NetbiosServer.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, UdpServer and NetbiosServer.
        /// </exception>
        void Stop();


        /// <summary>
        /// stop the specified listener.<para/>
        /// the underlayer transport must be TcpServer, UdpServer or NetbiosServer.
        /// </summary>
        /// <param name="localEndPoint">
        /// an object that specifies the listener.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpServer, UdpServer and NetbiosServer.
        /// </exception>
        void Stop(object localEndPoint);
    }
}
