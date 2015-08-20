// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// contains the information for ssl authentication and ssl security provider
    /// </summary>
    internal abstract class SslProvider
    {
        #region Fields

        /// <summary>
        /// a X509Certificate that specifies the certificate used to authenticate the client.
        /// </summary>
        private X509Certificate certificate;

        /// <summary>
        /// the SslProtocols value that represents the protocol used for authentication.
        /// </summary>
        private SslProtocols enabledSslProtocols;

        #endregion

        #region Properties

        /// <summary>
        /// get/set a X509Certificate that specifies the certificate used to authenticate the client.
        /// </summary>
        public X509Certificate Certificate
        {
            get
            {
                return this.certificate;
            }
            set
            {
                this.certificate = value;
            }
        }


        /// <summary>
        /// get/set the SslProtocols value that represents the protocol used for authentication.
        /// </summary>
        public SslProtocols EnabledSslProtocols
        {
            get
            {
                return this.enabledSslProtocols;
            }
        }

        #endregion

        #region Construcor

        /// <summary>
        /// Constructor
        /// </summary>
        protected SslProvider()
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the client.
        /// </param>
        /// <param name="enabledSslProtocols">
        /// the SslProtocols value that represents the protocol used for authentication.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when targetHost is null
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when certificate is null.
        /// </exception>
        protected SslProvider(X509Certificate certificate, SslProtocols enabledSslProtocols)
            : this()
        {
            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            this.certificate = certificate;
            this.enabledSslProtocols = enabledSslProtocols;
        }

        #endregion
    }

    /// <summary>
    /// contains the information for ssl authentication and ssl security provider
    /// </summary>
    internal class ClientSslProvider : SslProvider
    {
        #region Fields

        /// <summary>
        /// a string that indicates the name of the server that shares the SSL.
        /// </summary>
        private string targetHost;

        #endregion

        #region Properties

        /// <summary>
        /// get/set a string that indicates the name of the server that shares the SSL.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public string TargetHost
        {
            get
            {
                return this.targetHost;
            }
        }

        #endregion

        #region Construcor

        /// <summary>
        /// Constructor, for client side.
        /// </summary>
        /// <param name="targetHost">
        /// a string that indicates the name of the server that shares the SSL.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when targetHost is null
        /// </exception>
        public ClientSslProvider(string targetHost)
            : base()
        {
            if (targetHost == null)
            {
                throw new ArgumentNullException("targetHost");
            }

            this.targetHost = targetHost;
        }


        /// <summary>
        /// Constructor, for client side.
        /// </summary>
        /// <param name="targetHost">
        /// a string that indicates the name of the server that shares the SSL.
        /// </param>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the client.
        /// </param>
        /// <param name="enabledSslProtocols">
        /// the SslProtocols value that represents the protocol used for authentication.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when targetHost is null
        /// </exception>
        public ClientSslProvider(string targetHost, X509Certificate certificate, SslProtocols enabledSslProtocols)
            : base(certificate, enabledSslProtocols)
        {
            if (targetHost == null)
            {
                throw new ArgumentNullException("targetHost");
            }

            this.targetHost = targetHost;
        }

        #endregion
    }

    /// <summary>
    /// contains the information for ssl authentication and ssl security provider
    /// </summary>
    internal class ServerSslProvider : SslProvider
    {
        #region Fields

        /// <summary>
        /// A Boolean value that specifies whether the client must supply a certificate for authentication.
        /// </summary>
        private bool clientCertificateRequired;

        #endregion

        #region Properties

        /// <summary>
        /// A Boolean value that specifies whether the client must supply a certificate for authentication.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// thrown when this object is disposed.
        /// </exception>
        public bool ClientCertificateRequired
        {
            get
            {
                return this.clientCertificateRequired;
            }
        }

        #endregion

        #region Construcor

        /// <summary>
        /// constructor, for server side.
        /// </summary>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the client.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when certificate is null
        /// </exception>
        public ServerSslProvider(X509Certificate certificate)
            : base()
        {
            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            this.Certificate = certificate;
        }


        /// <summary>
        /// constructor, for server side.
        /// </summary>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the client.
        /// </param>
        /// <param name="enabledSslProtocols">
        /// The SslProtocols value that represents the protocol used for authentication.
        /// </param>
        /// <param name="clientCertificateRequired">
        /// A Boolean value that specifies whether the client must supply a certificate for authentication.
        /// </param>
        public ServerSslProvider(
            X509Certificate certificate, SslProtocols enabledSslProtocols, bool clientCertificateRequired)
            : base(certificate, enabledSslProtocols)
        {
            this.clientCertificateRequired = clientCertificateRequired;
        }

        #endregion
    }
}
