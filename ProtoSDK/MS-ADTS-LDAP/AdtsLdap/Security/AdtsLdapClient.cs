// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// the security provider APIs of LDAP client-side, <para/>
    /// provides a friendly security APIs, and  LDAP security operation APIs.
    /// </summary>
    public partial class AdtsLdapClient
    {
        #region Fields

        /// <summary>
        /// a const string that specifies the start TLS request name for ExtendedOperation.
        /// </summary>
        private const string START_TLS_REQUEST_NAME = "1.3.6.1.4.1.1466.20037";

        /// <summary>
        /// a const string that specifies the fast bind request name for ExtendedOperation.
        /// </summary>
        private const string FAST_BIND_REQUEST_NAME = "1.2.840.113556.1.4.1781";

        /// <summary>
        /// a const string that specifies the authentication name using NTLM of sicily
        /// </summary>
        private const string SICILY_AUTH_NAME_NTLM = "NTLM";

        /// <summary>
        /// a const string that specifies the mechanism using GSS-SPNEGO of SASL.
        /// </summary>
        private const string SASL_MECHANISM_GSS_SPNEGO = "GSS-SPNEGO";

        /// <summary>
        /// a SecurityLayer object that provides the security layer.
        /// </summary>
        private AdtsLdapSecurityLayer security;

        /// <summary>
        /// a bool value that indicates whether using message security to encrypt/decrypt message.
        /// </summary>
        private bool usingMessageSecurity;

        #endregion

        #region Properties

        /// <summary>
        /// get/set a SecurityLayer object that provides the security layer.
        /// </summary>
        public AdtsLdapSecurityLayer Security
        {
            get
            {
                return this.security;
            }
            set
            {
                this.security = value;
            }
        }

        /// <summary>
        /// get/set a bool value that indicates whether using message security to encrypt/decrypt message.
        /// </summary>
        public bool UsingMessageSecurity
        {
            get
            {
                return this.usingMessageSecurity;
            }
            set
            {
                this.usingMessageSecurity = value;
            }
        }

        #endregion

        #region Security Apis

        /// <summary>
        /// encrypt the data with the security layer.<para/>
        /// if the security layer is not ready or do not using message security, return the data directly.<para/>
        /// otherwise, return the encrypted data with the security layer.
        /// </summary>
        /// <param name="data">
        /// a bytes data that is used to encrypt.
        /// </param>
        /// <returns>
        /// a bytes data that contains the encrypted data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when security layer of ldap client is null
        /// </exception>
        public byte[] Encrypt(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (this.security == null)
            {
                throw new InvalidOperationException("security layer of ldap client is null");
            }

            this.security.UsingMessageSecurity = this.usingMessageSecurity;

            return this.security.Encode(data);
        }


        /// <summary>
        /// decrypt the data with the security layer.<para/>
        /// if the security layer is not ready or do not using message security, return the data directly.<para/>
        /// otherwise, return the decrypted data with the security layer.
        /// </summary>
        /// <param name="data">
        /// a bytes data that is used to decrypt.
        /// </param>
        /// <returns>
        /// a bytes data that contains the decrypted data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when security layer of ldap client is null
        /// </exception>
        public byte[] Decrypt(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (this.security == null)
            {
                throw new InvalidOperationException("security layer of ldap client is null");
            }

            this.security.UsingMessageSecurity = this.usingMessageSecurity;

            return this.security.Decode(data);
        }


        /// <summary>
        /// create the SASL bind request,<para/>
        /// which is sent to server to request a SASL bind to provide more strong security.
        /// </summary>
        /// <param name="securityContext">
        /// a ClientSecurityContext object that specifies the security provider.
        /// </param>
        /// <param name="enableMessageSecurity">
        /// a bool value that indicates whether enable message security.
        /// </param>
        /// <returns>
        /// a BindRequestPacket object that requests the SASL bind.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when securityContext is null.
        /// </exception>
        public AdtsBindRequestPacket CreateSaslBindRequest(
            ClientSecurityContext securityContext, bool enableMessageSecurity)
        {
            if (securityContext == null)
            {
                throw new ArgumentNullException("securityContext");
            }

            if (this.security == null)
            {
                this.usingMessageSecurity = enableMessageSecurity;
                this.security = new AdtsLdapSaslSecurityLayer(securityContext);
            }

            return this.CreateSaslBindRequest(SASL_MECHANISM_GSS_SPNEGO, securityContext.Token);
        }


        /// <summary>
        /// create the Sicily bind request, <para/>
        /// which is sent to server to request a sicily bind, windows implements this as NTLM authenticate.<para/>
        /// this is the first roundtrip, which contains the Sicily Discovery request.
        /// </summary>
        /// <param name="securityContext">
        /// a ClientSecurityContext object that specifies the security provider.
        /// </param>
        /// <param name="enableMessageSecurity">
        /// a bool value that indicates whether enable message security.
        /// </param>
        /// <returns>
        /// a BindRequestPacket object that requests the Sicily bind.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when securityContext is null.
        /// </exception>
        public AdtsBindRequestPacket CreateSicilyRequest(
            ClientSecurityContext securityContext, bool enableMessageSecurity)
        {
            if (securityContext == null)
            {
                throw new ArgumentNullException("securityContext");
            }

            if (this.security == null)
            {
                this.usingMessageSecurity = enableMessageSecurity;
                this.security = new AdtsLdapSaslSecurityLayer(securityContext);
            }

            if (securityContext.NeedContinueProcessing)
            {
                return this.CreateSicilyNegotiateBindRequest(SICILY_AUTH_NAME_NTLM, securityContext.Token);
            }
            else
            {
                return this.CreateSicilyResponseBindRequest(securityContext.Token);
            }
        }


        /// <summary>
        /// create the mutual authentication request<para/>
        /// which is sent to server to complete the mutual authentication.
        /// </summary>
        /// <param name="securityContext">
        /// a ClientSecurityContext object that specifies the security provider.<para/>
        /// the SecurityPackageType must be SecurityPackageType.Negotiate.
        /// </param>
        /// <param name="enableMessageSecurity">
        /// a bool value that indicates whether enable message security.
        /// </param>
        /// <returns>
        /// a BindRequestPacket object that requests the mutual authentication.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// if mutual authenticate mode, only SecurityPackageType.Negotiate is available
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// thrown when securityContext is null.
        /// </exception>
        public AdtsBindRequestPacket CreateMutualAuthenticationRequest(
            ClientSecurityContext securityContext, bool enableMessageSecurity)
        {
            if (securityContext == null)
            {
                throw new ArgumentNullException("securityContext");
            }

            if (securityContext.PackageType != SecurityPackageType.Negotiate)
            {
                throw new InvalidOperationException(
                    "if mutual authenticate mode, only SecurityPackageType.Negotiate is available");
            }

            return this.CreateSaslBindRequest(securityContext, enableMessageSecurity);
        }


        /// <summary>
        /// create the Start TLS request packet.<para/>
        /// client must connect as simple LDAP(without SSL/TLS),
        /// then sends this request to server to start TLS.
        /// </summary>
        /// <returns>
        /// an ExtendedRequestPacket object that requests the StartSTL operation.
        /// </returns>
        public AdtsExtendedRequestPacket CreateStartTlsRequest()
        {
            return this.CreateExtendedRequest(START_TLS_REQUEST_NAME, null);
        }


        /// <summary>
        /// create the Fast Bind request packet.<para/>
        /// both standard LDAP(without SSL/TLS) and SSL/TLS connection can send this request to server.
        /// </summary>
        /// <returns>
        /// an ExtendedRequestPacket object that requests the fast bind.
        /// </returns>
        public AdtsExtendedRequestPacket CreateFastBindRequest()
        {
            return this.CreateExtendedRequest(FAST_BIND_REQUEST_NAME, null);
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
        /// <param name="enableMessageSecurity">
        /// a bool value that indicates whether enable message security.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when targetHost is null.
        /// </exception>
        public void SslAuthenticate(string targetHost, bool enableMessageSecurity)
        {
            if (targetHost == null)
            {
                throw new ArgumentNullException("targetHost");
            }

            // if null or other security, initialize the ssl security.
            // when bind before start tls, the security will be set to other security.
            if (this.security == null || !(this.security is AdtsLdapSslTlsSecurityLayer))
            {
                this.security = new AdtsLdapSslTlsSecurityLayer(this);
                this.usingMessageSecurity = enableMessageSecurity;
            }

            (this.security as AdtsLdapSslTlsSecurityLayer).AuthenticateAsClient(targetHost);
        }

        #endregion
    }
}