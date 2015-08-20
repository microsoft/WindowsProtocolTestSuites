// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for ssl server<para/>
    /// provides ssl server authenticate services.
    /// </summary>
    internal interface ISslServer : ITransport
    {
        /// <summary>
        /// startup the ssl security provider.<para/>
        /// must invoke it before start server.<para/>
        /// the underlayer transport must be TcpServer.
        /// </summary>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the server.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when certificate is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when the underlayer transport is not TcpClient.
        /// </exception>
        void Startup(X509Certificate certificate);


        /// <summary>
        /// startup the ssl security provider.<para/>
        /// must invoke it before start server.<para/>
        /// the underlayer transport must be TcpServer.
        /// </summary>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the server.
        /// </param>
        /// <param name="clientCertificateRequired">
        /// A Boolean value that specifies whether the client must supply a certificate for authentication.
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
        void Startup(
            X509Certificate certificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols);
    }
}
