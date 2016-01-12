// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// the SSL/TLS security layer, over SSL/TLS,<para/>
    /// which provides authentication and message security over SSL/TLS specification.
    /// </summary>
    public class AdtsLdapSslTlsSecurityLayer : AdtsLdapSecurityLayer
    {
        #region Fields

        /// <summary>
        /// a const int value that indicates the milliseconds to wait ssl consume data.
        /// </summary>
        private const int MILLI_SECONDS_TO_WAIT_SSL_CONSUME_DATA = 10;

        /// <summary>
        /// an AdtsLdapClient object that provides the transport for SslAuthenticate.
        /// </summary>
        private AdtsLdapClient clientTransport;

        /// <summary>
        /// an AdtsLdapServer object that provides the transport for SslAuthenticate.
        /// </summary>
        private AdtsLdapServer serverTransport;

        /// <summary>
        /// an AdtsLdapContext object that provides the transport context for SslAuthenticate.
        /// </summary>
        private AdtsLdapContext serverContext;

        /// <summary>
        /// a SslStream object that specifies the SSL/TLS security provider.
        /// </summary>
        private SslStream sslStream;

        /// <summary>
        /// an AdtsLdapStreamProxy object that provides the stream proxy services.
        /// </summary>
        private AdtsLdapSecurityStream streamProxy;

        #endregion

        #region Properties

        /// <summary>
        /// get a bool value that specifies whether SslStream is authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return this.sslStream.IsAuthenticated;
            }
        }


        /// <summary>
        /// get an AdtsLdapClient object that provides the transport for SslAuthenticate.
        /// </summary>
        public AdtsLdapClient ClientTransport
        {
            get
            {
                return this.clientTransport;
            }
        }


        /// <summary>
        /// get an AdtsLdapServer object that provides the transport for SslAuthenticate.
        /// </summary>
        public AdtsLdapServer ServerTransport
        {
            get
            {
                return this.serverTransport;
            }
        }

        /// <summary>
        /// get an AdtsLdapContext object that provides the transport context for SslAuthenticate.
        /// </summary>
        public AdtsLdapContext ServerContext
        {
            get
            {
                return this.serverContext;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transport">
        /// an AdtsLdapClient object that provides the transport for SslAuthenticate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when transport is null.
        /// </exception>
        public AdtsLdapSslTlsSecurityLayer(AdtsLdapClient transport)
        {
            if (transport == null)
            {
                throw new ArgumentNullException("transport");
            }

            this.clientTransport = transport;
            this.streamProxy = new AdtsLdapSecurityStream(this);
            this.sslStream = new SslStream(this.streamProxy);
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transport">
        /// an AdtsLdapServer object that provides the transport for SslAuthenticate.
        /// </param>
        /// <param name="context">
        /// an AdtsLdapContext object that provides the transport context for SslAuthenticate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when transport is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when context is null.
        /// </exception>
        public AdtsLdapSslTlsSecurityLayer(AdtsLdapServer transport, AdtsLdapContext context)
        {
            if (transport == null)
            {
                throw new ArgumentNullException("transport");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.serverTransport = transport;
            this.serverContext = context;
            this.streamProxy = new AdtsLdapSecurityStream(this);
            this.sslStream = new SslStream(this.streamProxy);
        }

        #endregion

        #region Methods

        /// <summary>
        /// encoding the data with security provider.
        /// </summary>
        /// <param name="data">
        /// a bytes data that contains the data to be encoded with security provider.
        /// </param>
        /// <returns>
        /// a bytes data that contains the encoded data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        public override byte[] Encode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            // if authenticate is not complete, do not encode data.
            if (!this.sslStream.IsAuthenticated)
            {
                return data;
            }

            // if user do not want to encode data, do not encode.
            if (!this.UsingMessageSecurity)
            {
                return data;
            }

            this.sslStream.Write(data, 0, data.Length);

            byte[] encodedData = this.streamProxy.SentBuffer.Data;

            this.streamProxy.SentBuffer.Clear();

            return encodedData;
        }


        /// <summary>
        /// decoding the data with security provider
        /// </summary>
        /// <param name="data">
        /// a bytes data that contains the data to be decoded with security provider.
        /// </param>
        /// <returns>
        /// a bytes data that contains the decoded data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        public override byte[] Decode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            // if the ssl stream is authenticated, but user do not want to encrypted data
            // directly return the plaintext data.
            if (this.sslStream.IsAuthenticated && !this.UsingMessageSecurity)
            {
                this.consumedData = false;

                return data;
            }

            this.streamProxy.AddReceivedData(data);
            this.consumedData = true;
            this.consumedLength = data.Length;

            // if ssl is not authenticated, and ssl do not need to wait for data
            // that is, data is enough for current ssl, must wait for it to consumed it.
            while (!this.sslStream.IsAuthenticated
                    && !this.streamProxy.WaitingForDataComing)
            {
                Thread.Sleep(MILLI_SECONDS_TO_WAIT_SSL_CONSUME_DATA);
            }

            AdtsLdapSslTlsSecurityHeader header = new AdtsLdapSslTlsSecurityHeader();
            header.FromBytes(this.streamProxy.ReceivedBuffer.Peek(AdtsLdapSslTlsSecurityHeader.SIZE_OF_HEADER));

            // if data is received enough to decode.
            if (this.sslStream.IsAuthenticated && header.IsValid(this.streamProxy.ReceivedBuffer.Length))
            {
                byte[] decryptedData = new byte[header.Length];
                this.sslStream.Read(decryptedData, 0, decryptedData.Length);

                return decryptedData;
            }

            return new byte[0];
        }


        /// <summary>
        /// server authenticate over SSL/TLS with client.
        /// </summary>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the server.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when certificate is null.
        /// </exception>
        public void AuthenticateAsServer(X509Certificate certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            this.sslStream.AuthenticateAsServer(certificate);
        }


        /// <summary>
        /// complete the SSL/TLS authenticate<para/>
        /// which is used to start the SSL/TLS handshake with server,<para/>
        /// verify the certificate from server,<para/>
        /// establish the SSL/TLS security.
        /// </summary>
        /// <param name="targetHost">
        /// a string that indicates the name of the server that shares the SSL/TLS.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when targetHost is null.
        /// </exception>
        public void AuthenticateAsClient(string targetHost)
        {
            if (targetHost == null)
            {
                throw new ArgumentNullException("targetHost");
            }

            this.sslStream.AuthenticateAsClient(targetHost);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// the dispose flags 
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Release resources. 
        /// </summary>
        /// <param name = "disposing">
        /// If disposing equals true, Managed and unmanaged resources are disposed. if false, Only unmanaged resources 
        /// can be disposed. 
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.sslStream != null)
                    {
                        this.sslStream.Dispose();
                        this.sslStream = null;
                    }
                    if (this.streamProxy != null)
                    {
                        this.streamProxy.Dispose();
                        this.streamProxy = null;
                    }
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:

                this.disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}