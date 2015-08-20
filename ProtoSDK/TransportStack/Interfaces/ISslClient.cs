// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for ssl client<para/>
    /// provides ssl client authenticate services<para/>
    /// such as TcpClient.
    /// </summary>
    internal interface ISslClient : ITransport
    {
        /// <summary>
        /// startup the ssl security provider.<para/>
        /// must invoke it before connect to server.<para/>
        /// the underlayer transport must be TcpClient.
        /// </summary>
        /// <param name="targetHost">
        /// a string that indicates the name of the server that shares the SSL/TLS.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient.
        /// </exception>
        void Startup(string targetHost);


        /// <summary>
        /// startup the ssl security provider.<para/>
        /// must invoke it before connect to server.<para/>
        /// the underlayer transport must be TcpClient.
        /// </summary>
        /// <param name="targetHost">
        /// a string that indicates the name of the server that shares the SSL/TLS.
        /// </param>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the client.
        /// </param>
        /// <param name="enabledSslProtocols">
        /// The SslProtocols value that represents the protocol used for authentication.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when certificate is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient.
        /// </exception>
        void Startup(string targetHost, X509Certificate certificate, SslProtocols enabledSslProtocols);
    }
}
