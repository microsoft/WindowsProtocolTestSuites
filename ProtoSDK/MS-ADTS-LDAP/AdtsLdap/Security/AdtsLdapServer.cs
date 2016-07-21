// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;
using System;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// security provider for AdtsLdaptServer, <para/>
    /// provides a friendly security interfaces,
    /// and LDAP operation Apis.
    /// </summary>
    public partial class AdtsLdapServer
    {
        #region Properties

        /// <summary>
        /// get an AdtsLdapContextManager object that manage the contexts.
        /// </summary>
        internal AdtsLdapContextManager ContextManager
        {
            get
            {
                return this.contextManager;
            }
        }


        /// <summary>
        /// get a bool value that specifies whether the connection is overTCP(including SSL/TLS)
        /// if true, connection is over TCP; otherwise, over UDP.
        /// </summary>
        internal bool IsTcp
        {
            get
            {
                return this.isTcp;
            }
        }

        #endregion

        #region Raw Api

        /// <summary>
        /// expect client connect to server.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan object that specifies the timeout to wait for client.
        /// </param>
        /// <returns>
        /// an AdtsLdapContext object that indicates the identity of connected client.
        /// </returns>
        public AdtsLdapContext ExpectConnection(TimeSpan timeout)
        {
            TransportEvent transportEvent = this.transportStack.ExpectTransportEvent(timeout);

            if (transportEvent.EventType == EventType.Connected)
            {
                AdtsLdapContext context =
                    new AdtsLdapContext(AdtsLdapVersion.V3, transportEvent.EndPoint as IPEndPoint);

                this.contextManager.AddContext(context, this.isTcp);

                return context;
            }
            else if (transportEvent.EventType == EventType.Exception)
            {
                throw transportEvent.EventObject as Exception;
            }
            else
            {
                throw new InvalidOperationException("Unknown object received in transport.");
            }
        }

        #endregion

        #region Security Apis

        /// <summary>
        /// encrypt the data using the selected security mechanism.<para/>
        /// if encrypt successful, the decoder will decrypt received data automatically.<para/>
        /// if encrypt failed, return data directly, decoder will not decrypt received data.
        /// </summary>
        /// <param name="context">
        /// an AdtsLdapContext object that indicates the context of LDAP.
        /// </param>
        /// <param name="data">
        /// a bytes data that is used to encrypt.
        /// </param>
        /// <returns>
        /// a bytes data that contains the encrypted data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when context is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when security layer of ldap context is null
        /// </exception>
        public byte[] Encrypt(AdtsLdapContext context, byte[] data)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (context.Security == null)
            {
                throw new InvalidOperationException("security layer of ldap context is null");
            }

            context.Security.UsingMessageSecurity = context.UsingMessageSecurity;

            return context.Security.Encode(data);
        }


        /// <summary>
        /// decrypt the data using the selected security mechanism.
        /// </summary>
        /// <param name="context">
        /// an AdtsLdapContext object that indicates the context of LDAP.
        /// </param>
        /// <param name="data">
        /// a bytes data that is used to decrypt.
        /// </param>
        /// <returns>
        /// a bytes data that contains the decrypted data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when context is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when security layer of ldap context is null
        /// </exception>
        public byte[] Decrypt(AdtsLdapContext context, byte[] data)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (context.Security == null)
            {
                throw new InvalidOperationException("security layer of ldap context is null");
            }

            context.Security.UsingMessageSecurity = context.UsingMessageSecurity;

            return context.Security.Decode(data);
        }


        /// <summary>
        /// create sasl bind response packet.
        /// </summary>
        /// <param name="context">
        /// an AdtsLdapContext object that indicates the context of LDAP.
        /// </param>
        /// <param name="securityContext">
        /// a ServerSecurityContext object that specifies the security provider.
        /// </param>
        /// <param name="enableMessageSecurity">
        /// a bool value that indicates whether enable message security.
        /// </param>
        /// <returns>
        /// a BindResponsePacket object that responses the SASL bind request.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when context is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when securityContext is null.
        /// </exception>
        public AdtsBindResponsePacket CreateSaslBindResponse(
            AdtsLdapContext context, ServerSecurityContext securityContext, bool enableMessageSecurity)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (securityContext == null)
            {
                throw new ArgumentNullException("securityContext");
            }

            if (context.Security == null)
            {
                context.Security = new AdtsLdapSaslSecurityLayer(securityContext);
                context.UsingMessageSecurity = enableMessageSecurity;
            }

            ResultCode resultCode = ResultCode.Success;

            if (securityContext.NeedContinueProcessing)
            {
                resultCode = ResultCode.SaslBindInProgress;
            }

            return this.CreateBindResponse(
                context, resultCode,
                string.Empty, string.Empty, null, securityContext.Token);
        }


        /// <summary>
        /// create sicily bind response packet.
        /// </summary>
        /// <param name="context">
        /// an AdtsLdapContext object that indicates the context of LDAP.
        /// </param>
        /// <param name="securityContext">
        /// a ServerSecurityContext object that specifies the security provider.
        /// </param>
        /// <param name="enableMessageSecurity">
        /// a bool value that indicates whether enable message security.
        /// </param>
        /// <returns>
        /// a SicilyBindResponsePacket object that responses the Sicily bind request.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when context is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when securityContext is null.
        /// </exception>
        public AdtsSicilyBindResponsePacket CreateSicilyBindResponse(
            AdtsLdapContext context, ServerSecurityContext securityContext, bool enableMessageSecurity)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (securityContext == null)
            {
                throw new ArgumentNullException("securityContext");
            }

            if (context.Security == null)
            {
                context.Security = new AdtsLdapSaslSecurityLayer(securityContext);
                context.UsingMessageSecurity = enableMessageSecurity;
            }

            return this.CreateSicilyBindResponse(
                context, ResultCode.Success, securityContext.Token, string.Empty);
        }



        /// <summary>
        /// server authenticate over SSL/TLS with client.
        /// </summary>
        /// <param name="context">
        /// an AdtsLdapContext object that indicates the context of LDAP.
        /// </param>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the server.
        /// </param>
        /// <param name="encryptMessage">
        /// a bool value that indicates whether encrypt message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when context is null.
        /// </exception>
        public void SslStartup(AdtsLdapContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // if null or other security, initialize the ssl security.
            // when bind before start tls, the security will be set to other security.
            if (context.Security == null || !(context.Security is AdtsLdapSslTlsSecurityLayer))
            {
                context.Security = new AdtsLdapSslTlsSecurityLayer(this, context);
            }
        }


        /// <summary>
        /// server authenticate over SSL/TLS with client.
        /// </summary>
        /// <param name="context">
        /// an AdtsLdapContext object that indicates the context of LDAP.
        /// </param>
        /// <param name="certificate">
        /// a X509Certificate that specifies the certificate used to authenticate the server.
        /// </param>
        /// <param name="enableMessageSecurity">
        /// a bool value that indicates whether enable message security.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when context is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when certificate is null.
        /// </exception>
        public void SslAuthenticate(
            AdtsLdapContext context, X509Certificate certificate, bool enableMessageSecurity)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            this.SslStartup(context);

            AdtsLdapSslTlsSecurityLayer sslSecurity = context.Security as AdtsLdapSslTlsSecurityLayer;

            context.UsingMessageSecurity = enableMessageSecurity;

            sslSecurity.AuthenticateAsServer(certificate);
        }

        #endregion
    }
}